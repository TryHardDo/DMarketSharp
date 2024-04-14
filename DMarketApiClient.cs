using DMarketSharp.Helpers;

namespace DMarketSharp;

/// <summary>
///     Class for interacting with the DMarket API fast and easily.
/// </summary>
/// <param name="apiKey">The DMarket private API key wrapped as <see cref="ApiKey" /> struct.</param>
public class DMarketApiClient(ApiKey apiKey) : HttpClient
{
	private const string BaseUri = "https://api.dmarket.com";

	#region Synchronized methods

	public HttpResponseMessage CallEndpoint(ApiRequestBase apiRequest)
	{
		var baseRequestMessage = apiRequest.ConstructBaseHttpRequestMessage();
		var signedRequestMessage = SignRequestMessage(baseRequestMessage);

		var result = Send(signedRequestMessage);

		EnsureRequestSuccess(result);

		return result;
	}

	#endregion

	#region Asynchronous methods

	public async Task<HttpResponseMessage> CallEndpointAsync(ApiRequestBase apiRequest)
	{
		var baseRequestMessage = apiRequest.ConstructBaseHttpRequestMessage();
		var signedRequestMessage = SignRequestMessage(baseRequestMessage);

		var result = await SendAsync(signedRequestMessage);

		EnsureRequestSuccess(result);

		return result;
	}

	#endregion

	#region Helpers

	private HttpRequestMessage SignRequestMessage(HttpRequestMessage requestMessage)
	{
		var method = requestMessage.Method;
		var requestUri = requestMessage.RequestUri ??
		                 throw new ArgumentException("Could not sign a request with null Uri!");

		var bodyStringContent = requestMessage.Content?.ReadAsStringAsync().Result;
		var signatureClass = new RequestSignature(apiKey, method, requestUri);

		if (bodyStringContent != null) signatureClass.BodyContent = bodyStringContent;

		var signature = signatureClass.GetSignature(out var signTimeStamp);

		var headers = requestMessage.Headers;
		headers.Add("X-Api-Key", apiKey.PublicKey);
		headers.Add("X-Sign-Date", signTimeStamp.ToString());
		headers.Add("X-Request-Sign", $"dmar ed25519 {signature}");

		return requestMessage;
	}

	private static void EnsureRequestSuccess(HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode) return;

		var content = response.Content.ReadAsStringAsync().Result;
		throw new HttpRequestException(
			$"Api request failed with status code '{response.StatusCode}'. Body content:\n{content}");
	}

	#endregion
}