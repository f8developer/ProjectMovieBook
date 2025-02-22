### ProjectMovieBook Documentation

#### Book Author

- **Author.Id**  
  *Description:* The ID of the author.

- **Author.Name**  
  *Description:* The name of the author.

- **Author.Books**  
  *Description:* The list of books written by the author.

#### Book

- **Book.Id**  
  *Description:* The ID of the book.

- **Book.Title**  
  *Description:* The title of the book.

- **Book.AuthorId**  
  *Description:* The ID of the author of the book.

- **Book.Author**  
  *Description:* The author of the book.

- **Book.BookGenres**  
  *Description:* The list of genres associated with the book.

- **Book.Rating**  
  *Description:* The rating of the book (out of 5).

#### BookGenre

- **BookGenre.BookId**  
  *Description:* The ID of the book.

- **BookGenre.Book**  
  *Description:* The book associated with the genre.

- **BookGenre.GenreId**  
  *Description:* The ID of the genre.

- **BookGenre.Genre**  
  *Description:* The genre associated with the book.

#### Genre

- **Genre.Id**  
  *Description:* The ID of the genre.

- **Genre.Name**  
  *Description:* The name of the genre.

- **Genre.BookGenres**  
  *Description:* The list of books associated with the genre.

- **Genre.MovieGenres**  
  *Description:* The list of movies associated with the genre.

#### Movie Director

- **Director.Id**  
  *Description:* The ID of the director.

- **Director.Name**  
  *Description:* The name of the director.

- **Director.Movies**  
  *Description:* The list of movies directed by the director.

#### Movie

- **Movie.Id**  
  *Description:* The ID of the movie.

- **Movie.Title**  
  *Description:* The title of the movie.

- **Movie.DirectorId**  
  *Description:* The ID of the director of the movie.

- **Movie.Director**  
  *Description:* The director of the movie.

- **Movie.MovieGenres**  
  *Description:* The list of genres associated with the movie.

- **Movie.Rating**  
  *Description:* The rating of the movie (out of 5).

#### MovieGenre

- **MovieGenre.MovieId**  
  *Description:* The ID of the movie.

- **MovieGenre.Movie**  
  *Description:* The movie associated with the genre.

- **MovieGenre.GenreId**  
  *Description:* The ID of the genre.

- **MovieGenre.Genre**  
  *Description:* The genre associated with the movie.

### ApplicationDbContext Documentation

#### Overview
`ApplicationDbContext` is a custom `DbContext` for the `ProjectMovieBook` application, inheriting from `IdentityDbContext`. It contains `DbSet` properties for the main entities like `Book`, `Movie`, `Author`, `Director`, `Genre`, and their relationship models like `BookGenre` and `MovieGenre`.

#### DbSets

- **Books**  
  *Type:* `DbSet<Book>`  
  *Description:* Represents the collection of `Book` entities in the database.

- **Movies**  
  *Type:* `DbSet<Movie>`  
  *Description:* Represents the collection of `Movie` entities in the database.

- **Authors**  
  *Type:* `DbSet<Author>`  
  *Description:* Represents the collection of `Author` entities in the database.

- **Directors**  
  *Type:* `DbSet<Director>`  
  *Description:* Represents the collection of `Director` entities in the database.

- **Genres**  
  *Type:* `DbSet<Genre>`  
  *Description:* Represents the collection of `Genre` entities in the database.

- **BookGenres**  
  *Type:* `DbSet<BookGenre>`  
  *Description:* Represents the collection of `BookGenre` entities in the database, which establishes a many-to-many relationship between `Book` and `Genre`.

- **MovieGenres**  
  *Type:* `DbSet<MovieGenre>`  
  *Description:* Represents the collection of `MovieGenre` entities in the database, which establishes a many-to-many relationship between `Movie` and `Genre`.

#### Relationships Configured in OnModelCreating

- **Book and Genre (Many-to-Many through BookGenre)**  
  - A composite key is created for `BookGenre` using `BookId` and `GenreId`.  
  - A `Book` can have multiple genres, and a `Genre` can be associated with multiple books. The `BookGenre` entity serves as a bridge table for this many-to-many relationship.  
  - Cascade delete behavior is configured for both sides of the relationship.

- **Movie and Genre (Many-to-Many through MovieGenre)**  
  - A composite key is created for `MovieGenre` using `MovieId` and `GenreId`.  
  - A `Movie` can have multiple genres, and a `Genre` can be associated with multiple movies. The `MovieGenre` entity serves as a bridge table for this many-to-many relationship.  
  - Cascade delete behavior is configured for both sides of the relationship.

- **Author and Book (One-to-Many)**  
  - An author can have multiple books, and each book is associated with one author.  
  - Cascade delete behavior is configured to delete all books related to an author when the author is deleted.

- **Director and Movie (One-to-Many)**  
  - A director can have multiple movies, and each movie is associated with one director.  
  - Cascade delete behavior is configured to delete all movies related to a director when the director is deleted.

#### Constructor

- **ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)**  
  *Parameters:*  
  - `options`: The options for configuring the `DbContext` in a specific way (e.g., connection string).  
  *Description:* Initializes the context with the provided options, passing them to the base `IdentityDbContext` class.

### Controller Actions and Details:

#### 1. **Index** (GET: `/Books`)
- **Purpose:** Displays a list of books. Optionally filters books based on a search query (`searchQuery`).
- **Includes:**  
  - Author data  
  - BookGenres (many-to-many join between books and genres) and the Genre data  
- **Search Filtering:** Books are filtered by:
  - Title, Author Name, or Genre Name matching the search query.

#### 2. **Create** (GET: `/Books/Create`)
- **Purpose:** Displays the form to create a new book.
- **ViewData:** Populates the `SelectList` for choosing an `Author` from the available authors.

#### 3. **Create** (POST: `/Books/Create`)
- **Purpose:** Handles the form submission to create a new book.
- **Process:**
  - **Author:** Checks if the `AuthorName` exists. If not, creates a new `Author`.
  - **Book:** Saves the `Book` entity, then saves the many-to-many relationship between `Book` and `Genre`:
    - New genres are created if they don’t exist.
    - The `BookGenres` relationship table is populated with the book’s associated genres.
- **ViewData:** If validation fails, errors are shown, and the form is redisplayed.

#### 4. **Edit** (GET: `/Books/Edit/{id}`)
- **Purpose:** Displays the form to edit an existing book.
- **Includes:** Author and Genre details for the book.
- **ViewData:** Sets `AuthorName` and `Genres` for the edit form.

#### 5. **Edit** (POST: `/Books/Edit/{id}`)
- **Purpose:** Handles the form submission to update a book.
- **Process:**
  - **Author:** Updates or adds the author if necessary.
  - **Genres:** Updates the genres associated with the book:
    - Adds new genres.
    - Removes old genres that are no longer associated.
  - The `BookGenres` relationship table is updated with any new associations.
- **ViewData:** Passes `AuthorName` and `Genres` for the form.

#### 6. **Delete** (GET: `/Books/Delete/{id}`)
- **Purpose:** Displays the confirmation page to delete a book.
- **Includes:** Displays the book's details (Author and Book title).

#### 7. **DeleteConfirmed** (POST: `/Books/Delete/{id}`)
- **Purpose:** Deletes the specified book and its associated relationships.
- **Process:** Removes the `Book` from the context and saves changes.

#### Private Method: `BookExists(int id)`
- **Purpose:** Checks if a `Book` with the given `id` exists in the database.

---

### Key Points to Note:

- **Many-to-Many Relationship Handling:**  
  - Books are related to genres through the `BookGenres` table.
  - This relationship is properly managed during both `Create` and `Edit` actions, allowing the addition/removal of genres for a book.

- **Author Handling:**  
  - Authors are managed as entities that can be created if they don’t already exist.
  - The `AuthorName` field in both `Create` and `Edit` actions is handled manually to add a new author if needed.

- **Error Handling:**  
  - Model validation errors are logged in the `Create` and `Edit` actions and displayed back to the user if the form submission is invalid.

- **ViewData for Prepopulation:**  
  - The `AuthorName` and `Genres` are pre-populated in the form when editing a book, so users can see and modify existing data.


### MoviesController Overview:
The `MoviesController` is a controller in an ASP.NET Core MVC application responsible for managing movie-related actions such as creating, editing, deleting, and displaying movies, as well as filtering movies by title, director, and genre.

### Actions:

1. **Index (GET)**: 
   - Displays a list of all movies.
   - Accepts an optional search query to filter movies by title, director, or genre.
   - Filters are applied using `Contains` for case-insensitive partial matching.
   - Returns the movies matching the search criteria.

2. **Create (GET)**:
   - Renders a form for creating a new movie.
   - Provides a dropdown for selecting an existing director from the list of directors.
   - Returns the view with the movie creation form.

3. **Create (POST)**:
   - Handles the submission of the create movie form.
   - If the movie data is valid, a new movie record is created in the database.
   - The method handles the creation or lookup of the director and genres for the movie.
   - New genres are added if they don't exist in the database.
   - A many-to-many relationship is established between the movie and its genres.
   - Redirects to the `Index` action after successful creation.

4. **Edit (GET)**:
   - Displays the edit form for an existing movie.
   - Loads the movie along with its associated director and genres.
   - Sets the `DirectorName` and `Genres` in the view data to display the current values in the form.

5. **Edit (POST)**:
   - Handles the form submission for editing an existing movie.
   - Updates the movie's information, director, and genres.
   - Removes any genres that are no longer associated with the movie and adds new ones.
   - Redirects to the `Index` action after successful update.

6. **Delete (GET)**:
   - Displays the confirmation view for deleting a movie.
   - Loads the movie along with its associated director.

7. **DeleteConfirmed (POST)**:
   - Deletes the movie from the database.
   - Redirects to the `Index` action after successful deletion.

### Key Components:

- **Models Used**:
  - `Movie`: Represents a movie in the database. It contains properties like `Title`, `DirectorId`, and `Rating`.
  - `Director`: Represents the director of the movie. It contains properties like `Id` and `Name`.
  - `Genre`: Represents the genre of a movie. It contains properties like `Id` and `Name`.
  - `MovieGenre`: Represents the many-to-many relationship between movies and genres.

- **ViewData**:
  - The controller uses `ViewData` to pass additional data (e.g., director names, genres) to views during `Create`, `Edit`, and `Delete` actions.

- **Validation**:
  - The controller validates the input data before saving it to the database. If there are any validation errors, it provides feedback to the user.

### Example Flow:

1. **Create Movie**:
   - The user fills out a form to create a movie with a title, director, and genres.
   - The controller checks if the director exists in the database. If not, a new director is created.
   - The genres are handled similarly: existing genres are retrieved, and new ones are created if needed.
   - The movie is saved to the database, and a relationship between the movie and its genres is established through the `MovieGenre` table.

2. **Edit Movie**:
   - The user edits an existing movie's details, including the title, director, and genres.
   - The controller updates the director and genres of the movie, ensuring the correct relationships are maintained.
   - Any genres removed from the movie are deleted from the `MovieGenres` table.

3. **Delete Movie**:
   - The user can confirm the deletion of a movie. The movie is removed from the database.
