const express = require("express");
const cors = require('cors');

const app = express();
app.use(cors());

app.get("/api", (req, res) => {
    res.status(200).json({ message: "Hello World!!" });
});

app.listen(3000, () => console.log("API running on port 3000"));