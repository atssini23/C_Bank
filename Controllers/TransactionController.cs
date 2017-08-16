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
    public class TransactionController : Controller
    {
        private BankContext _context;
 
        public TransactionController(BankContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("account/{UserId}")]
        public IActionResult Account(int UserId)
        {
            System.Console.WriteLine("arrived!!!!!!!!!!!!!!!!");
            int? loggedUserInt =  ViewBag.User = HttpContext.Session.GetInt32("UserId");
            string loggedUserName = ViewBag.User = HttpContext.Session.GetString("FirstName");
            if (UserId != loggedUserInt)
            {
                return RedirectToAction("Index", "User");
            }
            ViewBag.UserName = loggedUserName;
            ViewBag.UserId = loggedUserInt;

            List<Transaction> Transaction = _context.Transaction.Where(transaction => transaction.UserId == loggedUserInt).ToList();
            ViewBag.Transaction = Transaction;
            double totalMoney = 0.00;
            foreach (var transaction in Transaction)
            {
                totalMoney +=transaction.Amount;
            }
            ViewBag.Money = totalMoney;
            return View();
        }
        [HttpPost]
        [Route("handel_tans")]
        public IActionResult HandelTransaction(double amount)
        {
            int? loggedUserInt = ViewBag.User = HttpContext.Session.GetInt32("UserId");
            string loggedUserName = ViewBag.User = HttpContext.Session.GetString("FirstName");
            Transaction newTrasaction = new Transaction
            {
                Amount = amount,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = (int)loggedUserInt

            };
            _context.Add(newTrasaction);
            _context.SaveChanges();
            return RedirectToAction("Account", new {UserId = loggedUserInt});
        }
    }
}