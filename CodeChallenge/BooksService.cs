using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeChallenge
{
    class BooksService
    {


        private ObservableCollection<Book> inventory;
        private static BooksService instance = null;

        private BooksService()
        {
            inventory = getAllInventory();
        }

        private ObservableCollection<Book> getAllInventory()
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
                Id = 1
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
    }
}
