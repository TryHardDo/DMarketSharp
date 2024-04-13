using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

/// <summary>
///     Getting general user profile information.
/// </summary>
public class UserDataEndpoint : EndpointBase
{
	public override HttpMethod Method => HttpMethod.Get;
	public override string RelativePath => "/account/v1/user";
	public override object? UriQueryParams => null;
	public override object? BodyContent => null;
}