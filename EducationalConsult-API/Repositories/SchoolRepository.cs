using EducationalConsultAPI.DBContext;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EducationalConsultAPI.Repositories {
    public class SchoolRepository : IRepository<School> {

        public SchoolRepository(EducationalDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void Add(School item) {
            _dbContext.Schools.Add(item);
        }

        public void Delete(School item) {
            _dbContext.Schools.Remove(item);
        }

        public void Delete(Guid id) {
            _dbContext.Remove(Get(id));
        }

        public School Get(Guid Id) {
            var school = _dbContext.Schools.Find(Id);
            _dbContext.Entry(school).Collection(x => x.Groups).Load();
            _dbContext.Entry(school).Collection(x => x.Classes).Load();
            return school;
        }

        public IQueryable<School> GetAll() {
            var set = _dbContext.Schools;
            set.Include(x => x.Classes).Load();
            set.Include(x => x.Groups).Load();
            return set;
        }

        public School LoadRefrencesTypes(School entity) {
            _dbContext.Entry(entity).Collection(x => x.Groups).Load();
            _dbContext.Entry(entity).Collection(x => x.Classes).Load();
            return entity;
        }

        public bool SaveChanges() {
            return _dbContext.SaveChanges() >= 0;
        }

        public void Update(School item) {
            _dbContext.Update(item);
        }

        private readonly EducationalDbContext _dbContext;
    }
}
