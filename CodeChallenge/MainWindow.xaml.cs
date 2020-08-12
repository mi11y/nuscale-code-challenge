using System.Collections.ObjectModel;
using System.Windows;

namespace CodeChallenge
{

    /// <summary>
    /// In order to support data binding between the XAML view and this code-behind,
    /// Dependency Properties are useful here as they extend the regular functionality
    /// of a regular class property to support binding (among other things).
    /// 
    /// By using Dependency Properties, the view can bind to these properties and update
    /// its presentation of the data when the data changes. Now we can have a code-behind
    /// whose responsibility is just to set the data that the view will present.
    /// </summary>
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
        public static readonly DependencyProperty IsDeleteEnabledProperty =
            DependencyProperty.Register("IsDeleteEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));



        public bool isUpdateEnabled
        {
            get { return (bool)GetValue(isUpdateEnabledProperty); }
            set { SetValue(isUpdateEnabledProperty, value); }
        }
        public static readonly DependencyProperty isUpdateEnabledProperty =
            DependencyProperty.Register("isUpdateEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));



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
        public static readonly DependencyProperty UpdateBookIdProperty =
            DependencyProperty.Register("UpdateBookId", typeof(int), typeof(MainWindow), new PropertyMetadata(0));



        public string FilterByTitle
        {
            get { return (string)GetValue(FilterByTitleProperty); }
            set { SetValue(FilterByTitleProperty, value); }
        }
        public static readonly DependencyProperty FilterByTitleProperty =
            DependencyProperty.Register("FilterByTitle", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));



        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Books = BooksService.getBookInventory();
        }


        /// <summary>
        /// These remaining methods handle events from the view, and rely on a different class for
        /// processing the data. Once that class returns results, these methods will update the
        /// dependency properties above, which will in turn update the data presented to the user.
        /// </summary>
        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            BooksService.removeBook(SelectedBook);
            Books = BooksService.refreshInventory();
            IsDeleteEnabled = isUpdateEnabled = false;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            BooksService.addNewBook(NewBookAuthor, NewBookPageCount, NewBookTitle);
            Books = BooksService.refreshInventory();
            IsDeleteEnabled = isUpdateEnabled = SelectedBook != null;
        }

        private void RowSelect(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(SelectedBook != null)
            {
                UpdateBookAuthor = SelectedBook.Author;
                UpdateBookTitle = SelectedBook.Title;
                UpdateBookPageCount = SelectedBook.PageCount;
                UpdateBookId = SelectedBook.Id;
                IsDeleteEnabled = isUpdateEnabled = true;
            }
            else
            {
                UpdateBookAuthor = string.Empty;
                UpdateBookTitle = string.Empty;
                UpdateBookPageCount = 0;
                UpdateBookId = 0;
                IsDeleteEnabled = isUpdateEnabled = false;
            }
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            if(SelectedBook != null)
            {
                bool successfulUpdate = BooksService.updateBook(SelectedBook, UpdateBookAuthor, UpdateBookId, UpdateBookPageCount, UpdateBookTitle);
                Books = BooksService.refreshInventory();
                IsDeleteEnabled = isUpdateEnabled = !successfulUpdate;
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            Books = BooksService.getBooksByTitle(FilterByTitle);
        }
    }
}
