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

        // Using a DependencyProperty as the backing store for Books.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BooksProperty =
            DependencyProperty.Register("Books", typeof(ObservableCollection<Book>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<Book>()));


        public Book SelectedBook
        {
            get { return (Book)GetValue(SelectedBookProperty); }
            set { SetValue(SelectedBookProperty, value); }
        }
        public static readonly DependencyProperty SelectedBookProperty =
            DependencyProperty.Register("SelectedBook", typeof(Book), typeof(MainWindow), new PropertyMetadata(new Book()));



        public bool IsButtonEnabled
        {
            get { return (bool)GetValue(IsButtonEnabledProperty); }
            set { SetValue(IsButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsButtonEnabledProperty =
            DependencyProperty.Register("IsButtonEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));



        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            Books = BooksService.getBookInventory();
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            BooksService.removeBook(SelectedBook);
            IsButtonEnabled = Books.Count == 0 ? false : true;
        }
    }
}
