﻿using System;
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
            if (!String.IsNullOrWhiteSpace(title)
                && !String.IsNullOrWhiteSpace(author)
                && pageCount > 0)
            {
                Instance.inventory.Add(new Book()
                {
                    Author = author,
                    Title = title,
                    PageCount = pageCount,
                    Id = Instance.inventory.Count + 1
                });
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
