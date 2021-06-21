using EducationalConsultAPI.DBContext;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EducationalConsultAPI.Repositories {
    public class UserRepository : IRepository<User>{

        public UserRepository(EducationalDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void Add(User item) {
            _dbContext.Users.Add(item);
        }

        public void Delete(User item) {
            _dbContext.Users.Remove(item);
        }

        public void Delete(Guid id) {
            var userToDelete = Get(id);
            Delete(userToDelete);
        }

        public User Get(Guid Id) {
            var user = _dbContext.Users.Find(Id);
            if (user != null) {
                _dbContext.Entry(user).Collection(x => x.Classes).Load();
                if (user.Classes is null) return user;
                foreach (var clss in user.Classes) {
                    _dbContext.Entry(clss).Collection(x => x.Resources).Load();
                    _dbContext.Entry(clss).Collection(x => x.Students).Load();
                    _dbContext.Entry(clss).Reference(x => x.School).Load();
                    _dbContext.Entry(clss).Reference(x => x.User).Load();
                }
            }
            return user;
        }

        public IQueryable<User> GetAll() {
            var set = _dbContext.Users;
            set.Include(x => x.Classes).Load();
            return set;
        }

        public User LoadRefrencesTypes(User entity) {
            _dbContext.Entry(entity).Collection(x => x.Classes).Load();
            return entity;
        }

        public bool SaveChanges() {
            return _dbContext.SaveChanges() >= 0;
        }

        public void Update(User item) {
            _dbContext.Users.Update(item);
        }

        private readonly EducationalDbContext _dbContext;
    }
}
