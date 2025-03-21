using System;
using System.Collections.Generic;

namespace InterviewSim.DAL.Entities
{
    public class Interview
    {
        public int InterviewId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } // ����� ������ (����: "In Progress", "Completed")
        public List<string> Questions { get; set; } // ������ �� ������
        public List<string> Answers { get; set; } // ������� �� ������
        public DateTime InterviewDate { get; set; } // ����� ������
    }
}
