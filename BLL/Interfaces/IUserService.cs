using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Infrastructure;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> CreateUserAsync(UserDTO userDto);
        UserDTO FindUserByUserName(string userName);
        Task<UserDTO> FindUserAsync(string userName, string password);
        Task<OperationDetails> DeleteUser(string userId);
        Task<IList<string>> GetRolesByUserId(string userId);
        Task<UserDTO> FindUserByIdAsync(string userId);
        Task<OperationDetails> ChangeProfileInformation(UserDTO userDTO);
        Task<OperationDetails> UploadImage(string photoUrl, string userId);

        
    }
}
