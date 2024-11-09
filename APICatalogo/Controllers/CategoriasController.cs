using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public CategoriasController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet("LerArquivoConfiguracao")]
    public string GetValores()
    {
        var valor1 = _configuration["chave1"];
        var valor2 = _configuration["chave2"];

        var secao1 = _configuration["secao1:chave2"];

        return $"Chave1 = {valor1} \nChave2 = {valor2} \nSecao1 => Chave2 = {secao1}";
    }

    [HttpGet("UsandoFromServices/{nome}")]
    public ActionResult<string> GetSaudacaoFromServices([FromServices] IMeuServico meuServico, string nome)
    {
        return meuServico.Saudacao(nome);
    }

    [HttpGet("SemUsarFromServices/{nome}")]
    public ActionResult<string> GetSaudacaoSemFromServices(IMeuServico meuServico, string nome)
    {
        return meuServico.Saudacao(nome);
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        return _context.Categorias.Include(p => p.Produtos).ToList();
        //return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).ToList();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        try
        {
            return _context.Categorias.AsNoTracking().ToList();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitacao");
        }

    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        throw new Exception("Excecao ao retornar a categoria pelo Id");
        //string[] teste = null;
        //if (teste.Length > 0)
        //{

        //}

        var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

        if (categoria == null)
        {
            return NotFound($"Categoria com id= {id} nao encontrada...");
        }

        return Ok(categoria);
    }

    //[HttpGet("{id:int}", Name = "ObterCategoria")]
    //public ActionResult<Categoria> Get(int id)
    //{
    //    try
    //    {
    //        var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

    //        if (categoria == null)
    //        {
    //            return NotFound($"Categoria com id= {id} nao encontrada...");
    //        }

    //        return Ok(categoria);
    //    }
    //    catch (Exception)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError,
    //            "Ocorreu um problema ao tratar a sua solicitacao");
    //    }
        
    //}

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria == null)
        {
            return BadRequest("Dados invalidos");
        }

        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return BadRequest($"Categoria com id= {id} nao encontrada...");
        }

        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

        if (categoria == null)
        {
            return NotFound("Categoria nao encontrada...");
        }

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return Ok(categoria);
    }
}
