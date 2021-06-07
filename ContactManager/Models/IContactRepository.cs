using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Models
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll();
        Contact GetById(int Id);
        void Insert(Contact contactToCreate);
        void Update(Contact contactToUpdate);
        void Delete(int contactToDelete);
        void Save();

    }
}
