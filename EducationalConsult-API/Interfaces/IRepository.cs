using EducationalConsultAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalConsultAPI.Interfaces {
    public interface IRepository<T> where T : ModelBase {
        IQueryable<T> GetAll();

        T Get(Guid Id);

        T LoadRefrencesTypes(T entity);

        void Add(T item);

        void AddRange(IEnumerable<T> items) => throw new NotImplementedException();

        void Update(T item);

        void Delete(T item);

        void DeleteRandom<TEntity>(Guid key) where TEntity : ModelBase => throw new NotImplementedException();

        void Delete(Guid id);

        bool SaveChanges();
    }
}
