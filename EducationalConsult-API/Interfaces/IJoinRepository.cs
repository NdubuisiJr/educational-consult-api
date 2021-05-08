using EducationalConsultAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalConsultAPI.Interfaces {
    public interface IJoinRepository<T> {
        T Get(Guid userId, Guid groupId);
        IQueryable<T> GetAll();
        IEnumerable<ModelBase> GetUsers(Guid groupId);
        IEnumerable<ModelBase> GetGroups(Guid userId);
        void Add(T item);
        void Delete(T item);
        void Update(T item);
        bool SaveChanges();
    }
}
