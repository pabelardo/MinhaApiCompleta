﻿using System;
using DevIO.Api.Controllers;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.V1.Controllers;

[ApiVersion("1.0", Deprecated = true)]
[Route("api/v{version:apiVersion}/[controller]")]
public class TesteController : MainController
{
    public TesteController(INotificador notificador, IUser appUser) : base(notificador, appUser) { }

    [HttpGet]
    [Obsolete("Use o método da versão 2 em vez desse")]
    public string Valor() => "Sou a V1";
}