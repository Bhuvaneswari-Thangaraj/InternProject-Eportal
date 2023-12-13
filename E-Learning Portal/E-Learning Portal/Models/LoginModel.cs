using System;
using System.ComponentModel.DataAnnotations;

namespace ElearningPortal.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="User name should not be empty")]
        public string Username{get; set;}
        [Required(ErrorMessage ="Kindly enter your password")]
        public string Password{get; set;}
        [Required(ErrorMessage="Must select your role")]
        public string Role{get; set;}
    }

    public class MailSettings{
        public string From { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials{get;set;}
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string UserEmail{get; set;}
    }
}