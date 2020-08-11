using System;
using System.Collections.Generic;
using System.Windows;

namespace CodeChallenge
{

    public partial class MainWindow : Window
    {

        public List<Book> Books
        {
            get { return (List<Book>)GetValue(BooksProperty); }
            set { SetValue(BooksProperty, value); }
        }
        public static readonly DependencyProperty BooksProperty =
            DependencyProperty.Register("Books", typeof(List<Book>), typeof(MainWindow), new PropertyMetadata(new List<Book>()));

        public Book SelectedBook
        {
            get { return (Book)GetValue(SelectedBookProperty); }
            set { SetValue(SelectedBookProperty, value); }
        }
        public static readonly DependencyProperty SelectedBookProperty =
            DependencyProperty.Register("SelectedBook", typeof(Book), typeof(MainWindow), new PropertyMetadata(new Book()));

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            Books = BooksService.getBookInventory();
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            BooksService.removeBook(SelectedBook);
            Console.WriteLine(Books.Count);
        }
    }
}
