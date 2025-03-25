import React from "react";
import useTypingEffect from "../hooks/useTypingEffect";
// מחזיר טקסט עם אנימציה של הקלדה

interface InterviewReportViewerProps {
  reportText: string;
}

const InterviewReportViewer: React.FC<InterviewReportViewerProps> = ({ reportText }) => {
  const typedReport = useTypingEffect(reportText, 30); // מציג את הטקסט בהדרגתיות

  return (
    <pre style={{ whiteSpace: "pre-wrap", textAlign: "left", padding: "10px", border: "1px solid #ccc", borderRadius: "5px" }}>
      {typedReport}
    </pre>
  );
};

export default InterviewReportViewer;
