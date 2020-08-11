using System.Collections.Generic;

namespace CodeChallenge
{
    class BooksService
    {
        public static List<Book> getBookInventory()
        {
            List<Book> retVal = new List<Book>();
            retVal.Add(new Book() { Author = "Brian Kernighan", Title = "The C Programming Language", PageCount = 276 });
            retVal.Add(new Book() { Author = "Brian Kernighan", Title = "The C Programming Language", PageCount = 276 });
            return retVal;

        }
    }
}
