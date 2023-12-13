using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ElearningPortal.Models;

public class Feedback
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id{get; set;}
    [Required(ErrorMessage="Name should not be empty")]
    public string? Name { get; set; }
    [EmailAddress(ErrorMessage = "Please enter a valid mail")]
    public string? Email { get; set; }
    public int? Rate { get; set; }
    public string? Message { get; set; }
}
