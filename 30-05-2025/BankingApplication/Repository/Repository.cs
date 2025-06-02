using System;
using System.Threading.Tasks;
using BankingApplication.Context;
using BankingApplication.Interfaces;

namespace BankingApplication.Repository;

public abstract class Repository<K, T> : IRepository<K, T> where T : class
{
    protected readonly BankDbContext _bankDbContext;

    public Repository(BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
    }
    public async Task<T> Create(T item)
    {
        _bankDbContext.Add(item);
        await _bankDbContext.SaveChangesAsync();
        return item;
    }

    public async Task<T> Update(K key, T item)
    {
        var OldItem = await GetById(key);
        _bankDbContext.Entry(OldItem).CurrentValues.SetValues(item);
        await _bankDbContext.SaveChangesAsync();
        return item;
    }

    public async Task<T> Delete(K id)
    {
        var item = await GetById(id);
        _bankDbContext.Remove(item);
        await _bankDbContext.SaveChangesAsync();
        return item;
    }
    public abstract Task<IEnumerable<T>> GetAll();

    public abstract Task<T> GetById(K id);
}
