using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;

namespace InterviewSim.BLL.Implementations
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly string _endpoint;

        public AIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _model = configuration["OpenAI:Model"];
            _endpoint = configuration["OpenAI:Endpoint"];
        }

        public async Task<string> AnalyzeResumeAsync(string resumeContent)
        {
            var prompt = $"Analyze this resume and identify the field of expertise: {resumeContent}";
            var result = await GenerateSummaryFromAI(prompt);
            return result;
        }

        public async Task<List<string>> GenerateQuestionsAsync(string category)
        {
            var personalPrompt = $"Generate personal interview questions for a {category} job.";
            var technicalPrompt = $"Generate technical interview questions for a {category} job.";

            var personalQuestions = await GenerateQuestionsFromAI(personalPrompt) ?? new List<string>();
            var technicalQuestions = await GenerateQuestionsFromAI(technicalPrompt) ?? new List<string>();

            if (!personalQuestions.Any() && !technicalQuestions.Any())
            {
                // שאלות ברירת מחדל למקרה שהבינה המלאכותית לא מחזירה כלום
                return new List<string>
        {
            "ספר לי על עצמך.",
            "מהן החוזקות שלך?",
            "איך אתה מתמודד עם לחץ?",
            "מהי השאיפה שלך בקריירה?",
            "תן לי דוגמה לאתגר שהתמודדת איתו בעבודה קודמת."
        };
            }

            var questions = new List<string>();
            questions.AddRange(personalQuestions);
            questions.AddRange(technicalQuestions);

            return questions;
        }


        public async Task<string> AnalyzeInterviewAsync(List<string> answers, List<string> questions)
        {
            var prompt = BuildPrompt(answers, questions);
            var result = await GenerateSummaryFromAI(prompt);
            return result;
        }

        private async Task<List<string>> GenerateQuestionsFromAI(string prompt)
        {
            var requestBody = new
            {
                model = _model,
                messages = new[] { new { role = "user", content = prompt } },
                max_tokens = 500
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync(_endpoint, content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseString);

            // הימנע מ-NullReferenceException: אם אין תגובה נכונה, החזר רשימה ריקה
            if (jsonResponse?.Choices?.Count > 0 && jsonResponse.Choices[0]?.Message?.Content != null)
            {
                var responseText = jsonResponse.Choices[0].Message.Content.Trim();

                // במקרה שהתשובה היא טקסט מרובה שורות, נוכל לפרק את התשובה לשורות
                return responseText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            return new List<string>(); // אם אין תשובה, החזר רשימה ריקה
        }

        private async Task<string> GenerateSummaryFromAI(string prompt)
        {
            var requestBody = new
            {
                model = _model,
                messages = new[] { new { role = "system", content = "You are an AI interview analysis tool." },
                                   new { role = "user", content = prompt } },
                max_tokens = 500
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync(_endpoint, content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseString);

            return jsonResponse?.Choices?[0].Message?.Content.Trim() ?? "Unable to generate summary";
        }

        private string BuildPrompt(List<string> answers, List<string> questions)
        {
            var prompt = "Summarize the interview based on the following answers and questions:\n\n";

            for (int i = 0; i < questions.Count; i++)
            {
                prompt += $"Question: {questions[i]}\nAnswer: {answers[i]}\n\n";
            }

            prompt += "Based on this, please provide a summary of the candidate's performance.";
            return prompt;
        }
    }
}
