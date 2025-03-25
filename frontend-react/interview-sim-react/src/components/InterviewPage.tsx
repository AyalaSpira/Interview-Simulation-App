import React, { useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";
import { Button, Typography, Spin } from "antd";
import InterviewComponent from "./Interview";
import { submitAnswers, startInterview, downloadInterviewReport, sendInterviewReport } from "../services/InterviewService";
import PdfViewer from "./InterviewReportViewer";
import InterviewReportViewer from "./InterviewReportViewer";
import ReportTextViewer from "./ReportTextViewer";

const { Title } = Typography;

interface Question {
  question: string;
  timer: number;
}

interface DecodedToken {
  nameid: number;
}

const InterviewPage: React.FC = () => {
  const [questions, setQuestions] = useState<Question[]>([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState<number>(0);
  const [answers, setAnswers] = useState<string[]>([]);
  const [showReport, setShowReport] = useState<boolean>(false);
  const [userId, setUserId] = useState<number | null>(null);
  const [interviewId, setInterviewId] = useState<number | null>(null);
  const [loading, setLoading] = useState<boolean>(false);  // מצב טעינה
  const [interviewText, setInterviewText] = useState<string | null>(null);

  useEffect(() => {
    const getUserIdFromToken = () => {
      try {
        const token = localStorage.getItem("token");
        if (!token) return;
        const decoded: DecodedToken = jwtDecode(token);
        setUserId(Number(decoded.nameid));
      } catch (error) {
        console.error("Error decoding token:", error);
      }
    };
    getUserIdFromToken();
  }, []);

  const handleShowPdf = async () => {
    if (!userId) {
      alert("User ID not found. Please log in again.");
      return;
    }
    setShowReport(true);  // נכנס כאן כשהראיון הושלם
  };

  const handleStartInterview = async () => {
    if (!userId) {
      alert("User ID not found. Please log in again.");
      return;
    }

    setLoading(true); // מתחילים טעינה

    try {
      const data = await startInterview(userId);  // שינוי כאן
      setQuestions(data.slice(0, 5)); // בחר רק 5 שאלות ראשונות
      setCurrentQuestionIndex(0);
    } catch (error) {
      alert("There was an error starting the interview.");
    } finally {
      setLoading(false); // מסיימים טעינה ברגע שהשאלות הגיעו
    }
  };

  const handleNextQuestion = (answer: string) => {
    setAnswers((prevAnswers) => [...prevAnswers, answer]);

    if (currentQuestionIndex < questions.length - 1) {
      setCurrentQuestionIndex(currentQuestionIndex + 1);
    } else {
      setShowReport(true);
      submitAllAnswers();
    }
  };

  const submitAllAnswers = async () => {
    if (userId && answers.length > 0) {
      try {
        const interview = await submitAnswers(userId, answers);  // עדכון כאן
        setInterviewId(interview.interviewId); // שמירה של ה-ID של הראיון
      } catch (error) {
        alert("Error submitting answers.");
      }
    }
  };

  const handleDownloadReport = async () => {
    if (interviewId) {
      try {
        const file = await downloadInterviewReport(interviewId);  // עדכון כאן
        const blob = new Blob([file], { type: "application/pdf" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = "interview_report.pdf";
        link.click();
      } catch (error) {
        alert("Error downloading the report.");
      }
    }
  };

  const handleSendEmail = async () => {
    if (interviewId) {
        await sendInterviewReport(interviewId); // עדכון כאן
        alert("The report has been sent to your email!");
      } else {
        alert("No interview ID found.");
    }
  };

  return (
    <div style={{ maxWidth: 700, margin: "auto", textAlign: "center", padding: 20 }}>
      <Title>Interview Simulation</Title>
      {loading ? ( // אם במצב טעינה, הצג את ה-Spin
        <Spin size="large" />
      ) : !showReport ? (
        questions.length > 0 && currentQuestionIndex < questions.length ? (
          <InterviewComponent
            questions={questions}
            currentQuestionIndex={currentQuestionIndex}
            onNextQuestion={handleNextQuestion}
            userId={userId}
          />
        ) : (
          <Button onClick={handleStartInterview} type="primary">
            Start New Interview
          </Button>
        )
      ) : (
        <div>
          <Title level={4}>Your interview is complete! Here is your report:</Title>
          <p>✅ Thank you for participating!</p>
          <Button onClick={handleDownloadReport} type="primary" style={{ marginRight: 10 }}>
            Download Interview Report
          </Button>
          <Button onClick={handleSendEmail} type="primary">
            Send Report via Email
          </Button>

          {/* אם הראיון הושלם, הצג רק את כפתור ה-PDF */}
          {!showReport && (
            <Button onClick={handleShowPdf} type="primary">
              Show PDF Report
            </Button>
          )}

          {/* הצג את ה-PDF לאחר סיום הראיון */}
          {showReport && interviewId && (
            <div>
              <ReportTextViewer interviewId={interviewId} />
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default InterviewPage;
