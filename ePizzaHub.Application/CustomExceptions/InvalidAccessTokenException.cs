using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.CustomExceptions
{
    public class InvalidAccessTokenException : Exception
    {
        public InvalidAccessTokenException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
