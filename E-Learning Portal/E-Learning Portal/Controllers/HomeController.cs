using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ElearningPortal.Models;
using ElearningPortal.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security;

namespace ElearningPortal.Controllers;
[Actions]
public class HomeController : Controller
{
    public const string SessionKeyName = "_Name";
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext applicationDbContext;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext appContext)
    {
        _logger = logger;
        applicationDbContext=appContext;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
public IActionResult Index(LoginModel loginModel)
{
    if (ModelState.IsValid)
    {
        _logger.LogInformation("Entered into verification block");
        if (loginModel.Role == "Admin")
        {
            bool adminLogin = Repository.adminSignin(new AdminDetails
            {
                Username = loginModel.Username,
                Password = loginModel.Password
            });
            if (adminLogin)
            {
                HttpContext.Session.SetString("_Name", loginModel.Username);
                _logger.LogInformation("Admin Login attempted");
                TempData["done"] = "Login Successful";
                return View("Roles");
            }
            else
            {
                ViewBag.message = "Invalid Username / Password";
                _logger.LogWarning("Login not successful");
                return View();
            }
        }
        else if (loginModel.Role == "Mentor")
        {
            _logger.LogInformation("mentor login");
            bool mentorLogin = applicationDbContext.mentorDbSet.Any(m => m.Username == loginModel.Username && m.Password == loginModel.Password);
            if (mentorLogin)
            {
                HttpContext.Session.SetString("_Name", loginModel.Username);
                _logger.LogInformation("Mentor signin attempted");
                TempData["done"] = "Login Successful";
                return RedirectToAction("Manage", "Mentor");
            }
            else
            {
                ViewBag.message = "Invalid Username/password";
                return View();
            }
        }
        else if (loginModel.Role == "Trainee")
        {
            _logger.LogInformation("trainee login");
            bool traineeLogin = applicationDbContext.traineeDbSet.Any(m => m.Username == loginModel.Username && m.Password == loginModel.Password);

            if (traineeLogin)
            {
                HttpContext.Session.SetString("_Name", loginModel.Username);
                _logger.LogInformation("Login was attempted");
                TempData["done"] = "Login Successful";
                return RedirectToAction("Profile", "Trainee"); 
            }
            else
            {
                ViewBag.message = "Invalid Username/password";
                return View();
            }
        }
        else
        {
            ViewBag.message = "Your login has some issue";
            return View();
        }
    }
    return View();
}
    public IActionResult Contact()
    {
        return View();
    }


     public IActionResult Logout()
    {
        try{
            HttpContext.Session.Remove("_Name");
         }
         catch(Exception exception)
         {  
            _logger.LogWarning("Session not closed",exception.ToString());
         }
        return RedirectToAction("Index");

    }

    public IActionResult Forgotpwd()
    {
        return View();
    }
     [HttpPost]
    public IActionResult Forgotpwd(MailSettings mailSettings)
    {
        Console.WriteLine("Forgot password triggered");
        var repository=new Repository();     
        if(repository.forgotPassword(mailSettings))
        {
            _logger.LogInformation("Execution block");
            TempData["done"]="Mail sent to your registered mail id";
            return RedirectToAction("Index");
        }
        else
        {
            _logger.LogWarning("Error block");
            ViewBag.message="Please enter a registered mail id";
            return View();
        }
        
        
    }
        public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
