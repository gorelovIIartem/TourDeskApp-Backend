using System.Collections.Generic;
using BLL.DTO;
using BLL.Infrastructure;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITourService
    {
        /// <summary>
        /// Get all tours by user id.
        /// </summary>
        /// <param name="userId"> Id of user associated with the tour</param>
        /// <returns> Return collection with tours.</returns>
        IEnumerable<TourDTO> GetToursUserVisited(string userId);
        /// <summary>
        /// Add new tour.
        /// </summary>
        /// <param name="tour"> Tour witch will be added.</param>
        Task<OperationDetails> AddTour(TourDTO tour);
        /// <summary>
        /// Delete tour by tour id.
        /// </summary>
        /// <param name="tourId"> Id of tour witch will be deleted.</param>
        Task<OperationDetails> DeleteTour(int tourId);
        /// <summary>
        /// Upload a photo in tour by tour id.
        /// </summary>
        /// <param name="ImageUrl"> Url of image with will be added.</param>
        /// <param name="tourId"> Id of tour, image will be added to.</param>
        Task<OperationDetails> UploadImage(string ImageUrl, int tourId);
        /// <summary>
        /// Update information of tour.
        /// </summary>
        /// <param name="tour"> Tour, witch will be updated.</param>
        Task<OperationDetails> ChangeTourInformation(TourDTO tour);
        /// <summary>
        /// Get all tours from DB.
        /// </summary>
        /// <returns> Return collection of tours.</returns>
        IEnumerable<TourDTO> GetAllTours();
        /// <summary>
        /// Get tour by id.
        /// </summary>
        /// <param name="tourId"> Id of tour witch will be got.</param>
        /// <returns></returns>
        TourDTO GetTour(int tourId);
        /// <summary>
        /// Make user with admin root guide for tour.
        /// </summary>
        /// <param name="tourId"> Id of touur, guide will be added to.</param>
        /// <param name="userId">Id of user who will be added to tour like guide.</param>
        Task<OperationDetails> MakeGuide(int tourId, string userId);
        /// <summary>
        /// Get all tours by guide id.
        /// </summary>
        /// <param name="userId"> Id of guide, who will get all tours with this guide id.</param>
        /// <returns>Return collection of tours.</returns>
        Task<IEnumerable<TourDTO>> GetGuideTours(string userId);
    }
}
