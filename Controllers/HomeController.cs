using System.Diagnostics;
using ERP.Models; // allows use of Staff model
using Microsoft.AspNetCore.Mvc;
using ERP.Data; // i can now use ApplicationDbContext 

namespace ERP.Controllers
{
    public class HomeController : Controller
    {
        // this lets the controller access the database
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context) // use the constructor which provides ApplicationDbContext
        {
            _context = context; // assign the passed param to DbContext so i can use itfor CRUD methods 
        }


        // Commented out because i now use Azure SQL via EF Core
        /*
        // Temporary in-memory DB
        public static List<Staff> staffList = new List<Staff>
        {
            new Staff {ID = 1, Name= "Andrew Qumsieh", PhoneNumber= "098765432"},
            new Staff {ID = 2, Name= "Test Case 1", PhoneNumber= "123456789"},
            new Staff {ID = 3, Name= "Test Case 2", PhoneNumber= "876123561"},
        };
        */

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // READ from Azure SQL
        public IActionResult Staff()
        {
            var staff = _context.Staff.ToList();
            // Pulls all rows from Azure table dbo.Staff via EF Core

            return View(staff);
            // Sends DB data to Views/Home/Staff.cshtml
        }

        // CREATE Form page
        public IActionResult Create()
        {
            return View(); // Returns empty form view
        }

        // In-memory version
        // Commented because it no longer writes to Azure DB

        /*
        [HttpPost] // Handles form submission
        public IActionResult Create(Staff staff)
        {
            staff.ID = staffList.Max(x => x.ID) + 1; // Manual ID generation
            staffList.Add(staff); // Adds to fake in-memory DB
            return RedirectToAction("Staff");
        }
        */

        // Create Azure SQL version, now does it dynamically instead of hardcoding 
        [HttpPost]
        public IActionResult Create(Staff staff)
        {
            _context.Staff.Add(staff);
            // Adds or stages record in EF Core 

            _context.SaveChanges();
            // Commits insert into Azure SQL dbo.Staff

            return RedirectToAction("Staff");
            // Reload table page with updated DB data
        }

        //Error handling, default comes with mvc
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}