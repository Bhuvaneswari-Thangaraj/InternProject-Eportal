using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;



namespace ElearningPortal.Models
{
    public class UserDetails
    {
      [Key]
        public int UserId{get; set;}

    [Required(ErrorMessage="User name field should not empty")]
    public string? Username{get; set;}

    [Required(ErrorMessage = "Please enter the password")]
  [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{6,}$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character")]
    public string? Password{get; set;}

    [Required(ErrorMessage = "Please enter confirm password")]
    [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
   [DataType(DataType.Password)]
    public string? CnfmPwd{get; set;}

    [Required(ErrorMessage = "Please enter a valid mail id")]
    [EmailAddress(ErrorMessage = "Please enter a valid mail")]
    public string? UserEmail{get; set;}

    [Required(ErrorMessage = "Please provide your department")]
    public string? UserDprt{get; set;}

    [Required(ErrorMessage = "Please enter your gender")]
    public string? UserGender{get; set;}
    public string? UserRole{get; set;}
    }


}
