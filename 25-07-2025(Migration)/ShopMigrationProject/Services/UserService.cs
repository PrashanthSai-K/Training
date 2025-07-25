using AutoMapper;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ChienVHShopOnline.Services;

public class UserService : IUserService
{
    private readonly ChienVHShopDBEntities _context;
    private readonly IMapper _mapper;

    public UserService(ChienVHShopDBEntities context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User> CreateUser(UserCreateDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            throw new Exception("User not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            throw new Exception("User not found");

        return user;
    }

    public async Task<IEnumerable<User>> GetUsers(int page, int pageSize)
    {
        var users = await _context.Users
                                       .OrderBy(c => c.Username)
                                       .Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
        return users;
    }
}