namespace Dnj.Colab.Samples.CustomersRestFul.Dtos;

public class CustomerGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Cellphone { get; set; }
    public DateTime Dateofcreation { get; set; }

    public CustomerGetDto() => Dateofcreation = DateTime.Now;
}