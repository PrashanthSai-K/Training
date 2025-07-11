using System;

namespace ClinicManagement.Interfaces;

public interface IRepository<K, T> where T : class
{
    Task<T> Create(T item);
    Task<T> Update(K key, T item);
    Task<T> Delete(K id);
    Task<T> GetById(K id);

    Task<IEnumerable<T>> GetAll();
}
