# Movie and Book Management Application

This is an ASP.NET MVC web application for managing movies, directors, books, authors and genres. It allows users to create, edit, delete, and view movies with associated directors and genres. The application also provides functionality for handling movie-genre and book-author relationships and managing directors.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete movies and books.
- **Director and Author Management**: Associate movies and books with directors and authors, and create new ones if needed.
- **Genre Management**: Add and manage genres associated with each movie or book.
- **Movie-Genre/Book-Author Relationships**: Manage and update movie-genre or book-author associations.
- **Database Support**: Uses Entity Framework Core for database interactions.
- **ASP.NET MVC**: Built using the Model-View-Controller pattern for clean, maintainable code.

## Requirements

- .NET 9
- SQL Server or a compatible database
- Visual Studio or another C# development environment

## Installation

1. Clone this repository to your local machine:

   ```bash
   git clone https://github.com/f8developer/ProjectMovieBook
   ```
2. Update the connection string inside of **appsettings.json**. You can also leavi it as is if you are using Visual Studio SQL.
    ```json
    "DefaultConnection": "..."
    ```
3. Open the **Package Manager Console** or **Terminal/PowerShell** and run:
    - **Package Manager Console**
        ```bash
        Update-Database
        ```
    - **Terminal/PowerShell**
        ```bash
        dotnet ef database update
        ```
4. Run the program:
    ```bash
    dotnet run
    ```

## FAQ
- If you have a problem with the Built-In Visual Studio database the most common solution is:
    - https://learn.microsoft.com/en-us/troubleshoot/sql/database-engine/database-file-operations/troubleshoot-os-4kb-disk-sector-size
