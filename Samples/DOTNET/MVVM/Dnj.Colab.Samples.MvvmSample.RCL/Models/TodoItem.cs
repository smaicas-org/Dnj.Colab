namespace Dnj.Colab.Samples.MvvmSample.RCL.Model;

public class TodoItem
{
    public TodoItem()
    {
        Id = new Guid();
        Done = false;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Notes { get; set; }
    public DateTime? Date { get; set; }
    public bool Done { get; set; }
}
