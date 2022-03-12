using System;
using System.Runtime.InteropServices;
using DevIO.Api.Controllers;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevIO.Api.V2.Controllers;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TesteController : MainController
{
    private readonly ILogger _logger;

    public TesteController(INotificador notificador, IUser appUser, ILogger<TesteController> logger) : base(notificador, appUser)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Valor()
    {
        _logger.LogTrace("Log de Trace"); // 1 -  registro de rastreamento, ou seja, o processo começou tal hora e terminou tal hora.

        _logger.LogDebug("Log de Debug"); // 2 - informações de debug.

        //O primeiro e o segundo, devem ser utilizados durante o ambiente de desenvolvimento. Do 3 em diante, utilizamos para sua aplicação.

        _logger.LogInformation("log de Informação"); // 3 - Grava qualquer coisa que você deseja informar que não seja nada de importante, mas que você queira registrar.

        _logger.LogWarning("Log de Aviso"); // 4 - Aqui você grava quando é um erro que não deveria acontecer.

        _logger.LogError("Log de Erro"); // 5 - É o log de erro propriamente dito.

        _logger.LogCritical("Log de Problema Critico"); // 6 - Esse é um erro que ameaça a saúde da sua aplicação. Aqui é um erro mais sério.

        return "Sou a V2";
    }
}