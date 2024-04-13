using System.Text;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DMarketSharp.Helpers;

/// <summary>
///     This class if a helper class to easily construct authorization signatures for
///     the API server.
///     In normal scenarios you don't need to use this class directly.
///     Use the <see cref="ApiClient" /> to make guided calls to the API.
/// </summary>
/// <param name="apiKey">The private API key wrapped as an <see cref="ApiKey"/> struct.</param>
/// <param name="method">The request's method to be signed.</param>
/// <param name="fullUri">The full <see cref="Uri" /> containing the paths and the query parameters.</param>
internal sealed class RequestSignature(ApiKey apiKey, HttpMethod method, Uri fullUri)
{
	public HttpMethod Method { get; } = method;
	public Uri FullUri { get; } = fullUri;

	/// <summary>
	///     Optional value.
	///		If the request which needs signature has body content (most likely for POST and PUT methods) it should be set under this property.
	///		It is important to include the exact same body content like the <see cref="HttpRequestMessage"/> holds to make sure
	///		we receive a valid authorization signature from the signer algorithm.
	/// </summary>
	public string? BodyContent { get; set; } = null;

	/// <summary>
	///     Generates a signature based on the constructed class instance data.
	/// </summary>
	/// <param name="signTimeStamp">The exact Unix timestamp when the signature was generated.</param>
	/// <returns>The signature as a hexadecimal string.</returns>
	public string GetSignature(out long signTimeStamp)
	{
		signTimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

		var unsignedStr = $"{Method}{FullUri.PathAndQuery}{BodyContent ?? string.Empty}{signTimeStamp}";

		var unsignedBytes = Encoding.UTF8.GetBytes(unsignedStr);
		var privateKeyBytes = Convert.FromHexString(apiKey.PrivateKey);

		var signer = new Ed25519Signer();
		signer.Init(true, new Ed25519PrivateKeyParameters(privateKeyBytes, 0));
		signer.BlockUpdate(unsignedBytes, 0, unsignedBytes.Length);

		var signature = signer.GenerateSignature();

		return BitConverter.ToString(signature).Replace("-", "");
	}
}