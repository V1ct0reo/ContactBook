using ContactBook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Services.DataAccess
{
    public interface IContactStorage
    {
        
        Contact? GetById(int id);
        IEnumerable<Contact> GetAll();
        int Add(Contact contact);
        void Update(Contact contact);
        void Delete(int id);
        void Delete(Contact contact);
        Contact GetByValues(Contact contact);
    }
}
