using System.Net;

namespace Domain.Exceptions;

public class EntityNotFoundException : BaseException
{
    public EntityNotFoundException(
        string entityName, 
        string parameterName , 
        object? value, 
        HttpStatusCode code = HttpStatusCode.NotFound
        
        ) : base($"{entityName} met {parameterName}:'{value?.ToString()}' is niet gevonden.", code)
    {
    }
}
