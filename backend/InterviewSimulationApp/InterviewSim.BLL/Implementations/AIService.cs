using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using InterviewSim.BLL.Interfaces;
using System.Linq;
using System;
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

        // מימוש הפונקציה לחילוץ תחום ההתמחות מתוך הרזומה
        public async Task<string> AnalyzeResumeAsync(string resumeContent)
        {
            var prompt = $"Analyze this resume and identify the field of expertise: {resumeContent}";
            return await GenerateSummaryFromAI(prompt);
        }

        // מימוש הפונקציה ליצירת שאלות בהתאם לקטגוריה
        public async Task<List<string>> GenerateQuestionsAsync(string resumeContent)
        {
            var category = await AnalyzeResumeAsync(resumeContent); // חילוץ הקטגוריה מתוך הרזומה
            var personalPrompt = $"Generate personal interview questions for a {category} job.";
            var technicalPrompt = $"Generate technical interview questions for a {category} job.";

            var personalQuestions = await GenerateQuestionsFromAI(personalPrompt) ?? new List<string>();
            var technicalQuestions = await GenerateQuestionsFromAI(technicalPrompt) ?? new List<string>();

            var questions = new List<string>();
            questions.AddRange(personalQuestions);
            questions.AddRange(technicalQuestions);

            return questions;
        }

        // מימוש הפונקציה לניתוח תשובות והפקת סיכום
        public async Task<string> AnalyzeInterviewAsync(List<string> answers, List<string> questions)
        {
            var prompt = BuildPrompt(answers, questions);
            return await GenerateSummaryFromAI(prompt);
        }

        // פונקציה פנימית ליצירת שאלות דרך ה-AI
        public async Task<List<string>> GenerateQuestionsFromAI(string prompt)
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

            try
            {
                var response = await _httpClient.PostAsync(_endpoint, content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response from OpenAI: " + responseString);  // הדפסת התגובה לעיון

                var jsonResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseString);

                // בדיקת התגובה
                if (jsonResponse?.Choices?.Count > 0 && jsonResponse.Choices[0]?.Message?.Content != null)
                {
                    var responseText = jsonResponse.Choices[0].Message.Content.Trim();
                    Console.WriteLine("Parsed response text: " + responseText);  // הדפסת הטקסט המפוענח

                    // ניפוי אם יש בעיות
                    if (string.IsNullOrEmpty(responseText))
                    {
                        Console.WriteLine("No questions generated.");
                    }

                    var questions = responseText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    return questions;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating questions: " + ex.Message);
            }

            return new List<string>();
        }

        // פונקציה פנימית להפקת סיכום
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

            try
            {
                var response = await _httpClient.PostAsync(_endpoint, content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseString);

                return jsonResponse?.Choices?[0].Message?.Content.Trim() ?? "Unable to generate summary";
            }
            catch (Exception ex)
            {
                return $"Error generating summary: {ex.Message}";
            }
        }

        // פונקציה פנימית לבניית הפלט
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
