import { useState } from "react";
import { Button } from "antd";
import InterviewReportViewer from "./InterviewReportViewer";

const ReportTextViewer = ({ interviewId }: { interviewId: number }) => {
  const [interviewText, setInterviewText] = useState<string | null>(null);
  const [showReport, setShowReport] = useState(false);

  const fetchReportText = async () => {
    try {
      const response = await fetch(`http://localhost:5000/api/interview/get-text-report?interviewId=${interviewId}`);
      if (!response.ok) throw new Error("Failed to fetch report.");
      const text = await response.text();
      setInterviewText(text);
      setShowReport(true);
    } catch (error) {
      console.error("Error fetching report:", error);
    }
  };

  return (
    <div style={{ textAlign: "center", marginTop: 20 }}>
      {!showReport ? (
        <Button onClick={fetchReportText} type="primary">
          הצג דוח טקסט
        </Button>
      ) : (
        interviewText && <InterviewReportViewer reportText={interviewText} />
      )}
    </div>
  );
};

export default ReportTextViewer;
