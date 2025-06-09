"use client"

import type React from "react"
import { useState } from "react"
import { Form, Input, Upload, Button, message, Card, Typography, Progress, Checkbox } from "antd"
import { motion, AnimatePresence } from "framer-motion"
import { UploadCloud, User, Mail, Lock, FileText, CheckCircle, Eye, EyeOff, Shield, Target } from "lucide-react"
import { registerUser } from "../services/authService"
import { useNavigate } from "react-router-dom"

const { Title, Text } = Typography

interface RegisterFormProps {
  onLogin: (token: string) => void
}

const RegisterForm: React.FC<RegisterFormProps> = ({ onLogin }) => {
  const [file, setFile] = useState<File | null>(null)
  const [password, setPassword] = useState<string>("")
  const [confirmPassword, setConfirmPassword] = useState<string>("")
  const [username, setUsername] = useState<string>("")
  const [userEmail, setUserEmail] = useState<string>("")
  const [loading, setLoading] = useState(false)
  const [showPassword, setShowPassword] = useState(false)
  const [showConfirmPassword, setShowConfirmPassword] = useState(false)
  const [agreedToTerms, setAgreedToTerms] = useState(false)
  const [currentStep, setCurrentStep] = useState(0)
  const navigate = useNavigate()

  const getPasswordStrength = (pwd: string) => {
    let strength = 0
    if (pwd.length >= 8) strength += 25
    if (/[A-Z]/.test(pwd)) strength += 25
    if (/[0-9]/.test(pwd)) strength += 25
    if (/[^A-Za-z0-9]/.test(pwd)) strength += 25
    return strength
  }

  const passwordStrength = getPasswordStrength(password)
  const getStrengthColor = () => {
    if (passwordStrength < 50) return "#ef4444"
    if (passwordStrength < 75) return "#f59e0b"
    return "#10b981"
  }

  const steps = [
    { title: "Personal Info", icon: <User size={20} /> },
    { title: "Security", icon: <Shield size={20} /> },
    { title: "Resume Upload", icon: <FileText size={20} /> },
  ]

  const handleRegister = async () => {
    if (!file || !username || !password || !userEmail) {
      message.error("Please fill all fields and upload a resume.")
      return
    }

    if (password !== confirmPassword) {
      message.error("Passwords don't match!")
      return
    }

    if (!agreedToTerms) {
      message.error("Please agree to the terms and conditions.")
      return
    }

    setLoading(true)
    try {
      const response = await registerUser(username, userEmail, password, file)
      if (response.token) {
        localStorage.setItem("token", response.token)
        onLogin(response.token)
        message.success("🎉 Welcome to InterviewAI Pro! Redirecting...")
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

  const nextStep = () => {
    if (currentStep < 2) setCurrentStep(currentStep + 1)
  }

  const prevStep = () => {
    if (currentStep > 0) setCurrentStep(currentStep - 1)
  }

  return (
    <div
      style={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #0f172a 0%, #1e293b 50%, #0f172a 100%)",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        padding: "40px 20px",
        position: "relative",
        overflow: "hidden",
      }}
    >
      {/* Animated Background */}
      <div style={{ position: "absolute", inset: 0 }}>
        {[...Array(12)].map((_, i) => (
          <motion.div
            key={i}
            style={{
              position: "absolute",
              width: Math.random() * 150 + 50,
              height: Math.random() * 150 + 50,
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
              x: [0, Math.random() * 50 - 25],
              y: [0, Math.random() * 50 - 25],
              scale: [1, 1.1, 1],
            }}
            transition={{
              duration: Math.random() * 6 + 6,
              repeat: Number.POSITIVE_INFINITY,
              repeatType: "reverse",
            }}
          />
        ))}
      </div>

      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.6 }}
        style={{ width: "100%", maxWidth: "600px", position: "relative", zIndex: 2 }}
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
          {/* Header */}
          <div style={{ textAlign: "center", marginBottom: "40px" }}>
            <motion.div
              animate={{ rotate: 360 }}
              transition={{ duration: 20, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}
              style={{
                width: "80px",
                height: "80px",
                margin: "0 auto 20px",
                borderRadius: "20px",
                background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <Target size={40} color="white" />
            </motion.div>

            <Title
              level={2}
              style={{
                color: "#fff",
                marginBottom: "8px",
                fontWeight: 700,
                fontSize: "2.2rem",
                background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                backgroundClip: "text",
                WebkitBackgroundClip: "text",
                WebkitTextFillColor: "transparent",
              }}
            >
              Join InterviewAI Pro
            </Title>
            <Text
              style={{
                color: "rgba(255, 255, 255, 0.7)",
                fontSize: "1.1rem",
              }}
            >
              Start your journey to interview success
            </Text>
          </div>

          {/* Progress Steps */}
          <div style={{ marginBottom: "40px" }}>
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                marginBottom: "20px",
              }}
            >
              {steps.map((step, index) => (
                <div
                  key={index}
                  style={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "center",
                    flex: 1,
                  }}
                >
                  <motion.div
                    animate={{
                      scale: index === currentStep ? 1.1 : 1,
                      backgroundColor: index <= currentStep ? "#a855f7" : "rgba(255, 255, 255, 0.2)",
                    }}
                    style={{
                      width: "48px",
                      height: "48px",
                      borderRadius: "50%",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      color: "#fff",
                      marginBottom: "8px",
                      transition: "all 0.3s ease",
                    }}
                  >
                    {step.icon}
                  </motion.div>
                  <Text
                    style={{
                      color: index <= currentStep ? "#fff" : "rgba(255, 255, 255, 0.5)",
                      fontSize: "0.9rem",
                      fontWeight: 600,
                    }}
                  >
                    {step.title}
                  </Text>
                </div>
              ))}
            </div>
            <Progress
              percent={(currentStep / 2) * 100}
              showInfo={false}
              strokeColor={{
                "0%": "#a855f7",
                "100%": "#3b82f6",
              }}
              style={{ marginBottom: "20px" }}
            />
          </div>

          <Form layout="vertical" onFinish={handleRegister} size="large">
            <AnimatePresence mode="wait">
              {/* Step 1: Personal Info */}
              {currentStep === 0 && (
                <motion.div
                  key="step1"
                  initial={{ opacity: 0, x: 50 }}
                  animate={{ opacity: 1, x: 0 }}
                  exit={{ opacity: 0, x: -50 }}
                  transition={{ duration: 0.3 }}
                >
                  <Form.Item
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>Full Name</Text>}
                    required
                  >
                    <Input
                      value={username}
                      onChange={(e) => setUsername(e.target.value)}
                      prefix={<User size={20} style={{ color: "#a855f7" }} />}
                      placeholder="Enter your full name"
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
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>Email Address</Text>}
                    required
                  >
                    <Input
                      value={userEmail}
                      onChange={(e) => setUserEmail(e.target.value)}
                      prefix={<Mail size={20} style={{ color: "#a855f7" }} />}
                      placeholder="Enter your email address"
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
                </motion.div>
              )}

              {/* Step 2: Security */}
              {currentStep === 1 && (
                <motion.div
                  key="step2"
                  initial={{ opacity: 0, x: 50 }}
                  animate={{ opacity: 1, x: 0 }}
                  exit={{ opacity: 0, x: -50 }}
                  transition={{ duration: 0.3 }}
                >
                  <Form.Item
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>Password</Text>}
                    required
                  >
                    <Input
                      type={showPassword ? "text" : "password"}
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
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
                      placeholder="Create a strong password"
                      style={{
                        height: "56px",
                        borderRadius: "16px",
                        background: "rgba(15, 23, 42, 0.6)",
                        border: "1px solid rgba(99, 102, 241, 0.3)",
                        color: "#fff",
                        fontSize: "1rem",
                      }}
                    />
                    {password && (
                      <div style={{ marginTop: "8px" }}>
                        <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "4px" }}>
                          <Text style={{ color: "rgba(255, 255, 255, 0.7)", fontSize: "0.9rem" }}>
                            Password Strength
                          </Text>
                          <Text style={{ color: getStrengthColor(), fontSize: "0.9rem", fontWeight: 600 }}>
                            {passwordStrength < 50 ? "Weak" : passwordStrength < 75 ? "Good" : "Strong"}
                          </Text>
                        </div>
                        <Progress
                          percent={passwordStrength}
                          showInfo={false}
                          strokeColor={getStrengthColor()}
                          size="small"
                        />
                      </div>
                    )}
                  </Form.Item>

                  <Form.Item
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>Confirm Password</Text>}
                    required
                  >
                    <Input
                      type={showConfirmPassword ? "text" : "password"}
                      value={confirmPassword}
                      onChange={(e) => setConfirmPassword(e.target.value)}
                      prefix={<Lock size={20} style={{ color: "#a855f7" }} />}
                      suffix={
                        <button
                          type="button"
                          onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                          style={{
                            background: "none",
                            border: "none",
                            color: "rgba(255, 255, 255, 0.5)",
                            cursor: "pointer",
                            padding: "4px",
                          }}
                        >
                          {showConfirmPassword ? <EyeOff size={20} /> : <Eye size={20} />}
                        </button>
                      }
                      placeholder="Confirm your password"
                      style={{
                        height: "56px",
                        borderRadius: "16px",
                        background: "rgba(15, 23, 42, 0.6)",
                        border: `1px solid ${
                          confirmPassword && password !== confirmPassword ? "#ef4444" : "rgba(99, 102, 241, 0.3)"
                        }`,
                        color: "#fff",
                        fontSize: "1rem",
                      }}
                    />
                    {confirmPassword && password !== confirmPassword && (
                      <Text style={{ color: "#ef4444", fontSize: "0.9rem", marginTop: "4px" }}>
                        Passwords don't match
                      </Text>
                    )}
                  </Form.Item>
                </motion.div>
              )}

              {/* Step 3: Resume Upload */}
              {currentStep === 2 && (
                <motion.div
                  key="step3"
                  initial={{ opacity: 0, x: 50 }}
                  animate={{ opacity: 1, x: 0 }}
                  exit={{ opacity: 0, x: -50 }}
                  transition={{ duration: 0.3 }}
                >
                  <Form.Item
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>Upload Resume</Text>}
                  >
                    <motion.div whileHover={{ scale: 1.02 }} transition={{ duration: 0.2 }}>
                      <Upload.Dragger
                        beforeUpload={(file: File) => {
                          setFile(file)
                          return false
                        }}
                        showUploadList={false}
                        style={{
                          background: "rgba(15, 23, 42, 0.6)",
                          border: "1px dashed rgba(99, 102, 241, 0.3)",
                          borderRadius: "16px",
                          padding: "30px",
                        }}
                      >
                        <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                          {file ? (
                            <motion.div
                              initial={{ scale: 0.8 }}
                              animate={{ scale: 1 }}
                              style={{
                                width: "80px",
                                height: "80px",
                                borderRadius: "20px",
                                background: "rgba(16, 185, 129, 0.2)",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                marginBottom: "16px",
                                position: "relative",
                              }}
                            >
                              <FileText size={40} style={{ color: "#10b981" }} />
                              <div
                                style={{
                                  position: "absolute",
                                  top: "-8px",
                                  right: "-8px",
                                  width: "24px",
                                  height: "24px",
                                  borderRadius: "50%",
                                  background: "#10b981",
                                  display: "flex",
                                  alignItems: "center",
                                  justifyContent: "center",
                                }}
                              >
                                <CheckCircle size={16} color="white" />
                              </div>
                            </motion.div>
                          ) : (
                            <div
                              style={{
                                width: "80px",
                                height: "80px",
                                borderRadius: "20px",
                                background: "rgba(99, 102, 241, 0.2)",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                marginBottom: "16px",
                              }}
                            >
                              <UploadCloud size={40} style={{ color: "#a855f7" }} />
                            </div>
                          )}

                          <Text
                            style={{
                              color: "#fff",
                              fontSize: "1.2rem",
                              fontWeight: 600,
                              marginBottom: "8px",
                            }}
                          >
                            {file ? file.name : "Drop your resume here"}
                          </Text>
                          <Text style={{ color: "rgba(255, 255, 255, 0.6)" }}>
                            {file ? "File ready for upload" : "PDF, DOCX, TXT formats supported"}
                          </Text>
                        </div>
                      </Upload.Dragger>
                    </motion.div>
                  </Form.Item>

                  <Form.Item>
                    <Checkbox
                      checked={agreedToTerms}
                      onChange={(e) => setAgreedToTerms(e.target.checked)}
                      style={{ color: "rgba(255, 255, 255, 0.7)" }}
                    >
                      <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>
                        I agree to the <a style={{ color: "#a855f7", fontWeight: 600 }}>Terms of Service</a> and{" "}
                        <a style={{ color: "#a855f7", fontWeight: 600 }}>Privacy Policy</a>
                      </Text>
                    </Checkbox>
                  </Form.Item>
                </motion.div>
              )}
            </AnimatePresence>

            {/* Navigation Buttons */}
            <div
              style={{
                display: "flex",
                gap: "16px",
                marginTop: "30px",
              }}
            >
              {currentStep > 0 && (
                <Button
                  onClick={prevStep}
                  style={{
                    height: "56px",
                    flex: 1,
                    borderRadius: "16px",
                    background: "rgba(255, 255, 255, 0.1)",
                    border: "1px solid rgba(255, 255, 255, 0.2)",
                    color: "#fff",
                    fontSize: "1rem",
                    fontWeight: 600,
                  }}
                >
                  Previous
                </Button>
              )}

              {currentStep < 2 ? (
                <Button
                  onClick={nextStep}
                  disabled={
                    (currentStep === 0 && (!username || !userEmail)) ||
                    (currentStep === 1 && (!password || !confirmPassword || password !== confirmPassword))
                  }
                  style={{
                    height: "56px",
                    flex: 1,
                    borderRadius: "16px",
                    background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                    border: "none",
                    color: "#fff",
                    fontSize: "1rem",
                    fontWeight: 600,
                  }}
                >
                  Next Step
                </Button>
              ) : (
                <motion.div style={{ flex: 1 }} whileHover={{ scale: 1.02 }} whileTap={{ scale: 0.98 }}>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={loading}
                    disabled={!file || !agreedToTerms}
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
                      boxShadow: "0 10px 30px rgba(168, 85, 247, 0.3)",
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
                        Creating your account...
                      </div>
                    ) : (
                      "Create Account"
                    )}
                  </Button>
                </motion.div>
              )}
            </div>

            <div style={{ textAlign: "center", marginTop: "24px" }}>
              <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>
                Already have an account?{" "}
                <motion.a
                  whileHover={{ scale: 1.05 }}
                  onClick={() => navigate("/login")}
                  style={{
                    color: "#a855f7",
                    fontWeight: 700,
                    cursor: "pointer",
                    textDecoration: "none",
                  }}
                >
                  Sign In
                </motion.a>
              </Text>
            </div>
          </Form>
        </Card>
      </motion.div>
    </div>
  )
}

export default RegisterForm
