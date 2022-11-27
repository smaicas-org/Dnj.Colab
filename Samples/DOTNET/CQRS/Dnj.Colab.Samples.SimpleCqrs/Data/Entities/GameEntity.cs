using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dnj.Colab.Samples.SimpleCqrs.Data.Entities;

[Table("Games")]
public class GameEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Platform { get; set; }
    [Required]
    public string Genre { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }

}
