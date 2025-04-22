using InterviewSim.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewSim.DAL.Repositories
{
    public class AdminRepository
    {
        private readonly InterviewSimContext _context;

        public AdminRepository(InterviewSimContext context)
        {
            _context = context;
        }

        public async Task<Admin> GetAdminByCredentialsAsync(string email, string password)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(u => u.Email == email); // נניח שיש שדה IsAdmin

            if (admin == null) return null;

            if (admin.Password == password)
                return admin;

            return null;
        }
    }
}
