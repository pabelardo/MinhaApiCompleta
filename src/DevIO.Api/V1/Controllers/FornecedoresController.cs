using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.Controllers;
using DevIO.Api.DTO;
using DevIO.Api.Extensions;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.V1.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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
        IEnderecoRepository enderecoRepository,
        IUser user) : base(notificador, user)
    {
        _fornecedorRepository = fornecedorRepository;
        _fornecedorService = fornecedorService;
        _mapper = mapper;
        _enderecoRepository = enderecoRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<FornecedorDTO>> ObterTodos()
    {
        var fornecedor = _mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepository.ObterTodos());

        return fornecedor;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FornecedorDTO>> ObterPorId(Guid id)
    {
        var fornecedor = await ObterFornecedorProdutosEndereco(id);

        if (fornecedor == null) return NotFound();

        return Ok(fornecedor);
    }

    [ClaimsAuthorize("Fornecedor", "Adicionar")]
    [HttpPost]
    public async Task<ActionResult<FornecedorDTO>> Adicionar(FornecedorDTO fornecedorDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        //Eu quero um fornecedor da entidade da classe de negócios e eu vou mapear do request que eu recebi
        //Preciso chamar o serviço, pois o mesmo grava as informações do banco.
        //Só devo chamar o repositório caso queira buscar algum dado no banco.

        if (await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorDto)))
        {
            fornecedorDto.Endereco.FornecedorId = fornecedorDto.Id;

            fornecedorDto.Endereco.Id = ObterEnderecoId(fornecedorDto.Endereco.FornecedorId).Result.Id;
        }

        return CustomResponse(fornecedorDto);
    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
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

    [ClaimsAuthorize("Fornecedor", "Excluir")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<FornecedorDTO>> Excluir(Guid id)
    {
        var fornecedorDto = await ObterFornecedorEndereco(id);

        if (fornecedorDto == null) return NotFound();

        await _fornecedorService.Remover(id);

        return CustomResponse();
    }

    [HttpGet("endereco/{id:guid}")]
    public async Task<EnderecoDTO> ObterEnderecoPorId(Guid id)
    {
        return _mapper.Map<EnderecoDTO>(await _enderecoRepository.ObterPorId(id));
    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPut("endereco/{id:guid}")]
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

    private async Task<FornecedorDTO> ObterFornecedorProdutosEndereco(Guid id) => _mapper.Map<FornecedorDTO>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));

    private async Task<FornecedorDTO> ObterFornecedorEndereco(Guid id) => _mapper.Map<FornecedorDTO>(await _fornecedorRepository.ObterFornecedorEndereco(id));

    private async Task<EnderecoDTO> ObterEnderecoId(Guid fornecedorId) => _mapper.Map<EnderecoDTO>(await _enderecoRepository.ObterEnderecoPorFornecedor(fornecedorId));
}