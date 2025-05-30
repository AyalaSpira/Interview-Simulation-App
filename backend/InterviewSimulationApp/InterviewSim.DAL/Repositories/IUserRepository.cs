﻿using InterviewSim.DAL.Entities;
using InterviewSim.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task<User> GetUserEntityByEmailAsync(string email);

    Task<UserDTO> GetUserByIdAsync(int userId);
    Task<UserDTO> GetUserByEmailAsync(string email);
    Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email);
    Task SaveAsync(User user);        // עדכון ב-User
    Task UpdateUserAsync(UserDTO userDTO);  // עדכון ב-User
    Task DeleteAsync(int userId);     // מחיקה של User
    Task<List<UserDTO>> GetAllUsersAsync();  // מחזיר UserDTO ולא User
    Task<IEnumerable<string>> GetAllResumeUrlsAsync();
    Task<IEnumerable<string>> GetAllReportUrlsAsync();
    Task UpdateResumeUrlToNullAsync(string fileUrl);

    Task<Admin> GetAdminByCredentialsAsync(string email, string password);


    Task AddUserAsync(User newUser);
    Task UpdateUserAsync(User user);
}
