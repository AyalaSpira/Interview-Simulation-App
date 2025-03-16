import React, { useState } from "react";

const API_URL = 'http://localhost:5000/api'; // שנה מ-https ל-http

export const RegisterForm = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [resume, setResume] = useState<File | null>(null); // כאן resume יהיה אובייקט File

  // פונקציה להחלת הקובץ שנבחר
  const registerUser = (event: React.ChangeEvent<HTMLInputElement>) => {
    setResume(event.target.files ? event.target.files[0] : null); // מזהים את הקובץ שהמשתמש העלה
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
  
    if (!username || !password || !resume) {
      console.error('All fields are required!');
      return;
    }
  
    const formData = new FormData();
    formData.append('username', username);  // מכניסים את שם המשתמש
    formData.append('password', password);  // מכניסים את הסיסמה
    formData.append('resume', resume);  // מכניסים את הקובץ שהמשתמש העלה
  
    try {
      const response = await fetch('http://localhost:5000/api/auth/register', {
        method: 'POST',
        body: formData,  // כאן אתה שולח את ה-FormData
      });
  
      if (!response.ok) {
        throw new Error('Registration failed');
      }
  
      const data = await response.json();
      console.log('Registration successful', data);
    } catch (error) {
      console.error('Error:', error);
    }
  };
  
  // פונקציה להעלאת קובץ רזומה
  const uploadResume = async (resume: File) => {
    const formData = new FormData();
    formData.append('resume', resume);

    try {
      const response = await fetch(`${API_URL}/interview/upload-resume`, {
        method: 'POST',
        body: formData,
      });

      if (!response.ok) {
        throw new Error('Failed to upload resume');
      }

      return await response.json();  // החזרת התגובה מהשרת
    } catch (error) {
      console.error('Error uploading resume:', error);
    }
  };

  // פונקציה להתחברות
  const loginUser = async (userData: { username: string; password: string }) => {
    const response = await fetch(`${API_URL}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(userData),
    });

    if (!response.ok) {
      throw new Error('Login failed');
    }

    return await response.json();
  };

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <input
          type="file"
          onChange={registerUser}
          accept=".pdf, .doc, .docx" // תוכל להוסיף כאן סיומות קבצים שיתאימו
        />
        <button type="submit">Register</button>
      </form>
    </div>
  );
};

