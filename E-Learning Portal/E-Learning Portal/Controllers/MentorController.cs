using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ElearningPortal.Models;
using ElearningPortal.Data;
using System.Dynamic;
using System.IO;

namespace ElearningPortal.Controllers;
[Actions]
public class MentorController : Controller
{
    
private readonly ApplicationDbContext applicationDbContext;

 public const string SessionKeyName = "_Name";
    public MentorController(ApplicationDbContext appContext)
    {
        applicationDbContext=appContext;
    }

     public IActionResult Display()
    {
       
        IEnumerable<MentorDetails> mentorList=applicationDbContext.mentorDbSet;

        return View(mentorList);
    }
     public IActionResult Add()
    {
        
        return View();
    }
     [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(MentorDetails mentorDetails)
    {
        if(ModelState.IsValid)
        {
            //check the account is already has an account or not
            Console.WriteLine("Checking the existing mail");
  bool checkAccount=applicationDbContext.mentorDbSet.Any(m => m.MentorEmail == mentorDetails.MentorEmail);
      if(checkAccount)
      {
        Console.WriteLine("Duplicate account error");
        ViewBag.Message2="The mail  already has account...";
        return View();
        }
        else{
            
        applicationDbContext.mentorDbSet.Add(mentorDetails);
        applicationDbContext.SaveChanges();
        TempData["done"]="Mentor details added successfully";
        return RedirectToAction("Display");

        }
        }
        else
        {
          TempData["error"]="Unexpected error occured!.. Try to re-enter details";
        }
        return View(mentorDetails);
    }

//Edit the course details by admin
     public IActionResult Edit(int? itemid)
    {   
        if(itemid==null || itemid==0)
        {
            return NotFound();
        }
        var categoryFromDb=applicationDbContext.mentorDbSet.Find(itemid);
        if(categoryFromDb==null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(MentorDetails mentorDetails)
    {
        if(ModelState.IsValid)
        {
        applicationDbContext.mentorDbSet.Update(mentorDetails);
        applicationDbContext.SaveChanges();
        TempData["done"]="Mentor details updated successfully";
        return RedirectToAction("Display");
        }
        else
        {
            TempData["error"]="Make sure your details";
        }
        return View(mentorDetails);
    }

//Delete the course details by admin
     public IActionResult Delete(int? itemid)
    {
        if(itemid==null || itemid==0)
        {
            return NotFound();
        }
        var categoryFromDb=applicationDbContext.mentorDbSet.Find(itemid);
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
       var delobj=applicationDbContext.mentorDbSet.Find(itemid);
       if(delobj==null)
       {
        return NotFound();

       }
       if(ModelState.IsValid)
       {    
        applicationDbContext.mentorDbSet.Remove(delobj);
        applicationDbContext.SaveChanges();
        TempData["done"]="Mentor details deleted successfully";
        return RedirectToAction("Display");
       }
       else{
        TempData["error"]="Details can't be deleted";
       }
       return View();
    }

    public IActionResult Manage()
    {
        if(HttpContext.Session.GetString(SessionKeyName)!= null) 
        {
            ViewBag.sessionName=Convert.ToString(HttpContext.Session.GetString(SessionKeyName));
            var mentor=Convert.ToString(HttpContext.Session.GetString(SessionKeyName));
            IEnumerable<UserDetails> studentList=applicationDbContext.traineeDbSet;
            IEnumerable<CourseDetails> courseList=applicationDbContext.courseDbSet.Where(m => m.Author == mentor);
            IEnumerable<Feedback> feedbackList=Repository.displayFeedback();
            dynamic dynamicModel=new ExpandoObject();
            dynamicModel.Trainee=studentList;
            dynamicModel.Course=courseList;
            dynamicModel.FeedbackList=feedbackList;

            return View(dynamicModel);
        }
        return RedirectToAction("Index","Home");
    }
    
    public IActionResult View(int id)
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
        return View();
    }
 


    
}