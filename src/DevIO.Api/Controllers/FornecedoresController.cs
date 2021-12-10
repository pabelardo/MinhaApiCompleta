using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.DTO;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                      IMapper mapper, IFornecedorService fornecedorService)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
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
            if (!ModelState.IsValid) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDto); //Eu quero um fornecedor da entidade da classe de negócios e eu vou mapear do request que eu recebi

            //Preciso chamar o serviço, pois o mesmo grava as informações do banco.
            //Só devo chamar o repositório caso queira buscar algum dado no banco.

            var adicionouNoBanco = await _fornecedorService.Adicionar(fornecedor);

            if (!adicionouNoBanco) return BadRequest();

            return Ok(fornecedor);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> Atualizar(Guid id, FornecedorDTO fornecedorDto)
        {
            if (id != fornecedorDto.Id) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDto); //Eu quero um fornecedor da entidade da classe de negócios e eu vou mapear do request que eu recebi

            //Preciso chamar o serviço, pois o mesmo grava as informações do banco.
            //Só devo chamar o repositório caso queira buscar algum dado no banco.

            var atualizouNoBanco = await _fornecedorService.Atualizar(fornecedor);

            if (!atualizouNoBanco) return BadRequest();

            return Ok(fornecedor);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> AtualizarEndereco(Guid id, EnderecoDTO enderecoDto)
        {
            if (id != enderecoDto.Id) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var enderecoFornecedor = _mapper.Map<Endereco>(enderecoDto); //Eu quero um fornecedor da entidade da classe de negócios e eu vou mapear do request que eu recebi

            //Preciso chamar o serviço, pois o mesmo grava as informações do banco.
            //Só devo chamar o repositório caso queira buscar algum dado no banco.

            var atualizouNoBanco = await _fornecedorService.AtualizarEndereco(enderecoFornecedor);

            if (!atualizouNoBanco) return BadRequest();

            return Ok(enderecoFornecedor);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> Excluir(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null) return NotFound();

            var removeuDoBanco = await _fornecedorService.Remover(id);

            if (!removeuDoBanco) return BadRequest();

            return Ok(fornecedor);
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