import { useState } from "react";

const API_URL = "http://localhost:5000/api";

const RegisterForm: React.FC = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [file, setFile] = useState<File | null>(null);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!file) {
      alert("New users must upload a resume.");
      return;
    }

    const formData = new FormData();
    formData.append("username", username);
    formData.append("password", password);
    formData.append("resume", file);

    try {
      const response = await fetch(`http://localhost:5000/api/auth/register`, {
        method: "POST",
        body: formData,
      });

      if (!response.ok) throw new Error("Registration failed");

      setSuccess("Registration successful! You can now log in.");
      setError("");
    } catch (err) {
      setError("Registration failed. Try again.");
    }
  };

  return (
    <div>
      <h2>Register</h2>
      <form onSubmit={handleRegister}>
        <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} placeholder="Username" required />
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required />
        <input type="file" onChange={(e) => setFile(e.target.files?.[0] || null)} required />
        <button type="submit">Register</button>
      </form>
      {error && <p style={{ color: "red" }}>{error}</p>}
      {success && <p style={{ color: "green" }}>{success}</p>}
    </div>
  );
};

export default RegisterForm;
