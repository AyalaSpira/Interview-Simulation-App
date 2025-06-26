using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Interfaces
{
    public interface IAIService
    {

        // חילוץ תחום ההתמחות מתוך הרזומה
        Task<string> AnalyzeResumeAsync(string resumeContent);
        // יצירת שאלות בהתבסס על תחום ההתמחות
        Task<List<string>> GenerateQuestionsAsync(string resumeContent,int num=5);
        // ניתוח תשובות והפקת סיכום הראיון
        Task<string> AnalyzeInterviewAsync(List<string> answers, List<string> questions);


    }
}