import React from 'react';
import { Link } from 'react-router-dom';

const Home: React.FC<{ onLogout: () => void }> = ({ onLogout }) => {
  return (
    <div>
      <h1>Welcome to the Interview Simulation App!</h1>
      <p>Please choose an option to get started:</p>
      <ul>
        <li>
          <Link to="/login">Login</Link>
        </li>
        <li>
          <Link to="/register">Register</Link>
        </li>
        <li>
          <Link to="/upload-resume">Upload Resume</Link>
        </li>
        <li>
          <Link to="/interview">Start Interview</Link>
        </li>
        <li>
          <Link to="/report">View Report</Link>
        </li>
        <li>
          <button onClick={onLogout}>Logout</button>
        </li>
      </ul>
    </div>
  );
};

export default Home;
