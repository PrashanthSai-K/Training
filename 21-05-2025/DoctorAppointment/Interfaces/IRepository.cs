using System;

namespace DoctorAppointment.Interfaces;

public interface IRepository<K, T> where T : class
{
    T Add(T item);
    T Delete(K id);
    T GetById(K id);
    ICollection<T> GetAll();
}
