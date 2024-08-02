using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Dictionary<string, HttpResponseMessage> _responses;

    public MockHttpMessageHandler(Dictionary<string, HttpResponseMessage> responses)
    {
        _responses = responses;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestUri = request.RequestUri.ToString();

        if (_responses.ContainsKey(requestUri))
        {
            return await Task.FromResult(_responses[requestUri]);
        }

        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }
}
