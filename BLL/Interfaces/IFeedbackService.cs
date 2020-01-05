using System;
using System.Collections.Generic;
using BLL.DTO;
using BLL.Infrastructure;
using System.Threading.Tasks;
namespace BLL.Interfaces
{
    public interface IFeedbackService
    {
        Task<ICollection<FeedbackDTO>> GetAllFeedbacksByUserId(string userId);
        Task<ICollection<FeedbackDTO>> GetAllFeedbacksSortedByDate(DateTime date);
        Task<ICollection<FeedbackDTO>> GetAllFeedbacksByTourId(int tourId);
        FeedbackDTO GetFeedback(int FeedbackId);
        Task<OperationDetails> DeleteFeedback(int FeedbackId);
        Task<OperationDetails> AddFeedback(FeedbackDTO feedbackDTO);

    }
}
