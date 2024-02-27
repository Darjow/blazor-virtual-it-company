using Domain.Exceptions;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text.Json;

public static class ResponseHelper
{
    public static async Task HandleErrorResponse(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorResponseContent = await response.Content.ReadAsStringAsync();
            var parsed = JObject.Parse(errorResponseContent);
            var message = parsed["message"]?.ToString();

            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    throw new ApplicationException(message);
                }
            }
            catch (JsonException)
            {
                throw new ApplicationException("Iets onverwachts is gebeurd.");
            }
        }
    }
}
