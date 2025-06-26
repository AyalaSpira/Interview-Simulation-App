"use client"

import type React from "react"

import { useState } from "react"
import { Upload, Button, message, Card, Typography, Divider, Badge, Modal } from "antd"
import { motion } from "framer-motion"
import { UploadCloud, FileText, RefreshCw, CheckCircle } from "lucide-react"
import { uploadResume, uploadNewResume } from "../services/authService"

const { Title, Text, Paragraph } = Typography

const UploadResume: React.FC = () => {
  const [file, setFile] = useState<File | null>(null)
  const [uploadResult, setUploadResult] = useState<{ ResumeUrl: string; Category: string } | null>(null)
  const [loading, setLoading] = useState(false)
  const [isNewResume, setIsNewResume] = useState(false)
  const [successModal, setSuccessModal] = useState(false)

  const handleUpload = async () => {
    if (!file) {
      message.error("Please select a file")
      return
    }

    setLoading(true)
    try {
      const result = isNewResume ? await uploadNewResume(file) : await uploadResume(file)
      setUploadResult(result)
      setSuccessModal(true)
    } catch {
      message.error("Failed to upload resume")
    } finally {
      setLoading(false)
    }
  }

  return (
    <div
      style={{
        height: "100%",
        overflow: "auto",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        padding: "40px 20px",
      }}
    >
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
        style={{ width: "100%", maxWidth: "500px" }}
      >
        <Card
          style={{
            background: "rgba(30, 41, 59, 0.7)",
            borderRadius: "24px",
            boxShadow: "0 20px 80px rgba(0, 0, 0, 0.3)",
            border: "1px solid rgba(255, 255, 255, 0.1)",
            backdropFilter: "blur(20px)",
            overflow: "hidden",
          }}
        >
          <Title
            level={2}
            style={{
              color: "#fff",
              textAlign: "center",
              marginBottom: "16px",
              fontWeight: 700,
              background: "linear-gradient(90deg, #a855f7, #3b82f6)",
              backgroundClip: "text",
              WebkitBackgroundClip: "text",
              WebkitTextFillColor: "transparent",
            }}
          >
            {isNewResume ? "Upload New Resume" : "Upload Resume"}
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
            Upload your resume to get personalized interview questions
          </Text>

          <motion.div whileHover={{ scale: 1.02 }} transition={{ duration: 0.2 }}>
            <Upload.Dragger
              beforeUpload={(file) => {
                setFile(file)
                return false
              }}
              showUploadList={false}
              style={{
                background: "rgba(15, 23, 42, 0.6)",
                border: "1px dashed rgba(99, 102, 241, 0.3)",
                borderRadius: "16px",
                padding: "30px",
                marginBottom: "30px",
              }}
            >
              <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                {file ? (
                  <Badge count={<CheckCircle size={16} style={{ color: "#10b981" }} />} offset={[-5, 5]}>
                    <div
                      style={{
                        width: "64px",
                        height: "64px",
                        borderRadius: "16px",
                        background: "rgba(99, 102, 241, 0.2)",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        marginBottom: "16px",
                      }}
                    >
                      <FileText size={32} style={{ color: "#a855f7" }} />
                    </div>
                  </Badge>
                ) : (
                  <div
                    style={{
                      width: "64px",
                      height: "64px",
                      borderRadius: "16px",
                      background: "rgba(99, 102, 241, 0.2)",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      marginBottom: "16px",
                    }}
                  >
                    <UploadCloud size={32} style={{ color: "#a855f7" }} />
                  </div>
                )}

                <Text
                  style={{
                    color: "#fff",
                    fontSize: "1.1rem",
                    fontWeight: 600,
                    marginBottom: "8px",
                  }}
                >
                  {file ? file.name : "Drag & Drop your resume"}
                </Text>
                <Text style={{ color: "rgba(255, 255, 255, 0.6)" }}>
                  {file ? "File selected" : "Supports PDF, DOCX, TXT formats"}
                </Text>
              </div>
            </Upload.Dragger>
          </motion.div>

          <div style={{ display: "flex", gap: "12px", marginBottom: "24px" }}>
            <Button
              onClick={handleUpload}
              type="primary"
              loading={loading}
              style={{
                flex: 1,
                height: "50px",
                borderRadius: "12px",
                background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                border: "none",
                fontSize: "1rem",
                fontWeight: 600,
              }}
            >
              {loading ? "Uploading..." : "Upload Resume"}
            </Button>

            <Button
              onClick={() => setIsNewResume(!isNewResume)}
              style={{
                height: "50px",
                borderRadius: "12px",
                background: "rgba(15, 23, 42, 0.6)",
                border: "1px solid rgba(99, 102, 241, 0.3)",
                color: "#fff",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                width: "50px",
                padding: 0,
              }}
              icon={<RefreshCw size={18} />}
            />
          </div>

          {uploadResult && (
            <motion.div initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.3 }}>
              <Card
                style={{
                  background: "rgba(15, 23, 42, 0.6)",
                  border: "1px solid rgba(99, 102, 241, 0.3)",
                  borderRadius: "16px",
                }}
              >
                <div style={{ display: "flex", alignItems: "center", marginBottom: "12px" }}>
                  <div
                    style={{
                      width: "40px",
                      height: "40px",
                      borderRadius: "12px",
                      background: "rgba(16, 185, 129, 0.2)",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      marginRight: "12px",
                    }}
                  >
                    <CheckCircle size={20} style={{ color: "#10b981" }} />
                  </div>
                  <div>
                    <Text style={{ color: "#fff", fontWeight: 600, display: "block" }}>
                      Resume Uploaded Successfully
                    </Text>
                    <Text style={{ color: "rgba(255, 255, 255, 0.6)", fontSize: "0.9rem" }}>
                      Your resume has been processed
                    </Text>
                  </div>
                </div>

                <Divider style={{ borderColor: "rgba(255, 255, 255, 0.1)", margin: "16px 0" }} />

                <div>
                  <Text style={{ color: "rgba(255, 255, 255, 0.6)", display: "block", marginBottom: "4px" }}>
                    Category:
                  </Text>
                  <Text style={{ color: "#fff", fontWeight: 600 }}>{uploadResult.Category}</Text>
                </div>
              </Card>
            </motion.div>
          )}
        </Card>
      </motion.div>

      {/* Success Modal */}
      <Modal
        open={successModal}
        footer={null}
        onCancel={() => setSuccessModal(false)}
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
          <motion.div
            initial={{ scale: 0.5, opacity: 0 }}
            animate={{ scale: 1, opacity: 1 }}
            transition={{ duration: 0.5 }}
          >
            <div
              style={{
                width: "80px",
                height: "80px",
                borderRadius: "50%",
                background: "rgba(16, 185, 129, 0.2)",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                marginBottom: "20px",
              }}
            >
              <CheckCircle size={40} style={{ color: "#10b981" }} />
            </div>
          </motion.div>
          <Typography
            style={{
              fontSize: "1.5rem",
              fontWeight: 600,
              color: "#fff",
              marginBottom: "16px",
            }}
          >
            Resume Uploaded!
          </Typography>
          <Typography
            style={{
              color: "rgba(255, 255, 255, 0.7)",
              marginBottom: "24px",
            }}
          >
            Your resume has been successfully uploaded and processed. You're now ready for your interview!
          </Typography>
          <Button
            type="primary"
            onClick={() => setSuccessModal(false)}
            style={{
              height: "40px",
              borderRadius: "12px",
              background: "linear-gradient(90deg, #10b981, #3b82f6)",
              border: "none",
              width: "100%",
            }}
          >
            Continue
          </Button>
        </div>
      </Modal>
    </div>
  )
}

export default UploadResume
