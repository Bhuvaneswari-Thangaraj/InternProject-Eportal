using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ElearningPortal.Models
{
    public class MentorDetails
    {
      [Key]
      [Required(ErrorMessage="User id field should not empty")]
      [RegularExpression(@"^[0-9]*$", ErrorMessage ="User id should be in numerical format")]
    public int MentorId{get; set;}

    [Required(ErrorMessage="Mentor name field should not empty")]
    public string? Username{get; set;}  

    [Required(ErrorMessage = "Please enter the password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 Upper and LowerCase letter, 1 Number and 1 Special Character")]
    public string? Password{get; set;}

    [Required(ErrorMessage = "Please enter confirm password")]
    [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
    [DataType(DataType.Password)]
    public string? CnfmPwd{get; set;}

    [EmailAddress(ErrorMessage = "Please enter a valid mail")]
    public string? MentorEmail{get; set;}

     [Required(ErrorMessage="Please provide your department")]
    public string? MentorDprt{get; set;}

     [Required(ErrorMessage="Please enter your gender")]
    public string? MentorGender{get; set;}
    public string? MentorRole{get; set;}
    }
}
