using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T?> Get(Guid IdEntity);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(Guid IdEntity);
    }
}
