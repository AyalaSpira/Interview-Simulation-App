import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './src/components/home';
// import Login from './components/Login';
// import Register from './components/Register';
import LoginForm from './src/components/LoginForm';
import RegisterForm from './src/components/RegisterForm';
import Questions from './src/components/Questions';
import Report from './src/components/Report';

const App = () => {
  return (
    <Router>
    <Routes>
      <Route path="/" element={<Home />} />
      {/* <Route path="/login" element={<Login />} /> */}
      {/* <Route path="/register" element={<Register />} /> */}
      <Route path="/login" element={<LoginForm />} />
      <Route path="/register" element={<RegisterForm />} />
      <Route path="/questions" element={<Questions />} />
      <Route path="/report" element={<Report />} />
    </Routes>
  </Router>
  
  )
};

export default App;
