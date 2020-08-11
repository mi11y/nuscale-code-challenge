using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace CodeChallenge
{

    public partial class MainWindow : Window
    {



        public ObservableCollection<Book> Books
        {
            get { return (ObservableCollection<Book>)GetValue(BooksProperty); }
            set { SetValue(BooksProperty, value); }
        }
        public static readonly DependencyProperty BooksProperty =
            DependencyProperty.Register("Books", typeof(ObservableCollection<Book>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<Book>()));

        public Book SelectedBook
        {
            get { return (Book)GetValue(SelectedBookProperty); }
            set { SetValue(SelectedBookProperty, value); }
        }
        public static readonly DependencyProperty SelectedBookProperty =
            DependencyProperty.Register("SelectedBook", typeof(Book), typeof(MainWindow), new PropertyMetadata(new Book()));



        public string NewBookTitle
        {
            get { return (string)GetValue(NewBookTitleProperty); }
            set { SetValue(NewBookTitleProperty, value); }
        }
        public static readonly DependencyProperty NewBookTitleProperty =
            DependencyProperty.Register("NewBookTitle", typeof(string), typeof(MainWindow), new PropertyMetadata("New Book Title"));



        public string NewBookAuthor
        {
            get { return (string)GetValue(NewBookAuthorProperty); }
            set { SetValue(NewBookAuthorProperty, value); }
        }
        public static readonly DependencyProperty NewBookAuthorProperty =
            DependencyProperty.Register("NewBookAuthor", typeof(string), typeof(MainWindow), new PropertyMetadata("New Book Author"));



        public int NewBookPageCount
        {
            get { return (int)GetValue(NewBookPageCountProperty); }
            set { SetValue(NewBookPageCountProperty, value); }
        }
        public static readonly DependencyProperty NewBookPageCountProperty =
            DependencyProperty.Register("NewBookPageCount", typeof(int), typeof(MainWindow), new PropertyMetadata(0));




        public bool IsDeleteEnabled
        {
            get { return (bool)GetValue(IsDeleteEnabledProperty); }
            set { SetValue(IsDeleteEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDeleteEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDeleteEnabledProperty =
            DependencyProperty.Register("IsDeleteEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));



        public string UpdateBookTitle
        {
            get { return (string)GetValue(UpdateBookTitleProperty); }
            set { SetValue(UpdateBookTitleProperty, value); }
        }
        public static readonly DependencyProperty UpdateBookTitleProperty =
            DependencyProperty.Register("UpdateBookTitle", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));



        public string UpdateBookAuthor
        {
            get { return (string)GetValue(UpdateBookAuthorProperty); }
            set { SetValue(UpdateBookAuthorProperty, value); }
        }
        public static readonly DependencyProperty UpdateBookAuthorProperty =
            DependencyProperty.Register("UpdateBookAuthor", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));



        public int UpdateBookPageCount
        {
            get { return (int)GetValue(UpdateBookPageCountProperty); }
            set { SetValue(UpdateBookPageCountProperty, value); }
        }
        public static readonly DependencyProperty UpdateBookPageCountProperty =
            DependencyProperty.Register("UpdateBookPageCount", typeof(int), typeof(MainWindow), new PropertyMetadata(0));



        public int UpdateBookId
        {
            get { return (int)GetValue(UpdateBookIdProperty); }
            set { SetValue(UpdateBookIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateBookId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateBookIdProperty =
            DependencyProperty.Register("UpdateBookId", typeof(int), typeof(MainWindow), new PropertyMetadata(0));



        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Books = BooksService.getBookInventory();
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            BooksService.removeBook(SelectedBook);
            IsDeleteEnabled = Books.Count == 0 ? false : true;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            IsDeleteEnabled = BooksService.addNewBook(NewBookTitle, NewBookAuthor, NewBookPageCount);
        }

        private void RowSelect(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateBookAuthor = SelectedBook.Author;
            UpdateBookTitle = SelectedBook.Title;
            UpdateBookPageCount = SelectedBook.PageCount;
            UpdateBookId = SelectedBook.Id;
        }

    }
}
