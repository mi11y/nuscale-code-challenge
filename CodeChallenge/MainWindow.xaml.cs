using System.Collections.Generic;
using System.Windows;

namespace CodeChallenge
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml 
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Book> Books
        {
            get { return (List<Book>)GetValue(BooksProperty); }
            set { SetValue(BooksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Books.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BooksProperty =
            DependencyProperty.Register("Books", typeof(List<Book>), typeof(MainWindow), new PropertyMetadata(new List<Book>()));



        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();

            this.Books = BooksService.getBookInventory();
        }

    }
}
