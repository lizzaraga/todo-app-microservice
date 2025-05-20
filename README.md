# Simple To-Do App

A **Simple To-Do Application** built with **ASP.NET Core** that features JWT authentication, secure task management, and a centralized API Gateway. This project showcases modern development practices by starting with a **monolithic architecture** and transitioning into a **microservices architecture** for better scalability and modularity.

---

## 💡 Features
- **JWT Authentication**: Secure user authentication and authorization via the Auth-Service.
- **Microservices Architecture**:
    - **Auth-Service**: Manages user authentication and authorization.
    - **Todo-Service**: Securely handles to-do entries and task management.
    - **API Gateway**: Central entry point for routing requests to the respective services.
- **Database**: Utilizes **SQLite** with **EF Core** for seamless data persistence and automatic migrations.
- **Service Discovery**: Integrated **Consul** for service registration and health checks.
- **API Documentation**: Swagger/OpenAPI for easy testing and exploration of API endpoints.
- **Error Handling**: Includes global exception handling middleware for consistent and comprehensive error responses.

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [Consul](https://www.consul.io/)

### Clone the Repository
```bash
git clone https://github.com/<your-username>/<your-repo-name>.git
cd <your-repo-name>
```

---

### Running the Monolithic Application
1. Switch to the `main` branch:
   ```bash
   git checkout main
   ```
2. Run the application:
   ```bash
   dotnet run --project ./src/ApiGateway
   ```
3. Access Swagger on: `http://localhost:<port>/swagger`

---

### Running the Microservices Architecture
1. Switch to the `microservices` branch:
   ```bash
   git checkout microservices
   ```
2. Use Docker to build and run the services:
   ```bash
   docker-compose up --build
   ```
3. Access Swagger of the API Gateway on: `http://localhost:<gateway-port>/swagger`

---

## 🛠️ Technologies Used
- **.NET 8**
- **C# 12**
- **ASP.NET Core**
- **Entity Framework Core (EF Core)**
- **SQLite Database**
- **JWT Authentication**
- **Swagger / OpenAPI**
- **Consul for Service Discovery**
- **Docker & Docker Compose**

---


## 📖 Learning Highlights
- Transition from **Monolithic Architecture** to **Microservices Architecture**.
- Implemented **API Gateway** for centralized and secure API routing.
- Hands-on experience with **Consul** for service discovery and health monitoring.
- Improved database management with **SQLite** and **EF Core migrations**.
- Built reusable **global exception handling middleware** for enhanced error responses.

---

## 🌱 What's Next?
The focus will remain on backend development as I explore more advanced backend concepts and build another exciting project!

---

## 🔗 Project Link
[**Check out the repository here!**](https://github.com/lizzaraga/todo-app-microservice)

---

## 🤝 Contributing
Contributions, issues, and suggestions are always welcome! Feel free to open a pull request or submit an issue.

---

## 📝 License
This project is licensed under the [MIT License](LICENSE).

---

## 🙌 Feedback
I’d love to hear your thoughts, feedback, or suggestions on the project. Feel free to connect with me or open an issue to start a discussion!