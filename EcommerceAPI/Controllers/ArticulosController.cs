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
    public class ArticulosController : ControllerBase
    {
        private readonly IArticuloService _articuloService;

        public ArticulosController(IArticuloService articuloService)
        {
            _articuloService = articuloService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticuloDTO>>> GetAll()
        {
            var articulos = await _articuloService.GetAllAsync();
            return Ok(articulos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArticuloDTO>> GetById(int id)
        {
            var articulo = await _articuloService.GetByIdAsync(id);
            if (articulo == null)
            {
                return NotFound($"Artículo con ID {id} no encontrado");
            }
            return Ok(articulo);
        }

        [HttpGet("codigo/{codigo}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArticuloDTO>> GetByCodigo(string codigo)
        {
            var articulo = await _articuloService.GetByCodigoAsync(codigo);
            if (articulo == null)
            {
                return NotFound($"Artículo con código {codigo} no encontrado");
            }
            return Ok(articulo);
        }

        [HttpGet("categoria/{categoriaId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticuloDTO>>> GetByCategoriaId(int categoriaId)
        {
            var articulos = await _articuloService.GetByCategoriaIdAsync(categoriaId);
            return Ok(articulos);
        }

        [HttpGet("subcategoria/{subcategoriaId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticuloDTO>>> GetBySubcategoriaId(int subcategoriaId)
        {
            var articulos = await _articuloService.GetBySubcategoriaIdAsync(subcategoriaId);
            return Ok(articulos);
        }

        [HttpGet("activos")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticuloDTO>>> GetActive()
        {
            var articulos = await _articuloService.GetActiveAsync();
            return Ok(articulos);
        }

        [HttpGet("categoria/{categoriaId}/activos")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticuloDTO>>> GetActiveByCategoria(int categoriaId)
        {
            var articulos = await _articuloService.GetActiveByCategoriaAsync(categoriaId);
            return Ok(articulos);
        }

        [HttpGet("subcategoria/{subcategoriaId}/activos")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticuloDTO>>> GetActiveBySubcategoria(int subcategoriaId)
        {
            var articulos = await _articuloService.GetActiveBySubcategoriaAsync(subcategoriaId);
            return Ok(articulos);
        }

        [HttpPost("buscar")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticuloBusquedaDTO>>> BuscarArticulos([FromBody] BusquedaRequest request)
        {
            try
            {
                var articulos = await _articuloService.BuscarArticulosAsync(request);
                return Ok(articulos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("buscar/count")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> GetTotalCount([FromQuery] string? termino, [FromQuery] int? categoriaId,
            [FromQuery] int? subcategoriaId, [FromQuery] decimal? precioMin, [FromQuery] decimal? precioMax,
            [FromQuery] bool soloConStock = false)
        {
            try
            {
                var request = new BusquedaRequest
                {
                    Termino = termino,
                    CategoriaId = categoriaId,
                    SubcategoriaId = subcategoriaId,
                    PrecioMin = precioMin,
                    PrecioMax = precioMax,
                    SoloConStock = soloConStock
                };

                var total = await _articuloService.GetTotalCountAsync(request);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ArticuloDTO>> Create(ArticuloDTO articuloDto)
        {
            try
            {
                var articulo = await _articuloService.CreateAsync(articuloDto);
                return CreatedAtAction(nameof(GetById), new { id = articulo.Id }, articulo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ArticuloDTO>> Update(int id, ArticuloDTO articuloDto)
        {
            try
            {
                var articulo = await _articuloService.UpdateAsync(id, articuloDto);
                return Ok(articulo);
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
                await _articuloService.DeleteAsync(id);
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
