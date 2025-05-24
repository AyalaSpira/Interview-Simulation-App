"use client"

import type React from "react"
import { useState } from "react"
import { sendInterviewReport } from "../services/InterviewService"
import { Button, Card, Typography, Alert } from "antd"
import { motion } from "framer-motion"
import { Mail, CheckCircle, AlertCircle } from "lucide-react"

const { Title, Text } = Typography

interface InterviewReportProps {
  interviewId: number | null
}

const InterviewReport: React.FC<InterviewReportProps> = ({ interviewId }) => {
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState(false)

  const handleSendReport = async () => {
    if (!interviewId) {
      setError("Invalid interview ID")
      return
    }

    setLoading(true)
    setError(null)
    setSuccess(false)

    try {
      const response = await sendInterviewReport(interviewId)
      console.log(response)
      setSuccess(true)
    } catch (err) {
      setError("Failed to send report. Please try again.")
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  return (
    <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.5 }}>
      <Card
        style={{
          width: "100%",
          maxWidth: "600px",
          margin: "0 auto",
          padding: "30px",
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
          Interview Report
        </Title>

        <Text
          style={{
            color: "rgba(255, 255, 255, 0.7)",
            fontSize: "1.1rem",
            display: "block",
            textAlign: "center",
            marginBottom: "30px",
          }}
        >
          Send your interview report to your email for future reference
        </Text>

        {success && (
          <motion.div
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
            style={{ marginBottom: "24px" }}
          >
            <Alert
              message="Success"
              description="Report sent successfully to your email!"
              type="success"
              showIcon
              icon={<CheckCircle size={20} />}
              style={{
                background: "rgba(16, 185, 129, 0.2)",
                border: "1px solid rgba(16, 185, 129, 0.3)",
                borderRadius: "12px",
              }}
            />
          </motion.div>
        )}

        {error && (
          <motion.div
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
            style={{ marginBottom: "24px" }}
          >
            <Alert
              message="Error"
              description={error}
              type="error"
              showIcon
              icon={<AlertCircle size={20} />}
              style={{
                background: "rgba(239, 68, 68, 0.2)",
                border: "1px solid rgba(239, 68, 68, 0.3)",
                borderRadius: "12px",
              }}
            />
          </motion.div>
        )}

        <Button
          onClick={handleSendReport}
          type="primary"
          loading={loading}
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
          icon={<Mail size={18} style={{ marginRight: "8px" }} />}
        >
          {loading ? "Sending..." : "Send Report to Email"}
        </Button>
      </Card>
    </motion.div>
  )
}

export default InterviewReport
