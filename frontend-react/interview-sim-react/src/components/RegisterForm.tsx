"use client"

import type React from "react"

import { useState } from "react"
import { Form, Input, Upload, Button, message, Card, Typography } from "antd"
import { motion } from "framer-motion"
import { UploadCloud, User, Mail, Lock, FileText } from "lucide-react"
import { registerUser } from "../services/authService"
import { useNavigate } from "react-router-dom"

const { Title, Text } = Typography

interface RegisterFormProps {
  onLogin: (token: string) => void
}

const RegisterForm: React.FC<RegisterFormProps> = ({ onLogin }) => {
  const [file, setFile] = useState<File | null>(null)
  const [password, setPassword] = useState<string>("")
  const [username, setUsername] = useState<string>("")
  const [userEmail, setUserEmail] = useState<string>("")
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()

  const handleRegister = async () => {
    if (!file || !username || !password || !userEmail) {
      message.error("Please fill all fields and upload a resume.")
      return
    }

    setLoading(true)
    try {
      const response = await registerUser(username, userEmail, password, file)
      if (response.token) {
        localStorage.setItem("token", response.token)
        onLogin(response.token)
        message.success("Registration successful! Redirecting to home...")
        setTimeout(() => navigate("/home"), 1500)
      } else {
        message.error("Registration failed. Please try again.")
      }
    } catch (error) {
      console.error("Registration failed. Error:", error)
      message.error("Registration failed. See console for details.")
    } finally {
      setLoading(false)
    }
  }

  return (
    <div
      style={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #0f172a, #1e293b)",
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
              marginBottom: "24px",
              fontWeight: 700,
              background: "linear-gradient(90deg, #a855f7, #3b82f6)",
              backgroundClip: "text",
              WebkitBackgroundClip: "text",
              WebkitTextFillColor: "transparent",
            }}
          >
            Create Account
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
            Register to start practicing interviews
          </Text>

          <Form layout="vertical" onFinish={handleRegister}>
            <Form.Item label={<Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>Username</Text>} required>
              <Input
                size="large"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                prefix={<User size={18} style={{ color: "#a855f7", marginRight: "10px" }} />}
                style={{
                  height: "50px",
                  borderRadius: "12px",
                  background: "rgba(15, 23, 42, 0.6)",
                  border: "1px solid rgba(99, 102, 241, 0.3)",
                  color: "#fff",
                }}
              />
            </Form.Item>

            <Form.Item label={<Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>Email</Text>} required>
              <Input
                size="large"
                value={userEmail}
                onChange={(e) => setUserEmail(e.target.value)}
                prefix={<Mail size={18} style={{ color: "#a855f7", marginRight: "10px" }} />}
                style={{
                  height: "50px",
                  borderRadius: "12px",
                  background: "rgba(15, 23, 42, 0.6)",
                  border: "1px solid rgba(99, 102, 241, 0.3)",
                  color: "#fff",
                }}
              />
            </Form.Item>

            <Form.Item label={<Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>Password</Text>} required>
              <Input.Password
                size="large"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                prefix={<Lock size={18} style={{ color: "#a855f7", marginRight: "10px" }} />}
                style={{
                  height: "50px",
                  borderRadius: "12px",
                  background: "rgba(15, 23, 42, 0.6)",
                  border: "1px solid rgba(99, 102, 241, 0.3)",
                  color: "#fff",
                }}
              />
            </Form.Item>

            <Form.Item label={<Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>Resume</Text>}>
              <Upload
                beforeUpload={(file: File) => {
                  setFile(file)
                  return false
                }}
                showUploadList={false}
              >
                <Button
                  icon={<UploadCloud size={18} style={{ marginRight: "10px" }} />}
                  size="large"
                  style={{
                    height: "50px",
                    width: "100%",
                    borderRadius: "12px",
                    background: "rgba(15, 23, 42, 0.6)",
                    border: "1px solid rgba(99, 102, 241, 0.3)",
                    color: "#fff",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                  }}
                >
                  {file ? file.name : "Upload Resume"}
                </Button>
              </Upload>
              {file && (
                <div
                  style={{
                    display: "flex",
                    alignItems: "center",
                    marginTop: "12px",
                    padding: "8px 12px",
                    borderRadius: "8px",
                    background: "rgba(99, 102, 241, 0.1)",
                  }}
                >
                  <FileText size={16} style={{ color: "#a855f7", marginRight: "8px" }} />
                  <Text style={{ color: "rgba(255, 255, 255, 0.7)", fontSize: "0.9rem" }}>{file.name}</Text>
                </div>
              )}
            </Form.Item>

            <Button
              type="primary"
              htmlType="submit"
              size="large"
              loading={loading}
              style={{
                height: "50px",
                width: "100%",
                borderRadius: "12px",
                background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                border: "none",
                fontSize: "1rem",
                fontWeight: 600,
                marginTop: "16px",
              }}
            >
              {loading ? "Registering..." : "Register"}
            </Button>

            <div style={{ textAlign: "center", marginTop: "24px" }}>
              <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>
                Already have an account?{" "}
                <a
                  onClick={() => navigate("/login")}
                  style={{
                    color: "#a855f7",
                    fontWeight: 600,
                    cursor: "pointer",
                  }}
                >
                  Login
                </a>
              </Text>
            </div>
          </Form>
        </Card>
      </motion.div>
    </div>
  )
}

export default RegisterForm
