import { useState } from "react";
import { Form, Input, Upload, Button, message, Card, Typography } from "antd";
import { UploadOutlined } from "@ant-design/icons";
import { registerUser } from "../services/authService";
import { CSSProperties } from "react";
import { useNavigate } from "react-router-dom";

const styles: { [key: string]: CSSProperties } = {
  container: {
    height: "100vh",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    background: "linear-gradient(135deg, #00c9a7, #5cb85c)",
  },
  card: {
    width: 400,
    padding: 30,
    borderRadius: 10,
    textAlign: "center",
    backdropFilter: "blur(10px)",
    boxShadow: "0 4px 10px rgba(0, 0, 0, 0.2)",
    backgroundColor: "rgba(255, 255, 255, 0.15)",
    color: "#fff",
  },
  title: {
    color: "#fff",
    fontWeight: "bold",
  },
  form: {
    textAlign: "left",
  },
  fileName: {
    display: "block",
    marginTop: 8,
    fontSize: 14,
    color: "#fff",
  },
};

interface RegisterFormProps {
  onLogin: (token: string) => void;
}

const RegisterForm: React.FC<RegisterFormProps> = ({ onLogin }) => {
  const [file, setFile] = useState<File | null>(null);
  const [password, setPassword] = useState<string>(''); // הוספתי את הסיסמה
  const [username, setUsername] = useState<string>(''); // הוספתי את שם המשתמש
  const [userEmail, setUserEmail] = useState<string>(''); // הוספתי את האימייל
  const navigate = useNavigate();

  const handleRegister = async () => {
    if (!file || !username || !password || !userEmail) {
      message.error("Please fill all fields and upload a resume.");
      return;
    }

    try {
      const response = await registerUser(username, userEmail, password, file); // הוספתי את האימייל כאן
      if (response.token) {
        localStorage.setItem("token", response.token);
        onLogin(response.token);
        message.success("Registration successful! Redirecting to home...");

        setTimeout(() => navigate("/home"), 1500); // ניווט עם עיכוב קצר
      } else {
        message.error("Registration failed. Please try again.");
      }
    } catch (error) {
      console.error("Registration failed. Error:", error);
      message.error("Registration failed. See console for details.");
    }
  };

  return (
    <div style={styles.container}>
      <Card style={styles.card}>
        <Typography.Title level={2} style={styles.title}>
          Register
        </Typography.Title>
        <Form layout="vertical" onFinish={handleRegister} style={styles.form}>
          <Form.Item label="Username" required>
            <Input size="large" value={username} onChange={(e) => setUsername(e.target.value)} />
          </Form.Item>
          <Form.Item label="Email" required>
            <Input size="large" value={userEmail} onChange={(e) => setUserEmail(e.target.value)} />
          </Form.Item>
          <Form.Item label="Password" required>
            <Input.Password size="large" value={password} onChange={(e) => setPassword(e.target.value)} />
          </Form.Item>
          <Form.Item label="Resume">
            <Upload beforeUpload={(file: File) => (setFile(file), false)} showUploadList={false}>
              <Button icon={<UploadOutlined />} size="large">Upload Resume</Button>
            </Upload>
            {file && <Typography.Text style={styles.fileName}>{file.name}</Typography.Text>}
          </Form.Item>
          <Button type="primary" htmlType="submit" size="large" block>
            Register
          </Button>
        </Form>
      </Card>
    </div>
  );
};

export default RegisterForm;
