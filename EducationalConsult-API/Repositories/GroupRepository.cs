using EducationalConsultAPI.DBContext;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EducationalConsultAPI.Repositories {
    public class GroupRepository : IRepository<Group> {

        public GroupRepository(EducationalDbContext dbContext) {
            _dbContext = dbContext;
        }
        public void Add(Group item) {
            _dbContext.Groups.Add(item);
        }

        public void Delete(Group item) {
            _dbContext.Groups.Remove(item);
        }

        public void Delete(Guid id) {
            _dbContext.Groups.Remove(Get(id));
        }

        public Group Get(Guid Id) {
            var group = _dbContext.Groups.Find(Id);
            _dbContext.Entry(group).Reference(x => x.School).Load();
            return group;
        }

        public IQueryable<Group> GetAll() {
            var set = _dbContext.Groups;
            set.Include(x => x.School).Load();
            return set;
        }

        public Group LoadRefrencesTypes(Group entity) {
            _dbContext.Entry(entity).Reference(x => x.School).Load();
            return entity;
        }

        public bool SaveChanges() {
            return _dbContext.SaveChanges() >= 0;
        }

        public void Update(Group item) {
            _dbContext.Update(item);
        }

        private EducationalDbContext _dbContext;
    }
}
