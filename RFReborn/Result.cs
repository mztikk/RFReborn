namespace RFReborn;

public class Result<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Value { get; set; }
}
