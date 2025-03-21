import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { useState, useEffect } from "react";
import Home from "./src/components/home";
import LoginForm from "./src/components/LoginForm";
import RegisterForm from "./src/components/RegisterForm";
import UploadResume from "./src/components/UploadResume";
import Interview from "./src/components/Interview";
import Report from "./src/components/Report";

const App = () => {
  const [token, setToken] = useState<string | null>(localStorage.getItem("token"));

  useEffect(() => {
    if (token) {
      localStorage.setItem("token", token);
    } else {
      localStorage.removeItem("token");
    }
  }, [token]);

  const handleLogout = () => {
    setToken(null);
  };

  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home onLogout={handleLogout} />} />
        <Route
          path="/login"
          element={!token ? <LoginForm onLogin={setToken} /> : <Navigate to="/interview" />}
        />
        <Route
          path="/register"
          element={!token ? <RegisterForm /> : <Navigate to="/interview" />}
        />
        <Route
          path="/upload-resume"
          element={token ? <UploadResume /> : <Navigate to="/login" />}
        />
        <Route
          path="/interview"
          element={token ? <Interview /> : <Navigate to="/login" />}
        />
        <Route
          path="/report"
          element={token ? <Report /> : <Navigate to="/login" />}
        />
      </Routes>
    </Router>
  );
};

export default App;
