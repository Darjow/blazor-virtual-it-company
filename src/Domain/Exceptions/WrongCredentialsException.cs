using System.Net;

namespace Domain.Exceptions
{
   public class WrongCredentialsException: BaseException
    {
        public WrongCredentialsException(
            string message = "Verkeerde inloggegevens.", 
            HttpStatusCode code = HttpStatusCode.Unauthorized
            
            ) : base(message, code) { }
    }
   
}
