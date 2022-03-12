﻿using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DevIO.Api.Extensions;
public class CustomAuthorize
{
    public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
    {
        return context.User.Identity is { IsAuthenticated: true } && context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
    }
}

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, claimValue) };
    }
}

public class RequisitoClaimFilter : IAuthorizationFilter
{
    private readonly Claim _claim;

    public RequisitoClaimFilter(Claim claim)
    {
        _claim = claim;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity is { IsAuthenticated: false }) //Se o usuário não está autenticado
        {
            context.Result = new StatusCodeResult(401);
            return;
        }

        if (!CustomAuthorize.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value)) //Se eu sei quem é o usuário mas ele não pode fazer o que ele está tentando
        {
            context.Result = new StatusCodeResult(403);
        }
    }
}

