"use client"

import type React from "react"
import { Link } from "react-router-dom"
import { Container, Typography, Button, Box, Grid, Card, CardContent, Avatar, LinearProgress } from "@mui/material"
import { motion, AnimatePresence } from "framer-motion"
import {
  FileText,
  Play,
  BarChart2,
  Award,
  Clock,
  TrendingUp,
  CheckCircle,
  Calendar,
  Target,
  Star,
  ArrowRight,
  Brain,
  Sparkles,
} from "lucide-react"
import { useState, useEffect } from "react"

interface HomeProps {
  onLogout: () => void
}

const Home: React.FC<HomeProps> = ({ onLogout }) => {
  const [currentQuote, setCurrentQuote] = useState(0)
  const [isVisible, setIsVisible] = useState(false)

  const motivationalQuotes = [
    "Success is where preparation and opportunity meet.",
    "The expert in anything was once a beginner.",
    "Your only limit is your mind.",
    "Great things never come from comfort zones.",
  ]

  useEffect(() => {
    setIsVisible(true)
    const interval = setInterval(() => {
      setCurrentQuote((prev) => (prev + 1) % motivationalQuotes.length)
    }, 4000)
    return () => clearInterval(interval)
  }, [])

  // Enhanced mock data
  const stats = {
    totalInterviews: 24,
    completedInterviews: 18,
    averageScore: 85,
    improvementRate: 23,
    skillsToImprove: [
      { skill: "Communication", progress: 78, color: "#a855f7" },
      { skill: "Technical Knowledge", progress: 65, color: "#3b82f6" },
      { skill: "Problem Solving", progress: 82, color: "#10b981" },
    ],
    upcomingInterviews: [
      { id: 1, title: "Senior Frontend Developer", company: "TechCorp", date: "Tomorrow, 10:00 AM", type: "Technical" },
      { id: 2, title: "React Specialist", company: "StartupXYZ", date: "Dec 28, 2:30 PM", type: "Behavioral" },
    ],
    recentActivity: [
      {
        id: 1,
        action: "Completed Technical Interview",
        time: "2 hours ago",
        score: 92,
        icon: <CheckCircle size={16} />,
      },
      { id: 2, action: "Updated Resume", time: "Yesterday", icon: <FileText size={16} /> },
      { id: 3, action: "Received AI Feedback", time: "2 days ago", rating: 4.8, icon: <Brain size={16} /> },
    ],
    achievements: [
      { title: "Interview Master", description: "Completed 10+ interviews", icon: <Award size={24} />, unlocked: true },
      { title: "Quick Learner", description: "Improved score by 20%", icon: <TrendingUp size={24} />, unlocked: true },
      { title: "Consistency King", description: "7-day streak", icon: <Target size={24} />, unlocked: false },
    ],
  }

  const cardVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: (i: number) => ({
      opacity: 1,
      y: 0,
      transition: {
        delay: i * 0.1,
        duration: 0.5,
      },
    }),
  }

  const quickActions = [
    {
      title: "Start Practice",
      description: "Begin a new interview session",
      icon: <Play size={24} />,
      color: "#a855f7",
      path: "/interview",
      gradient: "linear-gradient(135deg, #a855f7, #8b5cf6)",
    },
    {
      title: "Upload Resume",
      description: "Update your resume for better questions",
      icon: <FileText size={24} />,
      color: "#3b82f6",
      path: "/upload-resume",
      gradient: "linear-gradient(135deg, #3b82f6, #2563eb)",
    },
    {
      title: "View Analytics",
      description: "Check your performance metrics",
      icon: <BarChart2 size={24} />,
      color: "#10b981",
      path: "/report",
      gradient: "linear-gradient(135deg, #10b981, #059669)",
    },
  ]

  return (
    <Box
      sx={{
        height: "100%",
        overflow: "auto",
        py: 4,
        px: { xs: 2, md: 4 },
        position: "relative",
      }}
    >
      {/* Floating Background Elements */}
      <div style={{ position: "absolute", inset: 0, overflow: "hidden", pointerEvents: "none" }}>
        {[...Array(8)].map((_, i) => (
          <motion.div
            key={i}
            style={{
              position: "absolute",
              width: Math.random() * 100 + 50,
              height: Math.random() * 100 + 50,
              borderRadius: "50%",
              background: `radial-gradient(circle, ${
                i % 3 === 0
                  ? "rgba(168, 85, 247, 0.05)"
                  : i % 3 === 1
                    ? "rgba(59, 130, 246, 0.05)"
                    : "rgba(16, 185, 129, 0.05)"
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

      <Container maxWidth="xl" sx={{ height: "100%", position: "relative", zIndex: 1 }}>
        <Grid container spacing={3}>
          {/* Welcome Header */}
          <Grid item xs={12}>
            <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.6 }}>
              <Card
                sx={{
                  borderRadius: "24px",
                  overflow: "hidden",
                  boxShadow: "0 25px 100px rgba(0, 0, 0, 0.3)",
                  background: "rgba(30, 41, 59, 0.8)",
                  backdropFilter: "blur(20px)",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  p: 4,
                  position: "relative",
                }}
              >
                <Box sx={{ position: "relative", zIndex: 2 }}>
                  <Box sx={{ display: "flex", alignItems: "center", justifyContent: "space-between", mb: 3 }}>
                    <Box>
                      <Typography
                        variant="h3"
                        sx={{
                          fontWeight: 800,
                          mb: 1,
                          background: "linear-gradient(135deg, #fff 0%, #a855f7 50%, #3b82f6 100%)",
                          backgroundClip: "text",
                          textFillColor: "transparent",
                          WebkitBackgroundClip: "text",
                          WebkitTextFillColor: "transparent",
                        }}
                      >
                        Welcome Back! 👋
                      </Typography>
                      <Typography
                        variant="body1"
                        sx={{
                          color: "rgba(255, 255, 255, 0.7)",
                          fontSize: "1.2rem",
                          maxWidth: "600px",
                        }}
                      >
                        Ready to ace your next interview? Let's continue building your confidence.
                      </Typography>
                    </Box>

                    <motion.div
                      animate={{ rotate: 360 }}
                      transition={{ duration: 20, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}
                      style={{
                        width: "80px",
                        height: "80px",
                        borderRadius: "20px",
                        background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                      }}
                    >
                      <Sparkles size={40} color="white" />
                    </motion.div>
                  </Box>

                  {/* Motivational Quote */}
                  <Box
                    sx={{
                      p: 3,
                      borderRadius: "16px",
                      background: "rgba(15, 23, 42, 0.6)",
                      border: "1px solid rgba(99, 102, 241, 0.2)",
                      mb: 3,
                      textAlign: "center",
                    }}
                  >
                    <AnimatePresence mode="wait">
                      <motion.div
                        key={currentQuote}
                        initial={{ opacity: 0, y: 10 }}
                        animate={{ opacity: 1, y: 0 }}
                        exit={{ opacity: 0, y: -10 }}
                        transition={{ duration: 0.5 }}
                      >
                        <Typography
                          sx={{
                            color: "#fff",
                            fontSize: "1.1rem",
                            fontStyle: "italic",
                            fontWeight: 500,
                          }}
                        >
                          "{motivationalQuotes[currentQuote]}"
                        </Typography>
                      </motion.div>
                    </AnimatePresence>
                  </Box>

                  {/* Quick Actions */}
                  <Grid container spacing={2}>
                    {quickActions.map((action, index) => (
                      <Grid item xs={12} sm={4} key={index}>
                        <motion.div
                          whileHover={{ scale: 1.05, y: -5 }}
                          whileTap={{ scale: 0.95 }}
                          transition={{ duration: 0.2 }}
                        >
                          <Button
                            component={Link}
                            to={action.path}
                            fullWidth
                            sx={{
                              p: 2.5,
                              borderRadius: "16px",
                              background: action.gradient,
                              border: "none",
                              textTransform: "none",
                              color: "#fff",
                              display: "flex",
                              flexDirection: "column",
                              gap: 1,
                              height: "100px",
                              boxShadow: `0 10px 30px ${action.color}40`,
                              "&:hover": {
                                boxShadow: `0 15px 40px ${action.color}60`,
                              },
                            }}
                          >
                            {action.icon}
                            <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                              {action.title}
                            </Typography>
                            <Typography variant="caption" sx={{ opacity: 0.9 }}>
                              {action.description}
                            </Typography>
                          </Button>
                        </motion.div>
                      </Grid>
                    ))}
                  </Grid>
                </Box>

                {/* Background decorative elements */}
                <Box
                  sx={{
                    position: "absolute",
                    top: -50,
                    right: -50,
                    width: 200,
                    height: 200,
                    borderRadius: "50%",
                    background: "radial-gradient(circle, rgba(168, 85, 247, 0.1) 0%, transparent 70%)",
                    zIndex: 1,
                  }}
                />
              </Card>
            </motion.div>
          </Grid>

          {/* Performance Overview */}
          <Grid item xs={12} lg={8}>
            <motion.div custom={0} variants={cardVariants} initial="hidden" animate="visible">
              <Card
                sx={{
                  borderRadius: "24px",
                  overflow: "hidden",
                  boxShadow: "0 15px 60px rgba(0, 0, 0, 0.2)",
                  background: "rgba(30, 41, 59, 0.8)",
                  backdropFilter: "blur(20px)",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  height: "100%",
                }}
              >
                <CardContent sx={{ p: 4 }}>
                  <Box sx={{ display: "flex", alignItems: "center", mb: 3 }}>
                    <Avatar
                      sx={{
                        background: "rgba(168, 85, 247, 0.2)",
                        color: "#a855f7",
                        width: 56,
                        height: 56,
                        mr: 2,
                      }}
                    >
                      <BarChart2 size={28} />
                    </Avatar>
                    <Box>
                      <Typography variant="h5" sx={{ fontWeight: 700, color: "#fff", mb: 0.5 }}>
                        Performance Overview
                      </Typography>
                      <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>
                        Track your interview progress and improvements
                      </Typography>
                    </Box>
                  </Box>

                  {/* Stats Grid */}
                  <Grid container spacing={3} sx={{ mb: 4 }}>
                    <Grid item xs={6} md={3}>
                      <Box sx={{ textAlign: "center" }}>
                        <Typography
                          variant="h3"
                          sx={{
                            color: "#fff",
                            fontWeight: 800,
                            background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                            backgroundClip: "text",
                            WebkitBackgroundClip: "text",
                            WebkitTextFillColor: "transparent",
                          }}
                        >
                          {stats.totalInterviews}
                        </Typography>
                        <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>Total Interviews</Typography>
                      </Box>
                    </Grid>
                    <Grid item xs={6} md={3}>
                      <Box sx={{ textAlign: "center" }}>
                        <Typography
                          variant="h3"
                          sx={{
                            color: "#fff",
                            fontWeight: 800,
                            background: "linear-gradient(135deg, #10b981, #059669)",
                            backgroundClip: "text",
                            WebkitBackgroundClip: "text",
                            WebkitTextFillColor: "transparent",
                          }}
                        >
                          {stats.averageScore}%
                        </Typography>
                        <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>Average Score</Typography>
                      </Box>
                    </Grid>
                    <Grid item xs={6} md={3}>
                      <Box sx={{ textAlign: "center" }}>
                        <Typography
                          variant="h3"
                          sx={{
                            color: "#fff",
                            fontWeight: 800,
                            background: "linear-gradient(135deg, #f59e0b, #d97706)",
                            backgroundClip: "text",
                            WebkitBackgroundClip: "text",
                            WebkitTextFillColor: "transparent",
                          }}
                        >
                          +{stats.improvementRate}%
                        </Typography>
                        <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>Improvement</Typography>
                      </Box>
                    </Grid>
                    <Grid item xs={6} md={3}>
                      <Box sx={{ textAlign: "center" }}>
                        <Typography
                          variant="h3"
                          sx={{
                            color: "#fff",
                            fontWeight: 800,
                            background: "linear-gradient(135deg, #ef4444, #dc2626)",
                            backgroundClip: "text",
                            WebkitBackgroundClip: "text",
                            WebkitTextFillColor: "transparent",
                          }}
                        >
                          {stats.completedInterviews}
                        </Typography>
                        <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>Completed</Typography>
                      </Box>
                    </Grid>
                  </Grid>

                  {/* Skills Progress */}
                  <Typography variant="h6" sx={{ color: "#fff", mb: 2, fontWeight: 600 }}>
                    Skills Development
                  </Typography>
                  {stats.skillsToImprove.map((skill, index) => (
                    <Box key={index} sx={{ mb: 3 }}>
                      <Box sx={{ display: "flex", justifyContent: "space-between", mb: 1 }}>
                        <Typography sx={{ color: "rgba(255, 255, 255, 0.9)", fontWeight: 500 }}>
                          {skill.skill}
                        </Typography>
                        <Typography sx={{ color: skill.color, fontWeight: 600 }}>{skill.progress}%</Typography>
                      </Box>
                      <LinearProgress
                        variant="determinate"
                        value={skill.progress}
                        sx={{
                          height: 8,
                          borderRadius: 4,
                          backgroundColor: "rgba(255, 255, 255, 0.1)",
                          "& .MuiLinearProgress-bar": {
                            background: `linear-gradient(90deg, ${skill.color}, ${skill.color}dd)`,
                            borderRadius: 4,
                          },
                        }}
                      />
                    </Box>
                  ))}
                </CardContent>
              </Card>
            </motion.div>
          </Grid>

          {/* Upcoming Interviews */}
          <Grid item xs={12} lg={4}>
            <motion.div custom={1} variants={cardVariants} initial="hidden" animate="visible">
              <Card
                sx={{
                  borderRadius: "24px",
                  overflow: "hidden",
                  boxShadow: "0 15px 60px rgba(0, 0, 0, 0.2)",
                  background: "rgba(30, 41, 59, 0.8)",
                  backdropFilter: "blur(20px)",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  height: "100%",
                }}
              >
                <CardContent sx={{ p: 4 }}>
                  <Box sx={{ display: "flex", alignItems: "center", mb: 3 }}>
                    <Avatar
                      sx={{
                        background: "rgba(59, 130, 246, 0.2)",
                        color: "#3b82f6",
                        width: 56,
                        height: 56,
                        mr: 2,
                      }}
                    >
                      <Calendar size={28} />
                    </Avatar>
                    <Box>
                      <Typography variant="h6" sx={{ fontWeight: 700, color: "#fff" }}>
                        Upcoming Sessions
                      </Typography>
                      <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>Your scheduled practice</Typography>
                    </Box>
                  </Box>

                  {stats.upcomingInterviews.length > 0 ? (
                    stats.upcomingInterviews.map((interview, index) => (
                      <motion.div
                        key={interview.id}
                        initial={{ opacity: 0, x: -20 }}
                        animate={{ opacity: 1, x: 0 }}
                        transition={{ delay: index * 0.1 }}
                      >
                        <Box
                          sx={{
                            p: 3,
                            mb: 2,
                            borderRadius: "16px",
                            background: "rgba(15, 23, 42, 0.6)",
                            border: "1px solid rgba(99, 102, 241, 0.2)",
                            position: "relative",
                            overflow: "hidden",
                          }}
                        >
                          <Box
                            sx={{
                              position: "absolute",
                              top: 0,
                              left: 0,
                              width: "4px",
                              height: "100%",
                              background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                            }}
                          />

                          <Typography
                            variant="subtitle1"
                            sx={{
                              fontWeight: 700,
                              color: "#fff",
                              mb: 0.5,
                            }}
                          >
                            {interview.title}
                          </Typography>
                          <Typography
                            sx={{
                              color: "rgba(255, 255, 255, 0.6)",
                              fontSize: "0.9rem",
                              mb: 1,
                            }}
                          >
                            {interview.company}
                          </Typography>
                          <Box sx={{ display: "flex", alignItems: "center", justifyContent: "space-between" }}>
                            <Box sx={{ display: "flex", alignItems: "center" }}>
                              <Clock size={14} style={{ color: "#a855f7", marginRight: "6px" }} />
                              <Typography sx={{ color: "rgba(255, 255, 255, 0.7)", fontSize: "0.9rem" }}>
                                {interview.date}
                              </Typography>
                            </Box>
                            <Box
                              sx={{
                                px: 2,
                                py: 0.5,
                                borderRadius: "8px",
                                background: "rgba(168, 85, 247, 0.2)",
                                border: "1px solid rgba(168, 85, 247, 0.3)",
                              }}
                            >
                              <Typography
                                sx={{
                                  color: "#a855f7",
                                  fontSize: "0.8rem",
                                  fontWeight: 600,
                                }}
                              >
                                {interview.type}
                              </Typography>
                            </Box>
                          </Box>
                        </Box>
                      </motion.div>
                    ))
                  ) : (
                    <Box
                      sx={{
                        p: 4,
                        borderRadius: "16px",
                        background: "rgba(15, 23, 42, 0.6)",
                        textAlign: "center",
                        border: "1px dashed rgba(99, 102, 241, 0.3)",
                      }}
                    >
                      <Calendar size={48} style={{ color: "rgba(255, 255, 255, 0.3)", marginBottom: "16px" }} />
                      <Typography sx={{ color: "rgba(255, 255, 255, 0.7)", mb: 2 }}>No upcoming sessions</Typography>
                      <Button
                        component={Link}
                        to="/interview"
                        variant="contained"
                        sx={{
                          borderRadius: "12px",
                          textTransform: "none",
                          background: "rgba(99, 102, 241, 0.2)",
                          backdropFilter: "blur(10px)",
                          color: "#fff",
                          border: "1px solid rgba(99, 102, 241, 0.3)",
                          "&:hover": {
                            background: "rgba(99, 102, 241, 0.3)",
                          },
                        }}
                        startIcon={<Play size={16} />}
                      >
                        Start Practice
                      </Button>
                    </Box>
                  )}

                  <Button
                    component={Link}
                    to="/interview"
                    fullWidth
                    variant="contained"
                    sx={{
                      mt: 3,
                      py: 1.5,
                      borderRadius: "12px",
                      textTransform: "none",
                      fontSize: "1rem",
                      fontWeight: 600,
                      background: "linear-gradient(135deg, #3b82f6, #2563eb)",
                      "&:hover": {
                        background: "linear-gradient(135deg, #2563eb, #1d4ed8)",
                        transform: "translateY(-2px)",
                        boxShadow: "0 10px 25px rgba(59, 130, 246, 0.4)",
                      },
                      transition: "all 0.3s ease",
                    }}
                    endIcon={<ArrowRight size={18} />}
                  >
                    Schedule New Session
                  </Button>
                </CardContent>
              </Card>
            </motion.div>
          </Grid>

          {/* Recent Activity */}
          <Grid item xs={12} md={6}>
            <motion.div custom={2} variants={cardVariants} initial="hidden" animate="visible">
              <Card
                sx={{
                  borderRadius: "24px",
                  overflow: "hidden",
                  boxShadow: "0 15px 60px rgba(0, 0, 0, 0.2)",
                  background: "rgba(30, 41, 59, 0.8)",
                  backdropFilter: "blur(20px)",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  height: "100%",
                }}
              >
                <CardContent sx={{ p: 4 }}>
                  <Box sx={{ display: "flex", alignItems: "center", mb: 3 }}>
                    <Avatar
                      sx={{
                        background: "rgba(16, 185, 129, 0.2)",
                        color: "#10b981",
                        width: 56,
                        height: 56,
                        mr: 2,
                      }}
                    >
                      <CheckCircle size={28} />
                    </Avatar>
                    <Box>
                      <Typography variant="h6" sx={{ fontWeight: 700, color: "#fff" }}>
                        Recent Activity
                      </Typography>
                      <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>Your latest achievements</Typography>
                    </Box>
                  </Box>

                  {stats.recentActivity.map((activity, index) => (
                    <motion.div
                      key={activity.id}
                      initial={{ opacity: 0, x: -20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ delay: index * 0.1 }}
                    >
                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "flex-start",
                          p: 2.5,
                          mb: 2,
                          borderRadius: "16px",
                          background: "rgba(15, 23, 42, 0.6)",
                          border: "1px solid rgba(255, 255, 255, 0.05)",
                          "&:hover": {
                            background: "rgba(15, 23, 42, 0.8)",
                            transform: "translateX(4px)",
                          },
                          transition: "all 0.3s ease",
                        }}
                      >
                        <Box
                          sx={{
                            width: 40,
                            height: 40,
                            borderRadius: "12px",
                            background: `rgba(${
                              index === 0 ? "16, 185, 129" : index === 1 ? "59, 130, 246" : "168, 85, 247"
                            }, 0.2)`,
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                            mr: 2,
                            color: index === 0 ? "#10b981" : index === 1 ? "#3b82f6" : "#a855f7",
                          }}
                        >
                          {activity.icon}
                        </Box>
                        <Box sx={{ flex: 1 }}>
                          <Typography
                            variant="subtitle2"
                            sx={{
                              fontWeight: 600,
                              color: "#fff",
                              mb: 0.5,
                            }}
                          >
                            {activity.action}
                          </Typography>
                          <Typography
                            variant="caption"
                            sx={{
                              color: "rgba(255, 255, 255, 0.5)",
                            }}
                          >
                            {activity.time}
                          </Typography>
                          {activity.score && (
                            <Box sx={{ mt: 1 }}>
                              <Typography
                                sx={{
                                  color: "#10b981",
                                  fontSize: "0.9rem",
                                  fontWeight: 600,
                                }}
                              >
                                Score: {activity.score}%
                              </Typography>
                            </Box>
                          )}
                          {activity.rating && (
                            <Box sx={{ display: "flex", alignItems: "center", mt: 1 }}>
                              <Star size={14} fill="#f59e0b" color="#f59e0b" />
                              <Typography
                                sx={{
                                  color: "#f59e0b",
                                  fontSize: "0.9rem",
                                  fontWeight: 600,
                                  ml: 0.5,
                                }}
                              >
                                {activity.rating}
                              </Typography>
                            </Box>
                          )}
                        </Box>
                      </Box>
                    </motion.div>
                  ))}

                  <Button
                    component={Link}
                    to="/report"
                    fullWidth
                    variant="outlined"
                    sx={{
                      mt: 2,
                      py: 1.5,
                      borderRadius: "12px",
                      textTransform: "none",
                      fontSize: "1rem",
                      fontWeight: 600,
                      borderColor: "rgba(16, 185, 129, 0.3)",
                      color: "#10b981",
                      "&:hover": {
                        borderColor: "#10b981",
                        background: "rgba(16, 185, 129, 0.1)",
                      },
                    }}
                    endIcon={<BarChart2 size={18} />}
                  >
                    View All Reports
                  </Button>
                </CardContent>
              </Card>
            </motion.div>
          </Grid>

          {/* Achievements */}
          <Grid item xs={12} md={6}>
            <motion.div custom={3} variants={cardVariants} initial="hidden" animate="visible">
              <Card
                sx={{
                  borderRadius: "24px",
                  overflow: "hidden",
                  boxShadow: "0 15px 60px rgba(0, 0, 0, 0.2)",
                  background: "rgba(30, 41, 59, 0.8)",
                  backdropFilter: "blur(20px)",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  height: "100%",
                }}
              >
                <CardContent sx={{ p: 4 }}>
                  <Box sx={{ display: "flex", alignItems: "center", mb: 3 }}>
                    <Avatar
                      sx={{
                        background: "rgba(245, 158, 11, 0.2)",
                        color: "#f59e0b",
                        width: 56,
                        height: 56,
                        mr: 2,
                      }}
                    >
                      <Award size={28} />
                    </Avatar>
                    <Box>
                      <Typography variant="h6" sx={{ fontWeight: 700, color: "#fff" }}>
                        Achievements
                      </Typography>
                      <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>Your milestones and badges</Typography>
                    </Box>
                  </Box>

                  {stats.achievements.map((achievement, index) => (
                    <motion.div
                      key={index}
                      initial={{ opacity: 0, scale: 0.9 }}
                      animate={{ opacity: 1, scale: 1 }}
                      transition={{ delay: index * 0.1 }}
                    >
                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          p: 2.5,
                          mb: 2,
                          borderRadius: "16px",
                          background: achievement.unlocked ? "rgba(15, 23, 42, 0.6)" : "rgba(15, 23, 42, 0.3)",
                          border: achievement.unlocked
                            ? "1px solid rgba(245, 158, 11, 0.3)"
                            : "1px solid rgba(255, 255, 255, 0.05)",
                          opacity: achievement.unlocked ? 1 : 0.6,
                          position: "relative",
                          overflow: "hidden",
                        }}
                      >
                        {achievement.unlocked && (
                          <Box
                            sx={{
                              position: "absolute",
                              top: 0,
                              left: 0,
                              width: "100%",
                              height: "2px",
                              background: "linear-gradient(90deg, #f59e0b, #d97706)",
                            }}
                          />
                        )}

                        <Box
                          sx={{
                            width: 48,
                            height: 48,
                            borderRadius: "12px",
                            background: achievement.unlocked ? "rgba(245, 158, 11, 0.2)" : "rgba(255, 255, 255, 0.1)",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                            mr: 2,
                            color: achievement.unlocked ? "#f59e0b" : "rgba(255, 255, 255, 0.4)",
                          }}
                        >
                          {achievement.icon}
                        </Box>

                        <Box>
                          <Typography
                            variant="subtitle1"
                            sx={{
                              fontWeight: 600,
                              color: achievement.unlocked ? "#fff" : "rgba(255, 255, 255, 0.5)",
                              mb: 0.5,
                            }}
                          >
                            {achievement.title}
                            {achievement.unlocked && (
                              <CheckCircle
                                size={16}
                                style={{
                                  color: "#10b981",
                                  marginLeft: "8px",
                                  display: "inline",
                                }}
                              />
                            )}
                          </Typography>
                          <Typography
                            variant="body2"
                            sx={{
                              color: achievement.unlocked ? "rgba(255, 255, 255, 0.7)" : "rgba(255, 255, 255, 0.4)",
                            }}
                          >
                            {achievement.description}
                          </Typography>
                        </Box>
                      </Box>
                    </motion.div>
                  ))}
                </CardContent>
              </Card>
            </motion.div>
          </Grid>
        </Grid>
      </Container>
    </Box>
  )
}

export default Home
