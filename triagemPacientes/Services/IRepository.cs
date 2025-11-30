using System;
using System.Collections.Generic;

namespace triagemPacientes.Services
{
    public interface IRepository<T> where T : class, new()
    {
        List<T> GetAll();
        void Add(T item);
        void SaveAll(List<T> items);
        void Delete(Func<T, bool> predicate);
        void Update(Func<T, bool> predicate, Action<T> updateAction);
        List<T> Buscar(Func<T, bool> filtro);
    }
}
