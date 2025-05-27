using System;
using ClinicManagement.Interfaces;

namespace ClinicManagement.Repository;

public abstract class Repository<K, T> : IRepository<K, T> where T : class
{
    protected List<T> _items = new List<T>();
    public T Create(T item)
    {
        item.GetType().GetProperty("Id")?.SetValue(item, GenerateId());
        _items.Add(item);
         return item;
    }

    public T Update(T item)
    {
        var oldItem = GetById((K)item.GetType().GetProperty("Id").GetValue(item));
        var index = _items.IndexOf(oldItem);
        _items[index] = item;
        return item;
    }

    public T Delete(K id)
    {
        var item = GetById(id);
        _items.Remove(item);
        return item;
    }

    public abstract K GenerateId();
    public abstract ICollection<T> GetAll();

    public abstract T GetById(K id);

}
