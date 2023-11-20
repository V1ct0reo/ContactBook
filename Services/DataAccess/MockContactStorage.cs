using ContactBook.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContactBook.Services.DataAccess
{
    internal class MockContactStorage : IContactStorage
    {
        private List<Contact> contacts = new List<Contact>()
        {
            new Contact(0, "John Doe", "email@google.com", "Theodor-Heuss Allee", "12", "Obernbreit","97342", "015778974533"),
            new Contact(1, "Jane Doe", "j.doe@google.com", "Am Bollenberg", "12", "Obernbreit", "97342", "016714966562"),
            new Contact(2, "Max Muster", "j.doe@google.com", "Am Bollenberg", "12", "Obernbreit", "97342", "016714966562"),
            new Contact(3, "Julia", "j.doe@google.com", "Am Bollenberg", "12", "Obernbreit", "97342", "016714966562")
        };
        public int Add(Contact contact)
        {
            int nextID = contacts.Select(x => x.Id).Max()+1;
            contact.Id = nextID;
            contacts.Add(new Contact(contact));
            return nextID;
        }

        public void Delete(int id)
        {
            var itemToRemove = GetById(id);
            if (itemToRemove != null)
            {
                contacts.Remove(itemToRemove);
            }
        }
        public void Delete(Contact contact)
        { contacts.Remove(contacts.Where(x => x.Equals(contact)).FirstOrDefault()); }

        public IEnumerable<Contact> GetAll()
        {
            var res = new List<Contact>();
            res.AddRange(contacts);
            return res;
        }

        public Contact? GetById(int id)
        {
            return contacts.FirstOrDefault(x => x.Id == id);
        }
        public Contact? GetByValues(Contact contact)
        {
            var match = contacts.FirstOrDefault(x =>
            x.Name == contact.Name &&
            x.Street == contact.Street &&
            x.City == contact.City &&
            x.HouseNumber == contact.HouseNumber &&
            x.Email == contact.Email &&
            x.PhoneNumber == contact.PhoneNumber);
            return match;
        }

        public void Update(Contact contact)
        {
            Contact match = null;
            if (contact.Id == -1)
            {
                match = GetByValues(contact);
            }
            else
            {
                match = GetById(contact.Id);
            }
            if (match == null) return;

            match.Street = contact.Street;
            match.City = contact.City;
            match.PostalCode = contact.PostalCode;
            match.PhoneNumber = contact.PhoneNumber;
            match.HouseNumber = contact.HouseNumber;
            match.Email = contact.Email;
            match.Name = contact.Name;

        }
    }
}
