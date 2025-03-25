using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Interfaces
{
    // ממשק IMailService
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);

        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, byte[] fileBytes, string fileName);
    }

}
