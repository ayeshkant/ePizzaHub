using Razorpay.Api;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace ePizzaHub.UI.RazorPay
{
    public class RazorPayService : IRazorPayService
    {
        private readonly IConfiguration _configuration;
        private readonly RazorpayClient _razorpayClient;
        public RazorPayService(IConfiguration configuration)
        {
            _configuration = configuration;
            _razorpayClient = new RazorpayClient(_configuration["RazorPay:Key"], _configuration["RazorPay:Secret"]);
        }
        public string CreateOrder(decimal amount, string currency, string receipt)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"amount",Convert.ToInt32(amount)},
                {"currency",currency },
                {"receipt",receipt }
            };

            Order order = _razorpayClient.Order.Create(data);

            return order["id"].ToString();
        }

        public Payment GetPayment(string paymentId)
        {
            return _razorpayClient.Payment.Fetch(paymentId);
        }

        public bool VerifySignature(string signature, string orderId, string paymentId)
        {
            string payLoad = string.Format("{0}|{1}", orderId, paymentId);
            string secret = RazorpayClient.Secret;
            string actualSignature = GetActualSignature(payLoad, secret);

            return actualSignature.Equals(signature);
        }

        private string GetActualSignature(string payLoad, string secret)
        {
            byte[] secretBytes = StringEncode(secret);
            HMACSHA256 hashHmac = new HMACSHA256(secretBytes);
            var bytes = StringEncode(payLoad);

            return HashEncode(hashHmac.ComputeHash(bytes));
        }
        private byte[] StringEncode(string secret)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(secret);
        }
        private string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
