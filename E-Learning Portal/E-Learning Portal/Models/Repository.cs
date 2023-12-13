using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Windows;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Security;

namespace ElearningPortal.Models;
public class Repository
{
    
        public class AdminException : Exception
        {
            public AdminException(string Message) : base(Message)
            {
            }
        }       
        //Sign in for admin
        public static bool adminSignin(AdminDetails adminDetails)
        {
        SqlConnection connection=new SqlConnection(getConnection());
        connection.Open();
        try{
        SqlCommand adminLogin= new SqlCommand("GetLogin",connection);
        adminLogin.CommandType=CommandType.StoredProcedure;
        adminLogin.Parameters.AddWithValue("@username",adminDetails.Username);
        adminLogin.Parameters.AddWithValue("@password",adminDetails.Password);
        var result = adminLogin.ExecuteScalar(); 
        string username = result as string;   
        if(!string.IsNullOrEmpty(username))
        { 
            connection.Close();
            return true;
        }
        else
        {
            throw new AdminException("Admin signin has some issue...Make sure your credentials");
            return false;
        }
        }
        catch(AdminException adminException)
       {
        Console.WriteLine(adminException);
        return false;
       }
       catch(SqlException sqlException)
       {
        Console.WriteLine(sqlException.ToString());
        return false;
       }
       finally
       {
        connection.Close();
       }        
        }

    public static List<Feedback> displayFeedback()
        {
            List<Feedback> allfeedbacks = new List<Feedback>();
            SqlConnection connection = new SqlConnection(getConnection());
            SqlDataAdapter sqlDataAdapter =new SqlDataAdapter("Select * from feedbackDb",connection);
            DataTable dataTable= new DataTable();
            sqlDataAdapter.Fill(dataTable);

            foreach(DataRow row in dataTable.Rows)

            {
                Feedback feedback =new Feedback();
                feedback.Name=Convert.ToString(row["Name"]);
                feedback.Email=Convert.ToString(row["Email"]);
                feedback.Rate=Convert.ToInt32(row["Rate"]);
                feedback.Message=Convert.ToString(row["Message"]);
                allfeedbacks.Add(feedback);
            }

            return allfeedbacks;
        } 

    internal bool forgotPassword(MailSettings mailSettings)
    {
       Console.WriteLine("Forgot password function triggered");
        

         IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    var mail = configuration.GetSection("MailSettings").Get<MailSettings>(); 
    try{
    MailMessage message = new MailMessage();
    SmtpClient client=new SmtpClient();
    message.From = new MailAddress(mail.From);
    message.To.Add(mailSettings.UserEmail);
    message.Subject =mail.Subject ;
    message.Body =mail.Body ;

    client.Port=mail.SmtpPort;
    client.Host=mail.SmtpHost;
    
    client.EnableSsl = mail.EnableSsl;
    client.UseDefaultCredentials=mail.UseDefaultCredentials;
    client.Credentials = new NetworkCredential(mail.UserName,mail.Password);
                  
                
                
    client.DeliveryMethod=SmtpDeliveryMethod.Network;
    client.Send(message);
    return true;
    }
    catch(Exception exception)
    {
        Console.WriteLine("Mail sending exception : "+exception);
        return false;
    }

}
public static string? getConnection()         
        {
        var build = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json",optional:true,reloadOnChange:true);
        IConfiguration configuration = build.Build();
        string? connectionString = Convert.ToString(configuration.GetConnectionString("DefaultConnection"));
        return connectionString;
        }
}

