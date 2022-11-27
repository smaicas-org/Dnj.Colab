namespace Dnj.Colab.Samples.SimpleCqrs.RCL.Models;
public class GameDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Platform { get; set; }

    public string Genre { get; set; }

    public DateTime ReleaseDate { get; set; }
}
