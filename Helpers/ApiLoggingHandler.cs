using Serilog;

namespace UserService.Helpers;

public class ApiLoggingHandler : MessageProcessingHandler
{
    protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Log.Information($"Request - RequestUri = {request.RequestUri}, Method = {request.Method}, " +
                        $"Content = {SerializeRequestHttpContent(request.Content)}, Scheme = {request.RequestUri.Scheme}");
        return request;
    }

    protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        Log.Information($"Response - Status = {response.StatusCode}, Content = {response.Content.ReadAsStringAsync().Result}");
        return response;
    }

    private string SerializeRequestHttpContent(HttpContent httpContent)
    {
        if (httpContent == null)
        {
            return "";
        }

        var stream = new StreamReader(httpContent.ReadAsStreamAsync().Result);
        stream.BaseStream.Position = 0;
        return stream.ReadToEnd();
    }
}