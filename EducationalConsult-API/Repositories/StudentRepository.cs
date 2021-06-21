using EducationalConsultAPI.DBContext;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using System;
using System.Linq;

namespace EducationalConsultAPI.Repositories {
    public class StudentRepository : IRepository<Student> {

        public StudentRepository(EducationalDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void Add(Student item) {
            _dbContext.Students.Add(item);
        }

        public void Delete(Student item) {
            _dbContext.Students.Remove(item);
        }

        public void Delete(Guid id) {
            _dbContext.Students.Remove(Get(id));
        }

        public Student Get(Guid Id) {
            var student = _dbContext.Students.Find(Id);
            _dbContext.Entry(student).Collection(x => x.DailyReports).Load();
            _dbContext.Entry(student).Reference(x => x.Class).Load();
            return student;
        }

        public IQueryable<Student> GetAll() {
            return _dbContext.Students;
        }

        public Student LoadRefrencesTypes(Student entity) {
            _dbContext.Entry(entity).Collection(x => x.DailyReports).Load();
            _dbContext.Entry(entity).Reference(x => x.Class).Load();
            return entity;
        }

        public bool SaveChanges() {
            return _dbContext.SaveChanges() >= 0;
        }

        public void Update(Student item) {
            _dbContext.Students.Update(item);
        }

        private readonly EducationalDbContext _dbContext;
    }
}
