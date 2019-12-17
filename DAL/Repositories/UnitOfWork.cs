using System;
using DAL.EF;
using DAL.Entities;
using DAL.Identity;
using DAL.Interfaces;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationContext _database;
        private FeedbackRepository _feedbackManager;
        private UserRepository _qUserManager;
        private TourRepository _tourManager;
        private TicketRepository _ticketManager;
        private UserProfileRepository _profileManager;

        public ApplicationUserManager UserManager { get; }

        public ApplicationRoleManager RoleManager { get; }

        public UnitOfWork(ApplicationContext context, ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            _database = context;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public IRepository<Tour, int> TourManager
        {
            get
            {
                if (_tourManager == null)
                    _tourManager = new TourRepository(_database);
                return _tourManager;
            }
        }

        public IRepository<UserProfile, string> ProfileManager
        {
            get
            {
                if (_profileManager == null)
                    _profileManager = new UserProfileRepository(_database);
                return _profileManager;
            }
        }

        public IRepository<User, string> QUserManager
        {
            get
            {
                if (_qUserManager == null)
                    _qUserManager = new UserRepository(_database);
                return _qUserManager;
            }
        }

        public IRepository<Ticket, int> TicketManager
        {
            get
            {
                if (_ticketManager == null)
                    _ticketManager = new TicketRepository(_database);
                return _ticketManager;
            }
        }
        public IRepository<Feedback, int> FeedbackManager
        {
            get
            {
                if (_feedbackManager == null)
                    _feedbackManager = new FeedbackRepository(_database);
                return _feedbackManager;
            }
        }

        public async Task SaveAsync()
        {
                await _database.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _database.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
