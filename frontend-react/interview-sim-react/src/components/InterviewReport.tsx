import React, { useState } from "react";
import { sendInterviewReport } from "../services/InterviewService";  // מייבא את הפונקציה מהשירות

interface InterviewReportProps {
  interviewId: number | null;
}

const InterviewReport: React.FC<InterviewReportProps> = ({ interviewId }) => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSendReport = async () => {
    if (!interviewId) {
      setError("Invalid interview ID");
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const response = await sendInterviewReport(interviewId);
      console.log(response);
      alert("Report sent successfully!");  // הודעה למשתמש על שליחה מוצלחת
    } catch (err) {
      setError("Failed to send report. Please try again.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Interview Report</h2>
      <button onClick={handleSendReport} disabled={loading}>
        {loading ? "Sending..." : "Send Report"}
      </button>
      {error && <p>{error}</p>}
    </div>
  );
};

export default InterviewReport;
