using System;
using System.Threading.Tasks;
using Notify.Interfaces;
using Notify.Context;

namespace Notify.Repository;

public abstract class Repository<K, T> : IRepository<K, T> where T : class
{
    protected NotifyDbContext _context;

    public Repository(NotifyDbContext context)
    {
        _context = context;
    }
    public async Task<T> Create(T item)
    {
        _context.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<T> Update(K key, T item)
    {
        var OldItem = await GetById(key);
        _context.Entry(OldItem).CurrentValues.SetValues(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<T> Delete(K id)
    {
        var item = await GetById(id);
        _context.Remove(item);
        await _context.SaveChangesAsync();
        return item;
    }
    public abstract Task<IEnumerable<T>> GetAll();

    public abstract Task<T> GetById(K id);

}
