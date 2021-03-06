﻿using System;
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
    public class UserService : IUserService, IDisposable
    {
        public IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> CreateUserAsync(UserDTO userDTO)
        {

            await Database.RoleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole("admin"));
            await Database.SaveAsync();

            if (userDTO == null) throw new ValidationException("UserDTO is null", "");
            ApplicationUser user = await Database.UserManager.FindByNameAsync(userDTO.UserName);
            if (user != null) throw new ValidationException("UserWith this login already exists", userDTO.UserName);
            user = new ApplicationUser { UserName = userDTO.UserName };
            var result = await Database.UserManager.CreateAsync(user, userDTO.Password);
            if (result.Errors.Count() > 0) return new OperationDetails(false, string.Join(",", result.Errors.Select(p => p.Description)), null);
                    await Database.UserManager.AddToRoleAsync(user, userDTO.Role);

            User Quser = new User()
            {
                ApplicationUser = user,
                Login = user.UserName,
                Password = userDTO.Password,
                UserId = user.Id
            };
            UserProfile profile = new UserProfile()
            {
                UserId = Quser.UserId,
                User = Quser,
                FullName = userDTO.FullName
            };
            Database.ProfileManager.Create(profile);
            Database.QUserManager.Create(Quser);
            await Database.SaveAsync();
            return new OperationDetails(true, "Successful registration!", "user");
        }

        public UserDTO FindUserByUserName(string userName)
        {
            User qUser = Database.QUserManager.GetAll().Where(p => p.ApplicationUser.UserName == userName).FirstOrDefault();
            ApplicationUser user = qUser.ApplicationUser;
            if (user == null) throw new ValidationException("User is not found", userName);
            UserProfile userProfile = user.User.UserProfile;
            return new UserDTO()
            {
                FullName = userProfile.FullName,
                Age = userProfile.Age,
                Birthday = userProfile.Birthday,
                Phone = userProfile.Phone,
                Email = userProfile.Email,
                Address = userProfile.Address
            };
        
        }
        public async Task<IList<string>> GetRolesByUserId(string id)
        {
            ApplicationUser user = await Database.UserManager.FindByIdAsync(id);
            return await Database.UserManager.GetRolesAsync(user);
        }

        public async Task<OperationDetails> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("Incorect input data", userId);
            var user = await Database.UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new ValidationException("User is not found", userId);
            Database.QUserManager.Delete(userId);
            IEnumerable<Feedback> feedbacks = Database.FeedbackManager.GetAll().Where(p => p.UserId == userId);
            foreach (var feedback in feedbacks)
            {
                Database.FeedbackManager.Delete(feedback);
            }
            IEnumerable<Tour> tours = Database.TourManager.GetAll().Where(p => p.UserId == userId);
            foreach (var tour in tours)
            {
                tour.UserId = null;
            }
            IEnumerable<Ticket> tickets = Database.TicketManager.GetAll().Where(p => p.UserId == userId);
            foreach(var ticket in tickets)
            {
                Tour tour = Database.TourManager.Get(ticket.TourId);
                Database.TicketManager.Delete(ticket);
                tour.PlacesCount++;
            }
            await Database.UserManager.DeleteAsync(user);
            await Database.SaveAsync();
            return new OperationDetails(true, "Successfully deleted", userId);
        }

        public async Task<UserDTO> FindUserByIdAsync(string userId)
        {
            ApplicationUser user = await Database.UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new ValidationException("User is not found", userId);
            UserProfile profile = user.User.UserProfile;
            return new UserDTO()
            {
                FullName = profile.FullName,
                Address = profile.Address,
                Email = profile.Email,
                Phone = profile.Phone,
                Birthday = profile.Birthday,
                Age = profile.Age,
                UserName = user.UserName,
                Password = profile.User.Password,
                Id = user.Id,
                ImageUrl = profile.ImageUrl,
                Role = (await Database.UserManager.GetRolesAsync(user)).FirstOrDefault()
            };
        }
       

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task<OperationDetails> ChangeProfileInformation(UserDTO userDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByIdAsync(userDTO.Id);
            if (user == null)
                throw new ValidationException("User is not found", userDTO.Id);
            UserProfile profile = user.User.UserProfile;
            profile.FullName = userDTO.FullName;
            profile.Phone = userDTO.Phone;
            profile.Age = userDTO.Age;
            profile.Email = userDTO.Email;
            profile.Birthday = userDTO.Birthday;
            profile.Address = userDTO.Address;
            profile.ImageUrl = userDTO.ImageUrl;
            Database.ProfileManager.Update(profile);
            await Database.SaveAsync();
            return new OperationDetails(true, "Information updated succesfully", "UserProfile");

        }

        public async Task<UserDTO> FindUserAsync(string userName, string password)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(userName);
            if (user?.User.Password.Trim() == password)
            {
                UserProfile profile = user.User.UserProfile;
                return new UserDTO()
                {
                    FullName = profile.FullName,
                    Password = password,
                    Email = profile.Email,
                    Id = user.Id,
                    
                    Phone = profile.Phone,
                   
                    UserName = userName
                };
            }
            else return null;
        }

        public async Task<OperationDetails> UploadImage(string photoUrl, string userId)
        {
            ApplicationUser user = await Database.UserManager.FindByIdAsync(userId);
            if (user == null) throw new ValidationException("User does not exist", userId);
            UserProfile userProfile = user.User.UserProfile;
            userProfile.ImageUrl = photoUrl;
            Database.ProfileManager.Update(userProfile);
            await Database.SaveAsync();
            return new OperationDetails(true, photoUrl, "profile");
        }

        public async  Task<UserDTO> GetGuideByTourId(int tourId)
        {
            TourDTO tourDTO = Mapper.Map<TourDTO>(Database.TourManager.Get(tourId));
            if (tourDTO == null)
                throw new ValidationException("There is no information about this tour", $"{tourId}");
            ApplicationUser user = await Database.UserManager.FindByIdAsync(tourDTO.UserId);
            if (user == null)
                throw new ValidationException($"There is no information about this user {user.Id} in tour", "");
            UserProfile profile = user.User.UserProfile;
            return new UserDTO
            {
                FullName = profile.FullName,
                Age = profile.Age,
                Birthday = profile.Birthday,
                Email = profile.Email,
                Address = profile.Address,
                Phone = profile.Phone,
                ImageUrl = profile.ImageUrl,
                Id = user.Id,
                UserName = user.UserName
            };
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            IEnumerable<User> boofUsers =  Database.QUserManager.GetAll();
            List<UserDTO> users = new List<UserDTO>();
            foreach(var user in boofUsers)
            {
                ApplicationUser applicationUser = await Database.UserManager.FindByIdAsync(user.ApplicationUser.Id);
                UserProfile userProfile = applicationUser.User.UserProfile;
                users.Add(new UserDTO
                {
                    Id = applicationUser.Id,
                    FullName = userProfile.FullName,
                    Age = userProfile.Age,
                    Email = userProfile.Email,
                    Address = userProfile.Address,
                    Phone = userProfile.Phone,
                    ImageUrl = userProfile.ImageUrl,
                    UserName = applicationUser.UserName,
                    Birthday = userProfile.Birthday
                });
            }
            return users;
        }

    }

}
