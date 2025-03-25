import React, { useState, useEffect } from "react";
import { Button, Input, Typography, Card } from "antd";
import { motion } from "framer-motion"; // לשדרוג אנימציות

const { Title, Text } = Typography;

interface Question {
  question: string;
  timer: number;
}

interface InterviewComponentProps {
  questions: Question[];
  currentQuestionIndex: number;
  onNextQuestion: (answer: string) => void;
  userId: number | null;
}

const InterviewComponent: React.FC<InterviewComponentProps> = ({ questions, currentQuestionIndex, onNextQuestion, userId }) => {
  const [answer, setAnswer] = useState<string>("");
  const [timer, setTimer] = useState<number>(questions[currentQuestionIndex]?.timer || 0);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  useEffect(() => {
    setTimer(questions[currentQuestionIndex]?.timer || 0);
  }, [currentQuestionIndex]);

  useEffect(() => {
    let countdown: NodeJS.Timeout;
    if (timer > 0) {
      countdown = setInterval(() => {
        setTimer((prev) => (prev > 0 ? prev - 1 : 0));
      }, 1000);
    } else {
      onNextQuestion(answer);  // שולח את התשובה ל-InterviewPage
    }
    return () => clearInterval(countdown);
  }, [timer]);

  const handleSubmit = () => {
    if (answer.trim() === "") {
      alert("Please provide an answer.");
      return;
    }

    setIsSubmitting(true);
    onNextQuestion(answer);  // שולח את התשובה ל-InterviewPage
    setAnswer("");  // מאפס את השדה
    setIsSubmitting(false);
  };

  return (
    <motion.div
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ duration: 0.5 }}
      style={{ height: "100vh", overflow: "hidden" }} // הוספתי כאן את הסטייל למנוע גלילה
    >
      <Card
        style={{
          height: "100vh", // וודא שהכרטיס תופס את כל גובה המסך
          margin: 0,
          padding: "30px",
          textAlign: "center",
          background: "linear-gradient(135deg, #1D9D9B, #2EC8C3)", // צבעים של טורקיז וירוק
          color: "#fff",
          borderRadius: "10px",
          boxShadow: "0 4px 20px rgba(0, 0, 0, 0.2)",
          display: "flex",
          flexDirection: "column",
          justifyContent: "center", // מוודא שהכרטיס ממורכז
        }}
      >
        <Title level={2} style={{ color: "#fff" }}>
          Question {currentQuestionIndex + 1}
        </Title>
        <Text style={{ color: "#fff", fontSize: "18px", marginBottom: "20px" }}>
          {questions[currentQuestionIndex]?.question}
        </Text>
        <p style={{ fontSize: "24px", color: "#fff", marginTop: "20px" }}>
          ⏳ Time remaining: <span style={{ fontWeight: "bold" }}>{timer}s</span>
        </p>

        <Input.TextArea
          rows={4}
          value={answer}
          onChange={(e) => setAnswer(e.target.value)}
          disabled={isSubmitting}
          placeholder="Type your answer here..."
          style={{
            marginTop: "20px",
            padding: "10px",
            borderRadius: "5px",
            border: "1px solid #ffffff",
            backgroundColor: "#e0f7fa",
            color: "#004d40",
          }}
        />
        <Button
          onClick={handleSubmit}
          type="primary"
          style={{
            marginTop: "20px",
            backgroundColor: "#004d40",
            borderColor: "#004d40",
            width: "100%",
            fontSize: "18px",
          }}
          loading={isSubmitting}
        >
          {isSubmitting ? "Submitting..." : "Submit Answer"}
        </Button>
      </Card>
    </motion.div>
  );
};

export default InterviewComponent;
