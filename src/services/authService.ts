// authService.ts

import { toast } from "react-toastify";

const AUTH_API_URL = process.env.REACT_APP_API_URL?.replace("http://", "https://");


console.log(" API URL:",process.env.REACT_APP_API_URL);

// רישום משתמש חדש
export const registerUser = async (username: string, email: string, password: string, resume: File) => {
  const formData = new FormData();
  formData.append("username", username);
  formData.append("email", email); // הוספתי את האימייל
  formData.append("password", password);
  formData.append("resume", resume);

  const response = await fetch(`${AUTH_API_URL}/auth/register`, {
    method: "POST",
    body: formData,
  });

  return await response.json();
};

// התחברות
export const loginUser = async (email: string, password: string) => {
  try {
    console.log("Sending login request with email:", email, "and password:", password);

    const response = await fetch(`${AUTH_API_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    });

    if (!response.ok) {
      const errorData = await response.json();
      console.error("Login Error:", errorData?.error);
      return { error: errorData?.error || "Login failed" };
    }

    const data = await response.json();
    console.log("Login Response Data:", data);

    if (data.token && data.token !== "Invalid email or password") {
      localStorage.setItem("token", data.token);
      console.log("Token stored in localStorage");
      return { token: data.token };
    } else {
      console.error("Login failed: Invalid email or password");
      return { error: "Invalid email or password" };
    }

  } catch (error) {
    console.error("Error occurred during login:", error);
    return { error: "An unexpected error occurred" };
  }
};

// יציאה (Logout)
export const logoutUser = () => {
  localStorage.removeItem("token");
};

// העלאת קורות חיים
export const uploadResume = async (file: File) => {
  const formData = new FormData();
  formData.append("resume", file);
  const token = localStorage.getItem("token");

  const response = await fetch(`${AUTH_API_URL}/user/upload-resume`, {
    method: "POST",
    body: formData,
    headers: {
      Authorization: `Bearer ${token}`,
    }
  });

  if (!response.ok) throw new Error("Failed to upload resume");
  return response.json();
};

// העלאת קורות חיים חדשים

// העלאת קורות חיים חדשים
export const uploadNewResume = async (file: File) => {
  const formData = new FormData();
  formData.append("resume", file);

  const token = localStorage.getItem("token");

  if (!token) {
    console.error("No token found in localStorage");
    toast.error("לא נמצא טוקן, יש להתחבר מחדש");
    return null;
  }

  try {
    const response = await fetch(`${AUTH_API_URL}/user/upload-resume`, {
      method: "POST",
      body: formData,
      headers: {
        "Authorization": `Bearer ${token}`,
      },
    });

    const contentType = response.headers.get("content-type");

    if (!response.ok) {
      let errorMessage = "Unknown error";
      if (contentType && contentType.includes("application/json")) {
        const errorData = await response.json();
        errorMessage = errorData.error || errorMessage;
      } else {
        const text = await response.text();
        errorMessage = text || errorMessage;
      }
      toast.error(`שגיאה בהעלאת קובץ: ${errorMessage}`);
      throw new Error(`Server Error: ${errorMessage}`);
    }

    if (contentType && contentType.includes("application/json")) {
      const data = await response.json();
      toast.success("קובץ הועלה בהצלחה!");
      return data;
    } else {
      const text = await response.text();
      toast.success("קובץ הועלה בהצלחה (תגובה לא בפורמט JSON)");
      return { message: text };
    }
    
  } catch (error) {
    console.error("Error uploading resume:", error);
    toast.error("אירעה שגיאה כללית בהעלאת קובץ");
    return null;
  }
};
