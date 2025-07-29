using AutoMapper;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ChienVHShopOnline.Services;

public class OrdersService : IOrderService
{
    private readonly ChienVHShopDBEntities _context;
    private readonly IMapper _mapper;

    public OrdersService(ChienVHShopDBEntities context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Order> CreateOrder(OrderCreateDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        order.Status = "Completed";
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        Console.WriteLine(orderDto.OrderDetailsDto);

        // Add OrderDetails
        foreach (var item in orderDto.OrderDetailsDto)
        {
            var orderDetail = new OrderDetail
            {
                OrderID = order.OrderID,
                ProductID = item.ProductID,
                Price = item.Quantity * item.Price,
                Quantity = item.Quantity
            };

            _context.OrderDetails.Add(orderDetail);
        }

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Order> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            throw new Exception("Order not found");

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> GetOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
            throw new Exception("Order not found");

        var orderDetails = await _context.OrderDetails.Where(x => x.OrderID == order.OrderID).ToListAsync();

        return order;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        var orders = await _context.Orders
            .OrderByDescending(x => x.OrderID)
            .ToListAsync();

        var orderDetails = await _context.OrderDetails.OrderByDescending(x => x.OrderID).ToListAsync();

        return orders;
    }
}