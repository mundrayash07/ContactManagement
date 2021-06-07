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
        {
            _repository = new ContactRepository(new ContactManagerDBEntities());
        }

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

        [HttpGet]
        public ActionResult Index()
        {
            var model = _repository.GetAll();
            return View(model);
        }
        [HttpGet]
        public ActionResult AddContact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddContact(Contact model)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(model);
                _repository.Save();
                return RedirectToAction("Index", "Employee");
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditContact(int id)
        {
            Contact model = _repository.GetById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditContact(Contact model)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(model);
                _repository.Save();
                return RedirectToAction("Index", "Contact");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult DeleteContact(int id)
        {
            Contact model = _repository.GetById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);
            _repository.Save();
            return RedirectToAction("Index", "Contact");
        }
    }
}