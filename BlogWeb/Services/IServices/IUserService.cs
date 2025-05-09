﻿using BlogWeb.Models.Dto;

namespace BlogWeb.Services.IServices
{
    public interface IUserService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(UserCreateDTO userDTO);
        Task<T> UpdateAsync<T>(UserUpdateDTO userDTO);
        Task<T> DeleteAsync<T>(int id);
        
    }


}
