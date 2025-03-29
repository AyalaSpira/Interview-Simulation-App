// authService.ts
const AUTH_API_URL = process.env.REACT_APP_API_URL;
console.log("API URL:", AUTH_API_URL);

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

  if (!response.ok) {
    throw new Error("Registration failed");
  }

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

  const response = await fetch(`${AUTH_API_URL}/interview/upload-resume`, {
    method: "POST",
    body: formData,
  });

  if (!response.ok) throw new Error("Failed to upload resume");
  return response.json();
};

// העלאת קורות חיים חדשים
export const uploadNewResume = async (file: File) => {
  const formData = new FormData();
  formData.append("resume", file);

  const token = localStorage.getItem("token");

  if (!token) {
    console.error("No token found in localStorage");
    return;
  }

  try {
    const response = await fetch(`${AUTH_API_URL}/auth/upload-new-resume`, {
      method: "POST",
      body: formData,
      headers: {
        "Authorization": `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Server Error: ${errorData.error || "Unknown error"}`);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error uploading resume:", error);
  }
};
