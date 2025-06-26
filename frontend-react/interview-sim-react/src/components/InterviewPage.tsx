"use client"

import type React from "react"
import { useState, useEffect } from "react"
import { jwtDecode } from "jwt-decode"
import { Button, Typography, Spin, Card, Modal } from "antd"
import { motion } from "framer-motion"
import { Play, Download, Mail, CheckCircle } from "lucide-react"
import InterviewComponent from "./Interview"
import {
  submitAnswers,
  startInterview,
  downloadInterviewReport,
  sendInterviewReport,
} from "../services/InterviewService"
import ReportTextViewer from "./ReportTextViewer"

const { Title, Text } = Typography

interface Question {
  question: string
  timer: number
}

interface DecodedToken {
  nameid: number
}

const InterviewPage: React.FC = () => {
  const [questions, setQuestions] = useState<Question[]>([])
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState<number>(0)
  const [answers, setAnswers] = useState<string[]>([])
  const [showReport, setShowReport] = useState<boolean>(false)
  const [userId, setUserId] = useState<number | null>(null)
  const [interviewId, setInterviewId] = useState<number | null>(null)
  const [loading, setLoading] = useState<boolean>(false)
  const [interviewText, setInterviewText] = useState<string | null>(null)
  const [downloadLoading, setDownloadLoading] = useState<boolean>(false)
  const [emailLoading, setEmailLoading] = useState<boolean>(false)
  const [successModal, setSuccessModal] = useState<{ visible: boolean; message: string }>({
    visible: false,
    message: "",
  })

  useEffect(() => {
    const getUserIdFromToken = () => {
      try {
        const token = localStorage.getItem("token")
        if (!token) return
        const decoded: DecodedToken = jwtDecode(token)
        setUserId(Number(decoded.nameid))
      } catch (error) {
        console.error("Error decoding token:", error)
      }
    }
    getUserIdFromToken()
  }, [])

  const handleShowPdf = async () => {
    if (!userId) {
      alert("User ID not found. Please log in again.")
      return
    }
    setShowReport(true)
  }

  const handleStartInterview = async () => {
    if (!userId) {
      alert("User ID not found. Please log in again.")
      return
    }

    setLoading(true)

    try {
      const data = await startInterview(userId)
      setQuestions(data.slice(0, 5))
      setCurrentQuestionIndex(0)
    } catch (error) {
      alert("There was an error starting the interview.")
    } finally {
      setLoading(false)
    }
  }

  const handleNextQuestion = (answer: string) => {
    setAnswers((prevAnswers) => [...prevAnswers, answer])

    if (currentQuestionIndex < questions.length - 1) {
      setCurrentQuestionIndex(currentQuestionIndex + 1)
    } else {
      setShowReport(true)
      submitAllAnswers()
    }
  }

  const submitAllAnswers = async () => {
    
    if (userId && answers.length > 0) {
      try {

        const interview = await submitAnswers(userId, answers)
        setInterviewId(interview.interviewId)
      } catch (error) {
        alert("Error submitting answers.")
      }
    }


  }

  const handleDownloadReport = async () => {
    if (interviewId) {
      setDownloadLoading(true)
      try {
        const file = await downloadInterviewReport(interviewId)
        const blob = new Blob([file], { type: "application/pdf" })
        const link = document.createElement("a")
        link.href = URL.createObjectURL(blob)
        link.download = "interview_report.pdf"
        link.click()

        setSuccessModal({
          visible: true,
          message: "Report downloaded successfully!",
        })
      } catch (error) {
        alert("Error downloading the report.")
      } finally {
        setDownloadLoading(false)
      }
    }
  }

  const handleSendEmail = async () => {
    if (interviewId) {
      setEmailLoading(true)
      try {
        await sendInterviewReport(interviewId)
        setSuccessModal({
          visible: true,
          message: "The report has been sent to your email!",
        })
      } catch (error) {
        alert("Error sending the report.")
      } finally {
        setEmailLoading(false)
      }
    } else {
      alert("No interview ID found.")
    }
  }

  return (
    <div
      style={{
        height: "100%",
        overflow: "auto",
        padding: "20px",
      }}
    >
      {loading ? (
        <div
          style={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: "80vh",
          }}
        >
          <div style={{ textAlign: "center" }}>
            <Spin
              size="large"
              style={{
                color: "#a855f7",
              }}
            />
            <Typography style={{ color: "#fff", marginTop: "20px" }}>Preparing your interview questions...</Typography>
          </div>
        </div>
      ) : !showReport ? (
        questions.length > 0 && currentQuestionIndex < questions.length ? (
          <InterviewComponent
            questions={questions}
            currentQuestionIndex={currentQuestionIndex}
            onNextQuestion={handleNextQuestion}
            userId={userId}
          />
        ) : (
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5 }}
            style={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              height: "80vh",
            }}
          >
            <Card
              style={{
                width: "100%",
                maxWidth: "600px",
                padding: "40px",
                background: "rgba(30, 41, 59, 0.7)",
                borderRadius: "24px",
                boxShadow: "0 20px 80px rgba(0, 0, 0, 0.3)",
                border: "1px solid rgba(255, 255, 255, 0.1)",
                backdropFilter: "blur(20px)",
                textAlign: "center",
              }}
            >
              <Title
                level={2}
                style={{
                  color: "#fff",
                  marginBottom: "24px",
                  fontWeight: 700,
                  background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                  backgroundClip: "text",
                  WebkitBackgroundClip: "text",
                  WebkitTextFillColor: "transparent",
                }}
              >
                Ready for Your Interview?
              </Title>
              <Text
                style={{
                  color: "rgba(255, 255, 255, 0.7)",
                  fontSize: "1.1rem",
                  display: "block",
                  marginBottom: "40px",
                }}
              >
                Start your interview simulation to practice and improve your skills
              </Text>
              <Button
                onClick={handleStartInterview}
                type="primary"
                size="large"
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
                icon={<Play size={18} style={{ marginRight: "8px" }} />}
              >
                Start New Interview
              </Button>
            </Card>
          </motion.div>
        )
      ) : (
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
          style={{
            display: "flex",
            justifyContent: "center",
            alignItems: "flex-start",
            paddingTop: "40px",
          }}
        >
          <Card
            style={{
              width: "100%",
              maxWidth: "800px",
              padding: "40px",
              background: "rgba(30, 41, 59, 0.7)",
              borderRadius: "24px",
              boxShadow: "0 20px 80px rgba(0, 0, 0, 0.3)",
              border: "1px solid rgba(255, 255, 255, 0.1)",
              backdropFilter: "blur(20px)",
            }}
          >
            <Title
              level={2}
              style={{
                color: "#fff",
                textAlign: "center",
                marginBottom: "24px",
                fontWeight: 700,
                background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                backgroundClip: "text",
                WebkitBackgroundClip: "text",
                WebkitTextFillColor: "transparent",
              }}
            >
              Interview Complete!
            </Title>
            <Text
              style={{
                color: "rgba(255, 255, 255, 0.7)",
                fontSize: "1.1rem",
                display: "block",
                textAlign: "center",
                marginBottom: "40px",
              }}
            >
              Here's your personalized interview report and feedback
            </Text>

            <div
              style={{
                display: "grid",
                gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))",
                gap: "16px",
                marginBottom: "40px",
              }}
            >
              <Button
                onClick={handleDownloadReport}
                type="primary"
                loading={downloadLoading}
                style={{
                  height: "50px",
                  borderRadius: "12px",
                  background: "rgba(99, 102, 241, 0.2)",
                  border: "1px solid rgba(99, 102, 241, 0.3)",
                  color: "#fff",
                  fontSize: "1rem",
                  fontWeight: 600,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  transition: "all 0.3s ease",
                }}
                icon={<Download size={18} style={{ marginRight: "8px" }} />}
              >
                {downloadLoading ? "Downloading..." : "Download Report"}
              </Button>

              <Button
                onClick={handleSendEmail}
                type="primary"
                loading={emailLoading}
                style={{
                  height: "50px",
                  borderRadius: "12px",
                  background: "rgba(168, 85, 247, 0.2)",
                  border: "1px solid rgba(168, 85, 247, 0.3)",
                  color: "#fff",
                  fontSize: "1rem",
                  fontWeight: 600,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  transition: "all 0.3s ease",
                }}
                icon={<Mail size={18} style={{ marginRight: "8px" }} />}
              >
                {emailLoading ? "Sending..." : "Email Report"}
              </Button>
            </div>

            {showReport && interviewId && (
              <div>
                <ReportTextViewer interviewId={interviewId} />
              </div>
            )}
          </Card>
        </motion.div>
      )}

      {/* Success Modal */}
      <Modal
        open={successModal.visible}
        footer={null}
        onCancel={() => setSuccessModal({ visible: false, message: "" })}
        centered
        width={400}
        style={{
          borderRadius: "24px",
          overflow: "hidden",
        }}
        bodyStyle={{
          background: "rgba(30, 41, 59, 0.95)",
          backdropFilter: "blur(20px)",
          borderRadius: "24px",
          padding: "30px",
          textAlign: "center",
        }}
      >
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
          <div
            style={{
              width: "64px",
              height: "64px",
              borderRadius: "50%",
              background: "rgba(16, 185, 129, 0.2)",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              marginBottom: "20px",
            }}
          >
            <CheckCircle size={32} style={{ color: "#10b981" }} />
          </div>
          <Typography
            style={{
              fontSize: "1.25rem",
              fontWeight: 600,
              color: "#fff",
              marginBottom: "16px",
            }}
          >
            Success!
          </Typography>
          <Typography
            style={{
              color: "rgba(255, 255, 255, 0.7)",
              marginBottom: "24px",
            }}
          >
            {successModal.message}
          </Typography>
          <Button
            type="primary"
            onClick={() => setSuccessModal({ visible: false, message: "" })}
            style={{
              height: "40px",
              borderRadius: "12px",
              background: "linear-gradient(90deg, #10b981, #3b82f6)",
              border: "none",
              width: "100%",
            }}
          >
            Close
          </Button>
        </div>
      </Modal>
    </div>
  )
}

export default InterviewPage
