using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;
using System.Linq;

namespace DAL.Repositories
{
    public class FeedbackRepository : IRepository<Feedback, int>
    {
        private ApplicationContext _database;
        public FeedbackRepository(ApplicationContext applicationContext)
        {
            _database = applicationContext;
        }
        public void Create(Feedback item)
        {
            _database.FeedBacks.Add(item);
        }

        public void Delete(int key)
        {
            Feedback feedback = _database.FeedBacks.Find(key);
            if (feedback != null)
                _database.FeedBacks.Remove(feedback);
        }

        public Feedback Get(int id)
        {
            return _database.FeedBacks.Find(id);
        }

        public IEnumerable<Feedback> GetAll()
        {
            return _database.FeedBacks.ToList();
        }

        public void Update(Feedback item)
        {
            _database.Entry(item).State = EntityState.Modified;
        }
    }
}
