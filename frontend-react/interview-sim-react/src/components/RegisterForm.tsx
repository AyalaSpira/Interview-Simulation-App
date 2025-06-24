"use client"

import type React from "react"
import { useState } from "react"
import { Form, Input, Upload, Button, message, Card, Typography, Progress, Checkbox } from "antd"
import { motion, AnimatePresence } from "framer-motion"
import { UploadCloud, User, Mail, Lock, FileText, CheckCircle, Eye, EyeOff, Shield, Target } from "lucide-react"
import { registerUser } from "../services/authService"
import { useNavigate } from "react-router-dom"

const { Title, Text } = Typography

type RegisterFormProps = {
  onRegisterSuccess: () => void | Promise<void>
}


const RegisterForm: React.FC<RegisterFormProps> = ({ onRegisterSuccess }) => {
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
type RegisterFormProps = {
  onRegisterSuccess: () => void
}
  const passwordStrength = getPasswordStrength(password)
  const getStrengthColor = () => {
    if (passwordStrength < 50) return "#ef4444"
    if (passwordStrength < 75) return "#f59e0b"
    return "#10b981"
  }

  const steps = [
    { title: "פרטים אישיים", icon: <User size={18} /> },
    { title: "אבטחה", icon: <Shield size={18} /> },
    { title: "העלאת קורות חיים", icon: <FileText size={18} /> },
  ]

  const handleRegister = async () => {
    if (!file || !username || !password || !userEmail) {
      message.error("אנא מלא את כל השדות והעלה קורות חיים.")
      return
    }

    if (password !== confirmPassword) {
      message.error("הסיסמאות אינן תואמות!")
      return
    }

    if (!agreedToTerms) {
      message.error("אנא אשר את תנאי השימוש.")
      return
    }

    setLoading(true)
    try {
      const response = await registerUser(username, userEmail, password, file)
      if (response.token) {
        console.log("עובר ל-login...");

        message.success("🎉 ברוך הבא ל-InterviewAI Pro! מעביר לעמוד ההתחברות...")
        setTimeout(async () => { // הוספת async כאן
  await onRegisterSuccess() // ו-await כאן
  navigate("/login")
          console.log("עובר ל-login...");

}, 1500)
      } else {
        message.error("ההרשמה נכשלה. אנא נסה שוב.")
      }
    } catch (error) {
      console.error("Registration failed. Error:", error)
      message.error("ההרשמה נכשלה. ראה קונסול לפרטים.")
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
        padding: "20px",
        position: "relative",
        overflow: "hidden",
      }}
    >
      {/* Animated Background */}
      <div style={{ position: "absolute", inset: 0 }}>
        {[...Array(10)].map((_, i) => (
          <motion.div
            key={i}
            style={{
              position: "absolute",
              width: Math.random() * 120 + 40,
              height: Math.random() * 120 + 40,
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
              x: [0, Math.random() * 40 - 20],
              y: [0, Math.random() * 40 - 20],
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
        style={{ width: "100%", maxWidth: "500px", position: "relative", zIndex: 2 }}
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
          {/* Header */}
          <div style={{ textAlign: "center", marginBottom: "30px" }}>
            <motion.div
              animate={{ rotate: 360 }}
              transition={{ duration: 20, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}
              style={{
                width: "70px",
                height: "70px",
                margin: "0 auto 15px",
                borderRadius: "18px",
                background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <Target size={35} color="white" />
            </motion.div>

            <Title
              level={2}
              style={{
                color: "#fff",
                marginBottom: "8px",
                fontWeight: 700,
                fontSize: "1.8rem",
                background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                backgroundClip: "text",
                WebkitBackgroundClip: "text",
                WebkitTextFillColor: "transparent",
              }}
            >
              הצטרף ל-InterviewAI Pro
            </Title>
            <Text
              style={{
                color: "rgba(255, 255, 255, 0.7)",
                fontSize: "1rem",
              }}
            >
              התחל את המסע שלך להצלחה בראיונות
            </Text>
          </div>


          {/* Progress Steps */}
          <div style={{ marginBottom: "30px" }}>
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                marginBottom: "15px",
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
                      width: "40px",
                      height: "40px",
                      borderRadius: "50%",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      color: "#fff",
                      marginBottom: "6px",
                      transition: "all 0.3s ease",
                    }}
                  >
                    {step.icon}
                  </motion.div>
                  <Text
                    style={{
                      color: index <= currentStep ? "#fff" : "rgba(255, 255, 255, 0.5)",
                      fontSize: "0.8rem",
                      fontWeight: 600,
                      textAlign: "center",
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
              style={{ marginBottom: "15px" }}
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
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>שם מלא</Text>}
                    required
                  >
                    <Input
                      value={username}
                      onChange={(e) => setUsername(e.target.value)}
                      prefix={<User size={18} style={{ color: "#a855f7" }} />}
                      placeholder="הכנס את השם המלא שלך"
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
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>כתובת אימייל</Text>}
                    required
                  >
                    <Input
                      value={userEmail}
                      onChange={(e) => setUserEmail(e.target.value)}
                      prefix={<Mail size={18} style={{ color: "#a855f7" }} />}
                      placeholder="הכנס את כתובת האימייל שלך"
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
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>סיסמא</Text>}
                    required
                  >
                    <Input
                      type={showPassword ? "text" : "password"}
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
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
                      placeholder="צור סיסמא חזקה"
                      style={{
                        height: "48px",
                        borderRadius: "12px",
                        background: "rgba(15, 23, 42, 0.6)",
                        border: "1px solid rgba(99, 102, 241, 0.3)",
                        color: "#fff",
                        fontSize: "1rem",
                      }}
                    />
                    {password && (
                      <div style={{ marginTop: "8px" }}>
                        <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "4px" }}>
                          <Text style={{ color: "rgba(255, 255, 255, 0.7)", fontSize: "0.9rem" }}>חוזק הסיסמא</Text>
                          <Text style={{ color: getStrengthColor(), fontSize: "0.9rem", fontWeight: 600 }}>
                            {passwordStrength < 50 ? "חלשה" : passwordStrength < 75 ? "טובה" : "חזקה"}
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
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>אישור סיסמא</Text>}
                    required
                  >
                    <Input
                      type={showConfirmPassword ? "text" : "password"}
                      value={confirmPassword}
                      onChange={(e) => setConfirmPassword(e.target.value)}
                      prefix={<Lock size={18} style={{ color: "#a855f7" }} />}
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
                          {showConfirmPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                        </button>
                      }
                      placeholder="אשר את הסיסמא שלך"
                      style={{
                        height: "48px",
                        borderRadius: "12px",
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
                        הסיסמאות אינן תואמות
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
                    label={<Text style={{ color: "rgba(255, 255, 255, 0.8)", fontWeight: 600 }}>העלאת קורות חיים</Text>}
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
                          borderRadius: "12px",
                          padding: "25px",
                        }}
                      >
                        <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                          {file ? (
                            <motion.div
                              initial={{ scale: 0.8 }}
                              animate={{ scale: 1 }}
                              style={{
                                width: "70px",
                                height: "70px",
                                borderRadius: "18px",
                                background: "rgba(16, 185, 129, 0.2)",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                marginBottom: "12px",
                                position: "relative",
                              }}
                            >
                              <FileText size={35} style={{ color: "#10b981" }} />
                              <div
                                style={{
                                  position: "absolute",
                                  top: "-6px",
                                  right: "-6px",
                                  width: "20px",
                                  height: "20px",
                                  borderRadius: "50%",
                                  background: "#10b981",
                                  display: "flex",
                                  alignItems: "center",
                                  justifyContent: "center",
                                }}
                              >
                                <CheckCircle size={12} color="white" />
                              </div>
                            </motion.div>
                          ) : (
                            <div
                              style={{
                                width: "70px",
                                height: "70px",
                                borderRadius: "18px",
                                background: "rgba(99, 102, 241, 0.2)",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                marginBottom: "12px",
                              }}
                            >
                              <UploadCloud size={35} style={{ color: "#a855f7" }} />
                            </div>
                          )}

                          <Text
                            style={{
                              color: "#fff",
                              fontSize: "1.1rem",
                              fontWeight: 600,
                              marginBottom: "6px",
                            }}
                          >
                            {file ? file.name : "גרור את קורות החיים כאן"}
                          </Text>
                          <Text style={{ color: "rgba(255, 255, 255, 0.6)" }}>
                            {file ? "הקובץ מוכן להעלאה" : "תומך בפורמטים PDF, DOCX, TXT"}
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
                        אני מסכים ל<a style={{ color: "#a855f7", fontWeight: 600 }}>תנאי השימוש</a> ול
                        <a style={{ color: "#a855f7", fontWeight: 600 }}>מדיניות הפרטיות</a>
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
                gap: "12px",
                marginTop: "25px",
              }}
            >
              {currentStep > 0 && (
                <Button
                  onClick={prevStep}
                  style={{
                    height: "48px",
                    flex: 1,
                    borderRadius: "12px",
                    background: "rgba(255, 255, 255, 0.1)",
                    border: "1px solid rgba(255, 255, 255, 0.2)",
                    color: "#fff",
                    fontSize: "1rem",
                    fontWeight: 600,
                  }}
                >
                  הקודם
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
                    height: "48px",
                    flex: 1,
                    borderRadius: "12px",
                    background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                    border: "none",
                    color: "#fff",
                    fontSize: "1rem",
                    fontWeight: 600,
                  }}
                >
                  הבא
                </Button>
              ) : (
                <motion.div style={{ flex: 1 }} whileHover={{ scale: 1.02 }} whileTap={{ scale: 0.98 }}>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={loading}
                    disabled={!file || !agreedToTerms}
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
                      boxShadow: "0 10px 30px rgba(168, 85, 247, 0.3)",
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
                        יוצר את החשבון שלך...
                      </div>
                    ) : (
                      "צור חשבון"
                    )}
                  </Button>
                </motion.div>
              )}
            </div>

            <div style={{ textAlign: "center", marginTop: "20px" }}>
              <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>
                כבר יש לך חשבון?{" "}
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
                  התחבר
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
