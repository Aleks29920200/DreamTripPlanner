✈️ Dream Trip Planner
A full-stack web application built with C# and ASP.NET Core, designed to help users organize, discover, and plan their ultimate travel destinations. This project demonstrates proficiency in the .NET ecosystem, the MVC design pattern, and automated CI/CD workflows.

🚀 Key Features
Trip Organization: Plan and manage travel itineraries, destinations, and dates.

User Interface: Responsive front-end built with HTML, CSS, and vanilla JavaScript, integrated directly into the ASP.NET views.

Continuous Integration / Continuous Deployment (CI/CD): * Fully configured for automated builds and testing using Azure Pipelines (azure-pipelines.yml).

Ensures code quality and seamless deployment workflows.

🛠️ Technical Stack
Backend Language: C# (60.6%)

Framework: ASP.NET Core MVC (Web Application)

Frontend: HTML5 (37.1%), CSS3 (2.2%), JavaScript (0.1%)

IDE/Tooling: Visual Studio (.sln and .vs structure)

DevOps / CI/CD: Azure DevOps / Azure Pipelines

🏗️ Architecture
This project follows the standard ASP.NET Core MVC (Model-View-Controller) architecture:

Models: C# classes representing the data domain (e.g., Trips, Destinations).

Views: Razor syntax (.cshtml) / HTML files responsible for rendering the UI and presenting data to the user.

Controllers: C# classes that handle incoming HTTP requests, process user input, interact with data models, and return the appropriate views.

🔧 Setup & Local Development
To run this project locally, you will need Visual Studio (or Visual Studio Code with the C# Dev Kit) and the .NET SDK.

Clone the repository:

Bash
git clone https://github.com/Aleks29920200/DreamTripPlanner.git
cd DreamTripPlanner
Open the Solution:

Double-click the DreamTripPlanner.sln file to open the project in Visual Studio.

Restore Dependencies:

Visual Studio should automatically restore the required NuGet packages. If using the .NET CLI, run:

Bash
dotnet restore
Build and Run:

Press F5 or click the Start button in Visual Studio to launch the application with the debugger attached.

Alternatively, via the .NET CLI:

Bash
dotnet run --project WebApplication1
Access the application:

The application will launch in your default web browser (typically at https://localhost:5001 or http://localhost:5000).

☁️ Azure Pipelines Configuration
This repository includes several YAML files (e.g., azure-pipelines.yml, azure-pipelines-1.yml) to support Azure DevOps automation. These pipelines are configured to automatically build the .NET solution, run tests, and prepare artifacts whenever new code is pushed to the repository.
