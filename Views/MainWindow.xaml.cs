using ContactBook.Logic;
using ContactBook.Services.DataAccess;
using ContactBook.Services.Validation;
using System.Windows;

namespace ContactBook.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(IValidationService validationService, IContactStorage storage):this()
        {
           var manager = new ContactManager(validationService, storage);
            
        }
    }
}
