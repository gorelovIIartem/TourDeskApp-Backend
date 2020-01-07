using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Infrastructure;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="userDto"> User witch will be added.</param>
        Task<OperationDetails> CreateUserAsync(UserDTO userDto);
        /// <summary>
        /// Find user by user name.
        /// </summary>
        /// <param name="userName"> User witch will be found by this user name.</param>
        /// <returns>Return user.</returns>
        UserDTO FindUserByUserName(string userName);
        /// <summary>
        /// Find user by user name and password.
        /// </summary>
        /// <param name="userName"> User witch will be found by this user name.</param>
        /// <param name="password">User witch will be found by this user name.</param>
        /// <returns>Return user.</returns>
        Task<UserDTO> FindUserAsync(string userName, string password);
        /// <summary>
        /// Delete user by user Id.
        /// </summary>
        /// <param name="userId">Id of user witch will be deleted.</param>
        Task<OperationDetails> DeleteUser(string userId);
        /// <summary>
        /// Get roles of user by user id.
        /// </summary>
        /// <param name="userId">Id of user, roles will be got by. </param>
        /// <returns>List of roles.</returns>
        Task<IList<string>> GetRolesByUserId(string userId);
        /// <summary>
        /// Get user by user Id.
        /// </summary>
        /// <param name="userId">Id of user witch will be got.</param>
        /// <returns>Return user.</returns>
        Task<UserDTO> FindUserByIdAsync(string userId);
        /// <summary>
        /// Update information of user profile.
        /// </summary>
        /// <param name="userDTO">User profile witch will be updated</param>
        Task<OperationDetails> ChangeProfileInformation(UserDTO userDTO);
        /// <summary>
        /// Upload photo to user profile by user id.
        /// </summary>
        /// <param name="photoUrl">Url of photo with will be uploaded.</param>
        /// <param name="userId">Id of user profile, photo will be uploaded to.</param>
        Task<OperationDetails> UploadImage(string photoUrl, string userId);
        /// <summary>
        /// Get a tour guide by tour id.
        /// </summary>
        /// <param name="tourId">Id of tour, guide will be got of.</param>
        /// <returns>Return user.</returns>
        Task<UserDTO> GetGuideByTourId(int tourId);
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>Return collection of all users.</returns>
        Task<IEnumerable<UserDTO>> GetAllUsers();


    }
}
