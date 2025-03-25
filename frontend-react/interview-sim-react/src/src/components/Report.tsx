import React, { useState } from 'react';
const API_URL =  process.env.REACT_APP_API_URL;

const Report: React.FC = () => {
  const [report, setReport] = useState<string | null>(null);

  const fetchReport = async () => {
    try {
      const response = await fetch('http://${API_URL}/interview/report', {
        method: 'GET',
      });

      if (!response.ok) {
        throw new Error('Failed to fetch report');
      }

      const data = await response.json();
      setReport(data.report);
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div>
      <h2>Interview Report</h2>
      <button onClick={fetchReport}>Generate Report</button>
      {report && <div><h3>Report</h3><p>{report}</p></div>}
    </div>
  );
};

export default Report;
