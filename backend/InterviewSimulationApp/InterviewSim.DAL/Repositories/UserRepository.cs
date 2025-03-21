using InterviewSim.DAL.Entities;
using InterviewSim.BLL.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewSim.DAL;

public class UserRepository : IUserRepository
{
    private readonly InterviewSimContext _context;

    public UserRepository(InterviewSimContext context)
    {
        _context = context;
    }

    // ����� ����� ��� ���� �������
    public async Task SaveAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    // ����� ������ ������� �� ������ �� �� �� ������ �������
    public async Task<User> GetUserByIdAsync(string username, string password)
    {
        // ������ �� ������ ��� �� ������
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
        {
            // �� ������ �����, ������� �� ������
            return user;
        }

        // �� �� ���� �� ������� �� �����
        return null;
    }

    // ����� ����� ��� ���� �������
    public async Task AddUserAsync(User newUser)
    {
        if (newUser == null)
        {
            throw new ArgumentNullException(nameof(newUser), "User cannot be null.");
        }

        // ����� ������ ���� ���� �������
        await _context.Users.AddAsync(newUser);

        // ����� �� �������� ���� �������
        await _context.SaveChangesAsync();
    }

    // ����� �� �� ��������
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

}
