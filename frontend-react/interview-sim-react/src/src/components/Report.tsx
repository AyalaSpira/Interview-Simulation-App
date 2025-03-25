import React, { useState } from 'react';

const Report: React.FC = () => {
  const [report, setReport] = useState<string | null>(null);

  const fetchReport = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/interview/report', {
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
