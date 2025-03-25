import React, { useState } from 'react';

interface LoginFormProps {
  onLogin: (token: string) => void;
}

const LoginForm: React.FC<LoginFormProps> = ({ onLogin }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    console.log("🔵 Attempting login with:", { username, password });

    try {
        const response = await fetch('http://localhost:5000/api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        console.log("🟡 Response status:", response.status); // בדיקה לסטטוס מהשרת

        if (!response.ok) {
            const errorText = await response.text();
            console.error("🔴 Login failed. Server response:", errorText);
            throw new Error(`Login failed with status ${response.status}`);
        }

        const data = await response.json();
        console.log("🟢 Login successful! Token received:", data);

        // ✅ שמירת הטוקן ב-localStorage והעברת הנתון לאפליקציה
        // בדוק ש-data.token מכיל את ה-token, אם זה לא כך, אתה צריך לבדוק איך ה-token מתקבל מהשרת.
        const token = data.token;  // כאן אתה שולף את ה-token באופן ישיר
        localStorage.setItem("token", token); // שומר את ה-token ב-localStorage
        onLogin(token);  // מעביר את ה-token למקום הרלוונטי באפליקציה

    } catch (err) {
        console.error("🔴 Error during login:", err);
        setError('Login failed. Please try again.');
    }
  };

  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Username</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Password</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Login</button>
      </form>
      {error && <p>{error}</p>}
    </div>
  );
};

export default LoginForm;
