namespace Api.Mutations.Requests;

public class QuantityOperation
{
    public enum Operation
    {
        Add,
        Remove
    }

    public int Quantity { get; set; }
}