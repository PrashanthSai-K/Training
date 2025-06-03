using System;
using System.Threading.Tasks;
using ClinicManagement.Context;
using ClinicManagement.Interfaces;

namespace ClinicManagement.Repository;

public abstract class Repository<K, T> : IRepository<K, T> where T : class
{
    protected ClinicDBContext _clinicDBContext;

    public Repository(ClinicDBContext clinicDBContext)
    {
        _clinicDBContext = clinicDBContext;
    }
    public async Task<T> Create(T item)
    {
        _clinicDBContext.Add(item);
        await _clinicDBContext.SaveChangesAsync();
        return item;
    }

    public async Task<T> Update(K key, T item)
    {
        var OldItem = await GetById(key);
        _clinicDBContext.Entry(OldItem).CurrentValues.SetValues(item);
        await _clinicDBContext.SaveChangesAsync();
        return item;
    }

    public async Task<T> Delete(K id)
    {
        var item = await GetById(id);
        _clinicDBContext.Remove(item);
        await _clinicDBContext.SaveChangesAsync();
        return item;
    }
    public abstract Task<IEnumerable<T>> GetAll();

    public abstract Task<T> GetById(K id);

}
