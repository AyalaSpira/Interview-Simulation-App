using System;
using System.Collections.Generic;

namespace InterviewSim.DAL.Entities
{
    public class Interview
    {
        public int InterviewId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } // סטטוס הראיון (למשל: "In Progress", "Completed")
        public List<string> Questions { get; set; } // השאלות של הראיון
        public List<string> Answers { get; set; } // התשובות של המשתמש
        public DateTime InterviewDate { get; set; } // תאריך הראיון
    }
}
