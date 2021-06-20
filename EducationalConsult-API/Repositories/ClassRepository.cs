using EducationalConsultAPI.DBContext;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EducationalConsultAPI.Repositories {
    public class ClassRepository : IRepository<Class> {

        public ClassRepository(EducationalDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void Add(Class item) {
            _dbContext.Classes.Add(item);
        }

        public void Delete(Class item) {
            _dbContext.Classes.Remove(item);
        }

        public void Delete(Guid id) {
            Delete(Get(id));
        }

        public Class Get(Guid Id) {
            var item = _dbContext.Classes.Find(Id);
            _dbContext.Entry(item).Reference(x => x.User).Load();
            _dbContext.Entry(item).Reference(x => x.School).Load();
            _dbContext.Entry(item).Collection(x => x.Resources).Load();
            _dbContext.Entry(item).Collection(x => x.Students).Load();
            return item;
        }

        public IQueryable<Class> GetAll() {
            var set = _dbContext.Classes;
            set.Include(x => x.Resources).Load();
            set.Include(x => x.Students).Load();
            set.Include(x => x.User).Load();
            set.Include(x => x.School).Load();
            return set;
        }

        public Class LoadRefrencesTypes(Class entity) {
            _dbContext.Entry(entity).Reference(x => x.User).Load();
            _dbContext.Entry(entity).Reference(x => x.School).Load();
            _dbContext.Entry(entity).Collection(x => x.Resources).Load();
            _dbContext.Entry(entity).Collection(x => x.Students).Load();
            return entity;
        }

        public bool SaveChanges() {
            return _dbContext.SaveChanges() >= 0;
        }

        public void Update(Class item) {
            _dbContext.Classes.Update(item);
        }

        private readonly EducationalDbContext _dbContext;
    }
}
