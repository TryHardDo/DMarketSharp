using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

/// <summary>
///     Getting the current USD and DMC balance that is available for trading items /
///     buying subscriptions. The response format is in coins (cents for USD, dimoshi for DMC).
/// </summary>
public class BalanceApiRequest : ApiRequestBase
{
	public override HttpMethod Method => HttpMethod.Get;
	public override string RelativePath => "/account/v1/balance";
}