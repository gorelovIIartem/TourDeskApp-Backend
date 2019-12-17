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
        public async Task<ICollection<TourDTO>> GetToursUserVisited(string userId)
        {
            ICollection<TourDTO> tourDTO = null;
            await Task.Run(() =>
            {
                var user = DataBase.ProfileManager.Get(userId);
                if (user == null)
                    throw new ValidationException("User is not found", "Id");
                tourDTO = Mapper.Map<ICollection<TourDTO>>(DataBase.TourManager.GetAll().Where(p => p.UserId == userId));
            });
            return tourDTO;
        }

        public async Task<OperationDetails> AddTour(TourDTO tourDTO)
        {
            if (tourDTO == null)
                throw new ValidationException("There is no information about this tour", "Id");
            var tour = DataBase.TourManager.GetAll().Where(p => p.Name == tourDTO.Name || p.Id == tourDTO.Id).FirstOrDefault();
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

        public async Task<OperationDetails> UploadImage(int tourId, string imageUrl)
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
    }
}
