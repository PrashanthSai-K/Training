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
    public class NewsController : ControllerBase
    {
        // private readonly IUserService _userService;
              private readonly ChienVHShopDBEntities _context;
        private readonly IMapper _mapper;

        public NewsController(IUserService userService, ChienVHShopDBEntities dbEntities, IMapper mapper)
        {
            // _userService = userService;
            _context = dbEntities;
            _mapper = mapper;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var news = await _context.News.OrderBy(n => n.Title).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(news);
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNewsById(int id)
        {
            var news = await _context.News.FindAsync(id);

            if (news == null)
                return NotFound();

            return Ok(news);
        }

        // POST: api/News
        [HttpPost]
        public async Task<ActionResult<News>> CreateNews(NewsCreateDto newsDto)
        {
            var news = _mapper.Map<News>(newsDto);

            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNews), new { id = news.NewsId }, news);
        }

        // UPDATE: api/News/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, NewsUpdateDto newsdto)
        {
            if (id != newsdto.NewsId)
                return BadRequest("Id mismatch");

            var news = _mapper.Map<News>(newsdto);
            _context.Entry(news).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.News.Any(e => e.NewsId == id))
                    throw new Exception("news not found");
                else
                    throw;
            }

            return Ok(news);
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
                return NotFound();
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return Ok(news);
        }
    }
}
