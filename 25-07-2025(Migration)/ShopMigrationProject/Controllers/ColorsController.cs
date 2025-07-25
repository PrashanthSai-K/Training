using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChienVHShopOnline.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChienVHShopOnline.Models.Dto;
using AutoMapper;
using ChienVHShopOnline.Interfaces;

namespace ChienVHShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorsService _colorsService;

        public ColorsController(IColorsService colorsService)
        {
            _colorsService = colorsService;
        }

        // GET: api/Colors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors()
        {
            var colors = await _colorsService.GetColors();
            return Ok(colors);
        }

        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(int id)
        {
            var color = await _colorsService.GetColorById(id);
            if (color == null)
            {
                return NotFound();
            }

            return color;
        }

        // POST: api/Colors
        [HttpPost]
        public async Task<ActionResult<Color>> PostColor(ColorCreateDto colorDto)
        {
            var color = await _colorsService.CreateColor(colorDto);
            return CreatedAtAction(nameof(GetColor), new { id = color.ColorId }, color);
        }

        // PUT: api/Colors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(int id, ColorUpdateDto colorDto)
        {
            if (id != colorDto.ColorId)
            {
                return BadRequest("Color ID mismatch.");
            }
            var color = await _colorsService.UpdateColor(id, colorDto);

            return Ok(color);
        }

        // DELETE: api/Colors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var color = await _colorsService.DeleteColor(id);

            return Ok(color);
        }

    }
}
