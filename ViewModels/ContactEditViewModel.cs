using ContactBook.Models;
using ContactBook.Services.Validation;
using ImTools;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace ContactBook.ViewModels
{
    public class ContactEditViewModel : BindableBase, INotifyDataErrorInfo
    {
        private readonly IValidationService _emailValidationService;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        private int Id = -1;
        private Contact Contact;

        #region properties
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                validateText(nameof(Name), value, (a) => { return !string.IsNullOrEmpty(a); });
            }
        }
        private string _name = "";
        private string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
                validateText(nameof(Email), value, (a) =>
                {
                    if (string.IsNullOrEmpty(a)) return true;
                    return _emailValidationService.Validate(a).IsValid;
                });
            }
        }
        private string _houseNumber = "";
        public string HouseNumber
        {
            get { return _houseNumber; }
            set { SetProperty(ref _houseNumber, value); }
        }
        private string _street = "";
        public string Street
        {
            get { return _street; }
            set { SetProperty(ref _street, value); }
        }
        private string _city = "";
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }
        private string _postalCode = "";
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                SetProperty(ref _postalCode, value);
                validateText(nameof(PostalCode), value, (a) =>
                {
                    if (string.IsNullOrEmpty(value)) return true;
                    string pattern = @"^\d{5}$";
                    return Regex.IsMatch(a, pattern);
                });
            }
        }
        private string _phoneNumber = "";
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                SetProperty(ref _phoneNumber, value);
                validateText(nameof(PhoneNumber), value, (a) =>
                {
                    string pattern = @"^\+?[0-9]+[-.\\s]?[0-9]+$";
                    return Regex.IsMatch(a, pattern);
                });
            }
        }
        #endregion

        private ContactEditViewModel(Contact contact)
        {

            if (contact is not null)
            {
                Id = contact.Id;
                _name = contact.Name;
                _email = contact.Email;
                _houseNumber = contact.HouseNumber;
                _street = contact.Street;
                _city = contact.City;
                _postalCode = contact.PostalCode;
                _phoneNumber = contact.PhoneNumber;
            }
            Contact = contact;

            CloseCommand = new DelegateCommand(closeAction);
            SaveCommand = new DelegateCommand(saveAction);
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SaveBtnEnabled))
                SaveBtnEnabled = !HasErrors;
        }

        public ContactEditViewModel(Contact contact, IValidationService validationService) : this(contact)
        {
            this._emailValidationService = validationService;
        }

        #region  commands and actions
        public ICommand CloseCommand { get; set; }
        private void closeAction()
        {
            CloseRequestedEvent?.Invoke(this, EventArgs.Empty);
        }

        public ICommand SaveCommand { get; set; }
        private void saveAction()
        {
            validateAllProperties();
            if (HasErrors) return;

            var newContact = new Contact(
                    Id,
                    name: Name,
                    city: City,
                    email: Email,
                    street: Street,
                    houseNumber: HouseNumber,
                    phoneNumber: PhoneNumber,
                    postalcode: PostalCode);
            SaveRequestedEvent?.Invoke(
                this,
                newContact
                );
            Contact = newContact;
        }

        private void validateAllProperties()
        {
            Name = Name;
            City = City;
            Email = Email;
            Street = Street;
            HouseNumber = HouseNumber;
            PhoneNumber = PhoneNumber;
            PostalCode = PostalCode;
        }
        #endregion



        #region ErrorNotification
        private bool _saveBtnEnabled = false;
        public bool SaveBtnEnabled
        {
            get { return _saveBtnEnabled; }
            set { SetProperty(ref _saveBtnEnabled, value); }
        }

        private void validateText(string propertyName, string text, Func<string, bool> validationFunc)
        {
            ClearErrors(propertyName);
            if (validationFunc(text)) return;
            else AddError(propertyName, $"error with {propertyName}");
        }
        private void validateText(string propertyName, string text, Dictionary<string, Func<string, bool>> validationSteps)
        {
            ClearErrors(propertyName);
            foreach (var kv in validationSteps)
            {
                if (kv.Value(text)) continue;
                else AddError(propertyName, kv.Key);
            }
        }

        public bool HasErrors => _errorsByPropertyName.Any();
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
            _errorsByPropertyName[propertyName] : null;
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }
        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            SaveBtnEnabled = !HasErrors;
        }
        #endregion

        public event EventHandler CloseRequestedEvent;
        public event SaveRequestedEventHandler SaveRequestedEvent;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public delegate void SaveRequestedEventHandler(object sender, Contact updatedContact);
    }
}
