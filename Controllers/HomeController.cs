using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Bank.Models;
using System.Linq;

namespace Bank.Controllers
{
    public class HomeController : Controller
    {
        private BankContext _context;
 
        public HomeController(BankContext context)
        {
            _context = context;
        }
 
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            
            ViewBag.Errors = new List<string>();
            return View();
        }
        [HttpGet]
        [Route("login")]
        public IActionResult Log()
        {
            
            ViewBag.Errors = new List<string>();
            return View();
        }
        
        [HttpPost]
        [Route("register")]
        public IActionResult RegisterHandler(RegisterViewModel model)
        {
            //System.Console.WriteLine("IIIFFFF");
            if(ModelState.IsValid)
            {
                //System.Console.WriteLine("AFFFTTTTEEEERRR");
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User NewUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now

                };
                NewUser.Password = Hasher.HashPassword(NewUser, model.Password);

                _context.Add(NewUser);
                _context.SaveChanges();

                User justEnteredPerson = _context.User.SingleOrDefault(user => user.Email == model.Email);
                HttpContext.Session.SetString("FirstName", justEnteredPerson.FirstName);
                HttpContext.Session.SetInt32("UserId", justEnteredPerson.UserId);

                return RedirectToAction("Account", "Transaction", new {UserId = justEnteredPerson.UserId});

            }
            ViewBag.Errors = new List<string>();
            return View("Index");

        }
        [HttpPost]
        [Route("loggin")]
        public IActionResult Login(string Email, string Password)
        {
            User loggedUser = _context.User.SingleOrDefault( user => user.Email == Email);
            //System.Console.WriteLine("SSSSSSSSSSSSSS");
            if (loggedUser != null && Password != null )
            {
                //System.Console.WriteLine("EEEEEEEEEEEEEE");
                var Hasher = new PasswordHasher<User>();
                if( 0  != Hasher.VerifyHashedPassword(loggedUser, loggedUser.Password, Password))
                {
                    //System.Console.WriteLine("GGGGGGGGGGGGG");
                    HttpContext.Session.SetString("FirstName", loggedUser.FirstName);
                    HttpContext.Session.SetInt32("UserId", loggedUser.UserId);
                    return RedirectToAction("Account", "Transaction", new {UserId = loggedUser.UserId});
                }
            }
            ViewBag.Errors = new List<string>();
            return View("Index");
        }
        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            ViewBag.UserName = HttpContext.Session.GetString("FirstName");
            return View();
        }
    }
}
