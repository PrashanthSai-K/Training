using System;

namespace ClinicManagement.Interfaces;

public interface IRepository<K, T> where T : class
{
    T Create(T item);
    T Update(T item);
    T Delete(K id);
    T GetById(K id);

    ICollection<T> GetAll();
}
