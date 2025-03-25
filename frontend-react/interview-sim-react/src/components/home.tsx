import React from "react";
import { Link } from "react-router-dom";
import { Container, Card, CardContent, Typography, Button, Grid } from "@mui/material";

interface HomeProps {
  onLogout: () => void;
}

const Home: React.FC<HomeProps> = ({ onLogout }) => {
  return (
    <Container maxWidth="sm">
      <Card
        sx={{
          mt: 5,
          p: 3,
          borderRadius: 3,
          boxShadow: 6,
          background: "linear-gradient(135deg, #00c9a7, #5cb85c)",
          textAlign: "center",
          color: "#fff",
        }}
      >
        <CardContent>
          <Typography variant="h4" sx={{ fontWeight: "bold", mb: 2 }}>
            Welcome to the Interview Simulation App!
          </Typography>
          <Typography variant="body1" sx={{ mb: 3 }}>
            Navigate through the app
          </Typography>

          <Grid container spacing={2} justifyContent="center">
            <Grid item>
              <Button variant="contained" component={Link} to="/upload-resume" sx={{ color: "#fff" }}>
                Upload Resume
              </Button>
            </Grid>
            <Grid item>
              <Button variant="contained" component={Link} to="/interview" sx={{ color: "#fff" }}>
                Start Interview
              </Button>
            </Grid>
            <Grid item>
              <Button variant="contained" component={Link} to="/report" sx={{ color: "#fff" }}>
                View Report
              </Button>
            </Grid>
            <Grid item>
              <Button
                onClick={onLogout}
                variant="outlined"
                sx={{
                  mt: 3,
                  color: "#fff",
                  borderColor: "#fff",
                  "&:hover": {
                    borderColor: "#d4ffeb",
                    backgroundColor: "rgba(255, 255, 255, 0.2)",
                  },
                }}
              >
                Logout
              </Button>
            </Grid>
          </Grid>
        </CardContent>
      </Card>
    </Container>
  );
};

export default Home;
