using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElearningPortal.Models
{
    
    public class EnrollDetails
    {
      [Key]
      public int EId{get; set;}
      [Required]
      public int Cid{get; set;}
      [Required]
      public string Cname{get; set;}
      [Required]
      public string Mentor{get; set;}
      [Required]
      public string Tname{get; set;}
      [Required]
      public DateTime EnrollDate { get; set; }
    }
}
