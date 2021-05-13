using EducationalConsultAPI.DBContext;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalConsultAPI.Repositories {
    public class UserGroupRepository : IJoinRepository<UserGroup> {
        public UserGroupRepository(EducationalDbContext dbContext) {
            _dbContext = dbContext;
        }

        public UserGroup Get(Guid userId, Guid groupId) {
            var userGroup = _dbContext.UserGroups
                    .SingleOrDefault(x => x.UserId == userId && x.GroupId == groupId);
            _dbContext.Entry(userGroup).Reference(x => x.User).Load();
            _dbContext.Entry(userGroup).Reference(x => x.Group).Load();
            return userGroup;
        }

        public IQueryable<UserGroup> GetAll() {
            var set = _dbContext.Set<UserGroup>();
            set.Include(x => x.User).ThenInclude(x=>x.Classes).Load();
            set.Include(x => x.Group).ThenInclude(x=>x.School).ThenInclude(x=>x.Classes).Load();
            set.Include(x => x.Group).ThenInclude(x => x.School).ThenInclude(x => x.Groups).ThenInclude(x=>x.InvitedUsers).Load();
            return set;
        }

        public IEnumerable<ModelBase> GetUsers(Guid groupId) {
            return GetAll().Where(x => x.GroupId == groupId)
                           .Select(x => x.User);
        }

        public IEnumerable<ModelBase> GetGroups(Guid userId) {
            return GetAll().Where(x => x.UserId == userId)
                           .Select(x => x.Group);
        }

        public void Add(UserGroup item) {
            _dbContext.UserGroups.Add(item);
        }

        public void Delete(UserGroup item) {
            _dbContext.UserGroups.Remove(item);
        }

        public void Update(UserGroup item) {
            _dbContext.UserGroups.Update(item);
        }

        public bool SaveChanges() {
            return _dbContext.SaveChanges() >= 0;
        }

        private EducationalDbContext _dbContext;
    }
}
