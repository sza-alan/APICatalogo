using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IRepository<Produto> _repository;

    public ProdutosController(IRepository<Produto> repository, IProdutoRepository produtoRepository)
    {
        _repository = repository;
        _produtoRepository = produtoRepository;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        var produtos = _produtoRepository.GetProdutosPorCategoria(id);
        
        if (produtos is null)
            return NotFound();

        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _repository.GetAll();

        if (produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);
    }

    [HttpGet("{id}", Name="ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _repository.Get(p => p.ProdutoId == id);

        if(produto == null)
        {
            return NotFound("Produto nao encontrado...");
        }

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto == null)
        {
            return BadRequest();
        }

        var novoProduto = _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest();
        }

        var produtoPtualizado = _repository.Update(produto);

        return Ok(produtoPtualizado);
        
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _repository.Get(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound("Produto nao encontrado.");

        var produtoDeltado = _repository.Delete(produto);
        return Ok(produtoDeltado);
    }
}
