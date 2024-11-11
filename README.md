# RiK_Test
Steps to Install

Clone the Repository

Navigate to the project directory or open with visual studio

Choose .Server as startup item and run on https on visual studio

This application is built using a Blazor WebAssembly frontend and a .NET backend with a SQLite database. The architecture follows a layered approach with distinct roles for each layer.

Layers

Presentation Layer (Blazor WebAssembly):
This layer is responsible for the user interface and user interactions.
The Blazor components interact with the backend API and display data to the user.
Components include pages for creating, viewing, and managing participants and events.

API Layer (.NET Web API):
The API layer acts as a bridge between the frontend and the business logic.
It provides endpoints for CRUD operations on entities like Participant and Event.

Business Logic Layer (Services):
This layer contains the core application logic, housed in service classes.
Services handle business rules, validation, and any complex operations, such as registering participants to events.
Service classes depend on repository interfaces to interact with the data layer.

Data Access Layer (Repositories):
Repositories manage database interactions, abstracting away the details of Entity Framework Core operations.
Each entity has a dedicated repository (e.g., EventRepository, ParticipantRepository) to encapsulate data access logic.

Database Layer (SQLite):
The database stores entities Participant, Event, and EventParticipant (join table for many-to-many relationships).
Entity Framework Core is used to manage the database schema, migrations, and data access.

Code Structure
The project is organized into different folders, each corresponding to the main layers of the application:

Pages: Contains Blazor components representing different pages in the application.
ParticipantList.razor, EventList.razor, etc.

Services: Houses the business logic classes that interact with repositories.
Example: EventService, ParticipantService

Repositories: Manages data access logic, with one repository per entity.
Example: EventRepository, ParticipantRepository

Models: Defines the main data models and DTOs used across the application.
Example: Participant, Event, EventParticipant (join table)

Data: Contains the AppDbContext for database context configuration and migration management.

Notes: There are some issues with the css and styling it is not as it should be and error handling could be more elegant
