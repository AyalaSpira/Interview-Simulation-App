"use client"

import type React from "react"
import { Link } from "react-router-dom"
import { Container, Typography, Button, Box, Grid, Card, CardContent, Avatar, LinearProgress } from "@mui/material"
import { motion } from "framer-motion"
import { FileText, Play, BarChart2, LogOut, Award, Clock, TrendingUp, CheckCircle, Calendar } from "lucide-react"

interface HomeProps {
  onLogout: () => void
}

const Home: React.FC<HomeProps> = ({ onLogout }) => {
  // Mock data for statistics
  const stats = {
    totalInterviews: 12,
    completedInterviews: 8,
    averageScore: 78,
    skillsToImprove: ["Communication", "Technical Knowledge", "Problem Solving"],
    upcomingInterviews: [
      { id: 1, title: "Frontend Developer", date: "Tomorrow, 10:00 AM" },
      { id: 2, title: "React Specialist", date: "May 28, 2:30 PM" },
    ],
    recentActivity: [
      { id: 1, action: "Completed Interview", time: "2 hours ago" },
      { id: 2, action: "Updated Resume", time: "Yesterday" },
      { id: 3, action: "Received Feedback", time: "3 days ago" },
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

  return (
    <Box
      sx={{
        height: "100%",
        overflow: "auto",
        py: 4,
        px: { xs: 2, md: 4 },
      }}
    >
      <Container maxWidth="xl" sx={{ height: "100%" }}>
        <Grid container spacing={3}>
          {/* Welcome Section */}
          <Grid item xs={12}>
            <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.6 }}>
              <Card
                sx={{
                  borderRadius: "24px",
                  overflow: "hidden",
                  boxShadow: "0 20px 80px rgba(0, 0, 0, 0.3)",
                  background: "rgba(30, 41, 59, 0.7)",
                  backdropFilter: "blur(20px)",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  p: 4,
                  position: "relative",
                }}
              >
                <Box sx={{ position: "relative", zIndex: 2 }}>
                  <Typography
                    variant="h4"
                    sx={{
                      fontWeight: 700,
                      mb: 2,
                      background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                      backgroundClip: "text",
                      textFillColor: "transparent",
                      WebkitBackgroundClip: "text",
                      WebkitTextFillColor: "transparent",
                    }}
                  >
                    Welcome to Your Interview Dashboard
                  </Typography>
                  <Typography
                    variant="body1"
                    sx={{
                      mb: 4,
                      color: "rgba(255, 255, 255, 0.7)",
                      fontSize: "1.1rem",
                      maxWidth: "800px",
                    }}
                  >
                    Track your progress, practice interviews, and improve your skills. Your personalized dashboard helps
                    you prepare for your next job interview with AI-powered feedback and analytics.
                  </Typography>

                  <Grid container spacing={2} sx={{ mb: 2 }}>
                    <Grid item xs={12} sm={6} md={3}>
                      <Button
                        component={Link}
                        to="/upload-resume"
                        variant="contained"
                        fullWidth
                        sx={{
                          py: 1.5,
                          borderRadius: "12px",
                          textTransform: "none",
                          fontSize: "1rem",
                          fontWeight: 600,
                          background: "rgba(99, 102, 241, 0.8)",
                          backdropFilter: "blur(10px)",
                          transition: "all 0.3s ease",
                          "&:hover": {
                            background: "rgba(99, 102, 241, 1)",
                            transform: "translateY(-3px)",
                            boxShadow: "0 10px 25px rgba(99, 102, 241, 0.4)",
                          },
                        }}
                        startIcon={<FileText size={20} />}
                      >
                        Upload Resume
                      </Button>
                    </Grid>

                    <Grid item xs={12} sm={6} md={3}>
                      <Button
                        component={Link}
                        to="/interview"
                        variant="contained"
                        fullWidth
                        sx={{
                          py: 1.5,
                          borderRadius: "12px",
                          textTransform: "none",
                          fontSize: "1rem",
                          fontWeight: 600,
                          background: "rgba(168, 85, 247, 0.8)",
                          backdropFilter: "blur(10px)",
                          transition: "all 0.3s ease",
                          "&:hover": {
                            background: "rgba(168, 85, 247, 1)",
                            transform: "translateY(-3px)",
                            boxShadow: "0 10px 25px rgba(168, 85, 247, 0.4)",
                          },
                        }}
                        startIcon={<Play size={20} />}
                      >
                        Start Interview
                      </Button>
                    </Grid>

                    <Grid item xs={12} sm={6} md={3}>
                      <Button
                        component={Link}
                        to="/report"
                        variant="contained"
                        fullWidth
                        sx={{
                          py: 1.5,
                          borderRadius: "12px",
                          textTransform: "none",
                          fontSize: "1rem",
                          fontWeight: 600,
                          background: "rgba(59, 130, 246, 0.8)",
                          backdropFilter: "blur(10px)",
                          transition: "all 0.3s ease",
                          "&:hover": {
                            background: "rgba(59, 130, 246, 1)",
                            transform: "translateY(-3px)",
                            boxShadow: "0 10px 25px rgba(59, 130, 246, 0.4)",
                          },
                        }}
                        startIcon={<BarChart2 size={20} />}
                      >
                        View Report
                      </Button>
                    </Grid>

                    <Grid item xs={12} sm={6} md={3}>
                      <Button
                        onClick={onLogout}
                        variant="outlined"
                        fullWidth
                        sx={{
                          py: 1.5,
                          borderRadius: "12px",
                          textTransform: "none",
                          fontSize: "1rem",
                          fontWeight: 600,
                          borderColor: "rgba(255, 255, 255, 0.2)",
                          color: "rgba(255, 255, 255, 0.7)",
                          transition: "all 0.3s ease",
                          "&:hover": {
                            borderColor: "rgba(255, 255, 255, 0.5)",
                            background: "rgba(255, 255, 255, 0.05)",
                          },
                        }}
                        startIcon={<LogOut size={20} />}
                      >
                        Logout
                      </Button>
                    </Grid>
                  </Grid>
                </Box>

                {/* Background decorative elements */}
                <Box
                  sx={{
                    position: "absolute",
                    top: -100,
                    right: -100,
                    width: 300,
                    height: 300,
                    borderRadius: "50%",
                    background:
                      "radial-gradient(circle, rgba(168, 85, 247, 0.2) 0%, rgba(59, 130, 246, 0.1) 70%, transparent 100%)",
                    zIndex: 1,
                  }}
                />
                <Box
                  sx={{
                    position: "absolute",
                    bottom: -80,
                    left: -80,
                    width: 200,
                    height: 200,
                    borderRadius: "50%",
                    background:
                      "radial-gradient(circle, rgba(59, 130, 246, 0.2) 0%, rgba(168, 85, 247, 0.1) 70%, transparent 100%)",
                    zIndex: 1,
                  }}
                />
              </Card>
            </motion.div>
          </Grid>

          {/* Statistics Cards */}
          <Grid item xs={12} md={4}>
            <motion.div custom={0} variants={cardVariants} initial="hidden" animate="visible">
              <Card
                sx={{
                  borderRadius: "24px",
                  overflow: "hidden",
                  boxShadow: "0 10px 40px rgba(0, 0, 0, 0.2)",
                  background: "rgba(30, 41, 59, 0.7)",
                  backdropFilter: "blur(20px)",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  height: "100%",
                }}
              >
                <CardContent sx={{ p: 3 }}>
                  <Box sx={{ display: "flex", alignItems: "center", mb: 3 }}>
                    <Avatar
                      sx={{
                        background: "rgba(168, 85, 247, 0.2)",
                        color: "#a855f7",
                        width: 48,
                        height: 48,
                        mr: 2,
                      }}
                    >
                      <Award size={24} />
                    </Avatar>
                    <Typography
                      variant="h6"
                      sx={{
                        fontWeight: 600,
                        color: "#fff",
                      }}
                    >
                      Your Progress
                    </Typography>
                  </Box>

                  <Box sx={{ mb: 3 }}>
                    <Box sx={{ display: "flex", justifyContent: "space-between", mb: 1 }}>
                      <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>Interviews Completed</Typography>
                      <Typography sx={{ color: "#fff", fontWeight: 600 }}>
                        {stats.completedInterviews}/{stats.totalInterviews}
                      </Typography>
                    </Box>
                    <LinearProgress
                      variant="determinate"
                      value={(stats.completedInterviews / stats.totalInterviews) * 100}
                      sx={{
                        height: 8,
                        borderRadius: 4,
                        backgroundColor: "rgba(255, 255, 255, 0.1)",
                        "& .MuiLinearProgress-bar": {
                          background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                        },
                      }}
                    />
                  </Box>

                  <Box sx={{ mb: 3 }}>
                    <Box sx={{ display: "flex", justifyContent: "space-between", mb: 1 }}>
                      <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>Average Score</Typography>
                      <Typography sx={{ color: "#fff", fontWeight: 600 }}>{stats.averageScore}%</Typography>
                    </Box>
                    <LinearProgress
                      variant="determinate"
                      value={stats.averageScore}
                      sx={{
                        height: 8,
                        borderRadius: 4,
                        backgroundColor: "rgba(255, 255, 255, 0.1)",
                        "& .MuiLinearProgress-bar": {
                          background: "linear-gradient(90deg, #3b82f6, #10b981)",
                        },
                      }}
                    />
                  </Box>

                  <Typography
                    variant="subtitle2"
                    sx={{
                      color: "rgba(255, 255, 255, 0.7)",
                      mb: 2,
                    }}
                  >
                    Skills to Improve:
                  </Typography>

                  {stats.skillsToImprove.map((skill, index) => (
                    <Box
                      key={index}
                      sx={{
                        display: "flex",
                        alignItems: "center",
                        mb: 1,
                        p: 1.5,
                        borderRadius: "12px",
                        background: "rgba(15, 23, 42, 0.6)",
                      }}
                    >
                      <TrendingUp size={16} style={{ color: "#a855f7", marginRight: "8px" }} />
                      <Typography sx={{ color: "#fff" }}>{skill}</Typography>
                    </Box>
                  ))}
                </CardContent>
              </Card>
            </motion.div>
          </Grid>

          {/* Upcoming Interviews */}
          <Grid item xs={12} md={4}>
            <motion.div custom={1} variants={cardVariants} initial="hidden" animate="visible">
              <Card
                sx={{
                  borderRadius: "24px",
                  overflow: "hidden",
                  boxShadow: "0 10px 40px rgba(0, 0, 0, 0.2)",
                  background: "rgba(30, 41, 59, 0.7)",
                  backdropFilter: "blur(20px)",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  height: "100%",
                }}
              >
                <CardContent sx={{ p: 3 }}>
                  <Box sx={{ display: "flex", alignItems: "center", mb: 3 }}>
                    <Avatar
                      sx={{
                        background: "rgba(59, 130, 246, 0.2)",
                        color: "#3b82f6",
                        width: 48,
                        height: 48,
                        mr: 2,
                      }}
                    >
                      <Calendar size={24} />
                    </Avatar>
                    <Typography
                      variant="h6"
                      sx={{
                        fontWeight: 600,
                        color: "#fff",
                      }}
                    >
                      Upcoming Interviews
                    </Typography>
                  </Box>

                  {stats.upcomingInterviews.length > 0 ? (
                    stats.upcomingInterviews.map((interview, index) => (
                      <Box
                        key={interview.id}
                        sx={{
                          p: 2,
                          mb: 2,
                          borderRadius: "16px",
                          background: "rgba(15, 23, 42, 0.6)",
                          border: "1px solid rgba(99, 102, 241, 0.2)",
                        }}
                      >
                        <Typography
                          variant="subtitle1"
                          sx={{
                            fontWeight: 600,
                            color: "#fff",
                            mb: 1,
                          }}
                        >
                          {interview.title}
                        </Typography>
                        <Box sx={{ display: "flex", alignItems: "center" }}>
                          <Clock size={16} style={{ color: "#a855f7", marginRight: "8px" }} />
                          <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>{interview.date}</Typography>
                        </Box>
                      </Box>
                    ))
                  ) : (
                    <Box
                      sx={{
                        p: 3,
                        borderRadius: "16px",
                        background: "rgba(15, 23, 42, 0.6)",
                        textAlign: "center",
                      }}
                    >
                      <Typography sx={{ color: "rgba(255, 255, 255, 0.7)" }}>
                        No upcoming interviews scheduled
                      </Typography>
                      <Button
                        component={Link}
                        to="/interview"
                        variant="contained"
                        sx={{
                          mt: 2,
                          borderRadius: "12px",
                          textTransform: "none",
                          background: "rgba(99, 102, 241, 0.2)",
                          backdropFilter: "blur(10px)",
                          color: "#fff",
                          "&:hover": {
                            background: "rgba(99, 102, 241, 0.3)",
                          },
                        }}
                        startIcon={<Play size={16} />}
                      >
                        Start Practice Interview
                      </Button>
                    </Box>
                  )}

                  <Box sx={{ mt: 3 }}>
                    <Button
                      component={Link}
                      to="/interview"
                      fullWidth
                      variant="contained"
                      sx={{
                        py: 1.5,
                        borderRadius: "12px",
                        textTransform: "none",
                        fontSize: "1rem",
                        fontWeight: 600,
                        background: "rgba(59, 130, 246, 0.2)",
                        backdropFilter: "blur(10px)",
                        color: "#fff",
                        border: "1px solid rgba(59, 130, 246, 0.3)",
                        "&:hover": {
                          background: "rgba(59, 130, 246, 0.3)",
                        },
                      }}
                      startIcon={<Play size={18} />}
                    >
                      Schedule New Interview
                    </Button>
                  </Box>
                </CardContent>
              </Card>
            </motion.div>
          </Grid>

          {/* Recent Activity */}
          <Grid item xs={12} md={4}>
            <motion.div custom={2} variants={cardVariants} initial="hidden" animate="visible">
              <Card
                sx={{
                  borderRadius: "24px",
                  overflow: "hidden",
                  boxShadow: "0 10px 40px rgba(0, 0, 0, 0.2)",
                  background: "rgba(30, 41, 59, 0.7)",
                  backdropFilter: "blur(20px)",
                  border: "1px solid rgba(255, 255, 255, 0.1)",
                  height: "100%",
                }}
              >
                <CardContent sx={{ p: 3 }}>
                  <Box sx={{ display: "flex", alignItems: "center", mb: 3 }}>
                    <Avatar
                      sx={{
                        background: "rgba(16, 185, 129, 0.2)",
                        color: "#10b981",
                        width: 48,
                        height: 48,
                        mr: 2,
                      }}
                    >
                      <CheckCircle size={24} />
                    </Avatar>
                    <Typography
                      variant="h6"
                      sx={{
                        fontWeight: 600,
                        color: "#fff",
                      }}
                    >
                      Recent Activity
                    </Typography>
                  </Box>

                  {stats.recentActivity.map((activity, index) => (
                    <Box
                      key={activity.id}
                      sx={{
                        display: "flex",
                        alignItems: "flex-start",
                        p: 2,
                        mb: 2,
                        borderRadius: "16px",
                        background: "rgba(15, 23, 42, 0.6)",
                      }}
                    >
                      <Box
                        sx={{
                          width: 32,
                          height: 32,
                          borderRadius: "50%",
                          background: "rgba(99, 102, 241, 0.2)",
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "center",
                          mr: 2,
                          mt: 0.5,
                        }}
                      >
                        {index === 0 && <CheckCircle size={16} style={{ color: "#10b981" }} />}
                        {index === 1 && <FileText size={16} style={{ color: "#3b82f6" }} />}
                        {index === 2 && <BarChart2 size={16} style={{ color: "#a855f7" }} />}
                      </Box>
                      <Box>
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
                      </Box>
                    </Box>
                  ))}

                  <Box sx={{ mt: 3 }}>
                    <Button
                      component={Link}
                      to="/report"
                      fullWidth
                      variant="contained"
                      sx={{
                        py: 1.5,
                        borderRadius: "12px",
                        textTransform: "none",
                        fontSize: "1rem",
                        fontWeight: 600,
                        background: "rgba(16, 185, 129, 0.2)",
                        backdropFilter: "blur(10px)",
                        color: "#fff",
                        border: "1px solid rgba(16, 185, 129, 0.3)",
                        "&:hover": {
                          background: "rgba(16, 185, 129, 0.3)",
                        },
                      }}
                      startIcon={<BarChart2 size={18} />}
                    >
                      View All Activity
                    </Button>
                  </Box>
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
