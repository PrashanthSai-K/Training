using System;
using System.Threading.Tasks;
using CustomerSupport.Context;
using CustomerSupport.Interfaces;

namespace ClinicManagement.Repository;

public abstract class Repository<K, T> : IRepository<K, T> where T : class
{
    protected ChatDbContext _chatDbContext;

    public Repository(ChatDbContext chatDbContext)
    {
        _chatDbContext = chatDbContext;
    }
    public async Task<T> Create(T item)
    {
        _chatDbContext.Add(item);
        await _chatDbContext.SaveChangesAsync();
        return item;
    }

    public async Task<T> Update(K key, T item)
    {
        var OldItem = await GetById(key);
        _chatDbContext.Entry(OldItem).CurrentValues.SetValues(item);
        await _chatDbContext.SaveChangesAsync();
        return item;
    }

    public async Task<T> Delete(K id)
    {
        var item = await GetById(id);
        _chatDbContext.Remove(item);
        await _chatDbContext.SaveChangesAsync();
        return item;
    }
    public abstract Task<IEnumerable<T>> GetAll();

    public abstract Task<T> GetById(K id);

}
