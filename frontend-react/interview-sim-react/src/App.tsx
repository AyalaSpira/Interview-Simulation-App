import { useState, useEffect } from "react";
import { BrowserRouter as Router, Routes, Route, Navigate, useNavigate } from "react-router-dom";
import { AppBar, Toolbar, Button, Container, Box } from "@mui/material";
import Home from "./components/home";
import LoginForm from "./components/LoginForm";
import RegisterForm from "./components/RegisterForm";
import UploadResume from "./components/UploadResume";
import InterviewPage from "./components/InterviewPage";
import Report from "./components/InterviewReport";

const App = () => {
  const [token, setToken] = useState<string | null>(localStorage.getItem("token"));
  const [interviewId, setInterviewId] = useState<number | null>(null); // הוספת interviewId למערכת
  const navigate = useNavigate();

  useEffect(() => {
    if (token) {
      localStorage.setItem("token", token);
    } else {
      localStorage.removeItem("token");
    }
  }, [token]);

  // פונקציה להתחברות שמגדירה טוקן ומנווטת להום
  const handleLogin = (newToken: string) => {
    console.log("Token received:", newToken); // בדיקה שהטוקן מתקבל
    setToken(newToken);
    localStorage.setItem("token", newToken);
    navigate("/home");
  };
  
  // פונקציה להתנתקות
  const handleLogout = () => {
    setToken(null);
    navigate("/login");
  };

  return (
    <Box>
      {token && (
        <AppBar position="static" sx={{ bgcolor: "#00897B" }}>
          <Toolbar>
            <Button color="inherit" onClick={() => navigate("/interview")}>Start Interview</Button>
            <Button color="inherit" onClick={() => navigate("/upload-resume")}>Upload Resume</Button>
            <Button color="inherit" onClick={() => navigate("/report")}>View Report</Button>
            <Button color="inherit" onClick={handleLogout}>Logout</Button>
          </Toolbar>
        </AppBar>
      )}
      <Container sx={{ mt: 3 }}>
        <Routes>
          <Route path="/" element={token ? <Navigate to="/home" /> : <Navigate to="/login" />} />
          <Route path="/home" element={token ? <Home onLogout={handleLogout} /> : <Navigate to="/login" />} />
          <Route path="/login" element={!token ? <LoginForm onLogin={handleLogin} /> : <Navigate to="/home" />} />
          <Route path="/register" element={!token ? <RegisterForm onLogin={handleLogin} /> : <Navigate to="/home" />} />
          <Route path="/upload-resume" element={token ? <UploadResume /> : <Navigate to="/login" />} />
          <Route path="/interview" element={token ? <InterviewPage /> : <Navigate to="/login" />} />
          <Route path="/report" element={token ? <Report interviewId={interviewId} /> : <Navigate to="/login" />} />
        </Routes>
      </Container>
    </Box>
  );
};
export default App;
