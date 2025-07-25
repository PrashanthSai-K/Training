using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;
using AutoMapper;
using ChienVHShopOnline.Interfaces;

namespace ChienVHShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            _modelService = modelService;
        }

        // GET: api/Model
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model>>> GetModels([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var models = await _modelService.GetModels(page, pageSize);
            return Ok(models);
        }

        // GET: api/Model/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> GetModel(int id)
        {
            var model = await _modelService.GetModel(id);

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        // POST: api/Model
        [HttpPost]
        public async Task<ActionResult<Model>> CreateModel(ModelCreateDto modelDto)
        {
            var model = await _modelService.CreateModel(modelDto);
            return CreatedAtAction(nameof(GetModels), new { id = model.ModelId }, model);
        }

        // PUT: api/Model/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModel(int id, ModelUpdateDto modelDto)
        {
            if (id != modelDto.ModelId)
                return BadRequest();
            var model = await _modelService.UpdateModel(id, modelDto);
            return Ok(model);
        }

        // DELETE: api/Model/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var model = await _modelService.DeleteModel(id);
            return Ok(model);
        }
    }
}
