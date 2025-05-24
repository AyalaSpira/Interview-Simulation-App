"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { Routes, Route, Navigate, useNavigate } from "react-router-dom"
import { AppBar, Toolbar, Button, Container, Box, Typography, Avatar, Menu, MenuItem, IconButton } from "@mui/material"
import { motion } from "framer-motion"
import { ChevronDown, User, FileText, Play, BarChart2, LogOut, MenuIcon } from "lucide-react"
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
  const navigate = useNavigate()

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
    navigate("/login")
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
    },
  }

  return (
    <Box
      sx={{
        height: "100vh",
        display: "flex",
        flexDirection: "column",
        overflow: "hidden",
        background: "linear-gradient(135deg, #0f172a, #1e293b)",
      }}
    >
      {token && (
        <AppBar
          position="static"
          elevation={0}
          sx={{
            background: "rgba(15, 23, 42, 0.7)",
            backdropFilter: "blur(10px)",
            borderBottom: "1px solid rgba(255, 255, 255, 0.1)",
          }}
        >
          <Container maxWidth="xl">
            <Toolbar sx={{ justifyContent: "space-between" }}>
              <Box sx={{ display: "flex", alignItems: "center" }}>
                <motion.div
                  initial={{ opacity: 0, scale: 0.9 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ duration: 0.5 }}
                >
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
                    Interview Simulator
                  </Typography>
                </motion.div>

                {/* Desktop Navigation */}
                <Box sx={{ display: { xs: "none", md: "flex" } }}>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/home")}
                    startIcon={<User size={18} />}
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
                    Upload Resume
                  </Button>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/interview")}
                    startIcon={<Play size={18} />}
                    sx={navButtonStyle}
                  >
                    Start Interview
                  </Button>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/report")}
                    startIcon={<BarChart2 size={18} />}
                    sx={navButtonStyle}
                  >
                    View Report
                  </Button>
                </Box>
              </Box>

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
                    "&:hover": { background: "rgba(255, 255, 255, 0.1)" },
                  }}
                >
                  <Avatar
                    sx={{
                      width: 32,
                      height: 32,
                      background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                      mr: 1,
                    }}
                  >
                    <User size={16} />
                  </Avatar>
                  <Typography sx={{ ml: 1, mr: 1, fontWeight: 500 }}>My Account</Typography>
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
                      backdropFilter: "blur(10px)",
                      border: "1px solid rgba(255, 255, 255, 0.1)",
                      borderRadius: "12px",
                      boxShadow: "0 10px 40px rgba(0, 0, 0, 0.3)",
                      color: "#fff",
                      minWidth: "200px",
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
                      "&:hover": { background: "rgba(255, 255, 255, 0.05)" },
                    }}
                  >
                    <User size={16} style={{ marginRight: "10px" }} />
                    Profile
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
                      "&:hover": { background: "rgba(255, 255, 255, 0.05)" },
                    }}
                  >
                    <LogOut size={16} style={{ marginRight: "10px" }} />
                    Logout
                  </MenuItem>
                </Menu>
              </Box>
            </Toolbar>
          </Container>
        </AppBar>
      )}

      {/* Mobile Menu */}
      {token && mobileMenuOpen && (
        <motion.div
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
          exit={{ opacity: 0, y: -20 }}
          transition={{ duration: 0.3 }}
        >
          <Box
            sx={{
              background: "rgba(15, 23, 42, 0.95)",
              backdropFilter: "blur(10px)",
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
              startIcon={<User size={18} />}
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
              Upload Resume
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
              Start Interview
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
              View Report
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

      <Box
        sx={{
          flexGrow: 1,
          overflow: "hidden",
          display: "flex",
          flexDirection: "column",
        }}
      >
        <Routes>
          <Route path="/" element={token ? <Navigate to="/home" /> : <Navigate to="/login" />} />
          <Route path="/home" element={token ? <Home onLogout={handleLogout} /> : <Navigate to="/login" />} />
          <Route path="/login" element={!token ? <LoginForm onLogin={handleLogin} /> : <Navigate to="/home" />} />
          <Route path="/register" element={!token ? <RegisterForm onLogin={handleLogin} /> : <Navigate to="/home" />} />
          <Route path="/upload-resume" element={token ? <UploadResume /> : <Navigate to="/login" />} />
          <Route path="/interview" element={token ? <InterviewPage /> : <Navigate to="/login" />} />
          <Route path="/report" element={token ? <Report interviewId={interviewId} /> : <Navigate to="/login" />} />
        </Routes>
      </Box>
    </Box>
  )
}

export default App
