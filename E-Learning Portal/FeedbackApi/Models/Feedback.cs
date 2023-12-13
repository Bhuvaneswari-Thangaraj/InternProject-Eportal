using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FeedbackApi.Models;

public class Feedback
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int? Id{get; set;}
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int? Rate { get; set; }
    public string? Message { get; set; }
}
