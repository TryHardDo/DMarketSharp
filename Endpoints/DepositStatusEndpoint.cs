using DMarketSharp.Helpers;
using Flurl;

namespace DMarketSharp.Endpoints;

/// <summary>
///     Get information about current deposit transfers.
/// </summary>
public class DepositStatusEndpoint(string depositId) : EndpointBase
{
	public string DepositId { get; } = depositId;

	public override HttpMethod Method => HttpMethod.Get;
	public override string RelativePath => "/marketplace-api/v1/deposit-status".AppendPathSegment(DepositId);
	public override object? UriQueryParams => null;
	public override object? BodyContent => null;
}