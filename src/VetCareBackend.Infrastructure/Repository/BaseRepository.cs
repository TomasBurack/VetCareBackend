using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly VetCareDbContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(VetCareDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual List<T> GetAll()
        {
            return _dbSet.Where(a => !a.IsDeleted).ToList();
        }

        public virtual T? Get(Guid IdEntity)
        {
            return _dbSet.FirstOrDefault(a => a.Id == IdEntity && !a.IsDeleted);
        }
        public virtual T Add(T entity)
        {
            entity.UpdateDate = DateTime.UtcNow;
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public virtual void Update(T entity)
        {
            entity.UpdateDate = DateTime.UtcNow;
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
        public virtual void Delete(Guid IdEntity) 
        {
            var entity = Get(IdEntity);
            if (entity != null)
            {
                entity.DeleteDate = DateTime.UtcNow;
                entity.IsDeleted = true;

                _dbSet.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}
