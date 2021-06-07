using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ContactManager.Models.Validation;

namespace ContactManager.Models
{
    public class ContactManagerService : IContactManagerService
    {
        private IValidationDictionary _validationDictionary;
        private IContactRepository _repository;

        public ContactManagerService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new ContactRepository())
        { }

        public ContactManagerService(IValidationDictionary validationDictionary, IContactRepository repository)
        {
            _validationDictionary = validationDictionary;
            _repository = repository;
        }

        public bool ValidateContact(Contact contactToValidate)
        {
            if (contactToValidate.FirstName.Trim().Length == 0)
                _validationDictionary.AddError("FirstName", "First name is required.");
            if (contactToValidate.LastName.Trim().Length == 0)
                _validationDictionary.AddError("LastName", "Last name is required.");
            if (contactToValidate.Phone.Length > 0 && !Regex.IsMatch(contactToValidate.Phone, @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"))
                _validationDictionary.AddError("Phone", "Invalid phone number.");
            if (contactToValidate.Email.Length > 0 && !Regex.IsMatch(contactToValidate.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                _validationDictionary.AddError("Email", "Invalid email address.");
            return _validationDictionary.IsValid;
        }

        #region IContactManagerService Members

        public bool CreateContact(Contact contactToCreate)
        {
            // Validation logic
            if (!ValidateContact(contactToCreate))
                return false;

            // Database logic
            try
            {
                _repository.Insert(contactToCreate);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool EditContact(Contact contactToEdit)
        {
            // Validation logic
            if (!ValidateContact(contactToEdit))
                return false;

            // Database logic
            try
            {
                _repository.Update(contactToEdit);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool DeleteContact(int id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Contact GetContact(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Contact> ListContacts()
        {
            return _repository.GetAll();
        }

        #endregion
    }
}