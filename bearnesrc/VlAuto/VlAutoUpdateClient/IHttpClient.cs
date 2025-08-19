/* by Tuyết Nhi */
/* FIX warning CS8625: Cannot convert null literal to non-nullable reference type. */

using System.Net.Http.Headers;
using System.Text;
using System.Xml;

namespace VlAutoUpdateClient;

public interface IHttpClient
{
    System.Net.Http.HttpClient GetHttpClient();
    Task<HttpResponseMessage> Get(string uri, string? authorizationToken = null,
        string authorizationMethod = "Bearer");
    Task<HttpResponseMessage> Get(Uri uri);
    Task<Stream> GetStream(string url);
    Task<HttpResponseMessage> GetWithUserAgent(string uri, string userAgent, bool isAuthorization, string hostName);
    Task<HttpResponseMessage> HeadWithUserAgent(string uri, string userAgent);

    Task<HttpResponseMessage> Get(string uri, IDictionary<string, string> headers);

    Task<HttpResponseMessage> Post<T>(string uri, T item, string? authorizationToken = null,
        string authorizationMethod = "Bearer");


    Task<HttpResponseMessage> Post(string uri, MultipartFormDataContent content);
    Task<HttpResponseMessage> Post(string uri, FormUrlEncodedContent content);

    Task<HttpResponseMessage> Post(string uri, string content);

    Task<HttpResponseMessage> Post(string uri);
    Task<HttpResponseMessage> Post(string uri, IDictionary<string, string> headers);
    Task<HttpResponseMessage> Post<T>(string uri, T item, IDictionary<string, string> headers);

    Task<HttpResponseMessage> Delete(string uri, string? authorizationToken = null,
        string authorizationMethod = "Bearer");

    Task<HttpResponseMessage> Put<T>(string uri, T item, string? authorizationToken = null,
        string authorizationMethod = "Bearer");

    Task<HttpResponseMessage> Post(string uri, StringContent content);

    Task<HttpResponseMessage> Post(string uri, string action, XmlDocument doc);

    Task<HttpResponseMessage> PostSoap(string uri, string action, string content);

    Task<string> Upload(string url, string fileName, byte[] bytes);

    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption,
        CancellationToken cancellationToken);
}

public delegate IHttpClient HttpClientResolver(string key);

public delegate IHttpClient HttpClientResolverWithToken(string key, string token);


public class HttpClient : IHttpClient
{
    private readonly System.Net.Http.HttpClient _client;

    public HttpClient(System.Net.Http.HttpClient client)
    {
        _client = client;
    }
    public System.Net.Http.HttpClient GetHttpClient()
    {
        return _client;
    }
    public async Task<HttpResponseMessage> Get(string uri, string? authorizationToken = null,
        string authorizationMethod = "Bearer")
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        if (authorizationToken != null)
        {
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
        }

        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> Get(Uri uri)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        var response = await _client.SendAsync(requestMessage);
        return response;
    }
    public async Task<Stream> GetStream(string url)
    {
        var response = await _client.GetStreamAsync(url);
        return response;
    }
    public async Task<HttpResponseMessage> GetWithUserAgent(string uri, string userAgent, bool isAuthorization,
        string hostName)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        if (userAgent?.Length > 0)
        {
            requestMessage.Headers.Add("User-Agent", userAgent);
        }

        if (isAuthorization)
        {
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("local", "1");
        }

        if (hostName?.Length > 0)
        {
            requestMessage.Headers.Host = hostName;
        }

        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> HeadWithUserAgent(string uri, string userAgent)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Head, uri);
        if (userAgent?.Length > 0)
        {
            requestMessage.Headers.Add("User-Agent", userAgent);
        }

        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> Get(string uri, IDictionary<string, string> headers)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        if (headers?.Count > 0)
        {
            foreach (var header in headers)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> Post<T>(string uri, T item, string? authorizationToken = null,
        string authorizationMethod = "Bearer")
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
        string json = Common.JsonSerializeObject(item);
        requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
        if (authorizationToken != null)
        {
            requestMessage.Headers.Authorization = string.IsNullOrEmpty(authorizationMethod)
                ? new AuthenticationHeaderValue(authorizationToken)
                : new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
        }

        var response = await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
        return response;
    }

    public async Task<HttpResponseMessage> Post(string uri, MultipartFormDataContent content)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = content
        };
        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> Post(string uri, FormUrlEncodedContent content)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = content
        };
        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> Post(string uri, string content)
    {
        var requestMessage = content?.Length > 0
            ? new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json")
            }
            : new HttpRequestMessage(HttpMethod.Post, uri);
        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> Post(string uri)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> Post(string uri, IDictionary<string, string> headers)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
        if (headers?.Count > 0)
        {
            foreach (var header in headers)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> Post<T>(string uri, T item, IDictionary<string, string> headers)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
        string json = Common.JsonSerializeObject(item);
        requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
        if (headers?.Count > 0)
        {
            foreach (var header in headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        var response = await _client.SendAsync(requestMessage);
        return response;
    }

    public async Task<HttpResponseMessage> Put<T>(string uri, T item, string? authorizationToken = null,
        string authorizationMethod = "Bearer")
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
        string json = Common.JsonSerializeObject(item);
        requestMessage.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        if (authorizationToken != null)
        {
            requestMessage.Headers.Authorization = string.IsNullOrEmpty(authorizationMethod)
                ? new AuthenticationHeaderValue(authorizationToken)
                : new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
        }

        var response = await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
        return response;
    }

    public async Task<HttpResponseMessage> Delete(string uri, string? authorizationToken = null,
        string authorizationMethod = "Bearer")
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

        if (authorizationToken != null)
        {
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
        }

        return await _client.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> Post(string uri, StringContent content)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = content
        };
        return await _client.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> Post(string uri, string action, XmlDocument doc)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(doc.InnerXml, Encoding.UTF8, "text/xml")
        };
        requestMessage.Headers.Add("SOAPAction", action);

        return await _client.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> PostSoap(string uri, string action, string content)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(content, Encoding.UTF8, "text/xml")
        };
        requestMessage.Headers.Add("SOAPAction", action);

        return await _client.SendAsync(requestMessage);
    }

    public async Task<string> Upload(string url, string fileName, byte[] bytes)
    {
        try
        {
            //var url = (Url.EndsWith("/") ? $"{Url}Images" : $"{Url}/Images").ToLower();
            var fileContent = new ByteArrayContent(bytes);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = fileName,
                Name = fileName,
            };
            using var content = new MultipartFormDataContent();
            content.Add(fileContent, "files");
            // Stream stream = new MemoryStream(bytes);
            // var streamContent = new StreamContent(stream);
            // content.Add(streamContent, "files", fileName);
            var response = await _client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase})");
        }
        catch (HttpRequestException e)
        {
            e.Data["Param"] = new { fileName, bytes };
            throw;
        }
        catch (Exception e)
        {
            e.Data["Param"] = new { fileName, bytes };
            throw;
        }
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption,
        CancellationToken cancellationToken)
    {
        return _client.SendAsync(request, completionOption, cancellationToken);
    }


}
