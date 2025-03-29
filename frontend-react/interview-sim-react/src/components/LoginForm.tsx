import { useState, useEffect } from "react";
import { Form, Input, Button, message, Card, Typography } from "antd";
import { loginUser } from "../services/authService";  // שירות חדש
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
    backdropFilter: "blur(15px)",
    boxShadow: "0 4px 10px rgba(0, 0, 0, 0.2)",
    backgroundColor: "rgba(255, 255, 255, 0.2)",
    color: "#fff",
  },
  title: {
    color: "#fff",
    fontWeight: "bold",
  },
  form: {
    textAlign: "left",
  },
};

interface LoginFormProps {
  onLogin: (token: string) => void;
}

const LoginForm: React.FC<LoginFormProps> = ({ onLogin }) => {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      navigate("/home");
    }
  }, [navigate]);

  const handleLogin = async (values: { email: string; password: string }) => {
    console.log("API URL:",process.env.VITE_API_URL);
    console.log("API URL:", process.env.VITE_API_URL);
    setLoading(true);

    console.log("Form values:", values);  // לוג כדי לראות את הנתונים שנכנסו

    const response = await loginUser(values.email, values.password);  // שינוי מ-username ל-email
    setLoading(false);

    if (response.error) {
      // לוג אם יש שגיאה בתשובה
      console.error("Login failed with error:", response.error);
      message.error(response.error);
      navigate("/register");
      return;
    }

    if (response.token) {
      console.log("Login successful, token received:", response.token);
      localStorage.setItem("token", response.token);
      onLogin(response.token); // עדכון הסטייט
      message.success("Login successful!");
      navigate("/home");
    } else {
      message.error("Login failed. Please try again.");
    }
  };

  return (
    <div style={styles.container}>
      <Card style={styles.card}>
        <Typography.Title level={2} style={styles.title}>
          Login
        </Typography.Title>
        <Form layout="vertical" onFinish={handleLogin} style={styles.form}>
          <Form.Item
            label="Email"
            name="email"
            rules={[{ required: true, message: 'Email is required' }]}  // אם אימייל ריק, יצא הודעת שגיאה
          >
            <Input size="large" />
          </Form.Item>
          <Form.Item
            label="Password"
            name="password"
            rules={[{ required: true, message: 'Password is required' }]}>
            <Input.Password size="large" />
          </Form.Item>
          <Button type="primary" htmlType="submit" size="large" block loading={loading}>
            Login
          </Button>
        </Form>
      </Card>
    </div>
  );
}

export default LoginForm;
