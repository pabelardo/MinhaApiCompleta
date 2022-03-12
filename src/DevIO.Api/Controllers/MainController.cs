using System;
using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace DevIO.Api.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    private readonly INotificador _notificador;
    private readonly IUser AppUser;
    protected Guid UsuarioId { get; set; }
    protected bool UsuarioAutenticado { get; set; }

    protected MainController(INotificador notificador, IUser appUser)
    {
        _notificador = notificador;
        AppUser = appUser;

        if (appUser.IsAuthenticated())
        {
            UsuarioId = appUser.GetUserId();
            UsuarioAutenticado = true;
        }
    }

    protected bool OperacaoValida() => !_notificador.TemNotificacao();

    protected ActionResult CustomResponse(object result = null)
    {
        if(OperacaoValida())
            return Ok(new
            {
                sucess = true,
                data = result
            });

        return BadRequest(new
        {
            sucess = false,
            errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
        });
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        if(!ModelState.IsValid) NotificarErroModelInvalida(modelState);
        return CustomResponse();
    }

    protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);

        if(erros.Any())
            foreach (var erro in erros)
            {
                var errorMessage = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMessage);
            }
    }

    protected void NotificarErro(string message) => _notificador.Handle(new Notificacao(message));
}