✈️ Dream Trip Planner Web Application
A full-stack Java web application built with Spring Boot, designed to help users organize, discover, and plan their ultimate travel destinations. This project showcases practical implementation of Spring MVC, relational database mapping, and secure user session management.

🚀 Key Features
User Authentication: * Secure Registration and Login system with password hashing.

Route protection ensuring only logged-in users can create or modify trips.

Trip Management:

Users can add new dream trips, specifying details such as Destination, Description, Departure Date, and Category.

Trips are logically categorized (e.g., Beach, Mountain, City Break, Cultural).

Interactive Dashboard:

A dynamic home page displaying all shared trips from the community.

Users can view trip details, track estimated costs, or save specific itineraries to their personal list.

Personalized Collection:

A dedicated section where users can view the trips they have personally added or planned.

Functionality to remove or mark trips as "Completed".

Database Seeding: * Automatically initializes the database with required categories and roles upon the first application startup.

🛠️ Technical Stack
Backend: Java 11/17+, Spring Boot

Data Access: Spring Data JPA, Hibernate, MySQL

Template Engine: Thymeleaf, HTML5, CSS3, Bootstrap

Validation: Hibernate Validator (JSR 380 / Bean Validation)

Security: Custom Session-based Interceptors for user state management

🏗️ Architecture
The application strictly adheres to a Layered MVC Architecture to ensure clean separation of concerns and maintainability:

web (Controllers): Handles incoming HTTP requests, binds form data, and returns the appropriate Thymeleaf views.

service: Contains the core business logic, mapping between database entities and front-end Data Transfer Objects (DTOs).

repository: Interfaces extending JpaRepository for seamless, boilerplate-free database CRUD operations.

model:

Entities: Java classes mapped directly to MySQL tables (User, Trip, Category).

BindingModels: Objects used to capture and validate user input from HTML forms.

ViewModels: Objects formatted specifically for rendering data securely on the front end.

init: CommandLineRunner components used for initial database seeding.

🔧 Setup & Installation
Clone the repository:

Bash
git clone https://github.com/Aleks29920200/DreamTripPlanner.git
cd DreamTripPlanner
Configure the Database:

Open your MySQL environment and create a schema named dream_trip_planner (or the name specified in your properties).

Open src/main/resources/application.properties.

Update spring.datasource.username and spring.datasource.password with your local MySQL credentials.

Build and Run:

Run the application via your preferred IDE (IntelliJ IDEA / Eclipse) or use the Maven wrapper in the terminal:

Bash
mvn spring-boot:run
Access the application:

Open your web browser and navigate to http://localhost:8080.

📋 Data Constraints & Validation
Trips: Destination names must meet minimum length requirements. Departure dates cannot be in the past.

Categories: Selected from a predefined, seeded list ensuring data consistency.

Users: Must provide unique usernames and valid email formats during registration.
