"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { Form, Input, Button, message, Card, Typography } from "antd"
import { motion } from "framer-motion"
import { Mail, Lock } from "lucide-react"
import { loginUser } from "../services/authService"
import { useNavigate } from "react-router-dom"

const { Title, Text } = Typography

interface LoginFormProps {
  onLogin: (token: string) => void
}

const LoginForm: React.FC<LoginFormProps> = ({ onLogin }) => {
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()

  useEffect(() => {
    const token = localStorage.getItem("token")
    if (token) {
      navigate("/home")
    }
  }, [navigate])

  const handleLogin = async (values: { email: string; password: string }) => {
    setLoading(true)

    try {
      const response = await loginUser(values.email, values.password)

      if (response.error) {
        console.error("Login failed with error:", response.error)
        message.error(response.error)
        navigate("/register")
        return
      }

      if (response.token) {
        localStorage.setItem("token", response.token)
        onLogin(response.token)
        message.success("Login successful!")
        navigate("/home")
      } else {
        message.error("Login failed. Please try again.")
      }
    } catch (error) {
      console.error("Login error:", error)
      message.error("An error occurred during login.")
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
        style={{ width: "100%", maxWidth: "450px" }}
      >
        <Card
          style={{
            background: "rgba(30, 41, 59, 0.7)",
            borderRadius: "24px",
            boxShadow: "0 20px 80px rgba(0, 0, 0, 0.3)",
            border: "1px solid rgba(255, 255, 255, 0.1)",
            backdropFilter: "blur(20px)",
            overflow: "hidden",
            padding: "20px",
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
            Welcome Back
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
            Log in to continue your interview practice
          </Text>

          <Form layout="vertical" onFinish={handleLogin}>
            <Form.Item
              label={<Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>Email</Text>}
              name="email"
              rules={[{ required: true, message: "Email is required" }]}
            >
              <Input
                size="large"
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

            <Form.Item
              label={<Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>Password</Text>}
              name="password"
              rules={[{ required: true, message: "Password is required" }]}
            >
              <Input.Password
                size="large"
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

            <div style={{ textAlign: "right", marginBottom: "24px" }}>
              <a
                style={{
                  color: "#a855f7",
                  fontWeight: 600,
                }}
              >
                Forgot password?
              </a>
            </div>

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
                marginBottom: "24px",
              }}
            >
              {loading ? "Logging in..." : "Login"}
            </Button>

            <div style={{ textAlign: "center", marginTop: "16px" }}>
              <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>
                Don't have an account?{" "}
                <a
                  onClick={() => navigate("/register")}
                  style={{
                    color: "#a855f7",
                    fontWeight: 600,
                    cursor: "pointer",
                  }}
                >
                  Register
                </a>
              </Text>
            </div>
          </Form>
        </Card>
      </motion.div>
    </div>
  )
}

export default LoginForm
