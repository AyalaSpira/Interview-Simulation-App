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
            _apiKey = configuration["OpenAI_ApiKey"];  // שינוי לשם החדש
            _model = configuration["OpenAI_Model"];    // שינוי לשם החדש
            _endpoint = configuration["OpenAI_Endpoint"]; // שינוי לשם החדש

            Console.WriteLine("_apiKey", _apiKey, "_model", _model, "_endpoint", _endpoint);
        }


        // מקבלת תוכן רזומה ומנתחת את תחום העבודה בונה פרומט לבינה כדי להוציא את התחום מהתוכן
        public async Task<string> AnalyzeResumeAsync(string resumeContent)
        {
            var prompt = $@"
You are a professional HR and recruitment AI assistant.
Analyze the following resume text carefully.
Extract ONLY the primary job title, main professional domain, and top 3 relevant skills.
Respond in this strict JSON format ONLY, without any explanations or additional text:

{{
  ""JobTitle"": ""string"",
  ""Domain"": ""string"",
  ""TopSkills"": [""string"", ""string"", ""string""]
}}

Resume Text:
{resumeContent}
";

            var requestBody = new
            {
                model = _model,
                messages = new[] { new { role = "user", content = prompt } },
                max_tokens = 300
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

                var aiResponse = jsonResponse?.Choices?[0].Message?.Content.Trim();

                Console.WriteLine("AI Resume Analysis Result:\n" + aiResponse);

                return aiResponse ?? "Unable to extract job field.";
            }
            catch (Exception ex)
            {
                return $"Error analyzing resume: {ex.Message}";
            }
        }


        //GenerateQuestionsFromAI-שולחת לבינה 2 פרומט אחת אישי ואחת מקצועי
        public async Task<List<string>> GenerateQuestionsAsync(string category, int numberOfQuestions = 5)
        {
            var personalPrompt = $"Generate 2 personal interview questions for a {category} job. Do not add any introduction or explanations. Separate each question with a comma. Limit each question to a maximum of 3 sentences.";
            var technicalPrompt = $"Generate 2 technical interview questions for a {category} job. Do not add any introduction or explanations. Separate each question with a comma. Limit each question to a maximum of 3 sentences.";

            var personalQuestions = await GenerateQuestionsFromAI(personalPrompt);
            var technicalQuestions = await GenerateQuestionsFromAI(technicalPrompt);

            var questions = new List<string>();
            questions.AddRange(personalQuestions);
            questions.AddRange(technicalQuestions);

            return questions; // להגביל ל-5 שאלות
        }


        //  היא שולחת בעצמה לבינה פונקציה פנימית ליצירת שאלות דרך ה-AI
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

        #region   הפקת הסיכום בסוף
        //ראיון פונקציה פנימית להפקת סיכום
        private async Task<string> GenerateSummaryFromAI(string prompt)
        {
            var requestBody = new
            {
                model = _model,
                messages = new[] { new { role = "system", content = "You are an AI interview analysis tool." },
                                   new { role = "user", content = prompt } },
                max_tokens = 100
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
            var prompt = "Summarize the interview based on the following answers and questions\n\n";

            for (int i = 0; i < questions.Count&&i<answers.Count; i++)
            {
                prompt += $"Question: {questions[i]}\nAnswer: {answers[i]}\n\n";
            }

            prompt += "Based on this, please provide a summary of the candidate's performance.";
            return prompt;
        }


        public async Task<string> AnalyzeInterviewAsync(List<string> answers, List<string> questions)
        {
            // נבנה את ההוראה על פי השאלות והתשובות
            var prompt = BuildPrompt(answers, questions);

            // נשלח את הפלט ל-AI כדי לקבל סיכום
            return await GenerateSummaryFromAI(prompt);
        }
        #endregion


    }
} 