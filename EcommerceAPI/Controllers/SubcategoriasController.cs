using EcommerceAPI.DTOs;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubcategoriasController : ControllerBase
    {
        private readonly ISubcategoriaService _subcategoriaService;

        public SubcategoriasController(ISubcategoriaService subcategoriaService)
        {
            _subcategoriaService = subcategoriaService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SubcategoriaDTO>>> GetAll()
        {
            var subcategorias = await _subcategoriaService.GetAllAsync();
            return Ok(subcategorias);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<SubcategoriaDTO>> GetById(int id)
        {
            var subcategoria = await _subcategoriaService.GetByIdAsync(id);
            if (subcategoria == null)
            {
                return NotFound($"Subcategoría con ID {id} no encontrada");
            }
            return Ok(subcategoria);
        }

        [HttpGet("categoria/{categoriaId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SubcategoriaDTO>>> GetByCategoriaId(int categoriaId)
        {
            var subcategorias = await _subcategoriaService.GetByCategoriaIdAsync(categoriaId);
            return Ok(subcategorias);
        }

        [HttpGet("activas")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SubcategoriaDTO>>> GetActive()
        {
            var subcategorias = await _subcategoriaService.GetActiveAsync();
            return Ok(subcategorias);
        }

        [HttpGet("categoria/{categoriaId}/activas")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SubcategoriaDTO>>> GetActiveByCategoria(int categoriaId)
        {
            var subcategorias = await _subcategoriaService.GetActiveByCategoriaAsync(categoriaId);
            return Ok(subcategorias);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SubcategoriaDTO>> Create(SubcategoriaDTO subcategoriaDto)
        {
            try
            {
                var subcategoria = await _subcategoriaService.CreateAsync(subcategoriaDto);
                return CreatedAtAction(nameof(GetById), new { id = subcategoria.Id }, subcategoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SubcategoriaDTO>> Update(int id, SubcategoriaDTO subcategoriaDto)
        {
            try
            {
                var subcategoria = await _subcategoriaService.UpdateAsync(id, subcategoriaDto);
                return Ok(subcategoria);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _subcategoriaService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
