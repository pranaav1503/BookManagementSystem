# Book Management System

## Overview
The **Book Management System** is a web application built using **ASP.NET Core MVC**. It allows users to manage books with features such as adding, editing, viewing, and deleting book records. The application also supports image uploads for book covers and calculates prices after applying discounts.

---

## Project Structure
The project follows a standard **ASP.NET Core MVC** architecture with a well-organized folder structure:

### **1. Controllers**
- Contains the logic for handling HTTP requests and responses.
- **BooksController**: Manages CRUD operations for books.

### **2. Models**
- Defines the structure of the data used in the application.
- **Book.cs**:
  - Represents a book entity with properties like `Title`, `Author`, `Price`, `ImageUrl`, etc.
  - Includes a calculated property `PriceAfterDiscount`.

### **3. Views**
- Contains Razor views for the user interface.
- Organized into folders for each controller (e.g., `Views/Books`).
- Key Views:
  - `Index.cshtml`: Displays a list of books.
  - `Details.cshtml`: Shows detailed information about a book.
  - `Create.cshtml`: Form for adding a new book.
  - `Edit.cshtml`: Form for editing an existing book.

### **4. Services**
- Contains the business logic for interacting with the database.
- **IBookService** and **BookService**:
  - Encapsulate operations like adding, updating, deleting, and retrieving books.

### **5. wwwroot**
- Static files such as CSS, JavaScript, and uploaded images.
- `wwwroot/images`: Stores uploaded book cover images.

### **6. Data**
- Uses **SQLite** for data persistence.
- Migrations are managed using **Entity Framework Core**.

---

## Steps to Run the Application

### **1. Prerequisites**
Ensure the following are installed on your system:
- **.NET SDK** (Version 8.0)
- **SQLite** (optional, for database exploration)
- An IDE such as **Visual Studio** or **Visual Studio Code**

### **2. Clone the Repository**
```bash
git clone <repository_url>
cd BookManagementSystem
```

### **3. Configure the Database**
1. The application uses **SQLite** as its database.
2. Ensure the connection string in `appsettings.json` points to the desired location for the database file:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Data Source=bookmanagement.db"
   }
   ```
3. Apply migrations to create the database schema:
   ```bash
dotnet ef database update
   ```

### **4. Run the Application**
1. Build and run the application:
   ```bash
dotnet run
   ```
2. Open a browser and navigate to:
   ```
   http://localhost:{5000} {Change the port {{5000}} as needed}
   ```

### **5. Key Features to Test**
- **Add Book**: Navigate to `/Books/Create` to add a new book.
- **Edit Book**: Navigate to `/Books/Edit/{id}` to edit an existing book.
- **View Book Details**: Navigate to `/Books/Details/{id}` to view details of a book.
- **Delete Book**: Navigate to `/Books/Delete/{id}` to delete a book.

---

## Notes
- Ensure the `wwwroot/images` folder has appropriate permissions to store uploaded images.
- For deployment, update the database connection string and configure the hosting environment accordingly.

---

## Future Enhancements
- Implement user authentication and role-based access control.
- Deploy to Azure App Service or another hosting platform.

