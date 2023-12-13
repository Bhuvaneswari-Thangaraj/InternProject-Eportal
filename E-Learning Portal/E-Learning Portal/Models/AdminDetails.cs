using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ElearningPortal.Models
{
    //Admin Class
    public class AdminDetails
    {
        [Required(ErrorMessage ="Admin name should not be empty")]
            public string? Username{get; set;}
        [Required(ErrorMessage ="Please enter your password")]

            public string? Password{get; set;}
    }
}