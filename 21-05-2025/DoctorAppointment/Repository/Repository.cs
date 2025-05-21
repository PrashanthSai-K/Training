using System;
using DoctorAppointment.Exceptions;
using DoctorAppointment.Interfaces;

namespace DoctorAppointment.Repository;

public abstract class Repository<K, T> : IRepository<K,T> where T : class
{
    protected List<T> _items = new List<T>();

    protected abstract K GenerateId();

    public abstract T GetById(K id);

    public abstract ICollection<T> GetAll();

    public T Add(T item)
    {
        var id = GenerateId();
        var property = typeof(T).GetProperty("Id");
        if (property != null)
            property.SetValue(item, id);
        if (_items.Contains(item))
            throw new DuplicateEntityException("Duplicate Entity found");
        _items.Add(item);
        return item;
    }

    public T Update(T item)
    {
        var myItem = GetById((K)item.GetType().GetProperty("Id").GetValue(item));
        if(myItem == null)
            throw new KeyNotFoundException("Item not found");
        var index = _items.IndexOf(myItem);
        _items[index] = item;
        return item;
    }

    public T Delete(K id)
    {
        var myItem = GetById(id);
        if (myItem == null)
            throw new KeyNotFoundException("Item not found");
        _items.Remove(myItem);
        return myItem;
    }

}
