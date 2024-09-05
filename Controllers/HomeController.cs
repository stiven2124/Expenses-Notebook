using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;
using System.Diagnostics;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpendSmartDbContext _ctx;

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var allExpenses = _ctx.Expenses.ToList();
            var totalExpenses = allExpenses.Sum(x => x.Value);
            ViewBag.Expenses = totalExpenses;
            return View(allExpenses);
        }

        public IActionResult CreateEditExpense(int? id)
        {
            if(id != null) 
            {
                var expenseInDb = _ctx.Expenses.SingleOrDefault(expense => expense.Id == id);
                return View(expenseInDb);
            }
            
            return View();
        }

        public IActionResult CreateEditForm(Expense model)
        {
            if (model.Id == 0)
            {
                _ctx.Expenses.Add(model);
            }
            else
            {
                _ctx.Expenses.Update(model);
            }
            
            _ctx.SaveChanges();

            return RedirectToAction("Expenses");
        }

        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _ctx.Expenses.SingleOrDefault(expense => expense.Id == id);
            _ctx.Expenses.Remove(expenseInDb);
            _ctx.SaveChanges(); 

            return RedirectToAction("Expenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
