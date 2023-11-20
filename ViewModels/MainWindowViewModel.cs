using ContactBook.Logic;
using ContactBook.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Input;

namespace ContactBook.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ContactManager _manager;


        public MainWindowViewModel(ContactManager manager)
        {
            createDelegates();

            var contacts = manager.GetAll();
            foreach (var contact in contacts)
            {
                addContactItemToCollection(contact);
            }

            this._manager = manager;
            this._manager.CollecttionChangedEvent += _manager_CollecttionChangedEvent;
        }

        private void createDelegates()
        {
            AddContactCommand = new DelegateCommand(addContactAction);
        }

        #region Visu Properties
        private ObservableCollection<ContactItemViewModel> _contactItemVMs = new();
        public ObservableCollection<ContactItemViewModel> ContactItemsVMs
        {
            get { return _contactItemVMs; }
            set { SetProperty(ref _contactItemVMs, value); }
        }
        private ContactEditViewModel _editContactVM;
        public ContactEditViewModel EditContactVM
        {
            get { return _editContactVM; }
            set { SetProperty(ref _editContactVM, value); }
        }
        private ContactItemViewModel _selectedVM;
        public ContactItemViewModel SelectedVM
        {
            get { return _selectedVM; }
            set
            {
                SetProperty(ref _selectedVM, value);
            }
        }


        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private Visibility _modalVisibility = Visibility.Collapsed;
        public Visibility ModalVisibility
        {
            get { return _modalVisibility; }
            set { SetProperty(ref _modalVisibility, value); }
        } 
        #endregion

        #region ICommand And Actions
        public ICommand AddContactCommand { get; private set; }

        private void addContactAction()
        {
            openEditContactModal(null);
        }
        #endregion

        private void addContactItemToCollection(Contact contact)
        {
            var item = new ContactItemViewModel(contact);
            registerContactItemEvents(item);
            ContactItemsVMs.Add(item);
        }
        private void openEditContactModal(Contact contact)
        {
            if (EditContactVM != null)
            {
                unRegisterEditContactVMEvents();
            }
            EditContactVM = new(contact, _manager._validationService);
            registerEditContactVMEvents();
            ModalVisibility = Visibility.Visible;
        }
        private void editContact(Contact contact)
        {
            openEditContactModal(contact);
        }

        #region Event registration and handlers
        private void registerEditContactVMEvents()
        {
            EditContactVM.CloseRequestedEvent += editVM_CloseRequested;
            EditContactVM.SaveRequestedEvent += editVM_SaveRequested;
        }

        private void editVM_SaveRequested(object sender, Contact updatedContact)
        {
            _manager.UpdateContact(updatedContact, addIfNonExisting: true);
            ModalVisibility = Visibility.Collapsed;
        }

        private void unRegisterEditContactVMEvents()
        {
            EditContactVM.CloseRequestedEvent -= editVM_CloseRequested;
        }

        private void editVM_CloseRequested(object sender, EventArgs e)
        {
            ModalVisibility = Visibility.Collapsed;
        }

        private void registerContactItemEvents(ContactItemViewModel item)
        {
            item.EditContactTriggered += contactVM_EditTriggered;
        }

        private void _manager_CollecttionChangedEvent(object sender, System.Collections.Generic.List<Contact> e)
        {
            ContactItemsVMs.Clear();
            foreach (var contact in e)
            {
                addContactItemToCollection(contact);
            }
        }

        private void contactVM_EditTriggered(object sender, Contact e)
        {
            if (sender is not ContactItemViewModel vm) return;

            editContact(vm.Contact);
        } 
        #endregion
    }
}
