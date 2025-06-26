"use client"

import type React from "react"
import { useState, useEffect } from "react"
import { motion, AnimatePresence } from "framer-motion"
import {
  Play,
  Users,
  BarChart3,
  Star,
  ArrowRight,
  Brain,
  Target,
  Award,
  Sparkles,
  TrendingUp,
  Shield,
  Clock,
  Globe,
} from "lucide-react"
import { Button, Card, Typography, Avatar } from "antd"
import { useNavigate } from "react-router-dom"

const { Title, Text } = Typography

const LandingPage: React.FC = () => {
  const navigate = useNavigate()
  const [currentTestimonial, setCurrentTestimonial] = useState(0)
  const [isVisible, setIsVisible] = useState(false)

  useEffect(() => {
    setIsVisible(true)
    const interval = setInterval(() => {
      setCurrentTestimonial((prev) => (prev + 1) % testimonials.length)
    }, 4000)
    return () => clearInterval(interval)
  }, [])

  const features = [
    {
      icon: <Brain size={32} />,
      title: "AI-Powered Questions",
      description: "Smart questions tailored to your resume and industry",
      color: "#a855f7",
    },
    {
      icon: <Target size={32} />,
      title: "Real-time Feedback",
      description: "Instant analysis and improvement suggestions",
      color: "#3b82f6",
    },
    {
      icon: <Award size={32} />,
      title: "Performance Tracking",
      description: "Track your progress and skill development",
      color: "#10b981",
    },
    {
      icon: <Shield size={32} />,
      title: "Industry Standards",
      description: "Questions based on real interview scenarios",
      color: "#f59e0b",
    },
  ]

  const testimonials = [
    {
      name: "Sarah Chen",
      role: "Software Engineer at Google",
      avatar: "SC",
      text: "This platform helped me land my dream job! The AI feedback was incredibly accurate.",
      rating: 5,
    },
    {
      name: "Michael Rodriguez",
      role: "Product Manager at Meta",
      avatar: "MR",
      text: "The most realistic interview practice I've ever experienced. Highly recommended!",
      rating: 5,
    },
    {
      name: "Emily Johnson",
      role: "Data Scientist at Netflix",
      avatar: "EJ",
      text: "Improved my confidence dramatically. The personalized questions were spot-on.",
      rating: 5,
    },
  ]

  const stats = [
    { number: "50K+", label: "Successful Interviews", icon: <Users size={24} /> },
    { number: "95%", label: "Success Rate", icon: <TrendingUp size={24} /> },
    { number: "500+", label: "Companies", icon: <Globe size={24} /> },
    { number: "24/7", label: "Available", icon: <Clock size={24} /> },
  ]

  return (
    <div
      style={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #0f172a 0%, #1e293b 50%, #0f172a 100%)",
        overflow: "hidden",
        position: "relative",
      }}
    >
      {/* Animated Background Elements */}
      <div style={{ position: "absolute", inset: 0, overflow: "hidden" }}>
        {[...Array(20)].map((_, i) => (
          <motion.div
            key={i}
            style={{
              position: "absolute",
              width: Math.random() * 300 + 100,
              height: Math.random() * 300 + 100,
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
              duration: Math.random() * 10 + 10,
              repeat: Number.POSITIVE_INFINITY,
              repeatType: "reverse",
            }}
          />
        ))}
      </div>

      {/* Navigation */}
      <motion.nav
        initial={{ y: -100, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        transition={{ duration: 0.8 }}
        style={{
          position: "relative",
          zIndex: 10,
          padding: "20px 40px",
          background: "rgba(15, 23, 42, 0.8)",
          backdropFilter: "blur(20px)",
          borderBottom: "1px solid rgba(255, 255, 255, 0.1)",
        }}
      >
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            maxWidth: "1200px",
            margin: "0 auto",
          }}
        >
          <motion.div whileHover={{ scale: 1.05 }} style={{ display: "flex", alignItems: "center", gap: "12px" }}>
            <div
              style={{
                width: "40px",
                height: "40px",
                borderRadius: "12px",
                background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <Sparkles size={24} color="white" />
            </div>
            <Title
              level={3}
              style={{
                margin: 0,
                color: "#fff",
                background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                backgroundClip: "text",
                WebkitBackgroundClip: "text",
                WebkitTextFillColor: "transparent",
              }}
            >
              InterviewAI Pro
            </Title>
          </motion.div>

          <div style={{ display: "flex", gap: "16px" }}>
            <Button
              onClick={() => navigate("/login")}
              style={{
                height: "44px",
                borderRadius: "12px",
                background: "transparent",
                border: "1px solid rgba(255, 255, 255, 0.2)",
                color: "#fff",
                fontWeight: 600,
              }}
            >
              Login
            </Button>
            <Button
              onClick={() => navigate("/register")}
              style={{
                height: "44px",
                borderRadius: "12px",
                background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                border: "none",
                color: "#fff",
                fontWeight: 600,
              }}
            >
              Get Started
            </Button>
          </div>
        </div>
      </motion.nav>

      {/* Hero Section */}
      <div
        style={{
          position: "relative",
          zIndex: 5,
          padding: "80px 40px",
          textAlign: "center",
          maxWidth: "1200px",
          margin: "0 auto",
        }}
      >
        <motion.div
          initial={{ opacity: 0, y: 50 }}
          animate={{ opacity: isVisible ? 1 : 0, y: isVisible ? 0 : 50 }}
          transition={{ duration: 1, delay: 0.2 }}
        >
          <div style={{ marginBottom: "24px" }}>
            <motion.div
              animate={{ rotate: 360 }}
              transition={{ duration: 20, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}
              style={{
                width: "80px",
                height: "80px",
                margin: "0 auto 20px",
                borderRadius: "50%",
                background: "linear-gradient(135deg, #a855f7, #3b82f6, #10b981)",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <Brain size={40} color="white" />
            </motion.div>
          </div>

          <Title
            level={1}
            style={{
              fontSize: "4rem",
              fontWeight: 800,
              margin: "0 0 24px 0",
              background: "linear-gradient(135deg, #fff 0%, #a855f7 50%, #3b82f6 100%)",
              backgroundClip: "text",
              WebkitBackgroundClip: "text",
              WebkitTextFillColor: "transparent",
              lineHeight: 1.2,
            }}
          >
            Master Your Next
            <br />
            <span
              style={{
                background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                backgroundClip: "text",
                WebkitBackgroundClip: "text",
                WebkitTextFillColor: "transparent",
              }}
            >
              Job Interview
            </span>
          </Title>

          <Text
            style={{
              fontSize: "1.5rem",
              color: "rgba(255, 255, 255, 0.8)",
              marginBottom: "48px",
              display: "block",
              maxWidth: "600px",
              margin: "0 auto 48px auto",
              lineHeight: 1.6,
            }}
          >
            AI-powered interview simulation that adapts to your resume and provides real-time feedback to boost your
            confidence
          </Text>

          <div style={{ display: "flex", gap: "20px", justifyContent: "center", flexWrap: "wrap" }}>
            <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
              <Button
                onClick={() => navigate("/register")}
                size="large"
                style={{
                  height: "60px",
                  padding: "0 40px",
                  borderRadius: "16px",
                  background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                  border: "none",
                  fontSize: "1.2rem",
                  fontWeight: 700,
                  color: "#fff",
                  display: "flex",
                  alignItems: "center",
                  gap: "12px",
                  boxShadow: "0 10px 30px rgba(168, 85, 247, 0.4)",
                }}
                icon={<Play size={24} />}
              >
                Start Free Trial
              </Button>
            </motion.div>

            <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
              <Button
                size="large"
                style={{
                  height: "60px",
                  padding: "0 40px",
                  borderRadius: "16px",
                  background: "rgba(255, 255, 255, 0.1)",
                  border: "1px solid rgba(255, 255, 255, 0.2)",
                  fontSize: "1.2rem",
                  fontWeight: 600,
                  color: "#fff",
                  display: "flex",
                  alignItems: "center",
                  gap: "12px",
                  backdropFilter: "blur(10px)",
                }}
                icon={<BarChart3 size={24} />}
              >
                View Demo
              </Button>
            </motion.div>
          </div>
        </motion.div>

        {/* Stats Section */}
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8, delay: 0.6 }}
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))",
            gap: "24px",
            marginTop: "80px",
            padding: "40px",
            background: "rgba(30, 41, 59, 0.3)",
            borderRadius: "24px",
            backdropFilter: "blur(20px)",
            border: "1px solid rgba(255, 255, 255, 0.1)",
          }}
        >
          {stats.map((stat, index) => (
            <motion.div
              key={index}
              initial={{ opacity: 0, scale: 0.8 }}
              animate={{ opacity: 1, scale: 1 }}
              transition={{ duration: 0.5, delay: 0.8 + index * 0.1 }}
              style={{ textAlign: "center" }}
            >
              <div
                style={{
                  width: "60px",
                  height: "60px",
                  margin: "0 auto 16px",
                  borderRadius: "50%",
                  background: `linear-gradient(135deg, ${
                    index % 2 === 0 ? "#a855f7" : "#3b82f6"
                  }, ${index % 2 === 0 ? "#3b82f6" : "#10b981"})`,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                }}
              >
                {stat.icon}
              </div>
              <Title
                level={2}
                style={{
                  color: "#fff",
                  margin: "0 0 8px 0",
                  fontSize: "2.5rem",
                  fontWeight: 800,
                }}
              >
                {stat.number}
              </Title>
              <Text style={{ color: "rgba(255, 255, 255, 0.7)", fontSize: "1.1rem" }}>{stat.label}</Text>
            </motion.div>
          ))}
        </motion.div>
      </div>

      {/* Features Section */}
      <div
        style={{
          position: "relative",
          zIndex: 5,
          padding: "80px 40px",
          maxWidth: "1200px",
          margin: "0 auto",
        }}
      >
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8 }}
          style={{ textAlign: "center", marginBottom: "60px" }}
        >
          <Title
            level={2}
            style={{
              fontSize: "3rem",
              color: "#fff",
              marginBottom: "20px",
              fontWeight: 700,
            }}
          >
            Why Choose InterviewAI Pro?
          </Title>
          <Text
            style={{
              fontSize: "1.3rem",
              color: "rgba(255, 255, 255, 0.7)",
              maxWidth: "600px",
              margin: "0 auto",
              display: "block",
            }}
          >
            Advanced AI technology meets proven interview techniques
          </Text>
        </motion.div>

        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(auto-fit, minmax(280px, 1fr))",
            gap: "30px",
          }}
        >
          {features.map((feature, index) => (
            <motion.div
              key={index}
              initial={{ opacity: 0, y: 30 }}
              whileInView={{ opacity: 1, y: 0 }}
              whileHover={{ y: -10, scale: 1.02 }}
              transition={{ duration: 0.5, delay: index * 0.1 }}
            >
              <Card
                style={{
                  height: "100%",
                  background: "rgba(30, 41, 59, 0.4)",
                  borderRadius: "20px",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  backdropFilter: "blur(20px)",
                  padding: "30px",
                  textAlign: "center",
                }}
              >
                <div
                  style={{
                    width: "80px",
                    height: "80px",
                    margin: "0 auto 24px",
                    borderRadius: "20px",
                    background: `linear-gradient(135deg, ${feature.color}20, ${feature.color}40)`,
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    color: feature.color,
                  }}
                >
                  {feature.icon}
                </div>
                <Title level={4} style={{ color: "#fff", marginBottom: "16px" }}>
                  {feature.title}
                </Title>
                <Text style={{ color: "rgba(255, 255, 255, 0.7)", fontSize: "1.1rem" }}>{feature.description}</Text>
              </Card>
            </motion.div>
          ))}
        </div>
      </div>

      {/* Testimonials Section */}
      <div
        style={{
          position: "relative",
          zIndex: 5,
          padding: "80px 40px",
          maxWidth: "800px",
          margin: "0 auto",
        }}
      >
        <motion.div
          initial={{ opacity: 0 }}
          whileInView={{ opacity: 1 }}
          transition={{ duration: 0.8 }}
          style={{ textAlign: "center" }}
        >
          <Title
            level={2}
            style={{
              fontSize: "3rem",
              color: "#fff",
              marginBottom: "60px",
              fontWeight: 700,
            }}
          >
            Success Stories
          </Title>

          <AnimatePresence mode="wait">
            <motion.div
              key={currentTestimonial}
              initial={{ opacity: 0, x: 50 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -50 }}
              transition={{ duration: 0.5 }}
            >
              <Card
                style={{
                  background: "rgba(30, 41, 59, 0.4)",
                  borderRadius: "24px",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  backdropFilter: "blur(20px)",
                  padding: "40px",
                  textAlign: "center",
                }}
              >
                <div style={{ marginBottom: "24px" }}>
                  {[...Array(testimonials[currentTestimonial].rating)].map((_, i) => (
                    <Star key={i} size={24} fill="#f59e0b" color="#f59e0b" style={{ margin: "0 2px" }} />
                  ))}
                </div>

                <Text
                  style={{
                    fontSize: "1.4rem",
                    color: "#fff",
                    marginBottom: "30px",
                    display: "block",
                    fontStyle: "italic",
                    lineHeight: 1.6,
                  }}
                >
                  "{testimonials[currentTestimonial].text}"
                </Text>

                <div style={{ display: "flex", alignItems: "center", justifyContent: "center", gap: "16px" }}>
                  <Avatar
                    size={60}
                    style={{
                      background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                      fontSize: "1.2rem",
                      fontWeight: 600,
                    }}
                  >
                    {testimonials[currentTestimonial].avatar}
                  </Avatar>
                  <div style={{ textAlign: "left" }}>
                    <Text style={{ color: "#fff", fontSize: "1.2rem", fontWeight: 600, display: "block" }}>
                      {testimonials[currentTestimonial].name}
                    </Text>
                    <Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>{testimonials[currentTestimonial].role}</Text>
                  </div>
                </div>
              </Card>
            </motion.div>
          </AnimatePresence>

          <div style={{ display: "flex", justifyContent: "center", gap: "12px", marginTop: "30px" }}>
            {testimonials.map((_, index) => (
              <button
                key={index}
                onClick={() => setCurrentTestimonial(index)}
                style={{
                  width: "12px",
                  height: "12px",
                  borderRadius: "50%",
                  border: "none",
                  background: index === currentTestimonial ? "#a855f7" : "rgba(255, 255, 255, 0.3)",
                  cursor: "pointer",
                  transition: "all 0.3s ease",
                }}
              />
            ))}
          </div>
        </motion.div>
      </div>

      {/* CTA Section */}
      <div
        style={{
          position: "relative",
          zIndex: 5,
          padding: "80px 40px",
          textAlign: "center",
        }}
      >
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8 }}
          style={{
            maxWidth: "600px",
            margin: "0 auto",
            padding: "60px 40px",
            background: "linear-gradient(135deg, rgba(168, 85, 247, 0.2), rgba(59, 130, 246, 0.2))",
            borderRadius: "24px",
            border: "1px solid rgba(255, 255, 255, 0.1)",
            backdropFilter: "blur(20px)",
          }}
        >
          <Title
            level={2}
            style={{
              fontSize: "2.5rem",
              color: "#fff",
              marginBottom: "20px",
              fontWeight: 700,
            }}
          >
            Ready to Ace Your Interview?
          </Title>

          <Text
            style={{
              fontSize: "1.3rem",
              color: "rgba(255, 255, 255, 0.8)",
              marginBottom: "40px",
              display: "block",
            }}
          >
            Join thousands of successful candidates who improved their interview skills with our AI platform
          </Text>

          <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
            <Button
              onClick={() => navigate("/register")}
              size="large"
              style={{
                height: "60px",
                padding: "0 50px",
                borderRadius: "16px",
                background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                border: "none",
                fontSize: "1.3rem",
                fontWeight: 700,
                color: "#fff",
                display: "flex",
                alignItems: "center",
                gap: "12px",
                margin: "0 auto",
                boxShadow: "0 15px 40px rgba(168, 85, 247, 0.4)",
              }}
              icon={<ArrowRight size={24} />}
            >
              Start Your Journey Now
            </Button>
          </motion.div>
        </motion.div>
      </div>
    </div>
  )
}

export default LandingPage
