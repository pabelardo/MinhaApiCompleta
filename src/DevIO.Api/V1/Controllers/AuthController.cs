using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevIO.Api.Controllers;
using DevIO.Api.DTO;
using DevIO.Api.Extensions;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevIO.Api.V1.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
public class AuthController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager; //responsável por realizar a autenticação do usuário
    private readonly UserManager<IdentityUser> _userManager; //responsável por criar usuário
    private readonly AppSettings _appSettings;
    private readonly ILogger _logger;

    public AuthController(INotificador notificador,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager, 
        IOptions<AppSettings> appSettings,
        IUser user, ILogger<AuthController> logger) : base(notificador, user)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    [HttpPost("nova-conta")]
    public async Task<ActionResult> Registrar(RegisterUserDTO registerUser)
    {
        if(!ModelState.IsValid) return CustomResponse(ModelState);

        var user = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);

            return CustomResponse(await GerarJwt(user.Email));
        }

        foreach (var error in result.Errors) 
            NotificarErro(error.Description);

        return CustomResponse(registerUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserDTO loginUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

        if (result.Succeeded)
        {
            _logger.LogInformation($"Usuário {loginUser.Email} logado com sucesso");

            return CustomResponse(await GerarJwt(loginUser.Email));
        }

        if (result.IsLockedOut)
        {
            NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
            return CustomResponse(loginUser);   
        }

        NotificarErro("Usuário ou Senha incorretos");
            
        return CustomResponse(loginUser);
    }

    /// <summary>
    /// Método responsável por gerar o JSON Web Token
    /// </summary>
    /// <returns>Retorna o token em formato string</returns>
    private async Task<LoginResponseDTO> GerarJwt(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user); //Obtenho as claims do meu usuário
        var userRoles = await _userManager.GetRolesAsync(user); 

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id)); //Sub = Usuário
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email)); //Email = Email do usuário
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); //Um ID do Token gerado como Guid
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString())); //
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)); //Identifica o momento em que o JWT foi emitido

        foreach (var userRole in userRoles)
            claims.Add(new Claim("role", userRole));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
        {
            Issuer = _appSettings.Emissor,
            Audience = _appSettings.ValidoEm,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        var encodedToken = tokenHandler.WriteToken(token);

        var response = new LoginResponseDTO
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
            User = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c=> new ClaimDTO{ Type = c.Type, Value = c.Value})
            }
        };

        return response;
    }

    private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
}