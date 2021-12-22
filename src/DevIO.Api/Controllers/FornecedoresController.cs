using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.DTO;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                      IMapper mapper,
                                      IFornecedorService fornecedorService,
                                      INotificador notificador, 
                                      IEnderecoRepository enderecoRepository) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
            _enderecoRepository = enderecoRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<FornecedorDTO>> ObterTodos()
        {
            // quando você coloca o await, quer dizer que já é o resultado da variável fornecedor, caso contrário ele é só uma Task
            var fornecedor = _mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepository.ObterTodos());

            return fornecedor;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> ObterPorId(Guid id)
        {
            // quando você coloca o await, quer dizer que já é o resultado da variável fornecedor, caso contrário ele é só uma Task
            var fornecedor = await ObterFornecedorProdutosEndereco(id);

            if (fornecedor == null) return NotFound();

            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorDTO>> Adicionar(FornecedorDTO fornecedorDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            //Eu quero um fornecedor da entidade da classe de negócios e eu vou mapear do request que eu recebi
            //Preciso chamar o serviço, pois o mesmo grava as informações do banco.
            //Só devo chamar o repositório caso queira buscar algum dado no banco.

            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorDto));

            return CustomResponse(fornecedorDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> Atualizar(Guid id, FornecedorDTO fornecedorDto)
        {
            if (id != fornecedorDto.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query.");
                return CustomResponse(fornecedorDto);

            }

            if(!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorDto));

            return CustomResponse(fornecedorDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> Excluir(Guid id)
        {
            var fornecedorDto = await ObterFornecedorEndereco(id);

            if (fornecedorDto == null) return NotFound();

            await _fornecedorService.Remover(id);

            return CustomResponse();
        }

        [HttpGet("obter-endereco/{id:guid}")]
        public async Task<EnderecoDTO> ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<EnderecoDTO>(await _enderecoRepository.ObterPorId(id));
        }

        [HttpPut("atualizar-endereco/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoDTO enderecoDto)
        {
            if (id != enderecoDto.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query.");
                return CustomResponse(enderecoDto);

            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoDto));

            return CustomResponse(enderecoDto);
        }

        public async Task<FornecedorDTO> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }

        public async Task<FornecedorDTO> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
    }
}