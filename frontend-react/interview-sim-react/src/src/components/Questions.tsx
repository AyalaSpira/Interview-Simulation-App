import React, { useState } from 'react';

const Questions: React.FC = () => {
  const [questions, setQuestions] = useState<string[]>(['What is your experience with React?', 'Why do you want this job?']);
  const [newQuestion, setNewQuestion] = useState('');

  const handleAddQuestion = () => {
    setQuestions([...questions, newQuestion]);
    setNewQuestion('');
  };

  return (
    <div>
      <h2>Interview Questions</h2>
      <ul>
        {questions.map((question, index) => (
          <li key={index}>{question}</li>
        ))}
      </ul>
      <div>
        <input
          type="text"
          value={newQuestion}
          onChange={(e) => setNewQuestion(e.target.value)}
        />
        <button onClick={handleAddQuestion}>Add Question</button>
      </div>
    </div>
  );
};

export default Questions;
