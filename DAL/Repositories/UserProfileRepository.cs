using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    class UserProfileRepository : IRepository<UserProfile, string>
    {
        private ApplicationContext _database;

        public UserProfileRepository(ApplicationContext applicationContext)
        {
            _database = applicationContext;
        }
        public void Create(UserProfile item)
        {
            _database.Profiles.Add(item);
        }

        public void Delete(string id)
        {
            UserProfile userProfile = _database.Profiles.Find(id);
            if (userProfile != null)
                _database.Profiles.Remove(userProfile);
        }

        public void Delete(UserProfile item)
        {
            _database.Remove(item);
        }

        public UserProfile Get(string id)
        {
            return _database.Profiles.Find(id);
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return _database.Profiles.ToList();
        }

        public void Update(UserProfile item)
        {
            _database.Entry(item).State = EntityState.Modified;
        }
    }
}
