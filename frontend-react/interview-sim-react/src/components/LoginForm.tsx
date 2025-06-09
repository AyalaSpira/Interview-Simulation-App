"use client"

import type React from "react"
import { useState, useEffect } from "react"
import { Form, Input, Button, message, Card, Typography, Checkbox } from "antd"
import { motion, AnimatePresence } from "framer-motion"
import { Mail, Lock, Eye, EyeOff, Sparkles, Shield, Zap } from "lucide-react"
import { loginUser } from "../services/authService"
import { useNavigate } from "react-router-dom"

const { Title, Text } = Typography

interface LoginFormProps {
  onLogin: (token: string) => void
}

const LoginForm: React.FC<LoginFormProps> = ({ onLogin }) => {
  const [loading, setLoading] = useState(false)
  const [showPassword, setShowPassword] = useState(false)
  const [rememberMe, setRememberMe] = useState(false)
  const [currentFeature, setCurrentFeature] = useState(0)
  const navigate = useNavigate()

  const features = [
    { icon: <Sparkles size={20} />, text: "AI-Powered Questions" },
    { icon: <Shield size={20} />, text: "Secure & Private" },
    { icon: <Zap size={20} />, text: "Instant Feedback" },
  ]

  useEffect(() => {
    const token = localStorage.getItem("token")
    if (token) {
      navigate("/home")
    }

    const interval = setInterval(() => {
      setCurrentFeature((prev) => (prev + 1) % features.length)
    }, 2000)
    return () => clearInterval(interval)
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
        message.success("Welcome back! 🎉")
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
        background: "linear-gradient(135deg, #0f172a 0%, #1e293b 50%, #0f172a 100%)",
        display: "flex",
        position: "relative",
        overflow: "hidden",
      }}
    >
      {/* Animated Background */}
      <div style={{ position: "absolute", inset: 0 }}>
        {[...Array(15)].map((_, i) => (
          <motion.div
            key={i}
            style={{
              position: "absolute",
              width: Math.random() * 200 + 50,
              height: Math.random() * 200 + 50,
              borderRadius: "50%",
              background: `radial-gradient(circle, ${
                i % 3 === 0
                  ? "rgba(168, 85, 247, 0.1)"
                  : i % 3 === 1
                    ? "rgba(59, 130, 246, 0.1)"
                    : "rgba(16, 185, 129, 0.1)"
              } 0%, transparent 70%)`,
              left: `${Math.random() * 100}%`,
              top: `${Math.random() * 100}%`,
            }}
            animate={{
              x: [0, Math.random() * 100 - 50],
              y: [0, Math.random() * 100 - 50],
              scale: [1, 1.2, 1],
            }}
            transition={{
              duration: Math.random() * 8 + 8,
              repeat: Number.POSITIVE_INFINITY,
              repeatType: "reverse",
            }}
          />
        ))}
      </div>

      {/* Left Side - Branding */}
      <div
        style={{
          flex: 1,
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
          alignItems: "center",
          padding: "40px",
          position: "relative",
          zIndex: 2,
        }}
      >
        <motion.div
          initial={{ opacity: 0, x: -50 }}
          animate={{ opacity: 1, x: 0 }}
          transition={{ duration: 0.8 }}
          style={{ textAlign: "center", maxWidth: "500px" }}
        >
          <motion.div
            animate={{ rotate: 360 }}
            transition={{ duration: 20, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}
            style={{
              width: "120px",
              height: "120px",
              margin: "0 auto 30px",
              borderRadius: "30px",
              background: "linear-gradient(135deg, #a855f7, #3b82f6, #10b981)",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              boxShadow: "0 20px 60px rgba(168, 85, 247, 0.3)",
            }}
          >
            <Sparkles size={60} color="white" />
          </motion.div>

          <Title
            level={1}
            style={{
              fontSize: "3.5rem",
              fontWeight: 800,
              margin: "0 0 20px 0",
              background: "linear-gradient(135deg, #fff 0%, #a855f7 50%, #3b82f6 100%)",
              backgroundClip: "text",
              WebkitBackgroundClip: "text",
              WebkitTextFillColor: "transparent",
            }}
          >
            InterviewAI Pro
          </Title>

          <Text
            style={{
              fontSize: "1.4rem",
              color: "rgba(255, 255, 255, 0.8)",
              marginBottom: "40px",
              display: "block",
              lineHeight: 1.6,
            }}
          >
            Master your next job interview with AI-powered practice sessions
          </Text>

          {/* Rotating Features */}
          <div style={{ height: "60px", display: "flex", alignItems: "center", justifyContent: "center" }}>
            <AnimatePresence mode="wait">
              <motion.div
                key={currentFeature}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0, y: -20 }}
                transition={{ duration: 0.5 }}
                style={{
                  display: "flex",
                  alignItems: "center",
                  gap: "12px",
                  padding: "16px 24px",
                  background: "rgba(30, 41, 59, 0.4)",
                  borderRadius: "16px",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  backdropFilter: "blur(20px)",
                }}
              >
                <div style={{ color: "#a855f7" }}>{features[currentFeature].icon}</div>
                <Text style={{ color: "#fff", fontSize: "1.1rem", fontWeight: 600 }}>
                  {features[currentFeature].text}
                </Text>
              </motion.div>
            </AnimatePresence>
          </div>
        </motion.div>
      </div>

      {/* Right Side - Login Form */}
      <div
        style={{
          flex: 1,
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          padding: "40px",
          position: "relative",
          zIndex: 2,
        }}
      >
        <motion.div
          initial={{ opacity: 0, x: 50 }}
          animate={{ opacity: 1, x: 0 }}
          transition={{ duration: 0.8, delay: 0.2 }}
          style={{ width: "100%", maxWidth: "450px" }}
        >
          <Card
            style={{
              background: "rgba(30, 41, 59, 0.8)",
              borderRadius: "24px",
              boxShadow: "0 25px 100px rgba(0, 0, 0, 0.4)",
              border: "1px solid rgba(255, 255, 255, 0.1)",
              backdropFilter: "blur(30px)",
              overflow: "hidden",
              padding: "20px",
            }}
          >
            <div style={{ textAlign: "center", marginBottom: "40px" }}>
              <Title
                level={2}
                style={{
                  color: "#fff",
                  marginBottom: "8px",
                  fontWeight: 700,
                  fontSize: "2rem",
                }}
              >
                Welcome Back! 👋
              </Title>
              <Text
                style={{
                  color: "rgba(255, 255, 255, 0.7)",
                  fontSize: "1.1rem",
                }}
              >
                Sign in to continue your interview journey
              </Text>
            </div>

            <Form layout="vertical" onFinish={handleLogin} size="large">
              <Form.Item
                label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>Email Address</Text>}
                name="email"
                rules={[
                  { required: true, message: "Please enter your email" },
                  { type: "email", message: "Please enter a valid email" },
                ]}
              >
                <Input
                  prefix={<Mail size={20} style={{ color: "#a855f7" }} />}
                  placeholder="Enter your email"
                  style={{
                    height: "56px",
                    borderRadius: "16px",
                    background: "rgba(15, 23, 42, 0.6)",
                    border: "1px solid rgba(99, 102, 241, 0.3)",
                    color: "#fff",
                    fontSize: "1rem",
                  }}
                />
              </Form.Item>

              <Form.Item
                label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>Password</Text>}
                name="password"
                rules={[{ required: true, message: "Please enter your password" }]}
              >
                <Input
                  type={showPassword ? "text" : "password"}
                  prefix={<Lock size={20} style={{ color: "#a855f7" }} />}
                  suffix={
                    <button
                      type="button"
                      onClick={() => setShowPassword(!showPassword)}
                      style={{
                        background: "none",
                        border: "none",
                        color: "rgba(255, 255, 255, 0.5)",
                        cursor: "pointer",
                        padding: "4px",
                      }}
                    >
                      {showPassword ? <EyeOff size={20} /> : <Eye size={20} />}
                    </button>
                  }
                  placeholder="Enter your password"
                  style={{
                    height: "56px",
                    borderRadius: "16px",
                    background: "rgba(15, 23, 42, 0.6)",
                    border: "1px solid rgba(99, 102, 241, 0.3)",
                    color: "#fff",
                    fontSize: "1rem",
                  }}
                />
              </Form.Item>

              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                  marginBottom: "30px",
                }}
              >
                <Checkbox
                  checked={rememberMe}
                  onChange={(e) => setRememberMe(e.target.checked)}
                  style={{ color: "rgba(255, 255, 255, 0.7)" }}
                >
                  <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>Remember me</Text>
                </Checkbox>
                <a style={{ color: "#a855f7", fontWeight: 600 }}>Forgot password?</a>
              </div>

              <motion.div whileHover={{ scale: 1.02 }} whileTap={{ scale: 0.98 }}>
                <Button
                  type="primary"
                  htmlType="submit"
                  loading={loading}
                  style={{
                    height: "56px",
                    width: "100%",
                    borderRadius: "16px",
                    background: loading
                      ? "linear-gradient(90deg, #6366f1, #8b5cf6)"
                      : "linear-gradient(90deg, #a855f7, #3b82f6)",
                    border: "none",
                    fontSize: "1.1rem",
                    fontWeight: 700,
                    marginBottom: "24px",
                    boxShadow: "0 10px 30px rgba(168, 85, 247, 0.3)",
                    transition: "all 0.3s ease",
                  }}
                >
                  {loading ? (
                    <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                      <motion.div
                        animate={{ rotate: 360 }}
                        transition={{ duration: 1, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}
                        style={{
                          width: "20px",
                          height: "20px",
                          border: "2px solid rgba(255, 255, 255, 0.3)",
                          borderTop: "2px solid #fff",
                          borderRadius: "50%",
                        }}
                      />
                      Signing you in...
                    </div>
                  ) : (
                    "Sign In"
                  )}
                </Button>
              </motion.div>

              <div style={{ textAlign: "center" }}>
                <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>
                  Don't have an account?{" "}
                  <motion.a
                    whileHover={{ scale: 1.05 }}
                    onClick={() => navigate("/register")}
                    style={{
                      color: "#a855f7",
                      fontWeight: 700,
                      cursor: "pointer",
                      textDecoration: "none",
                    }}
                  >
                    Create Account
                  </motion.a>
                </Text>
              </div>
            </Form>
          </Card>
        </motion.div>
      </div>
    </div>
  )
}

export default LoginForm
