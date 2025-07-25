using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;

namespace ChienVHShopOnline.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrders();
    Task<Order> GetOrder(int id);
    Task<Order> CreateOrder(OrderCreateDto orderDto);
    Task<Order> DeleteOrder(int id);
}