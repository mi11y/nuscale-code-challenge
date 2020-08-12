using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CodeChallenge
{

    /// <summary>
    /// This class is responsible for processing user data and storing it.
    /// This class behaves "like" a singleton in that there is one instance
    /// of this type that is accessible behind static methods.
    /// 
    /// This is so the code-behind of a view can access and use this class'
    /// functionality without having to make an instance and be responsible for it.
    /// In order to interact with this class, a code-behind should rely on the static
    /// methods defined at the bottom.
    /// 
    /// For storing the data, this class can either "mock" storing the data in
    /// a collection locally, and will mimick a database. However changes made will
    /// not persist, and IDs will not to be unique.
    /// 
    /// An optional function this class provides is to use a MySQL database. When
    /// enabled, the class will instead connect to a remote MySQL database and
    /// create / read / update / delete from that database.
    /// </summary>
    class BooksService
    {

        /// <summary>
        /// In order to enable database functionality, change the bellow constant
        /// "USE_DB" to true, and then update the constant "CONNECTION_STRING"
        /// with the data as it applies to your database.
        /// </summary>
        private readonly bool USE_DB = false;
        private readonly string CONNECTION_STRING = "server=0.0.0.0;user=USER;password=PASSWORD;database=DB";

        private ObservableCollection<Book> inventory;
        private MySqlConnection mariaDB;
        private static BooksService instance = null;

        private BooksService()
        {
            
            if (USE_DB)
            {
                mariaDB = new MySqlConnection(CONNECTION_STRING);
                inventory = selectAllFromBooks();
            }
            else
            {
                inventory = mockInventory();
            }
        }

        private ObservableCollection<Book> selectAllFromBooks()
        {

            ObservableCollection<Book> queryResult = new ObservableCollection<Book>();

            try
            {
                mariaDB.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM books;", mariaDB);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    queryResult.Add(new Book()
                    {
                        Id          = (int)rdr[0],
                        Author      = (string)rdr[1],
                        Title       = (string)rdr[2],
                        PageCount   = (int)rdr[3]
                    });
                }
                rdr.Close();
            }
            catch (MySqlException e)
            {

                MessageBox.Show(e.Message);
            }
            mariaDB.Close();

            return queryResult;
        }

        private bool deleteByBook(Book toDelete)
        {
            try
            {
                mariaDB.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM books WHERE book_id = @book_id;", mariaDB);
                cmd.Parameters.AddWithValue("@book_id", toDelete.Id);
                MySqlDataReader rdr = cmd.ExecuteReader();

                rdr.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            mariaDB.Close();
            return true;
        }

        private bool insertNewBook(string author, int pageCount, string title)
        {
            try
            {
                mariaDB.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO books (author, title, page_count) VALUES (@author, @title, @page_count);", mariaDB);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@page_count", pageCount);
                MySqlDataReader rdr = cmd.ExecuteReader();

                rdr.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            mariaDB.Close();
            return true;
        }

        private bool updateBookById(string author, int id, int pageCount, string title)
        {
            try
            {
                mariaDB.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE books SET author = @author, title = @title, page_count = @page_count WHERE book_id = @book_id;", mariaDB);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@book_id", id);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@page_count", pageCount);
                MySqlDataReader rdr = cmd.ExecuteReader();

                rdr.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            mariaDB.Close();
            return true;
        }

        private ObservableCollection<Book> mockInventory()
        {
            ObservableCollection<Book> queryResult = new ObservableCollection<Book>();
            queryResult.Add(new Book()
            {
                Title = "The C Programming Language",
                Author = "Brian Kernighan",
                PageCount = 276,
                Id = 1
            });
            queryResult.Add(new Book()
            {
                Title = "The Rust Programming Language",
                Author = "Steve Klabnik",
                PageCount = 276,
                Id = 2
            });
            return queryResult;
        }

        private bool mockRemoveBook(Book toRemove)
        {
            if (Instance.inventory.Remove(toRemove))
            {
                System.Console.WriteLine("Removed");
                return true;
            }
            else
            {
                System.Console.WriteLine("Not Found");
                return false;
            }
        }

        private bool mockAddNewBook(string author, int id, int pageCount, string title)
        {
            Book newBook = new Book()
            {
                Author = author,
                Title = title,
                PageCount = pageCount
            };
            // Keep the old ID if provided (such as during an update), or 
            // assign the new book a new ID (may not be unique).
            newBook.Id = id == 0 ? Instance.inventory.Count + 1 : id;
            Instance.inventory.Add(newBook);
            return true;
        }

        private static BooksService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BooksService();
                }
                return instance;
            }
        }

        private static bool validateInput(string author, int pageCount, string title)
        {
            return !String.IsNullOrWhiteSpace(title) && !String.IsNullOrWhiteSpace(author) && pageCount > 0;
        }

        /// <summary>
        /// This class provides the locally stored inventory of books.
        /// If there is no current instance of the singleton, one will
        /// be created and will query the data store for records.
        /// </summary>
        public static ObservableCollection<Book> getBookInventory()
        {
            return Instance.inventory;
        }


        /// <summary>
        /// This class queries the data store for all records, and replaces
        /// the locally stored collection with the results.
        /// </summary>
        public static ObservableCollection<Book> refreshInventory()
        {
            if (Instance.USE_DB)
            {
                Instance.inventory = Instance.selectAllFromBooks();
            }
            return getBookInventory();
        }


        /// <summary>
        /// This and the following methods provide create / update / delete
        /// functionality. If using a database, these changes will be performed
        /// on the database, otherwise they will be applied to the local data
        /// store.
        /// </summary>
        public static bool removeBook(Book toRemove)
        {
            if(Instance.USE_DB)
            {
                if(Instance.inventory.Contains(toRemove))
                {
                    return Instance.deleteByBook(toRemove);
                }
                return false;
            }
            else
            {
                return Instance.mockRemoveBook(toRemove);
            }
        }


        /// <summary>
        /// A code-behind should use this method of adding a new book, as
        /// this will call the neighboring overloaded method, passing an id of 0
        /// to indicate that a new ID should be assigned to this book (unlike when
        /// an update is called and an ID is provided).
        /// </summary>
        public static bool addNewBook(string author,  int pageCount, string title)
        {
            return addNewBook(author, 0, pageCount, title);
        }

        private static bool addNewBook(string author, int id, int pageCount, string title)
        {
            if (validateInput(author, pageCount, title))
            {
                if(Instance.USE_DB)
                {
                    return Instance.insertNewBook(author, pageCount, title);
                }
                else
                {
                    return Instance.mockAddNewBook(author, id, pageCount, title);
                }
            }
            return false;
        }


        /// <summary>
        /// When updating a book without a database, the book is first removed from the local
        /// data store and then re-added with the new information, but the same ID. This is done
        /// to emulate what would be a database for inventory.
        /// TODO: Add input checking similar to that of addNewBook()
        /// </summary>
        public static bool updateBook(Book toReplace, string author, int id, int pageCount, string title)
        {
            if(validateInput(author, pageCount, title))
            {
                if (Instance.USE_DB)
                {
                    return Instance.updateBookById(author, id, pageCount, title);
                }
                else if (removeBook(toReplace))
                {
                    return addNewBook(author, id, pageCount, title);
                }
            }
            return false;
        }


        /// <summary>
        /// Locally filter the inventory by a book's title using LINQ.
        /// </summary>
        public static ObservableCollection<Book> getBooksByTitle(string title)
        {
            if(!string.IsNullOrWhiteSpace(title))
            {
                ObservableCollection<Book> result = new ObservableCollection<Book>();
                title = title.ToLower();
                var booksQuery =
                    from book in Instance.inventory.ToList<Book>()
                    where book.Title.ToLower().Contains(title)
                    || book.Title.ToLower().StartsWith(title)
                    || book.Title.ToLower().EndsWith(title)
                    select book;

                foreach (Book book in booksQuery)
                {
                    result.Add(book);
                }
                return result;
            }
            else
            {
                return getBookInventory();
            }
        }
    }
}
