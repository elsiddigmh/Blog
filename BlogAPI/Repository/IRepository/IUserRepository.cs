using BlogAPI.Models;
using BlogAPI.Models.Dto;

namespace BlogAPI.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public bool IsUiniqueUser(string  username);
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<User> UpdateAsync(User entity);
    }
}
