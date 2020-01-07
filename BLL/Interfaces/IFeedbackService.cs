using BLL.DTO;
using BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BLL.Interfaces
{
    public interface IFeedbackService
    {
        /// <summary>
        /// Get all feedbacks by user id. 
        /// </summary>
        /// <param name="userId">Id of user associated with feedbacks.</param>
        /// <returns>Return list of feedbacks.</returns>
        Task<ICollection<FeedbackDTO>> GetAllFeedbacksByUserId(string userId);
        /// <summary>
        /// Get all feedbacks by tour id.
        /// </summary>
        /// <param name="tourId"> Id of tour associated with feedbacks.</param>
        /// <returns> Return list of feedbacks.</returns>
        ICollection<FeedbackDTO> GetAllFeedbacksByTourId(int tourId);
        /// <summary>
        /// Get feedback by id.
        /// </summary>
        /// <param name="FeedbackId"> Id of feedback which will be got.</param>
        /// <returns> Return FeedbackDTO.</returns>
        FeedbackDTO GetFeedback(int FeedbackId);
        /// <summary>
        /// Delete feedback by id.
        /// </summary>
        /// <param name="FeedbackId"> Id of feedback which will be deleted.</param>
        Task<OperationDetails> DeleteFeedback(int FeedbackId);
        /// <summary>
        /// Create new feedback.
        /// </summary>
        /// <param name="feedbackDTO"> Feedback wich wil be added.</param>
        Task<OperationDetails> AddFeedback(FeedbackDTO feedbackDTO);

    }
}
