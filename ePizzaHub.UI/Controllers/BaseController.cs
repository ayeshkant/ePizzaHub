using ePizzaHub.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ePizzaHub.UI.Controllers
{
    public class BaseController : Controller
    {
        protected UserViewModel? CurrentUser
        {
            get
            {
                if (User.Claims.Any())
                {
                    string email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                    string userId = User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
                    string userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

                    return new UserViewModel()
                    {
                        UserId = Convert.ToInt32(userId),
                        Email = email,
                        Name = userName,
                        PhoneNumber="9999999999"
                    };
                }
                return null;
            }
        }
    }
}
