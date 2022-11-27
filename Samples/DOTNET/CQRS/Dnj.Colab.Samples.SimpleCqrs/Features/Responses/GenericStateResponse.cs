namespace Dnj.Colab.Samples.SimpleCqrs.Features.Responses;

public class GenericStateResponse
{
    public StateEnum State { get; set; }
    public string Message { get; set; } = string.Empty;
}

public enum StateEnum
{
    Ok,
    Ko
}
