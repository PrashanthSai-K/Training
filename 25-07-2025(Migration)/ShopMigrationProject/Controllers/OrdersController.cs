using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChienVHShopOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChienVHShopOnline.Models.Dto;
using ChienVHShopOnline.Interfaces;

namespace ChienVHShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrders();

            return Ok(orders);
        }

        // GET: api/order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrder(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            if (orderDto == null || orderDto.OrderDetailsDto == null || !orderDto.OrderDetailsDto.Any())
                return BadRequest("Order or Order details are invalid.");

            var order = await _orderService.CreateOrder(orderDto);

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderID }, order);
        }

        // DELETE: api/order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.DeleteOrder(id);
            return Ok(order);
        }
    }
}
