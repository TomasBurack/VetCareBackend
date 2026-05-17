using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        T? Get(Guid IdEntity);
        T Add (T entity);
        void Update (T entity);

        void Delete (Guid IdEntity);
    }
}
