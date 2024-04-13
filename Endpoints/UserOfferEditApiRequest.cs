using DMarketSharp.Helpers;
using DMarketSharp.Utils;

namespace DMarketSharp.Endpoints;

public class UserOfferEditApiRequest : ApiRequestBase
{
	public Dictionary<string, object?> OfferEditCache { get; } = [];

	public override HttpMethod Method => HttpMethod.Post;
	public override string RelativePath => "/marketplace-api/v1/user-offers/edit";

	public override object? BodyContent => new
	{
		Offers = OfferEditCache
	};

	public bool AddToDict(string offerId, string assetId, int priceInCents,
		string currencyType = SiteConstants.CurrencyTypes.UnitedStatesDollar)
	{
		var assetEditObject = new
		{
			OfferID = offerId,
			AssetID = assetId,
			Price = new
			{
				Currency = currencyType,
				Amount = priceInCents
			}
		};

		return OfferEditCache.TryAdd(offerId, assetEditObject);
	}

	public bool RemoveFromDict(string offerId)
	{
		return OfferEditCache.Remove(offerId);
	}
}