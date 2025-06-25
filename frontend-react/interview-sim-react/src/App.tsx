// App.tsx

"use client"

import type React from "react"
import { useState, useEffect } from "react"
import { Routes, Route, Navigate, useNavigate, useLocation, BrowserRouter } from "react-router-dom" // ודא ש-BrowserRouter מיובא אם הוא עוטף כאן את App
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

// ייבוא קומפוננטות (ודא שהנתיבים נכונים אצלך)
import Home from "./components/home"
import LoginForm from "./components/LoginForm"
import RegisterForm from "./components/RegisterForm"
import UploadResume from "./components/UploadResume"
import InterviewPage from "./components/InterviewPage"
import Report from "./components/InterviewReport"

const App = () => {
  const [token, setToken] = useState<string | null>(localStorage.getItem("token"))
  const [interviewId, setInterviewId] = useState<number | null>(null) // יש לוודא שימוש ב-interviewId אם הוא רלוונטי
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null)
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false)
  const [notificationCount, setNotificationCount] = useState(3)
  const [isTokenChecked, setIsTokenChecked] = useState(false)

  const navigate = useNavigate()
  const location = useLocation()
useEffect(() => {
  const savedToken = localStorage.getItem("token")
  setToken(savedToken)
  setIsTokenChecked(true)
}, [])

  // ניהול טוקן ב-localStorage
  useEffect(() => {
    if (token) {
      localStorage.setItem("token", token)
    } else {
      localStorage.removeItem("token")
    }
  }, [token])

  // ניתוב אוטומטי לעמוד הלוגין בכניסה ראשונה או אם אין טוקן ב-root path
 useEffect(() => {
  if (isTokenChecked && !token && location.pathname === "/") {
    navigate("/login")
  }
}, [isTokenChecked, token, location.pathname])


  // פונקציית התחברות
  const handleLogin = (newToken: string) => {
    console.log("Token received:", newToken)
    setToken(newToken)
    localStorage.setItem("token", newToken)
    navigate("/home") // מעבר ל-HOME אחרי לוגין מוצלח
  }

  // פונקציית יציאה
  const handleLogout = () => {
    setToken(null)
    localStorage.removeItem("token")
    navigate("/login") // חזרה ללוגין אחרי יציאה
  }

  // פונקציות לניהול תפריטים ו-UI
  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const handleMobileMenuToggle = () => {
    setMobileMenuOpen(!mobileMenuOpen)
  }

  const isAuthPage = location.pathname === "/login" || location.pathname === "/register"

  // סגנונות כפתורי ניווט ב-AppBar
  const navButtonStyle = {
    color: "#fff",
    mx: 0.5,
    py: 0.8,
    px: 1.5,
    borderRadius: "10px",
    textTransform: "none",
    fontSize: "0.9rem",
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
        background: isAuthPage ? "transparent" : "linear-gradient(135deg, #0f172a, #1e293b)",
      }}
    >
      {/* AppBar (סרגל ניווט עליון) - מוצג רק אם יש טוקן וזה לא עמוד אימות */}
      {token && !isAuthPage && (
        <AppBar
          position="static"
          elevation={0}
          sx={{
            background: "rgba(15, 23, 42, 0.8)",
            backdropFilter: "blur(20px)",
            borderBottom: "1px solid rgba(255, 255, 255, 0.1)",
            minHeight: "64px",
          }}
        >
          <Container maxWidth="xl">
            <Toolbar sx={{ justifyContent: "space-between", py: 0.5, minHeight: "64px !important" }}>
              <Box sx={{ display: "flex", alignItems: "center" }}>
                <motion.div
                  initial={{ opacity: 0, scale: 0.9 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ duration: 0.5 }}
                  style={{ display: "flex", alignItems: "center", gap: "8px" }}
                >
                  <motion.div
                    animate={{ rotate: 360 }}
                    transition={{ duration: 20, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}
                    style={{
                      width: "36px",
                      height: "36px",
                      borderRadius: "10px",
                      background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                    }}
                  >
                    <Sparkles size={20} color="white" />
                  </motion.div>
                  <Typography
                    variant="h6"
                    sx={{
                      fontWeight: 700,
                      background: "linear-gradient(90deg, #a855f7, #3b82f6)",
                      backgroundClip: "text",
                      WebkitBackgroundClip: "text",
                      WebkitTextFillColor: "transparent",
                      mr: 2,
                      fontSize: "1.3rem",
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
                    startIcon={<HomeIcon size={16} />}
                    sx={navButtonStyle}
                  >
                    דשבורד
                  </Button>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/upload-resume")}
                    startIcon={<FileText size={16} />}
                    sx={navButtonStyle}
                  >
                    קורות חיים
                  </Button>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/interview")}
                    startIcon={<Play size={16} />}
                    sx={navButtonStyle}
                  >
                    ראיון
                  </Button>
                  <Button
                    color="inherit"
                    onClick={() => navigate("/report")}
                    startIcon={<BarChart2 size={16} />}
                    sx={navButtonStyle}
                  >
                    דוחות
                  </Button>
                </Box>
              </Box>

              {/* Right Side Actions */}
              <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                {/* Search Button */}
                <IconButton
                  sx={{
                    color: "#fff",
                    background: "rgba(255, 255, 255, 0.05)",
                    "&:hover": { background: "rgba(255, 255, 255, 0.1)" },
                    display: { xs: "none", md: "flex" },
                    width: 36,
                    height: 36,
                  }}
                >
                  <Search size={18} />
                </IconButton>

                {/* Notifications */}
                <IconButton
                  sx={{
                    color: "#fff",
                    background: "rgba(255, 255, 255, 0.05)",
                    "&:hover": { background: "rgba(255, 255, 255, 0.1)" },
                    position: "relative",
                    display: { xs: "none", md: "flex" },
                    width: 36,
                    height: 36,
                  }}
                >
                  <Bell size={18} />
                  {notificationCount > 0 && (
                    <Box
                      sx={{
                        position: "absolute",
                        top: 2,
                        right: 2,
                        width: 14,
                        height: 14,
                        borderRadius: "50%",
                        background: "linear-gradient(135deg, #ef4444, #dc2626)",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        fontSize: "0.6rem",
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
                      width: 36,
                      height: 36,
                    }}
                  >
                    <MenuIcon size={20} />
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
                      borderRadius: "10px",
                      px: 1.5,
                      py: 0.5,
                      "&:hover": {
                        background: "rgba(255, 255, 255, 0.1)",
                        transform: "translateY(-2px)",
                      },
                      transition: "all 0.3s ease",
                      minWidth: "auto",
                    }}
                  >
                    <Avatar
                      sx={{
                        width: 28,
                        height: 28,
                        background: "linear-gradient(135deg, #a855f7, #3b82f6)",
                        mr: 0.5,
                        fontSize: "0.8rem",
                        fontWeight: 600,
                      }}
                    >
                      <User size={14} />
                    </Avatar>
                    <Typography sx={{ ml: 0.5, mr: 0.5, fontWeight: 500, fontSize: "0.9rem" }}>חשבון</Typography>
                    <ChevronDown size={14} />
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
                        borderRadius: "12px",
                        boxShadow: "0 20px 60px rgba(0, 0, 0, 0.4)",
                        color: "#fff",
                        minWidth: "180px",
                        overflow: "hidden",
                      },
                    }}
                  >
                    <MenuItem
                      onClick={() => {
                        handleMenuClose();
                        navigate("/home");
                      }}
                      sx={{
                        py: 1,
                        px: 2,
                        "&:hover": { background: "rgba(168, 85, 247, 0.1)" },
                        transition: "all 0.2s ease",
                        fontSize: "0.9rem",
                      }}
                    >
                      <User size={14} style={{ marginRight: "8px" }} />
                      פרופיל
                    </MenuItem>
                    <MenuItem
                      onClick={() => {
                        handleMenuClose();
                      }}
                      sx={{
                        py: 1,
                        px: 2,
                        "&:hover": { background: "rgba(168, 85, 247, 0.1)" },
                        transition: "all 0.2s ease",
                        fontSize: "0.9rem",
                      }}
                    >
                      <Settings size={14} style={{ marginRight: "8px" }} />
                      הגדרות
                    </MenuItem>
                    <MenuItem
                      onClick={() => {
                        handleMenuClose();
                        handleLogout();
                      }}
                      sx={{
                        py: 1,
                        px: 2,
                        color: "#f87171",
                        "&:hover": { background: "rgba(239, 68, 68, 0.1)" },
                        transition: "all 0.2s ease",
                        fontSize: "0.9rem",
                      }}
                    >
                      <LogOut size={14} style={{ marginRight: "8px" }} />
                      יציאה
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
        {token && mobileMenuOpen && !isAuthPage && (
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
                py: 1,
                px: 2,
              }}
            >
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  navigate("/home");
                  setMobileMenuOpen(false);
                }}
                startIcon={<HomeIcon size={16} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 0.5,
                  mx: 0,
                }}
              >
                דשבורד
              </Button>
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  navigate("/upload-resume");
                  setMobileMenuOpen(false);
                }}
                startIcon={<FileText size={16} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 0.5,
                  mx: 0,
                }}
              >
                קורות חיים
              </Button>
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  navigate("/interview");
                  setMobileMenuOpen(false);
                }}
                startIcon={<Play size={16} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 0.5,
                  mx: 0,
                }}
              >
                ראיון
              </Button>
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  navigate("/report");
                  setMobileMenuOpen(false);
                }}
                startIcon={<BarChart2 size={16} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 0.5,
                  mx: 0,
                }}
              >
                דוחות
              </Button>
              <Button
                fullWidth
                color="inherit"
                onClick={() => {
                  handleLogout();
                  setMobileMenuOpen(false);
                }}
                startIcon={<LogOut size={16} />}
                sx={{
                  ...navButtonStyle,
                  justifyContent: "flex-start",
                  my: 0.5,
                  mx: 0,
                  color: "#f87171",
                }}
              >
                יציאה
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
          {/* ניתוב ראשי - אם אין טוקן, מעבר ללוגין. אם יש, מעבר ל-home */}
          <Route path="/" element={token ? <Navigate to="/home" /> : <Navigate to="/login" />} />

          {/* עמודי אימות */}
          {/* LoginForm: אם אין טוקן, הצג את טופס הלוגין, אחרת נווט ל-home */}
          <Route path="/login" element={!token ? <LoginForm onLogin={handleLogin} /> : <Navigate to="/home" />} />
          {/* RegisterForm: אם אין טוקן, הצג את טופס הרישום, אחרת נווט ל-home.
             אין צורך להעביר onRegisterSuccess כי הניווט הוא עכשיו כפתור ידני בתוך הרכיב. */}
          <Route
            path="/register"
            element={!token ? <RegisterForm /> : <Navigate to="/home" />}
          />

          {/* עמודים מוגנים - דורשים טוקן. אם אין, נווט ללוגין */}
          <Route path="/home" element={token ? <Home onLogout={handleLogout} /> : <Navigate to="/login" />} />
          <Route path="/upload-resume" element={token ? <UploadResume /> : <Navigate to="/login" />} />
          <Route path="/interview" element={token ? <InterviewPage /> : <Navigate to="/login" />} />
          <Route path="/report" element={token ? <Report /> : <Navigate to="/login" />} />
        </Routes>
      </Box>
    </Box>
  )
}

export default App


/*

*/