using System.Collections.Generic;
using DAL.Interfaces;
using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    public class TourRepository : IRepository<Tour, int>
    {
        private ApplicationContext _database;

        public TourRepository(ApplicationContext applicationContext)
        {
            _database = applicationContext;
        }
        public void Create(Tour item)
        {
            _database.Tours.Add(item);
        }

        public void Delete(int id)
        {
            Tour tour = _database.Tours.Find(id);
            if (tour != null)
                _database.Tours.Remove(tour);
        }

        public void Delete(Tour item)
        {
            _database.Remove(item);
        }

        public Tour Get(int id)
        {
            return _database.Tours.Find(id);
        }

        public IEnumerable<Tour> GetAll()
        {
            return _database.Tours.ToList();
        }

        public void Update(Tour item)
        {
            _database.Entry(item).State = EntityState.Modified;
        }
    }
}
