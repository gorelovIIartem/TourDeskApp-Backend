using System.Collections.Generic;
using DAL.Interfaces;
using System.Linq;
using DAL.Entities;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository : IRepository<User, string>
    {
        private ApplicationContext _database;

        public UserRepository(ApplicationContext applicationContext)
        {
            _database = applicationContext;
        }
        public void Create(User item)
        {
            _database.QUsers.Add(item);
        }

        public void Delete(string id)
        {
            User user = _database.QUsers.Find(id);
            if (user != null)
                _database.QUsers.Remove(user);
        }

        public void Delete(User item)
        {
            throw new System.NotImplementedException();
        }

        public User Get(string id)
        {
            return _database.QUsers.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _database.QUsers.ToList();
        }

        public void Update(User item)
        {
            _database.Entry(item).State = EntityState.Modified;

        }
    }
}
