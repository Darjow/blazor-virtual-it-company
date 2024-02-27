using System.Net;

namespace Domain.Exceptions;
public class EntityAlreadyExistsException : BaseException
{
    public EntityAlreadyExistsException(
        string entityName,
        string parameterName, 
        string? parameterValue, 
        HttpStatusCode code = HttpStatusCode.Conflict
        
        ): base($"'{entityName}' met waarde '{parameterName}':'{parameterValue}' bestaat al.", code) { }

}