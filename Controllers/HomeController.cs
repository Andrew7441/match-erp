using System.Diagnostics;
using ERP.Models; // allows use of Staff model
using Microsoft.AspNetCore.Mvc;
using ERP.Data;
using Microsoft.EntityFrameworkCore; // i can now use ApplicationDbContext 

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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // GET /Home/Edit/{id}
        // Loads one staff member and shows the edit form.
        public IActionResult Edit(int id)
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            //Hold staff object or be null
            Staff? staff = _context.Staff.FirstOrDefault(s => s.ID == id);

            if(staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [HttpPost]
        public IActionResult Edit(Staff staff)
        {
            //Authentication
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            //If modeled properly 
            if (!ModelState.IsValid)
            {
                return View("Staff");
            }

            _context.Staff.Update(staff);
            _context.SaveChanges();

            return RedirectToAction("Staff");
        }

        // GET /Home/Delete/{id}
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            Staff? staff = _context.Staff.FirstOrDefault(u => u.ID == id);

            if (staff == null)
            {
                return NotFound();
            }

            _context.Staff.Remove(staff);
            _context.SaveChanges();

            return RedirectToAction("Staff");
        }


        // GET /Home/Staff
        public IActionResult Staff()
        {
            //Authentication
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            var staff = _context.Staff.ToList();
            // Pulls all rows from Azure table dbo.Staff via EF Core

            return View(staff);
            // Sends DB data to Views/Home/Staff.cshtml
        }

        // GET /Home/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }
            return View(); // Returns empty form view of current action
        }


        // Create Azure SQL version, now does it dynamically instead of hardcoding 
        [HttpPost]
        public IActionResult Create(Staff staff)
        {
            if (!ModelState.IsValid) // if ModelState validatation checker is not passed in correctly from create.cshtml then it 
            {                        // would return the errors back to the form
                return View(staff);
            }

            //Authentication
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            _context.Staff.Add(staff);
            // Adds or stages record in EF Core 

            _context.SaveChanges();
            // Commits insert into Azure SQL dbo.Staff

            return RedirectToAction("Staff");
            // Reload table page with updated DB data
        }

        //Get /Home/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            bool emailExists = _context.Users.Any(u => u.Email == user.Email);

            if (emailExists)
            {
                ModelState.AddModelError("","Email already exists!");
                return View(user);
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        //GET shows login form
        public IActionResult Login()
        {
            return View();
        }

        //Post - handles form submission
        [HttpPost]
        public IActionResult Login(User user) // MVC puts values of login in User user
        {
            //email empty
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            //LINQ Query to check 
            var dbuser = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

            //wrong or doesnt exist
            if(dbuser == null)
            {
                ModelState.AddModelError("", "Invalid username or password"); // "" is for key, second param for message
                return View(); // View() alone would return to action's view which is Login.cshtml, passing user keeps the email they typed
            }
              
            // Stores the logged in user's email in session so protected pages can verify the user is authenticated
            HttpContext.Session.SetString("UserEmail", dbuser.Email);

            return RedirectToAction("Staff");
        }

        //Get Home/Logout
        //Clears session so app forgets the logged in user
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
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