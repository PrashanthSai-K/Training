using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;

namespace ChienVHShopOnline.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers(int page, int pageSize);
    Task<User> GetUser(int id);
    Task<User> CreateUser(UserCreateDto userDto);
    Task<User> DeleteUser(int id);
}