using ContactManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;



namespace ContactManager.Controllers
{
    public class ContactController : Controller
    {
        private IContactRepository _repository;
        public ContactController()
            : this(new EntityContactManagerRepository())
        { }

        public ContactController(IContactRepository repository)
        {
            _repository = repository;
        }

        private ContactManagerDBEntities _entities = new ContactManagerDBEntities();

        //
        // GET: /Home/

        protected void ValidateContact(Contact contactToValidate)
        {
            if (contactToValidate.FirstName.Trim().Length == 0)
                ModelState.AddModelError("FirstName", "First name is required.");
            if (contactToValidate.LastName.Trim().Length == 0)
                ModelState.AddModelError("LastName", "Last name is required.");
            if (contactToValidate.Phone.Length > 11)
                ModelState.AddModelError("Phone", "Invalid phone number.");
            if (contactToValidate.Email.Length > 0 && !Regex.IsMatch(contactToValidate.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                ModelState.AddModelError("Email", "Invalid email address.");
        }

        public ActionResult Index()
        {
            return View(_entities.ContactSet.ToList());
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "Id")] Contact contactToCreate)
        {
            ValidateContact(contactToCreate);

            if (!ModelState.IsValid)
                return View();

            try
            {
                _entities.AddToContactSet(contactToCreate);
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id)
        {
            var contactToEdit = (from c in _entities.ContactSet
                                 where c.Id == id
                                 select c).FirstOrDefault();

            return View(contactToEdit);
        }
        //
        // POST: /Home/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Contact contactToEdit)
        {
            ValidateContact(contactToEdit);
            if (!ModelState.IsValid)
                return View();

            try
            {
                var originalContact = (from c in _entities.ContactSet
                                       where c.Id == contactToEdit.Id
                                       select c).FirstOrDefault();
                //_entities.ApplyPropertyChanges(originalContact, contactToEdit);
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            var contactToDelete = (from c in _entities.ContactSet
                                   where c.Id == id
                                   select c).FirstOrDefault();

            return View(contactToDelete);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.Contact contactToDelete)
        {
            try
            {
                var originalContact = (from c in _entities.ContactSet
                                       where c.Id == contactToDelete.Id
                                       select c).FirstOrDefault();

               
                _entities.DeleteObject(originalContact);
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}