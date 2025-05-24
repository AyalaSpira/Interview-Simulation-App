"use client"

import type React from "react"
import { useState, useEffect } from "react"
import { Button, Input, Typography, Card } from "antd"
import { motion, AnimatePresence } from "framer-motion"
import { Clock, Send } from "lucide-react"

const { Title, Text } = Typography

interface Question {
  question: string
  timer: number
}

interface InterviewComponentProps {
  questions: Question[]
  currentQuestionIndex: number
  onNextQuestion: (answer: string) => void
  userId: number | null
}

const InterviewComponent: React.FC<InterviewComponentProps> = ({
  questions,
  currentQuestionIndex,
  onNextQuestion,
  userId,
}) => {
  const [answer, setAnswer] = useState<string>("")
  const [timer, setTimer] = useState<number>(questions[currentQuestionIndex]?.timer || 0)
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false)
  const [progress, setProgress] = useState<number>(100)

  useEffect(() => {
    setTimer(questions[currentQuestionIndex]?.timer || 0)
    setProgress(100)
  }, [currentQuestionIndex, questions])

  useEffect(() => {
    let countdown: NodeJS.Timeout
    if (timer > 0) {
      countdown = setInterval(() => {
        setTimer((prev) => {
          const newTime = prev > 0 ? prev - 1 : 0
          setProgress((newTime / (questions[currentQuestionIndex]?.timer || 1)) * 100)
          return newTime
        })
      }, 1000)
    } else {
      onNextQuestion(answer)
    }
    return () => clearInterval(countdown)
  }, [timer, answer, onNextQuestion, currentQuestionIndex, questions])

  const handleSubmit = () => {
    if (answer.trim() === "") {
      alert("Please provide an answer.")
      return
    }

    setIsSubmitting(true)
    onNextQuestion(answer)
    setAnswer("")
    setIsSubmitting(false)
  }

  return (
    <AnimatePresence mode="wait">
      <motion.div
        key={currentQuestionIndex}
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        exit={{ opacity: 0, y: -20 }}
        transition={{ duration: 0.5 }}
        style={{ minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center" }}
      >
        <Card
          style={{
            width: "100%",
            maxWidth: "800px",
            padding: "40px",
            background: "linear-gradient(135deg, rgba(15, 23, 42, 0.9), rgba(30, 41, 59, 0.9))",
            borderRadius: "24px",
            boxShadow: "0 20px 80px rgba(0, 0, 0, 0.3)",
            border: "1px solid rgba(255, 255, 255, 0.1)",
            backdropFilter: "blur(20px)",
          }}
        >
          <div style={{ marginBottom: "30px" }}>
            <motion.div
              initial={{ opacity: 0, scale: 0.9 }}
              animate={{ opacity: 1, scale: 1 }}
              transition={{ delay: 0.2, duration: 0.5 }}
            >
              <Title
                level={2}
                style={{
                  color: "#fff",
                  textAlign: "center",
                  fontWeight: 700,
                  background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                  backgroundClip: "text",
                  WebkitBackgroundClip: "text",
                  WebkitTextFillColor: "transparent",
                }}
              >
                Question {currentQuestionIndex + 1} of {questions.length}
              </Title>
            </motion.div>

            <motion.div
              initial={{ opacity: 0, y: 10 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: 0.3, duration: 0.5 }}
            >
              <Text
                style={{
                  color: "rgba(255, 255, 255, 0.9)",
                  fontSize: "1.25rem",
                  display: "block",
                  textAlign: "center",
                  marginTop: "16px",
                  marginBottom: "30px",
                  lineHeight: 1.6,
                }}
              >
                {questions[currentQuestionIndex]?.question}
              </Text>
            </motion.div>

            <div
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                marginBottom: "30px",
              }}
            >
              <div
                style={{
                  display: "flex",
                  alignItems: "center",
                  padding: "12px 24px",
                  background: "rgba(15, 23, 42, 0.6)",
                  borderRadius: "12px",
                  border: "1px solid rgba(99, 102, 241, 0.3)",
                }}
              >
                <Clock size={20} style={{ color: "#a855f7", marginRight: "10px" }} />
                <Text
                  style={{
                    color: "#fff",
                    fontSize: "1.1rem",
                    fontWeight: 600,
                  }}
                >
                  {timer}s remaining
                </Text>
              </div>
            </div>

            <div
              style={{
                height: "6px",
                width: "100%",
                background: "rgba(15, 23, 42, 0.6)",
                borderRadius: "3px",
                overflow: "hidden",
                marginBottom: "30px",
              }}
            >
              <motion.div
                initial={{ width: "100%" }}
                animate={{ width: `${progress}%` }}
                transition={{ duration: 0.5 }}
                style={{
                  height: "100%",
                  background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                  borderRadius: "3px",
                }}
              />
            </div>
          </div>

          <Input.TextArea
            rows={6}
            value={answer}
            onChange={(e) => setAnswer(e.target.value)}
            disabled={isSubmitting}
            placeholder="Type your answer here..."
            style={{
              padding: "16px",
              borderRadius: "12px",
              border: "1px solid rgba(99, 102, 241, 0.3)",
              background: "rgba(15, 23, 42, 0.6)",
              color: "#fff",
              fontSize: "1rem",
              resize: "none",
              marginBottom: "24px",
            }}
          />

          <Button
            onClick={handleSubmit}
            type="primary"
            style={{
              height: "50px",
              width: "100%",
              borderRadius: "12px",
              background: "linear-gradient(90deg, #a855f7, #3b82f6)",
              border: "none",
              fontSize: "1rem",
              fontWeight: 600,
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
            }}
            loading={isSubmitting}
            icon={<Send size={18} style={{ marginRight: "8px" }} />}
          >
            {isSubmitting ? "Submitting..." : "Submit Answer"}
          </Button>
        </Card>
      </motion.div>
    </AnimatePresence>
  )
}

export default InterviewComponent
