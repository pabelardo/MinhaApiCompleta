using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace DevIO.Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificador _notificador;

        public MainController(INotificador notificador)
        {
            _notificador = notificador;
        }

        public bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

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

        protected void NotificarErro(string message)
        {
            _notificador.Handle(new Notificacao(message));
        }
    }
}
