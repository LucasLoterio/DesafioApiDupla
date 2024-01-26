using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioApi.Data;
using DesafioApi.Entidades;
using Microsoft.AspNetCore.Authorization;
using DesafioApi.Dto;

namespace DesafioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoesController : ControllerBase
    {
        private readonly ApiContext _context;

        public ProdutoesController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet("/api/[controller]/pages")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            List<Produto> produtos = await _context.Produtos.ToListAsync();
            foreach (Produto produto in produtos)
            {
                produto.Categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == produto.CategoriaId);
            }
            return produtos;
        }
        [HttpGet("api/[controller]/filtro")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosByDescricao([FromQuery] string descricao)
        {
            List<Produto> lista = await _context.Produtos.ToListAsync();
            var produtos = (from prod in lista
                            where prod.Descricao.ToLower().Contains(descricao.ToLower())
                            select prod).ToList(); 
            foreach (Produto produto in produtos)
            {
                produto.Categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == produto.CategoriaId);
            }
            return produtos;
        }

        // GET: api/Produtoes
        [HttpGet]
        [Authorize(Roles = "admin, gerente, funcionario")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            List<Produto> produtos = await _context.Produtos.ToListAsync();
            foreach (Produto produto in produtos)
            {
                produto.Categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == produto.CategoriaId);
            }
            return produtos;
        }

        [HttpGet("/api/[controller]/categoria/{id}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosByCategoria(
            [FromRoute] int id)
        {
            List<Produto> produtos = await _context.Produtos.ToListAsync();
            var prodByCategoria =
                (from prod in produtos
                 where prod.CategoriaId == id
                 select prod).ToList();

            Categoria categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            foreach (Produto produto in prodByCategoria)
            {
                produto.Categoria = categoria;
            }
            return prodByCategoria;
            /* return (from prod in produtos
                     where prod.CategoriaId == id
                     select prod).ToList();*/

        }

        // GET: api/Produtoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // PUT: api/Produtoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin, gerente")]
        public async Task<IActionResult> PutProduto(int id, CreateProdutoDto produtoDto)
        {
            Produto produto = new Produto();
            produto.Descricao = produtoDto.Descricao;
            produto.Preco = produtoDto.Preco;
            produto.Estoque = produtoDto.Estoque;
            produto.Categoria = _context.Categorias.FirstOrDefault(c => c.Id == produtoDto.CategoriaId);
            produto.Id = id;
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Produtoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin, gerente, funcionario")]
        public async Task<ActionResult<Produto>> PostProduto(CreateProdutoDto produtoDto)
        {
            Produto produto = new Produto();
            produto.Descricao = produtoDto.Descricao;
            produto.Preco = produtoDto.Preco;
            produto.Estoque = produtoDto.Estoque;
            produto.Categoria = _context.Categorias.FirstOrDefault(c => c.Id == produtoDto.CategoriaId);
            _context.Produtos.Add(produto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProdutoExists(produto.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduto", new { id = produto.Id }, produto);
        }

        // DELETE: api/Produtoes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
