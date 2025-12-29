using ePizzaHub.UI.Models.Response;

namespace ePizzaHub.UI.Models.ViewModels
{
    public class PaymentViewModel
    {
        public string Receipt {  get; set; }

        public string Currency { get; set; }

        public string Name { get; set; }


        public string RazorPayKey { get; set; }

        public decimal GrantTotal { get; set; }

        public string Description { get; set; }


        public string OrderId {  get; set; }

        public CartResponseDto Cart { get; set; }

    }
}
