namespace Service;


public class PaymentResponseModel
{
    public string OrderId { get; set; }
    public string PaymentStatus { get; set; }
    public decimal Amount { get; set; }
}