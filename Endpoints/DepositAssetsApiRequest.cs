using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

/// <summary>
///     Transferring items from a 3rd party inventory (e.g. a Steam game) to a DMarket inventory.
/// </summary>
public class DepositAssetsApiRequest : ApiRequestBase
{
	public List<string> AssetIds = [];

	public DepositAssetsApiRequest(params string[] assetIds)
	{
		AssetIds.AddRange(assetIds);
	}

	public DepositAssetsApiRequest(IEnumerable<string> assetIds)
	{
		AssetIds.AddRange(assetIds);
	}

	public override HttpMethod Method => HttpMethod.Post;
	public override string RelativePath => "/marketplace-api/v1/deposit-assets";
	public override object? BodyContent => new
	{
		AssetID = AssetIds
	};
}