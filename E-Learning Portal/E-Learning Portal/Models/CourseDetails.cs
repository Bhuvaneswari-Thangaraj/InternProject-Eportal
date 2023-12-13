using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElearningPortal.Models
{
    
     public class CourseDetails
    {
    [Key]
    
    public int CId{get; set;}
        [Required(ErrorMessage ="Course name should not be empty")]
    public string CName{get; set;}
        [Required(ErrorMessage ="Author name should not be empty")]
    public string Author{get; set;}
    [Required(ErrorMessage="Kindly choose your notes type!")]
    public string SourceType{get; set;}
         [Required(ErrorMessage ="Please provide the valid notes")]
    public byte[] Source{get; set;}   
    }

}
