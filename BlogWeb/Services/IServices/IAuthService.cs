using BlogWeb.Models.Dto;

namespace BlogWeb.Services.IServices
{
	public interface IAuthService : IBaseService
	{
		Task<T> LoginAsync<T>(LoginRequestDTO userDTO);
	}
}
