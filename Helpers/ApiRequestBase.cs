using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using Flurl;

namespace DMarketSharp.Helpers;

public abstract class ApiRequestBase
{
	private const string BaseUri = "https://api.dmarket.com";

	public abstract HttpMethod Method { get; }
	public abstract string RelativePath { get; }
	public virtual object? UriQueryParams => null;
	public virtual object? BodyContent => null;

	public HttpRequestMessage ConstructBaseHttpRequestMessage()
	{
		var fullUri = BaseUri.AppendPathSegment(RelativePath)
			.AppendQueryParam(UriQueryParams).ToUri();

		var requestMessage = new HttpRequestMessage(Method, fullUri);

		if (BodyContent == null) return requestMessage;

		var serializedBodyContent = JsonSerializer.Serialize(BodyContent);

		requestMessage.Content = new StringContent(serializedBodyContent,
			MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json));

		return requestMessage;
	}
}