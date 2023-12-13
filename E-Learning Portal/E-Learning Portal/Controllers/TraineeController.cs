using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ElearningPortal.Models;
using ElearningPortal.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Dynamic;

namespace ElearningPortal.Controllers;
[Actions]
public class TraineeController : Controller
{
Uri baseAddress = new Uri("http://localhost:5260");

public const string SessionKeyName = "_Name";
    HttpClient client;
    private readonly ApplicationDbContext applicationDbContext;

    public TraineeController(ApplicationDbContext appContext)
    {
        applicationDbContext=appContext;
        client=new HttpClient();
        client.BaseAddress=baseAddress;
    }
    
     public IActionResult Display()
    {
      
        IEnumerable<UserDetails> traineeList=applicationDbContext.traineeDbSet;

        return View(traineeList);
    }
     public IActionResult Add()
    {
        
        return View();
    }
     [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(UserDetails userDetails)
    {
        if(ModelState.IsValid)
        {
             //check the account is already has an account or not
  bool checkAccount=applicationDbContext.traineeDbSet.Any(m => m.UserEmail == userDetails.UserEmail);
      if(checkAccount)
      {

 ViewBag.Message2="The mail  already has account...";
        return View();
          }
          else{
       applicationDbContext.traineeDbSet.Add(userDetails);
        applicationDbContext.SaveChanges();
        TempData["done"]="Trainee details added successfully";
        return RedirectToAction("Display");
       
        }
        }
        else
        {
          TempData["error"]="Unexpected error occured!.. Try to re-enter details";
        }
        
        return View(userDetails);
    }

//Edit the course details by admin
     public IActionResult Edit(int? itemid)
    {
        if(itemid==null || itemid==0)
        {
            return NotFound();
        }
        var categoryFromDb=applicationDbContext.traineeDbSet.Find(itemid);
        if(categoryFromDb==null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(UserDetails userDetails)
    {
        if(ModelState.IsValid)
        {
        applicationDbContext.traineeDbSet.Update(userDetails);
        applicationDbContext.SaveChanges();
        TempData["done"]="Trainee details updated successfully";
        return RedirectToAction("Display");
        }
        else{
         TempData["error"]="Make sure your details";

        }
        return View(userDetails);
    }

//Delete the course details by admin
     public IActionResult Delete(int? itemid)
    {
        if(itemid==null || itemid==0)
        {
            return NotFound();
        }
        var categoryFromDb=applicationDbContext.traineeDbSet.Find(itemid);
        if(categoryFromDb==null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    [HttpPost,ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteData(int? itemid)
    {
       var delobj=applicationDbContext.traineeDbSet.Find(itemid);
       if(delobj==null)
       {
        return NotFound();

       }
       if(ModelState.IsValid)
        {
        applicationDbContext.traineeDbSet.Remove(delobj);
        applicationDbContext.SaveChanges();
        TempData["done"]="Trainee details deleted successfully";
        return RedirectToAction("Display");
        }
        else{
         TempData["error"]="Details can't be deleted";

        }
        return View();
    }
      public IActionResult Profile()
    {
       if(HttpContext.Session.GetString(SessionKeyName)!= null) 
        {
            ViewBag.sessionName=Convert.ToString(HttpContext.Session.GetString(SessionKeyName));
            var trainee=Convert.ToString(HttpContext.Session.GetString(SessionKeyName));
            IEnumerable<CourseDetails> courseList = applicationDbContext.courseDbSet;
            IEnumerable<EnrollDetails> enrollList=applicationDbContext.enrollDbSet.Where(e => e.Tname == trainee);
            dynamic dynamicModel=new ExpandoObject();
            dynamicModel.Course=courseList;
            dynamicModel.Enroll=enrollList;
            return View(dynamicModel);
        }
        return RedirectToAction("Index","Home");

    }
    
   public IActionResult MoreDetails()
   {
    if(HttpContext.Session.GetString(SessionKeyName)!= null) 
        {
            ViewBag.sessionName=Convert.ToString(HttpContext.Session.GetString(SessionKeyName));
            IEnumerable<CourseDetails> courseList = applicationDbContext.courseDbSet;
            return View(courseList);
    
        }
        return RedirectToAction("Index","Home");
   }


    public IActionResult Feedback()
    {
        return View();
    }

[HttpPost]
public IActionResult Feedback(Feedback feedback)
    {
        if(feedback!=null)
        {
            string data=JsonConvert.SerializeObject(feedback);
            StringContent content=new StringContent(data,Encoding.UTF8,"application/json");
            HttpResponseMessage responseMessage=client.PostAsync("/Feedback",content).Result;
              if(responseMessage.IsSuccessStatusCode)
        { 
            TempData["done"]="Thank you for your feedback!";
            return View("Profile","Trainee");
        }
        else{
            return View();
        }       
        }
        return View();
    }

    // Enrolling the course
    public IActionResult Enroll()
    {
        return View();
    }
[HttpPost]
public IActionResult Enroll(EnrollDetails enrollDetails, int courseId, string authorName, string courseName)
{
  
    var CurrentTrainee = Convert.ToString(HttpContext.Session.GetString(SessionKeyName));
    
    enrollDetails.Cid = courseId;
    enrollDetails.Cname = courseName;
    enrollDetails.Mentor = authorName;
    enrollDetails.Tname = CurrentTrainee;
    enrollDetails.EnrollDate = DateTime.Now;

    bool isEnrolled = applicationDbContext.enrollDbSet.Any(a => a.Tname == CurrentTrainee && a.Cid == courseId);

    if (isEnrolled)
    {
        TempData["error"] = "You've already enrolled!";
    }
    else
    {
        applicationDbContext.enrollDbSet.Add(enrollDetails);
        applicationDbContext.SaveChanges();
        TempData["done"] = "You have enrolled successfully";
    }

    return RedirectToAction("MoreDetails", "Trainee");
}


public IActionResult ViewContent(int id)
{
    var traineeName = Convert.ToString(HttpContext.Session.GetString(SessionKeyName));
    bool isEnrolled = applicationDbContext.enrollDbSet.Any(a => a.Tname == traineeName && a.Cid == id);

    if (isEnrolled)
    {
        CourseDetails course = applicationDbContext.courseDbSet.FirstOrDefault(c => c.CId == id);

        MemoryStream contentStream = new MemoryStream(course.Source);

        if (course.SourceType == "Pdf")
        {
            return new FileStreamResult(contentStream, "application/pdf");
        }
        else if (course.SourceType == "Video")
        {
            return new FileStreamResult(contentStream, "video/mp4");
        }
    }
    else
    {
        TempData["error"] = "You are not enrolled for this course! Kindly enroll to continue";
        return RedirectToAction("MoreDetails", "Trainee"); 
    }
    return NotFound();
}

}