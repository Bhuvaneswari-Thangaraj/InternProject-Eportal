using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ElearningPortal.Models;
using ElearningPortal.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ElearningPortal.Controllers;
[Actions]
public class CourseController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ApplicationDbContext applicationDbContext;
    
    public const string SessionKeyName = "_Name";

    public CourseController(ApplicationDbContext appContext,IWebHostEnvironment webHostEnvironment)
    {
        applicationDbContext=appContext;
        _webHostEnvironment = webHostEnvironment;
    }
     public IActionResult Display()
    {
        IEnumerable<CourseDetails> courseList=applicationDbContext.courseDbSet;
        return View(courseList);
    }
     public IActionResult Add()
    {
        return View();
    }   

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Add(CourseDetails courseDetails)
{
    List<MemoryStream> files=new List<MemoryStream>();
    if(files != null)
    {
     foreach (var file in Request.Form.Files)
    {
        MemoryStream stream=new MemoryStream();
        file.CopyTo(stream);
        files.Add(stream);
    }
    courseDetails.Source=files.FirstOrDefault().ToArray();
    applicationDbContext.courseDbSet.Add(courseDetails);
    applicationDbContext.SaveChanges();
    TempData["done"] = "Course added successfully";
    return RedirectToAction("Display");
    }
    else
    {
        TempData["error"] = "Unexpected error occurred! Please try to re-enter details.";
    }
    return View(courseDetails);
}

public IActionResult ViewContent(int id)
{
    CourseDetails course = applicationDbContext.courseDbSet.FirstOrDefault(c => c.CId == id);
    if (course != null && course.Source != null)
    {
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
    return NotFound();
}

//Edit the course details by admin
     public IActionResult Edit(int? itemid)
    {
        if(itemid==null || itemid==0)
        {
            return NotFound();
        }
        var categoryFromDb=applicationDbContext.courseDbSet.Find(itemid);
        if(categoryFromDb==null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CourseDetails courseDetails)
    {
    List<MemoryStream> files=new List<MemoryStream>();
    if(files != null)
    {
     foreach (var file in Request.Form.Files)
    {
        MemoryStream stream=new MemoryStream();
        file.CopyTo(stream);
        files.Add(stream);
    }
    courseDetails.Source=files.FirstOrDefault().ToArray();
    applicationDbContext.courseDbSet.Update(courseDetails);
    applicationDbContext.SaveChanges();
    TempData["done"] = "Course updated successfully";
    return RedirectToAction("Display");
    }
    else
    {
        TempData["error"] = "Make sure you provide valid details.";
    }
    return View(courseDetails);
    }

//Delete the course details by admin
     public IActionResult Delete(int? itemid)
    {
        if(itemid==null || itemid==0)
        {
            return NotFound();
        }
        var categoryFromDb=applicationDbContext.courseDbSet.Find(itemid);
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
       var delobj=applicationDbContext.courseDbSet.Find(itemid);
       if(delobj==null)
       {
        return NotFound();
       }
       if(ModelState.IsValid)
       {
        applicationDbContext.courseDbSet.Remove(delobj);
        applicationDbContext.SaveChanges();
        TempData["done"]="Course deleted successfully";
        return RedirectToAction("Display");
       }
       else{
        TempData["error"]="Details can't be deleted";
       }
       return View();   
    }
}