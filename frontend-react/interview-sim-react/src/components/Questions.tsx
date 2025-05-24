"use client"

import type React from "react"
import { useState } from "react"
import { Card, Typography, Input, Button, List, Divider } from "antd"
import { motion } from "framer-motion"
import { Plus, MessageSquare } from "lucide-react"

const { Title, Text } = Typography

const Questions: React.FC = () => {
  const [questions, setQuestions] = useState<string[]>([
    "What is your experience with React?",
    "Why do you want this job?",
  ])
  const [newQuestion, setNewQuestion] = useState("")

  const handleAddQuestion = () => {
    if (newQuestion.trim() === "") return
    setQuestions([...questions, newQuestion])
    setNewQuestion("")
  }

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      style={{ padding: "20px", maxWidth: "800px", margin: "0 auto" }}
    >
      <Card
        style={{
          background: "rgba(30, 41, 59, 0.7)",
          borderRadius: "24px",
          boxShadow: "0 20px 80px rgba(0, 0, 0, 0.3)",
          border: "1px solid rgba(255, 255, 255, 0.1)",
          backdropFilter: "blur(20px)",
          overflow: "hidden",
        }}
      >
        <Title
          level={2}
          style={{
            color: "#fff",
            textAlign: "center",
            marginBottom: "24px",
            fontWeight: 700,
            background: "linear-gradient(90deg, #a855f7, #3b82f6)",
            backgroundClip: "text",
            WebkitBackgroundClip: "text",
            WebkitTextFillColor: "transparent",
          }}
        >
          Interview Questions
        </Title>

        <Text
          style={{
            color: "rgba(255, 255, 255, 0.7)",
            fontSize: "1.1rem",
            display: "block",
            textAlign: "center",
            marginBottom: "30px",
          }}
        >
          Manage your interview questions library
        </Text>

        <div
          style={{
            background: "rgba(15, 23, 42, 0.6)",
            borderRadius: "16px",
            padding: "24px",
            marginBottom: "30px",
          }}
        >
          <List
            itemLayout="horizontal"
            dataSource={questions}
            renderItem={(question, index) => (
              <motion.div
                initial={{ opacity: 0, x: -10 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ delay: index * 0.1, duration: 0.3 }}
              >
                <List.Item
                  style={{
                    padding: "16px",
                    borderRadius: "12px",
                    background: "rgba(30, 41, 59, 0.5)",
                    marginBottom: "12px",
                    border: "1px solid rgba(99, 102, 241, 0.2)",
                  }}
                >
                  <List.Item.Meta
                    avatar={
                      <div
                        style={{
                          width: "36px",
                          height: "36px",
                          borderRadius: "50%",
                          background: "rgba(99, 102, 241, 0.2)",
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "center",
                        }}
                      >
                        <MessageSquare size={18} style={{ color: "#a855f7" }} />
                      </div>
                    }
                    title={<Text style={{ color: "#fff", fontWeight: 600 }}>Question {index + 1}</Text>}
                    description={<Text style={{ color: "rgba(255, 255, 255, 0.7)" }}>{question}</Text>}
                  />
                </List.Item>
              </motion.div>
            )}
          />
        </div>

        <Divider style={{ borderColor: "rgba(255, 255, 255, 0.1)" }} />

        <div style={{ display: "flex", gap: "12px" }}>
          <Input
            value={newQuestion}
            onChange={(e) => setNewQuestion(e.target.value)}
            placeholder="Type a new interview question..."
            style={{
              flex: 1,
              height: "50px",
              borderRadius: "12px",
              background: "rgba(15, 23, 42, 0.6)",
              border: "1px solid rgba(99, 102, 241, 0.3)",
              color: "#fff",
              fontSize: "1rem",
            }}
            onPressEnter={handleAddQuestion}
          />
          <Button
            onClick={handleAddQuestion}
            type="primary"
            style={{
              height: "50px",
              borderRadius: "12px",
              background: "linear-gradient(90deg, #a855f7, #3b82f6)",
              border: "none",
              fontSize: "1rem",
              fontWeight: 600,
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              width: "auto",
              padding: "0 20px",
            }}
            icon={<Plus size={18} style={{ marginRight: "8px" }} />}
          >
            Add
          </Button>
        </div>
      </Card>
    </motion.div>
  )
}

export default Questions
