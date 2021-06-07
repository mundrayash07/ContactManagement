using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ContactManager.Models
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactManagerDBEntities _context;
        //private ContactManagerDBEntities _entities = new ContactManagerDBEntities();

        public ContactRepository()
        {
            _context = new ContactManagerDBEntities();
        }
        public ContactRepository(ContactManagerDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<Contact> GetAll()
        {
            return _context.Contacts.ToList();
        }
        public Contact GetById(int Id)
        {
            return _context.Contacts.Find(Id);
        }
        public void Insert(Contact contact)
        {
            _context.Contacts.Add(contact);
        }
        public void Update(Contact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
        }
        public void Delete(int Id)
        {
            Contact contact = _context.Contacts.Find(Id);
            _context.Contacts.Remove(contact);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        

    }
}