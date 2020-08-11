using System;
using System.Collections.Generic;

namespace CodeChallenge
{
    class BooksService
    {


        private List<Book> inventory;
        private static BooksService instance = null;

        private BooksService()
        {
            inventory = getAllInventory();
        }

        private List<Book> getAllInventory()
        {
            List<Book> queryResult = new List<Book>();
            queryResult.Add(new Book()
            {
                Title = "The C Programming Language",
                Author = "Brian Kernighan",
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

        public static List<Book> getBookInventory()
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
