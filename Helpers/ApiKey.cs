using System.Text;

namespace DMarketSharp.Helpers;

/// <summary>
///     A readonly helper struct to work with DMarket API key.
/// </summary>
/// <param name="privateKey">The private API key from DMarket.</param>
public readonly struct ApiKey(string privateKey)
{
	public string PrivateKey { get; } = privateKey;
	public string PublicKey => PrivateKey.Substring(64, 64);

	/// <summary>
	///     Turns the private API key (the full key) into a byte array.
	/// </summary>
	/// <returns>The private api key in bytes.</returns>
	public byte[] ToByteArray()
	{
		return Encoding.UTF8.GetBytes(PrivateKey);
	}
}