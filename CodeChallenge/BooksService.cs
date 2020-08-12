﻿using MySqlConnector;
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

        public static BooksService Instance
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

        public static bool removeBook(Book toRemove)
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
            else
            {
                return false;
            }
        }

        public static bool updateBook(Book toReplace, string author, int id, int pageCount, string title)
        {
            if (removeBook(toReplace))
            {
                return addNewBook(author, id, pageCount, title);
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
