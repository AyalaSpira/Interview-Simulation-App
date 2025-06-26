"use client"

import { useState } from "react"
import { Button, Spin } from "antd"
import { motion } from "framer-motion"
import { FileText } from "lucide-react"
import InterviewReportViewer from "./InterviewReportViewer"
const API_URL = process.env.REACT_APP_API_URL

const ReportTextViewer = ({ interviewId }: { interviewId: number }) => {
  const [interviewText, setInterviewText] = useState<string | null>(null)
  const [showReport, setShowReport] = useState(false)
  const [loading, setLoading] = useState(false)

  const fetchReportText = async () => {
    setLoading(true)
    try {
      const response = await fetch(`${API_URL}/interview/get-text-report?interviewId=${interviewId}`)
      if (!response.ok) throw new Error("Failed to fetch report.")
      const text = await response.text()
      setInterviewText(text)
      setShowReport(true)
    } catch (error) {
      console.error("Error fetching report:", error)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div style={{ textAlign: "center", marginTop: "30px" }}>
      {!showReport ? (
        <motion.div whileHover={{ scale: 1.05 }} transition={{ duration: 0.2 }}>
          <Button
            onClick={fetchReportText}
            type="primary"
            size="large"
            loading={loading}
            style={{
              height: "50px",
              borderRadius: "12px",
              background: "linear-gradient(90deg, #a855f7, #3b82f6)",
              border: "none",
              fontSize: "1rem",
              fontWeight: 600,
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              margin: "0 auto",
              padding: "0 30px",
            }}
            icon={<FileText size={18} style={{ marginRight: "8px" }} />}
          >
            {loading ? "Loading Report..." : "View Text Report"}
          </Button>
        </motion.div>
      ) : loading ? (
        <div style={{ padding: "40px", textAlign: "center" }}>
          <Spin size="large" />
        </div>
      ) : (
        interviewText && <InterviewReportViewer reportText={interviewText} />
      )}
    </div>
  )
}

export default ReportTextViewer
