using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Models
{
    public class Contact
    {
        public Contact(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
            Email = contact.Email;
            HouseNumber = contact.HouseNumber;
            Street = contact.Street;
            City = contact.City;
            PostalCode = contact.PostalCode;
            PhoneNumber = contact.PhoneNumber;
        }
        public Contact(string name, string email, string street, string houseNumber, string city, string postalcode, string phoneNumber)
            : this(-1, name, email, street, houseNumber, city, postalcode, phoneNumber)
        {
        }

        public Contact(
            int id,
            string name,
            string email,
            string street,
            string houseNumber,
            string city,
            string postalcode,
            string phoneNumber)
        {
            Id = id;
            Name = name;
            Email = email;
            HouseNumber = houseNumber;
            Street = street;
            City = city;
            PostalCode = postalcode;
            PhoneNumber = phoneNumber;
        }

        public int Id { get; set; } = -1;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Contact other = (Contact)obj;
            return
                Id == other.Id &&
                Name == other.Name &&
                Email == other.Email &&
                HouseNumber == other.HouseNumber &&
                Street == other.Street &&
                City == other.City &&
                PostalCode == other.PostalCode &&
                PhoneNumber == other.PhoneNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Id,
                Name,
                Email,
                HouseNumber,
                Street,
                City,
                PostalCode,
                PhoneNumber
            );
        }
    }
}
