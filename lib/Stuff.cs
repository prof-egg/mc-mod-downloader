using MikManager.Utility;

static class HttpResponseMessageExtensions
{
    internal static void WriteRequestToConsole(this HttpResponseMessage response)
    {
        if (response is null)
        {
            return;
        }

        var request = response.RequestMessage;
        Debug.LogInfo($"{request?.Method} {request?.RequestUri} HTTP/{request?.Version}");
    }
}

