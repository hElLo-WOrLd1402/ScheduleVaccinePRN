namespace Service;

public class PaymentInformationModel
{
    public string OrderId { get; set; }
    public decimal Amount { get; set; }
    public string OrderDescription { get; set; }
    public string OrderType { get; set; }
}