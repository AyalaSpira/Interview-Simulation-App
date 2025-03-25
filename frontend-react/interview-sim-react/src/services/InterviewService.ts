const API_URL = "http://localhost:5000/api/interview";
// פונקציה שמתחילה ראיון
export const startInterview = async (userId: number) => {
  try {
    console.log(`Starting interview for userId: ${userId}`); // לוג התחלת ראיון

    const response = await fetch(`${API_URL}/start?userId=${userId}`, {
      method: "POST",
    });

    if (!response.ok) throw new Error("Failed to start interview");

    const jsonData = await response.json();
    console.log("Raw response from server:", jsonData); // לוג תגובת השרת הגולמית

    // נוודא שהנתונים מתקבלים בפורמט נכון
    if (!Array.isArray(jsonData)) throw new Error("Unexpected response format");
    
    // פיצול שאלות שמכילות תווי ירידת שורה והסרת רווחים
    const allQuestions = jsonData
      .flatMap(q => q.split("\n"))
      .map(q => q.trim())
      .filter(q => 
        !q.includes("Technical Interview Question:") && 
        !/^\*\*(.*?)\*\*$/.test(q)
      );

    console.log("Formatted questions:", allQuestions); // לוג השאלות המפולחות

    return allQuestions.map((q, index) => ({
      question: `Q${index + 1}: ${q}`,
      timer: 60,
    }));

  } catch (error) {
    console.error("Error starting interview:", error);
    throw error;
  }
};
// פונקציה שתפרק את הטקסט לרשימת שאלות
const parseQuestions = (text: string) => {
  const questionRegex = /\d+\.\s\*\*(.*?)\*\*\n\s*-\s*"([^"]+)"/g;
  let match;
  const questions = [];

  console.log("Parsing questions from text:", text); // לוג טקסט שמפולח לשאלות

  // חיפוש כל השאלות בטקסט והפיכן לאובייקטים
  while ((match = questionRegex.exec(text)) !== null) {
    questions.push({
      question: `${match[1]}: ${match[2]}`,
      timer: 60,
    });
  }

  console.log("Parsed questions:", questions); // לוג של השאלות המפולחות
  return questions;
};
// פונקציה לשליחת תשובות
export const submitAnswers = async (userId: number, answers: string[]) => {
  try {
    console.log(`Submitting answers for userId: ${userId}`, answers);

    const response = await fetch(`${API_URL}/submit-answers`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        userId,
        answers: answers.map(answer => answer.trim()),  // ניקוי רווחים לא נחוצים
      }),
    });

    if (!response.ok) throw new Error("Error submitting answers");

    const jsonResponse = await response.json();
    console.log("Response JSON after submitting answers:", jsonResponse);

    return jsonResponse;
  } catch (error) {
    console.error("Error submitting answers:", error);
    throw error;
  }
};
// פונקציה להורדת דוח של ראיון
export const downloadInterviewReport = async (interviewId: number) => {
  console.log(`Downloading report for interview ID: ${interviewId}`); // לוג של הורדת דוח ראיון

  try {
    const response = await fetch(`${API_URL}/download-report?interviewId=${interviewId}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });

    console.log("Response from server while downloading report:", response); // לוג תגובה מהשרת

    if (!response.ok) {
      throw new Error("Failed to download interview report.");
    }

    return await response.blob();
  } catch (error) {
    console.error("Error downloading interview report:", error);
    throw error;
  }
};
// פונקציה לשליחת דוח ראיון במייל
export const sendInterviewReport = async (interviewId: number) => {
  console.log(`Sending report for interview ID: ${interviewId}`); // לוג של שליחת הדוח

  try {
    const response = await fetch(`${API_URL}/send-report?interviewId=${interviewId}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
    });

    console.log("Response from server while sending report:", response); // לוג תגובה מהשרת

    if (!response.ok) {
      throw new Error("Failed to send interview report.");
    }

    const jsonResponse = await response.json();
    console.log("Server response after sending report:", jsonResponse);

    return jsonResponse;
  } catch (error) {
    console.error("Error sending interview report:", error);
    throw error;
  }
};
