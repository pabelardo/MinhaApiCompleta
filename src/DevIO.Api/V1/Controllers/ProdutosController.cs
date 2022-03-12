using System;
using System.Collections.Generic;
using System.IO;
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
public class ProdutosController : MainController
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IProdutoService _produtoService;
    private readonly IMapper _mapper;

    public ProdutosController(
        INotificador notificador,
        IProdutoRepository produtoRepository,
        IProdutoService produtoService,
        IMapper mapper,
        IUser user) : base(notificador, user)
    {
        _produtoRepository = produtoRepository;
        _produtoService = produtoService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<ProdutoDTO>> ObterTodos() => _mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.ObterProdutosFornecedores());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProdutoDTO>> ObterPorId(Guid id)
    {
        var produtoDto = await ObterProduto(id);

        if(produtoDto == null) 
            return NotFound();

        return produtoDto;
    }

    [ClaimsAuthorize("Produto", "Adicionar")]
    [HttpPost]
    public async Task<ActionResult<ProdutoDTO>> Adicionar(ProdutoDTO produtoDTO)
    {
        if (!ModelState.IsValid) 
            return CustomResponse(ModelState);

        string imagemNome = string.Concat(Guid.NewGuid(), "_", produtoDTO.Imagem);

        if (!UploadArquivo(produtoDTO.ImagemUpload, imagemNome)) 
            return CustomResponse(produtoDTO);

        produtoDTO.Imagem = imagemNome;

        await _produtoService.Adicionar(_mapper.Map<Produto>(produtoDTO));

        return CustomResponse(produtoDTO);
    }
    
    [ClaimsAuthorize("Produto", "Atualizar")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar (Guid id, ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.Id)
        {
            NotificarErro("Os ids informados não são iguais!");

            return CustomResponse();
        }

        var produtoAtualizacao = await ObterProduto(id);

        produtoDTO.Imagem = produtoAtualizacao.Imagem;

        if (!ModelState.IsValid) 
            return CustomResponse(ModelState);

        if(!string.IsNullOrEmpty(produtoDTO.ImagemUpload))
        {
            var imagemNome = string.Concat(Guid.NewGuid(), "_", produtoDTO.Imagem);

            if(!UploadArquivo(produtoDTO.ImagemUpload, imagemNome)) 
                return CustomResponse(ModelState);

            produtoAtualizacao.Imagem = imagemNome;
        }

        produtoAtualizacao.Nome = produtoDTO.Nome;

        produtoAtualizacao.Descricao = produtoDTO.Descricao;

        produtoAtualizacao.Valor = produtoDTO.Valor;

        produtoAtualizacao.Ativo = produtoDTO.Ativo;

        await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

        return CustomResponse(produtoDTO);
    }

    [ClaimsAuthorize("Produto", "Excluir")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ProdutoDTO>> Excluir(Guid id)
    {
        var produto = await ObterProduto(id);

        if(produto == null) 
            return NotFound();

        await _produtoService.Remover(id);

        return CustomResponse(produto);
    }

    private bool UploadArquivo(string arquivo, string imgNome)
    {
        if (string.IsNullOrEmpty(arquivo))
        {
            NotificarErro("Forneça uma imagem para este produto!");
            return false;
        }

        var imageDataByteArray = Convert.FromBase64String(arquivo);

        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgNome);

        if (System.IO.File.Exists(filePath))
        {
            NotificarErro("Já existe um arquivo com este nome!");
            return false;
        }

        System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

        return true;
    }

    private async Task<ProdutoDTO> ObterProduto(Guid id) => _mapper.Map<ProdutoDTO>(await _produtoRepository.ObterProdutoFornecedor(id));
}