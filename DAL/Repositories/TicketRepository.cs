using System.Collections.Generic;
using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    public class TicketRepository : IRepository<Ticket, int>
    {
        private ApplicationContext _database;
        public TicketRepository(ApplicationContext applicationContext)
        {
            _database = applicationContext;
        }
        public void Create(Ticket item)
        {
            _database.Tickets.Add(item);
        }

        public void Delete(int id)
        {
            Ticket ticket = _database.Tickets.Find(id);
            if (ticket != null)
                _database.Tickets.Remove(ticket);
        }
        public void Delete(Ticket item)
        {
            _database.Remove(item);
        }

        public Ticket Get(int id)
        {
            return _database.Tickets.Find(id);
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _database.Tickets.ToList();
        }

        public void Update(Ticket item)
        {
            _database.Entry(item).State = EntityState.Modified;
        }
    }
}
