# Arcadia

Arcadia is a versatile boilerplate that combines **Blazor** with a **Web API**, designed to kickstart a variety of projects across businesses and industries. It provides a modular and extensible foundation, offering out-of-the-box functionality for tasks ranging from CMS development and customer portals to reporting tools and IT utilities. With Arcadia, you can build robust, scalable solutions faster while integrating powerful features often found in IT systems.

## Features

- **Blazor WebAssembly**: Client-side web UI framework for building interactive web applications with C#.
- **ASP.NET Core Web API**: Backend API for managing data and business logic.
- **Shared Components**: Common models and services shared between the client and server for consistency and efficiency.
- **Docker Support**: Includes Dockerfiles and Compose configurations for containerized deployment.
- **Modular Design**: Easily extend or customize to fit specific project requirements.

## What Arcadia Can Do

Arcadia provides tools and features to streamline development across a broad range of applications:

- **Content Management Systems (CMS)**: Build and manage rich, flexible content-driven websites.
- **Customer Portals**: Create user-friendly dashboards for customers or employees.
- **Systems Monitoring and Diagnostics**: Integrate monitoring tools to track performance and resolve issues efficiently.
- **Custom Reporting**: Generate detailed reports with customizable layouts and data sources.
- **Automation and Workflows**: Simplify repetitive tasks and streamline business processes.
- **Utility Functions**: Perform tasks like log parsing, file compression, and data validation.
- **Integration-Ready**: Connect seamlessly with external APIs, databases, or third-party systems.
- **Adaptable for Any Project**: From IT toolboxes to e-commerce platforms, Arcadia is flexible enough to handle it all.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for containerization)

## Getting Started

1. **Clone the Repository**:

```bash
git clone https://github.com/repasscloud/Arcadia.git
cd Arcadia
```

2. **Build and Run the Application**:

- Using Visual Studio:
  - Open `Arcadia.sln`
  - Set the startup projects to `Arcadia.API` and `Arcadia.WebApp`
  - Press F5 to build and run the solution

- Using Command Line:

```bash
dotnet build
dotnet run --project Arcadia.API
dotnet run --project Arcadia.WebApp
```

3. **Access the Application**:

- Navigate to `http://localhost:5000` to access the Blazor WebAssembly application.
- The Web API will be running at `http://localhost:5001`.

## Docker Deployment

1. **Build Docker Images**:

```bash
docker compose build
```

2. **Run Containers**:

```bash
docker compose up
```

3. **Access the Application**:

- Navigate to `http://localhost:5000` to access the Blazor WebAssembly application.
- The Web API will be running at `http://localhost:5001`.

## Project Structure

- **Arcadia.API**: ASP.NET Core Web API project
- **Arcadia.WebApp**: Blazor WebAssembly project.
- **Arcadia.Shared**: Shared models and services between the client and server.
- **Dockerfile.API**: Dockerfile for the API project.
- **Dockerfile.WebApp**: Dockerfile for the WebApp project.
- **compose.yaml**: Docker Compose configuration file.

## Why Choose Arcadia?

- **Out-of-the-Box Functionality**: Get started quickly with ready-made features that save time.
- **Highly Customizable**: Adapt Arcadia to meet the unique needs of your project or business.
- **Broad Applicability**: Whether building a CMS, customer portal, IT toolbox, or something entirely different, Arcadia provides the tools to make it happen.
- **Focus on Extensibility**: Add or modify modules to grow with your project's needs.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.