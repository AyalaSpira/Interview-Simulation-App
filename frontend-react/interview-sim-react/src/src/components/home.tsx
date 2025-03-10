// src/components/Home.tsx

import React from 'react';
import { Link } from 'react-router-dom';

const Home: React.FC = () => {
  return (
    <div>
      <h1>Welcome to the Interview Simulation App!</h1>
      <p>
        Please choose an option to get started:
      </p>
      <ul>
        <li>
          <Link to="/login">Login</Link>
        </li>
        <li>
          <Link to="/register">Register</Link>
        </li>
        <li>
          <Link to="/questions">Start Interview</Link>
        </li>
        <li>
          <Link to="/report">View Report</Link>
        </li>
      </ul>
    </div>
  );
};

export default Home;
