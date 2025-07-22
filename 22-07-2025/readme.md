# 🚀 Azure Pipelines for .NET App Build & Docker Push

- 📦 **Pipeline 1**: Build and test a .NET project.
- 🐳 **Pipeline 2**: Build a Docker image of the .NET app and push it to Docker Hub.

## 🔨 Step 1: Create a Git Repository and Push Your .NET Project

> ![Create Git Repo](./images/git-repo.png)

## 🔧 Step 2: Setup a Self-Hosted Azure Pipeline Agent

- Set up an self hosted agent in my local machine to run jobs because azure hosted agents are not free.

> ![Agent config](./images/agent-config.png)

> ![Agen run](./images/agent-runn.png)

## ⚙️ Step 4: Setup Azure Pipeline to Build .NET App (Pipeline 1)

> ![Dotnet Pipeline](./images/dotnet-build-job.png)

## 🐳 Step 5: Write a Dockerfile

> ![Docker file](./images/docker-file.png)

## 🔁 Step 6: Setup Azure Pipeline to Build & Push Docker Image to Docker Hub (Pipeline 2)

> ![Docker Pipeline](./images/docker-build-job.png)

## 📌 Outputs

> ![Pipelines](./images/pipelines.png)

> ![Pipeline running](./images/pipelines-running.png)

> ![Docker hub](./images/dockerhub-image.png)

