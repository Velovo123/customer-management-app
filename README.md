# Customer Management Application

This is a simple **Customer Management Application** built using **ASP.NET Core** and **MongoDB** for managing customer data. The application follows a layered architecture consisting of the **Repository**, **Service**, and **Controller** layers. Each layer has its specific responsibilities.

## Features
- **Customer Management**: Manage customers by adding, editing, viewing, and deleting customer records.
- **MVC Architecture**: Uses Model-View-Controller (MVC) pattern to separate concerns in the application.
- **Unit Testing**: Includes unit tests for the repository, service, and controller layers to ensure the reliability and correctness of the application.
  
## Layers Breakdown

### Repository Layer (`CustomerRepository`)
- **Responsibilities**: 
  - Handles data access operations.
  - Retrieves and manipulates data from the MongoDB database.
  - Does not include any additional logic such as validation or business rules.

### Service Layer (`CustomerService`)
- **Responsibilities**:
  - Handles business logic and validation.
  - Ensures data integrity, applies business rules, and prepares data for the repository layer.
  - Includes logic encapsulated in `CustomerService`.

### Controller Layer (`CustomerController`)
- **Responsibilities**:
  - Performs additional validation using `ModelState.IsValid` to ensure that incoming data meets required criteria before passing it to the service layer.
  - Defines the endpoints for managing customers: `Index`, `Details`, `Create`, `Edit`, and `Delete`.
 
### GuidSerializationProvider Explanation
The `GuidSerializationProvider` class is used to customize the serialization and deserialization of `Guid` values in MongoDB. 

- It implements the `IBsonSerializationProvider` interface, which allows for the customization of how specific types, like `Guid`, are serialized into BSON format.
- When MongoDB tries to serialize a `Guid`, the `GetSerializer` method checks if the type is `Guid`. If it is, it returns a `GuidSerializer` with the `GuidRepresentation.Standard`. This ensures that `Guid` values are stored in the standard format.
- If the type isn't `Guid`, the method returns `null`, meaning no custom serializer is applied for other types.

In short, this class ensures that `Guid` values are serialized using the `GuidRepresentation.Standard` format when interacting with MongoDB.


## Getting Started

### Prerequisites
Before you can run this project locally, make sure you have the following installed:

- **.NET 8**: You can download it from the official [Microsoft website](https://dotnet.microsoft.com/download/dotnet/8.0).
- **MongoDB**: Make sure you have MongoDB installed and running locally, or use a remote MongoDB instance. You can download MongoDB from [here](https://www.mongodb.com/try/download/community).

### Installation

1. **Clone the repository**:
   Clone the project repository to your local machine using Git:
   ```bash
   git clone https://github.com/Velovo123/customer-management-app.git
   
2. **Navigate to the project directory**
   Change to the project folder:
   ```bash
   cd customer-management-app
3. **Restore the dependencies**
   Run the following command to restore the NuGet packages:
   ```bash
   dotnet restore
   
4. **Configure MongoDB connection**
   Update the `appsettings.json` file with your MongoDB connection string:
   ```json
   {
     "MongoSettings": {
       "ConnectionString": "your_mongo_connection_string_here",
       "DatabaseName": "CustomerDB"
     }
   }
   
5. **Run the application**
   After setting up your MongoDB connection, start the application by running:
   ```bash
   dotnet run

6. **Verify the application**
   Open your browser and navigate to `http://localhost:XXXX` to check if the application is running as expected. You should see the main page of the customer management application.

### Running Tests
To run the unit tests for the project, use the following command(xUnit was used for writing and running the tests):
```bash
dotnet test

