using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CodeChallenge
{
    class BooksService
    {
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

        public static ObservableCollection<Book> getBookInventory()
        {
            return Instance.inventory;
        }

        public static ObservableCollection<Book> refreshInventory()
        {
            if (Instance.USE_DB)
            {
                Instance.inventory = Instance.selectAllFromBooks();
            }
            return getBookInventory();
        }

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

        public static bool addNewBook(string title, string author, int pageCount)
        {
            return addNewBook(author, 0, pageCount, title);
        }

        public static bool addNewBook(string author, int id, int pageCount, string title)
        {
            if (!String.IsNullOrWhiteSpace(title)
                && !String.IsNullOrWhiteSpace(author)
                && pageCount > 0)
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
            else
            {
                return false;
            }
        }

        public static bool updateBook(Book toReplace, string author, int id, int pageCount, string title)
        {
            if(Instance.USE_DB)
            {
                return Instance.updateBookById(author, id, pageCount, title);
            }
            else
            {
                if (removeBook(toReplace))
                {
                    return addNewBook(author, id, pageCount, title);
                }
            }
            return false;
        }

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
