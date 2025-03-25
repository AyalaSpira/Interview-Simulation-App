import React, { useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";

interface Question {
  question: string;
  timer: number;
}

interface DecodedToken {
  nameid: number;
}

const Interview: React.FC = () => {
  const [questions, setQuestions] = useState<Question[]>([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState<number>(0);
  const [answer, setAnswer] = useState<string>(""); 
  const [timer, setTimer] = useState<number>(0); 
  const [showReport, setShowReport] = useState<boolean>(false);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [userId, setUserId] = useState<number | null>(null);

  useEffect(() => {
    const getUserIdFromToken = () => {
      try {
        const token = localStorage.getItem("token");
        if (!token) {
          console.error("Token not found in localStorage");
          return;
        }

        const decoded: DecodedToken = jwtDecode(token);
        setUserId(Number(decoded.nameid));
      } catch (error) {
        console.error("Error decoding token:", error);
      }
    };

    getUserIdFromToken();
  }, []);

  useEffect(() => {
    let countdown: NodeJS.Timeout;
    if (timer > 0) {
      countdown = setInterval(() => {
        setTimer((prev) => (prev > 0 ? prev - 1 : 0));
      }, 1000);
    } else {
      if (currentQuestionIndex < questions.length - 1) {
        handleNextQuestion();
      }
    }
    return () => clearInterval(countdown);
  }, [timer]);

  const startInterview = async () => {
    if (!userId) {
      alert("User ID not found. Please log in again.");
      return;
    }

    try {
      const response = await fetch(`http://{apiUrl}/interview/start?userId=${userId}`, {
        method: "POST",
      });

      if (response.ok) {
        const data = await response.json();
        console.log(data);
        
          setQuestions(data);  // לקחת רק 5 שאלות
          setCurrentQuestionIndex(0);
          //setTimer(data[0].timer);  // הגדרת זמן השאלה הראשונה
       
      } else {
        alert("There was an error starting the interview.");
      }
    } catch (error) {
      alert("There was an error starting the interview.");
    }
  };


  const handleNextQuestion = () => {
    if (currentQuestionIndex < questions.length - 1) {
      setCurrentQuestionIndex(currentQuestionIndex + 1);
      setTimer(questions[currentQuestionIndex + 1].timer);  // עדכון זמן השאלה הבאה
    } else {
      setShowReport(true);
    }
  };

  const submitAnswer = async () => {
    if (answer.trim() === "") {
      alert("Please provide an answer.");
      return;
    }

    setIsSubmitting(true);
    try {
      const response = await fetch("https://${apiUrl}/interview/submit-answers", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ interviewId: currentQuestionIndex + 1, userId, answers: [answer] }),
      });

      if (response.ok) {
        setIsSubmitting(false);
        setAnswer("");
        handleNextQuestion();
      } else {
        throw new Error("Error submitting answer");
      }
    } catch (error) {
      setIsSubmitting(false);
      alert("There was an error submitting your answer.");
    }
  };

  return (
    <div>
      <h2>Interview Simulation</h2>
      {!showReport ? (
        <div>
          {questions.length > 0 && currentQuestionIndex < questions.length ? (
            <>
              <p>Question {currentQuestionIndex + 1}:</p>
              <p>{questions[currentQuestionIndex]?.question}</p>
              <p>Time remaining: {timer}s</p>
              <input
                type="text"
                value={answer}
                onChange={(e) => setAnswer(e.target.value)}
                disabled={isSubmitting}
              />
              <button onClick={submitAnswer} disabled={isSubmitting}>
                {isSubmitting ? "Submitting..." : "Submit Answer"}
              </button>
            </>
          ) : (
            <p>Loading questions...</p>
          )}
        </div>
      ) : (
        <div>
          <p>Your interview is complete! Here is your report:</p>
        </div>
      )}
      <button onClick={startInterview} disabled={isSubmitting}>
        Start New Interview
      </button>
    </div>
  );
};

export default Interview;