using System.Collections.Generic;
using BLL.DTO;
using BLL.Infrastructure;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITourService
    {
        Task<ICollection<TourDTO>> GetToursUserVisited(string userId);
        Task<OperationDetails> AddTour(TourDTO tour);
        Task<OperationDetails> DeleteTour(int tourId);
        Task<OperationDetails> MakeSale(string userId, int tourId);
        Task<OperationDetails> UploadImage(int tourId, string imageUrl);
        Task<OperationDetails> ChangeTourInformation(TourDTO tour);

        TourDTO GetTour(int tourId);
    }
}
