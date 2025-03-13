import React, { useState } from "react";

interface Question {
  question: string;
  timer: number;
}

const Interview: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const [questions, setQuestions] = useState<Question[]>([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState<number>(0);
  const [answer, setAnswer] = useState<string>("");
  const [timer, setTimer] = useState<number>(0);
  const [timerActive, setTimerActive] = useState<boolean>(false);
  const [showReport, setShowReport] = useState<boolean>(false);
  
  // פונקציה להעלאת קובץ
  const handleFileUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      setFile(event.target.files[0]);
    }
  };

  // פונקציה להתחיל את הראיון
  const startInterview = async () => {
    if (file) {
      const formData = new FormData();
      formData.append("file", file);

      // שלח את הקובץ לשרת לעיבוד AI (השרת יקרא את הקובץ ויחזיר שאלות)
      const response = await fetch("http://localhost:5000/api/interview/start", {
        method: "POST",
        body: formData,
      });

      if (response.ok) {
        const data = await response.json();
        setQuestions(data.questions); // השאלות שנשלחו מה-API
        startTimer(data.questions[0].timer); // אתחול טיימר
      }
    }
  };

  // הפעלת טיימר עבור השאלה
  const startTimer = (time: number) => {
    setTimer(time);
    setTimerActive(true);
    const countdown = setInterval(() => {
      setTimer((prev) => {
        if (prev <= 0) {
          clearInterval(countdown);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);
  };

  // שליחה של תשובה
  const submitAnswer = () => {
    if (answer) {
      // שלח את התשובה לשרת
      fetch("http://localhost:5000/api/interview/answer", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ answer, questionId: questions[currentQuestionIndex].question }),
      }).then((response) => {
        if (response.ok) {
          // אם תשובה הוזנה נכון, עבור לשאלה הבאה
          if (currentQuestionIndex < questions.length - 1) {
            setCurrentQuestionIndex(currentQuestionIndex + 1);
            startTimer(questions[currentQuestionIndex + 1].timer); // טיימר חדש לשאלה הבאה
          } else {
            setShowReport(true); // הצג את הדוח לאחר סיום השאלות
          }
          setAnswer(""); // ניקוי התשובה
        }
      });
    }
  };

  return (
    <div>
      <h2>Interview Simulation</h2>

      {/* Upload Resume */}
      {!file && (
        <div>
          <input type="file" onChange={handleFileUpload} />
          <button onClick={startInterview} disabled={!file}>
            Start Interview
          </button>
        </div>
      )}

      {/* Show current question */}
      {questions.length > 0 && !showReport && (
        <div>
          <h3>{questions[currentQuestionIndex].question}</h3>
          <p>Time left: {timer} seconds</p>
          <input
            type="text"
            value={answer}
            onChange={(e) => setAnswer(e.target.value)}
          />
          <button onClick={submitAnswer} disabled={timer <= 0}>
            Submit Answer
          </button>
        </div>
      )}

      {/* Report */}
      {showReport && (
        <div>
          <h3>Your Report:</h3>
          <button onClick={() => alert("Generate Report")}>Generate Report</button>
        </div>
      )}
    </div>
  );
};

export default Interview;
