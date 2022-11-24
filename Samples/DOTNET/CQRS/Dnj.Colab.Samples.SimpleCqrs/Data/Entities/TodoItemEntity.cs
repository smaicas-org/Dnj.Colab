using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dnj.Colab.Samples.SimpleCqrs.Data.Entities;

[Table("TodoItems")]
public class TodoItemEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Notes { get; set; }
    [Required]
    public DateTime? Date { get; set; }
    [Required]
    public bool Done { get; set; } = false;
}
