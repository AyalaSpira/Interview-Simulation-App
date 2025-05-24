"use client"

import type React from "react"
import useTypingEffect from "../hooks/useTypingEffect"
import { motion } from "framer-motion"

interface InterviewReportViewerProps {
  reportText: string
}

const InterviewReportViewer: React.FC<InterviewReportViewerProps> = ({ reportText }) => {
  const typedReport = useTypingEffect(reportText, 30)

  return (
    <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.5 }}>
      <pre
        style={{
          whiteSpace: "pre-wrap",
          textAlign: "left",
          padding: "24px",
          borderRadius: "16px",
          background: "rgba(15, 23, 42, 0.6)",
          border: "1px solid rgba(99, 102, 241, 0.3)",
          color: "rgba(255, 255, 255, 0.9)",
          fontFamily: "'Inter', sans-serif",
          fontSize: "1rem",
          lineHeight: 1.7,
          maxHeight: "70vh",
          overflowY: "auto",
          boxShadow: "0 10px 30px rgba(0, 0, 0, 0.2)",
          backdropFilter: "blur(10px)",
        }}
      >
        {typedReport}
      </pre>
    </motion.div>
  )
}

export default InterviewReportViewer
