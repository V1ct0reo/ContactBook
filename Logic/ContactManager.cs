using ContactBook.Models;
using ContactBook.Services.DataAccess;
using ContactBook.Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Logic
{
    public class ContactManager
    {
        public IValidationService _validationService { get; }
        private readonly IContactStorage _storage;

        public ContactManager(IValidationService validationService, IContactStorage storage)
        {
            this._validationService = validationService;
            this._storage = storage;
        }

        public List<Contact> GetAll()
        {
            return _storage.GetAll().ToList();
        }

        internal void UpdateContact(Contact updatedContact, bool addIfNonExisting = false)
        {
            if (_storage.GetById(updatedContact.Id) != null)
                _storage.Update(updatedContact);
            else
            {
                if (addIfNonExisting)
                {
                    int newID = _storage.Add(updatedContact);
                    updatedContact.Id = newID;
                }
                else
                    return;
            }
            CollecttionChangedEvent?.Invoke(this, GetAll());
        }
        public event EventHandler<List<Contact>> CollecttionChangedEvent;
    }
}
