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
    { icon: <Sparkles size={18} />, text: "שאלות מותאמות בינה מלאכותית" },
    { icon: <Shield size={18} />, text: "בטוח ופרטי לחלוטין" },
    { icon: <Zap size={18} />, text: "משוב מיידי ומדויק" },
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
        message.error("שגיאה בהתחברות: " + response.error)
        return
      }

      if (response.token) {
        localStorage.setItem("token", response.token)
        onLogin(response.token)
        message.success("ברוך הבא! 🎉")
        // הניתוב יקרה אוטומטית דרך onLogin
      } else {
        message.error("ההתחברות נכשלה. אנא נסה שוב.")
      }
    } catch (error) {
      console.error("Login error:", error)
      message.error("אירעה שגיאה במהלך ההתחברות.")
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
        padding: "20px",
      }}
    >
      {/* Animated Background */}
      <div style={{ position: "absolute", inset: 0 }}>
        {[...Array(12)].map((_, i) => (
          <motion.div
            key={i}
            style={{
              position: "absolute",
              width: Math.random() * 150 + 40,
              height: Math.random() * 150 + 40,
              borderRadius: "50%",
              background: `radial-gradient(circle, ${
                i % 3 === 0
                  ? "rgba(168, 85, 247, 0.08)"
                  : i % 3 === 1
                    ? "rgba(59, 130, 246, 0.08)"
                    : "rgba(16, 185, 129, 0.08)"
              } 0%, transparent 70%)`,
              left: `${Math.random() * 100}%`,
              top: `${Math.random() * 100}%`,
            }}
            animate={{
              x: [0, Math.random() * 50 - 25],
              y: [0, Math.random() * 50 - 25],
              scale: [1, 1.1, 1],
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
          padding: "20px",
          position: "relative",
          zIndex: 2,
        }}
      >
        <motion.div
          initial={{ opacity: 0, x: -50 }}
          animate={{ opacity: 1, x: 0 }}
          transition={{ duration: 0.8 }}
          style={{ textAlign: "center", maxWidth: "90%" }}
        >
          <motion.div
            animate={{ rotate: 360 }}
            transition={{ duration: 20, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}
            style={{
              width: "100px",
              height: "100px",
              margin: "0 auto 20px",
              borderRadius: "25px",
              background: "linear-gradient(135deg, #a855f7, #3b82f6, #10b981)",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              boxShadow: "0 20px 60px rgba(168, 85, 247, 0.3)",
            }}
          >
            <Sparkles size={50} color="white" />
          </motion.div>

          <Title
            level={1}
            style={{
              fontSize: "clamp(2rem, 5vw, 3rem)",
              fontWeight: 800,
              margin: "0 0 15px 0",
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
              fontSize: "clamp(1rem, 2.5vw, 1.3rem)",
              color: "rgba(255, 255, 255, 0.8)",
              marginBottom: "30px",
              display: "block",
              lineHeight: 1.6,
            }}
          >
            שלוט בראיון העבודה הבא שלך עם סימולציות מונעות בינה מלאכותית
          </Text>

          {/* Rotating Features */}
          <div style={{ height: "50px", display: "flex", alignItems: "center", justifyContent: "center" }}>
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
                  gap: "10px",
                  padding: "12px 20px",
                  background: "rgba(30, 41, 59, 0.4)",
                  borderRadius: "12px",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  backdropFilter: "blur(20px)",
                }}
              >
                <div style={{ color: "#a855f7" }}>{features[currentFeature].icon}</div>
                <Text style={{ color: "#fff", fontSize: "1rem", fontWeight: 600 }}>
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
          padding: "20px",
          position: "relative",
          zIndex: 2,
        }}
      >
        <motion.div
          initial={{ opacity: 0, x: 50 }}
          animate={{ opacity: 1, x: 0 }}
          transition={{ duration: 0.8, delay: 0.2 }}
          style={{ width: "100%", maxWidth: "400px" }}
        >
          <Card
            style={{
              background: "rgba(30, 41, 59, 0.8)",
              borderRadius: "20px",
              boxShadow: "0 25px 100px rgba(0, 0, 0, 0.4)",
              border: "1px solid rgba(255, 255, 255, 0.1)",
              backdropFilter: "blur(30px)",
              overflow: "hidden",
              padding: "15px",
            }}
          >
            <div style={{ textAlign: "center", marginBottom: "30px" }}>
              <Title
                level={2}
                style={{
                  color: "#fff",
                  marginBottom: "8px",
                  fontWeight: 700,
                  fontSize: "1.8rem",
                }}
              >
                ברוך הבא! 👋
              </Title>
              <Text
                style={{
                  color: "rgba(255, 255, 255, 0.7)",
                  fontSize: "1rem",
                }}
              >
                התחבר כדי להמשיך את מסע הראיונות שלך
              </Text>
            </div>

            <Form layout="vertical" onFinish={handleLogin} size="large">
              <Form.Item
                label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>כתובת אימייל</Text>}
                name="email"
                rules={[
                  { required: true, message: "אנא הכנס את האימייל שלך" },
                  { type: "email", message: "אנא הכנס אימייל תקין" },
                ]}
              >
                <Input
                  prefix={<Mail size={18} style={{ color: "#a855f7" }} />}
                  placeholder="הכנס את האימייל שלך"
                  style={{
                    height: "48px",
                    borderRadius: "12px",
                    background: "rgba(15, 23, 42, 0.6)",
                    border: "1px solid rgba(99, 102, 241, 0.3)",
                    color: "#fff",
                    fontSize: "1rem",
                  }}
                />
              </Form.Item>

              <Form.Item
                label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>סיסמא</Text>}
                name="password"
                rules={[{ required: true, message: "אנא הכנס את הסיסמא שלך" }]}
              >
                <Input
                  type={showPassword ? "text" : "password"}
                  prefix={<Lock size={18} style={{ color: "#a855f7" }} />}
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
                      {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                    </button>
                  }
                  placeholder="הכנס את הסיסמא שלך"
                  style={{
                    height: "48px",
                    borderRadius: "12px",
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
                  marginBottom: "25px",
                }}
              >
                <Checkbox
                  checked={rememberMe}
                  onChange={(e) => setRememberMe(e.target.checked)}
                  style={{ color: "rgba(255, 255, 255, 0.7)" }}
                >
                  <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>זכור אותי</Text>
                </Checkbox>
                <a style={{ color: "#a855f7", fontWeight: 600 }}>שכחת סיסמא?</a>
              </div>

              <motion.div whileHover={{ scale: 1.02 }} whileTap={{ scale: 0.98 }}>
                <Button
                  type="primary"
                  htmlType="submit"
                  loading={loading}
                  style={{
                    height: "48px",
                    width: "100%",
                    borderRadius: "12px",
                    background: loading
                      ? "linear-gradient(90deg, #6366f1, #8b5cf6)"
                      : "linear-gradient(90deg, #a855f7, #3b82f6)",
                    border: "none",
                    fontSize: "1rem",
                    fontWeight: 700,
                    marginBottom: "20px",
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
                          width: "18px",
                          height: "18px",
                          border: "2px solid rgba(255, 255, 255, 0.3)",
                          borderTop: "2px solid #fff",
                          borderRadius: "50%",
                        }}
                      />
                      מתחבר...
                    </div>
                  ) : (
                    "התחבר"
                  )}
                </Button>
              </motion.div>

              <div style={{ textAlign: "center" }}>
                <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>
                  אין לך חשבון?{" "}
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
                    צור חשבון
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
