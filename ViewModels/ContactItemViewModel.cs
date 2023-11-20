using ContactBook.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactBook.ViewModels
{
    public class ContactItemViewModel : BindableBase
    {
        public Contact Contact { get; private set; }

        public ContactItemViewModel(Contact contact)
        {
            Contact = contact;

            createDelegates();
        }

        private void createDelegates()
        {
            EditBtnCommand = new DelegateCommand(EditBtnAction);
        }

      
        public ICommand EditBtnCommand { get; private set; }
        public void EditBtnAction()
        {
            EditContactTriggered?.Invoke(this, Contact);
        }

        public event EventHandler<Contact> EditContactTriggered;

    }
}
