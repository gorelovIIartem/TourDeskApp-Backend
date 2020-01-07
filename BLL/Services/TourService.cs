using System;
using System.Collections.Generic;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;

namespace BLL.Services
{
    public class TourService : ITourService, IDisposable
    {
        private readonly IUnitOfWork DataBase;

        public TourService(IUnitOfWork uow)
        {
            DataBase = uow;
        }
        public IEnumerable<TourDTO> GetToursUserVisited(string userId)
        {
            ICollection<TicketDTO> tickets = null;
                var user =  DataBase.ProfileManager.Get(userId);
            if (user == null)
                    throw new ValidationException("User is not found", "Id");
                tickets = Mapper.Map<ICollection<TicketDTO>>(DataBase.TicketManager.GetAll().Where(p=>p.UserId==userId));
            ICollection<DTO.TourDTO> tours = Mapper.Map<ICollection<DTO.TourDTO>>(DataBase.TourManager.GetAll());
            List<DTO.TourDTO> visitedtours = new List<DTO.TourDTO>();
            foreach(var ticket in tickets)
            {
                foreach(var tour in tours)
                {
                    if(ticket.TourId == tour.Id)
                    {
                        visitedtours.Add(tour);
                    }
                }
            }
            
            return visitedtours ;
        }

        public async Task<OperationDetails> AddTour(TourDTO tourDTO)
        {
            if (tourDTO == null)
                throw new ValidationException("There is no information about this tour", "Id");
            var tour = DataBase.TourManager.GetAll().Where(p => p.Name == tourDTO.Name).FirstOrDefault();
            if (tour != null)
                throw new ValidationException("This tour already exists", "Name");
            DataBase.TourManager.Create(Mapper.Map<TourDTO, Tour>(tourDTO));
            await DataBase.SaveAsync();
            return new OperationDetails(true, "Tour added successfully", "tour");

        }

        public async Task<OperationDetails> DeleteTour(int tourId)
        {
            Tour tour = DataBase.TourManager.Get(tourId);
            if (tour == null)
                throw new ValidationException("Tour with this id does not exist. Check it out Id", "Id");
            IEnumerable<Feedback> feedbacks = DataBase.FeedbackManager.GetAll().Where(p => p.TourId == tourId);
            foreach(var feedback in feedbacks)
            {
                DataBase.FeedbackManager.Delete(feedback);
            }
            DataBase.TourManager.Delete(tourId);
            await DataBase.SaveAsync();
            return new OperationDetails(true, "This tour was deleted succesfully", "tour");
        }

        public async Task<OperationDetails> MakeSale(string userId, int tourId)
        {
            var tour = DataBase.TourManager.Get(tourId);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", "");
            var user =DataBase.ProfileManager.Get(userId);
            if (user == null)
                throw new ValidationException("There is no information about user", "");
            if (tour.Date == user.Birthday)
                tour.Price -= tour.Price * 0.3;
            await DataBase.SaveAsync();
            return new OperationDetails(true, "Sale made succesfully", "tour");
        }
        public void Dispose()
        {
            DataBase.Dispose();
        }

        public TourDTO GetTour(int tourId)
        {
            var tour = DataBase.TourManager.Get(tourId);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", "tour");
            return Mapper.Map<Tour, TourDTO>(tour);
        }

        public async Task<OperationDetails> UploadImage(string imageUrl, int tourId)
        {
            Tour tour = DataBase.TourManager.Get(tourId);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", "");
            tour.ImageUrl = imageUrl;
            DataBase.TourManager.Update(tour);
            await DataBase.SaveAsync();
            return new OperationDetails(true, imageUrl, "tour");
        }

        public async Task<OperationDetails> ChangeTourInformation(TourDTO tourDTO)
        {
            Tour tour = DataBase.TourManager.Get(tourDTO.Id);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", "");
            tour.City = tourDTO.City;
            tour.Name = tourDTO.Name;
            tour.Location = tourDTO.Location;
            tour.Price = tourDTO.Price;
            DataBase.TourManager.Update(tour);
            await DataBase.SaveAsync();
            return new OperationDetails(true, "Tour successfully changed", "Tour");
        }

        public IEnumerable<TourDTO> GetAllTours()
        {
           IEnumerable<TourDTO> allTours = Mapper.Map<IEnumerable<TourDTO>>(DataBase.TourManager.GetAll());
            return allTours;
        }

        public async Task<OperationDetails> MakeGuide(int tourId, string userId)
        {
            Tour tour = DataBase.TourManager.Get(tourId);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", $"{tourId}");
            UserDTO userDTO = Mapper.Map<UserDTO>(DataBase.QUserManager.Get(userId));
            if (userDTO == null)
                throw new ValidationException("There is no information about this user", userId);
            tour.UserId = userId;
            DataBase.TourManager.Update(tour);
            await DataBase.SaveAsync();
            return new OperationDetails(true, "Guide for this tour updated", "Tour");
        }

        public async Task<IEnumerable<TourDTO>> GetGuideTours(string userId)
        {
            ApplicationUser user = await DataBase.UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new ValidationException("There is no information about this user", "Incorrect id");
            List<TourDTO> tours = new List<TourDTO>();
            IEnumerable<TourDTO> tourDTOs = Mapper.Map<IEnumerable<DTO.TourDTO>>(DataBase.TourManager.GetAll().Where(p => p.UserId == userId));
            foreach(var toursIE in tourDTOs)
            {
                tours.Add(toursIE);
            }
            return tours;
        }
    }
}
