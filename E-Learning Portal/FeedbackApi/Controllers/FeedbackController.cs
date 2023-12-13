using Microsoft.EntityFrameworkCore;
using FeedbackApi.Data;
using Microsoft.AspNetCore.Mvc;
using FeedbackApi.Models;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

// your controller code here



namespace FeedbackApi.Controllers;

[ApiController]
[Route("[controller]")]

public class FeedbackController : ControllerBase
{

    private readonly ApplicationDbContext dbContext;

     public FeedbackController(ApplicationDbContext appContext)
    {
        dbContext=appContext;
    }
    
[HttpPost]
    
    public IActionResult Add(Feedback feedback)
    {
    //     //  if (ModelState.IsValid)
    //     // {
    //         try
    //         {
    //         dbContext.feedbackDbSet.Add(feedback);
    //         dbContext.SaveChanges();

    //         return Ok("Thank you for your feedback!");
    //         }
    //         catch(Exception exception)
    //         {
    //             Console.WriteLine("Exception in inserting Feedback : "+exception.ToString());
    //         }
    //         return BadRequest();
    //     // }
    //     // else
    //     // {
    //     //     return BadRequest(ModelState);
    //     // }

        if (ModelState.IsValid)
{
    

    var sql = "INSERT INTO [feedbackDb] ([Email], [Message], [Name], [Rate]) " +
              "VALUES (@Email, @Message, @Name, @Rate)";
    
 var parameters = new List<SqlParameter>
{
    new SqlParameter("@Email", feedback.Email),
    new SqlParameter("@Message", feedback.Message),
    new SqlParameter("@Name", feedback.Name),
    new SqlParameter("@Rate", feedback.Rate)
};



    try
    {
        dbContext.Database.ExecuteSqlRaw(sql, parameters);
        return Ok("Thank you for your feedback!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception: "+ex.ToString());
    }
    
}

    return BadRequest();


        
    
    }
}

    
