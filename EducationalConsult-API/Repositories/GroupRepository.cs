using EducationalConsultAPI.DBContext;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalConsultAPI.Repositories {
    public class GroupRepository : IRepository<Group> {

        public GroupRepository(EducationalDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void Add(Group item) {
            _dbContext.Groups.Add(item);
        }

        public void AddRange(IEnumerable<Group> items) {
            foreach (var group in items)
                Add(group);
        }

        public void Delete(Group item) {
            _dbContext.Groups.Remove(item);
        }

        public void Delete(Guid id) {
            _dbContext.Groups.Remove(Get(id));
        }

        public void DeleteRandom<TEnity>(Guid key) where TEnity : ModelBase {
            var item = _dbContext.Find<TEnity>(key);
            if (item is { })
                _dbContext.Remove(item);
        }

        public Group Get(Guid Id) {
            var group = _dbContext.Groups.Find(Id);
            _dbContext.Entry(group).Reference(x => x.School).Load();
            _dbContext.Entry(group).Collection(x => x.InvitedUsers).Load();
            return group;
        }

        public IQueryable<Group> GetAll() {
            var set = _dbContext.Groups;
            set.Include(x => x.School).Load();
            return set;
        }

        public Group LoadRefrencesTypes(Group entity) {
            _dbContext.Entry(entity).Reference(x => x.School).Load();
            _dbContext.Entry(entity).Collection(x => x.InvitedUsers).Load();
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
