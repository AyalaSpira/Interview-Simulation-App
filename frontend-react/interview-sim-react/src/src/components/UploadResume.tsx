import { useState } from "react";

const API_URL = "http://localhost:5001/api";

const UploadResume: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const [uploadResult, setUploadResult] = useState<{ ResumeUrl: string; Category: string } | null>(null);

  const handleUpload = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!file) {
      alert("Please select a file");
      return;
    }

    const formData = new FormData();
    formData.append("resume", file);

    try {
      const response = await fetch(`${API_URL}/interview/upload-resume`, {
        method: "POST",
        body: formData,
      });

      if (!response.ok) throw new Error("Failed to upload resume");

      const result = await response.json();
      setUploadResult(result);
    } catch (error) {
      console.error("Error uploading resume:", error);
    }
  };

  return (
    <div>
      <h2>Upload Resume</h2>
      <form onSubmit={handleUpload}>
        <input type="file" onChange={(e) => setFile(e.target.files?.[0] || null)} />
        <button type="submit">Upload</button>
      </form>

      {uploadResult && <p>Resume uploaded! Category: {uploadResult.Category}</p>}
    </div>
  );
};

export default UploadResume;
