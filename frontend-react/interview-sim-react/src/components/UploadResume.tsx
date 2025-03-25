import { useState } from "react";
import { Upload, Button, message, Card, Typography } from "antd";
import { UploadOutlined } from "@ant-design/icons";
import { uploadResume, uploadNewResume } from "../services/authService";

const { Title, Paragraph } = Typography;

const UploadResume: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const [uploadResult, setUploadResult] = useState<{ ResumeUrl: string; Category: string } | null>(null);
  const [loading, setLoading] = useState(false);
  const [isNewResume, setIsNewResume] = useState(false);

  const handleUpload = async () => {
    if (!file) {
      message.error("Please select a file");
      return;
    }

    setLoading(true);
    try {
      const result = isNewResume ? await uploadNewResume(file) : await uploadResume(file);
      setUploadResult(result);
      message.success("Resume uploaded successfully!");
    } catch {
      message.error("Failed to upload resume");
    }
    setLoading(false);
  };

  return (
    <Card style={{ maxWidth: 400, margin: "auto", textAlign: "center" }}>
      <Title level={3}>{isNewResume ? "Upload New Resume" : "Upload Resume"}</Title>
      
      <Upload beforeUpload={(file) => (setFile(file), false)} showUploadList={false}>
        <Button icon={<UploadOutlined />}>Select File</Button>
      </Upload>
      
      <Button type="primary" onClick={handleUpload} loading={loading} style={{ marginTop: 10 }}>
        {isNewResume ? "Upload New Resume" : "Upload Resume"}
      </Button>

      <Button 
        onClick={() => setIsNewResume(!isNewResume)} 
        style={{ marginTop: 10, marginLeft: 10 }}
      >
        {isNewResume ? "Switch to Regular Resume Upload" : "Switch to New Resume Upload"}
      </Button>

      {uploadResult && (
        <Card style={{ marginTop: 20 }}>
          <Paragraph strong>Resume uploaded successfully!</Paragraph>
          <Paragraph>Category: {uploadResult.Category}</Paragraph>
        </Card>
      )}
    </Card>
  );
};

export default UploadResume;
