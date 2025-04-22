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

    }
}
