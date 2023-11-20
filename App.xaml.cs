using ContactBook.Services.DataAccess;
using ContactBook.Services.Validation;
using ContactBook.Views;
using Prism.Ioc;
using System.Windows;

namespace ContactBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IValidationService, MockEmailValidation>();
            containerRegistry.Register<IContactStorage, MockContactStorage>();
        }
    }
}
