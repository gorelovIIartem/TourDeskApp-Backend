using System;
using DAL.Identity;
using DAL.Entities;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveAsync();
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IRepository<Ticket, int> TicketManager { get; }
        IRepository<Tour, int> TourManager { get; }
        IRepository<Feedback, int> FeedbackManager { get; }
        IRepository<User, string> QUserManager { get; }
        IRepository<UserProfile, string> ProfileManager { get; }
    }
}
