﻿using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BLL.Services
{
    public class FeedbackService : IFeedbackService, IDisposable
    {
        private readonly IUnitOfWork DataBase;

        public FeedbackService(IUnitOfWork uow)
        {
            DataBase = uow;
        }


        public async Task<ICollection<FeedbackDTO>> GetAllFeedbacksByUserId(string userId)
        {
            var user = DataBase.ProfileManager.Get(userId);
            if (user == null)
                throw new ValidationException("There is no information about this user", "");
            ICollection<FeedbackDTO> feedbackDTO = null;
            await Task.Run(() =>
            {
                feedbackDTO = Mapper.Map<ICollection<FeedbackDTO>>(DataBase.FeedbackManager.GetAll().Where(p => p.UserId == userId));
            });
            return feedbackDTO;
        }

        public ICollection<FeedbackDTO> GetAllFeedbacksByTourId(int tourId)
        {
            var tour = DataBase.TourManager.Get(tourId);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", "");
            List<FeedbackDTO> feedbackList = new List<FeedbackDTO>();
            ICollection<FeedbackDTO> feedbackDTO = Mapper.Map<ICollection<FeedbackDTO>>(DataBase.FeedbackManager.GetAll().Where(p => p.TourId == tourId));
            foreach(var feedbacks in feedbackDTO)
            {
                feedbackList.Add(feedbacks);
            }
            return feedbackList;
        }

        public FeedbackDTO GetFeedback(int FeedbackId)
        {
            FeedbackDTO feedback = null;
            Feedback feedbackBoof = DataBase.FeedbackManager.Get(FeedbackId);
            if (feedbackBoof != null)
                feedback = Mapper.Map<Feedback, FeedbackDTO>(feedbackBoof);
            return feedback;
        }

        public async Task<OperationDetails> DeleteFeedback(int FeedbackId)
        {
            Feedback feedback = DataBase.FeedbackManager.Get(FeedbackId);
            if (feedback == null)
                throw new ValidationException("There is no information about this feedback", "");
            DataBase.FeedbackManager.Delete(FeedbackId);
            await DataBase.SaveAsync();
            return new OperationDetails(true, "Succes deleted", "feedback");
        }

        public async Task<OperationDetails> AddFeedback(FeedbackDTO feedbackDTO)
        {
            if (feedbackDTO == null)
                throw new ValidationException("There is no information about this feedback", "");
            var feedback = DataBase.FeedbackManager.GetAll().Where(p => p.Id == feedbackDTO.Id).FirstOrDefault();
            if (feedback != null)
                throw new ValidationException("This feedback already exists", "");
            ApplicationUser user = await DataBase.UserManager.FindByIdAsync(feedbackDTO.UserId);
            if (user == null)
                throw new ValidationException("There is no information about this user", "");
            DataBase.FeedbackManager.Create(Mapper.Map<FeedbackDTO, Feedback>(feedbackDTO));
            await DataBase.SaveAsync();
            return new OperationDetails(true, "Feedback added succesfully", "feedback");
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }

    }
}
