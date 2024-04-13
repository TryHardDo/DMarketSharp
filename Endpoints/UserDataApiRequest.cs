using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

/// <summary>
///     Getting general user profile information.
/// </summary>
public class UserDataApiRequest : ApiRequestBase
{
	public override HttpMethod Method => HttpMethod.Get;
	public override string RelativePath => "/account/v1/user";
}