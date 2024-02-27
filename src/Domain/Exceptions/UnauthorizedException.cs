using System.Net;

namespace Domain.Exceptions
{
    public class UnauthorizedException: BaseException
    {
        public UnauthorizedException(
            string message = "Je hebt geen rechten om deze actie te doen." , 
            HttpStatusCode code = HttpStatusCode.Forbidden
            
            ) : base(message, code) { }

    }
}
