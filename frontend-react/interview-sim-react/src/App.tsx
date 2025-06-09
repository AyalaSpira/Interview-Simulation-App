"use client"

import type React from "react"
import { useState, useEffect } from "react"
import { Routes, Route, Navigate, useNavigate, useLocation } from "react-router-dom"
import { AppBar, Toolbar, Button, Container, Box, Typography, Avatar, Menu, MenuItem, IconButton } from "@mui/material"
import { motion, AnimatePresence } from "framer-motion"
import {
  ChevronDown,
  User,
  FileText,
  Play,
  BarChart2,
  LogOut,
  MenuIcon,
  HomeIcon,
  Settings,
  Bell,
  Search,
  Sparkles,
} from "lucide-react"
import LandingPage from "./components/LandingPage"
import Home from "./components/home"
import LoginForm from "./components/LoginForm"
import RegisterForm from "./components/RegisterForm"
import UploadResume from "./components/UploadResume"
import InterviewPage from "./components/InterviewPage"
import Report from "./components/InterviewReport"

const App = () => {
  const [token, setToken] = useState<string | null>(localStorage.getItem("token"))
  const [interviewId, setInterviewId] = useState<number | null>(null)
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null)
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false)
  const [notificationCount, setNotificationCount] = useState(3)
  const navigate = useNavigate()
  const location = useLocation()

  useEffect(() => {
    if (token) {
      localStorage.setItem("token", token)
    } else {
      localStorage.removeItem("token")
    }
  }, [token])

  const handleLogin = (newToken: string) => {
    console.log("Token received:", newToken)
    setToken(newToken)
    localStorage.setItem("token", newToken)
    navigate("/home")
  }

  const handleLogout = () => {
    setToken(null)
    navigate("/")
  }

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const handleMobileMenuToggle = () => {
    setMobileMenuOpen(!mobileMenuOpen)
  }

  const isLandingPage = location.pathname === "/" || location.pathname === "/login" || location.pathname === "/register"

  const navButtonStyle = {
    color: "#fff",
    mx: 1,
    py: 1,
    px: 2,
    borderRadius: "12px",
    textTransform: "none",
    fontSize: "0.95rem",
    fontWeight: 600,
    transition: "all 0.3s ease",
    "&:hover": {
      background: "rgba(255, 255, 255, 0.1)",
      transform: "translateY(-2px)",
      boxShadow: "0 4px 12px rgba(168, 85, 247, 0.3)",
    },
  }

  return (
    <Box
      sx={{
        height: "100vh",
        display: "flex",
        flexDirection: "column",
        overflow: "hidden",
        background: isLandingPage ? "transparent" : "linear-gradient(135deg, #0f172a, #1e293b)",
      }}
    >
      {token && !isLandingPage && (
        <AppBar
          position="static"
          elevation={0}
          sx={{
            background: "rgba(15, 23, 42, 0.8)",
            backdropFilter: "blur(20px)",
            borderBottom: "1px solid rgba(255, 255, 255, 0.1)",
          }}
        >
          <Container maxWidth="xl">
            <Toolbar sx={{ justifyContent: "space-between", py: 1 }}>
              <Box sx={{ display: "flex", alignItems: "center" }}>
                <motion.div
                  initial={{ opacity: 0, scale: 0.9 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ duration: 0.5 }}
                  style={{ display: "flex", alignItems: "center", gap: "12px" }}
                >
                  <motion.div
                    animate={{ rotate: 360 }}
                    transition={{ duration: 20, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}
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
                  </motion.div>
                  <Typography
                    variant="h5"
                    sx={{
                      fontWeight: 700,
                      background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                      backgroundClip: "text",
                      WebkitBackgroundClip: "text",
                      WebkitTextFillColor: "transparent",
                      mr: 4,
                    }}
                  >
                    InterviewAI Pro
                  </Typography>
                </motion.div>

                {/* Desktop Navigation */}
                <Box sx={{ display: { xs: "none", md: "flex" } }}>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/home")}
                    startIcon={<HomeIcon size={18} />}
                    sx={navButtonStyle}
                  >
                    Dashboard
                  </Button>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/upload-resume")}
                    startIcon={<FileText size={18} />}
                    sx={navButtonStyle}
                  >
                    Resume
                  </Button>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/interview")}
                    startIcon={<Play size={18} />}
                    sx={navButtonStyle}
                  >
                    Interview
                  </Button>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/report")}
                    startIcon={<BarChart2 size={18} />}
                    sx={navButtonStyle}
                  >
                    Reports
                  </Button>
                </Box>
              </Box>

              {/* Right Side Actions */}
              <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
                {/* Search Button */}
                <IconButton
                  sx={{
                    color: "#fff",
                    background: "rgba(255, 255, 255, 0.05)",
                    "&:hover": { background: "rgba(255, 255, 255, 0.1)" },
                    display: { xs: "none", md: "flex" },
                  }}
                >
                  <Search size={20} />
                </IconButton>

                {/* Notifications */}
                <IconButton
                  sx={{
                    color: "#fff",
                    background: "rgba(255, 255, 255, 0.05)",
                    "&:hover": { background: "rgba(255, 255, 255, 0.1)" },
                    position: "relative",
                    display: { xs: "none", md: "flex" },
                  }}
                >
                  <Bell size={20} />
                  {notificationCount > 0 && (
                    <Box
                      sx={{
                        position: "absolute",
                        top: 4,
                        right: 4,
                        width: 16,
                        height: 16,
                        borderRadius: "50%",
                        background: "linear-gradient(135deg, #ef4444, #dc2626)",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        fontSize: "0.7rem",
                        fontWeight: 600,
                        color: "#fff",
                      }}
                    >
                      {notificationCount}
                    </Box>
                  )}
                </IconButton>

                {/* Mobile Menu Button */}
                <Box sx={{ display: { xs: "flex", md: "none" } }}>
                  <IconButton
                    color="inherit"
                    onClick={handleMobileMenuToggle}
                    sx={{
                      color: "#fff",
                      background: "rgba(255, 255, 255, 0.05)",
                      "&:hover": { background: "rgba(255, 255, 255, 0.1)" },
                    }}
                  >
                    <MenuIcon size={24} />
                  </IconButton>
                </Box>

                {/* User Menu */}
                <Box sx={{ display: { xs: "none", md: "flex" }, alignItems: "center" }}>
                  <Button
                    onClick={handleMenuOpen}
                    sx={{
                      color: "#fff",
                      textTransform: "none",
                      display: "flex",
                      alignItems: "center",
                      background: "rgba(255, 255, 255, 0.05)",
                      borderRadius: "12px",
                      px: 2,
                      py: 1,
                      "&:hover": {
                        background: "rgba(255, 255, 255, 0.1)",
                        transform: "translateY(-2px)",
                      },
                      transition: "all 0.3s ease",
                    }}
                  >
                    <Avatar
                      sx={{
                        width: 32,
                        height: 32,
                        background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                        mr: 1,
                        fontSize: "0.9rem",
                        fontWeight: 600,
                      }}
                    >
                      <User size={16} />
                    </Avatar>
                    <Typography sx={{ ml: 1, mr: 1, fontWeight: 500 }}>Account</Typography>
                    <ChevronDown size={16} />
                  </Button>
                  <Menu
                    anchorEl={anchorEl}
                    open={Boolean(anchorEl)}
                    onClose={handleMenuClose}
                    PaperProps={{
                      sx: {
                        mt: 1,
                        background: "rgba(30, 41, 59, 0.95)",
                        backdropFilter: "blur(20px)",
                        border: "1px solid rgba(255, 255, 255, 0.1)",
                        borderRadius: "16px",
                        boxShadow: "0 20px 60px rgba(0, 0, 0, 0.4)",
                        color: "#fff",
                        minWidth: "220px",
                        overflow: "hidden",
                      },
                    }}
                  >
                    <MenuItem
                      onClick={() => {
                        handleMenuClose()
                        navigate("/home")
                      }}
                      sx={{
                        py: 1.5,
                        px: 2,
                        "&:hover": { background: "rgba(168, 85, 247, 0.1)" },
                        transition: "all 0.2s ease",
                      }}
                    >
                      <User size={16} style={{ marginRight: "12px" }} />
                      Profile
                    </MenuItem>
                    <MenuItem
                      onClick={() => {
                        handleMenuClose()
                      }}
                      sx={{
                        py: 1.5,
                        px: 2,
                        "&:hover": { background: "rgba(168, 85, 247, 0.1)" },
                        transition: "all 0.2s ease",
                      }}
                    >
                      <Settings size={16} style={{ marginRight: "12px" }} />
                      Settings
                    </MenuItem>
                    <MenuItem
                      onClick={() => {
                        handleMenuClose()
                        handleLogout()
                      }}
                      sx={{
                        py: 1.5,
                        px: 2,
                        color: "#f87171",
                        "&:hover": { background: "rgba(239, 68, 68, 0.1)" },
                        transition: "all 0.2s ease",
                      }}
                    >
                      <LogOut size={16} style={{ marginRight: "12px" }} />
                      Logout
                    </MenuItem>
                  </Menu>
                </Box>
              </Box>
            </Toolbar>
          </Container>
        </AppBar>
      )}

      {/* Mobile Menu */}
      <AnimatePresence>
        {token && mobileMenuOpen && !isLandingPage && (
          <motion.div
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: -20 }}
            transition={{ duration: 0.3 }}
          >
            <Box
              sx={{
                background: "rgba(15, 23, 42, 0.95)",
                backdropFilter: "blur(20px)",
                borderBottom: "1px solid rgba(255, 255, 255, 0.1)",
                py: 2,
                px: 3,
              }}
            >
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  navigate("/home")
                  setMobileMenuOpen(false)
                }}
                startIcon={<HomeIcon size={18} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 1,
                  mx: 0,
                }}
              >
                Dashboard
              </Button>
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  navigate("/upload-resume")
                  setMobileMenuOpen(false)
                }}
                startIcon={<FileText size={18} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 1,
                  mx: 0,
                }}
              >
                Resume
              </Button>
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  navigate("/interview")
                  setMobileMenuOpen(false)
                }}
                startIcon={<Play size={18} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 1,
                  mx: 0,
                }}
              >
                Interview
              </Button>
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  navigate("/report")
                  setMobileMenuOpen(false)
                }}
                startIcon={<BarChart2 size={18} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 1,
                  mx: 0,
                }}
              >
                Reports
              </Button>
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  handleLogout()
                  setMobileMenuOpen(false)
                }}
                startIcon={<LogOut size={18} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 1,
                  mx: 0,
                  color: "#f87171",
                }}
              >
                Logout
              </Button>
            </Box>
          </motion.div>
        )}
      </AnimatePresence>

      <Box
        sx={{
          flexGrow: 1,
          overflow: "hidden",
          display: "flex",
          flexDirection: "column",
        }}
      >
        <Routes>
          <Route path="/" element={!token ? <LandingPage /> : <Navigate to="/home" />} />
          <Route path="/home" element={token ? <Home onLogout={handleLogout} /> : <Navigate to="/" />} />
          <Route path="/login" element={!token ? <LoginForm onLogin={handleLogin} /> : <Navigate to="/home" />} />
          <Route path="/register" element={!token ? <RegisterForm onLogin={handleLogin} /> : <Navigate to="/home" />} />
          <Route path="/upload-resume" element={token ? <UploadResume /> : <Navigate to="/" />} />
          <Route path="/interview" element={token ? <InterviewPage /> : <Navigate to="/" />} />
          <Route path="/report" element={token ? <Report interviewId={interviewId} /> : <Navigate to="/" />} />
        </Routes>
      </Box>
    </Box>
  )
}

export default App
