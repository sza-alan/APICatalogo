using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    //[HttpGet("/primeiro")]
    //public ActionResult<Produto> GetPrimeiro()
    //{
    //    var produto = _context.Produtos.FirstOrDefault();

    //    if (produto == null)
    //    {
    //        return NotFound("Produtos nao encontrados...");
    //    }

    //    return produto;
    //}

    //[HttpGet("{valor:alpha:length(5)}")]
    //public ActionResult<Produto> Get2(string valor)
    //{
    //    var teste = valor;
    //    return _context.Produtos.FirstOrDefault();
    //}

    //[HttpGet]
    //public ActionResult<Produto> Get()
    //{
    //    var produto = _context.Produtos.FirstOrDefault();

    //    if (produto == null)
    //    {
    //        return NotFound("Produtos nao encontrados...");
    //    }

    //    return produto;
    //}

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get2()
    {
        return  await _context.Produtos.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id}", Name="ObterProduto")]
    public async Task<ActionResult<Produto>> Get(int id)
    {
        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.ProdutoId == id);

        if(produto == null)
        {
            return NotFound("Produto nao encontrado...");
        }

        return produto;
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto == null)
        {
            return BadRequest();
        }

        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        _context.Produtos?.Add(produto);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest();
        }

        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
        //var produto = _context.Produtos.Find(id);

        if (produto == null)
        {
            return NotFound("Produto nao localizado...");
        }

        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return Ok(produto);
    }
}
