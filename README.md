# DMarketSharp
This library enables you to make calls to the DMarket API using your private API key provided by the site. It automatically generates the required authorization header signatures during calls and also offers a straightforward method to implement your own API endpoint class for advanced use.

I've already incorporated some of the available endpoints provided by DMarket. Additionally, I've included examples demonstrating how you can implement them according to your specific needs.

## Main features
- Automatic authorization signature generation for all requests made through the DMarketApiClient.cs class.
- Simple and intuitive method to implement your custom request structure using the Helpers/ApiRequestBase.cs abstract class.
- Comprehensive collection of constant values utilized by the DMarket API, such as game type codes and currency types. You can access these under the DMarketSharp.Utils.SiteConstants namespace.
- Predefined endpoint structures for expedited and effortless API calls.
- Detailed error handling ensures easy debugging throughout your API interactions.

## Usage
### You have to wrap your private API key into `ApiKey` struct.
```csharp
using DMarketSharp.Helpers;

namespace MyDMarkeBot;

internal class Program
{
	private static void Main(string[] args)
	{
		var apiKey = new ApiKey("youPrivateApiKeyGoesHere");
	}
}
```

> [!IMPORTANT]
> I highly recommend implementing a configuration system to manage sensitive information like your private API key is indeed crucial for security purposes.

### Making a new DMarketApiKey instance.
This is our custom client which will be our "best friend" when we make calls to the API. It contains all the neccesary code to make a successful and fast request to DMarket. We have to provide the `ApiKey` that we initalized before as a constructor parameter.
```csharp
using DMarketSharp;
using DMarketSharp.Helpers;

namespace Testing;

internal class Program
{
	private static void Main(string[] args)
	{
		var apiKey = new ApiKey("youPrivateApiKeyGoesHere");
		var client = new DMarketApiClient(apiKey);
	}
}
```

Now the code is ready to send requests to the API. Lets continue our journey and discover how does the `ApiRequestBase.cs` works.

The following section is only important for you if you want to implement an API endpoint by yourself.
For default I pre-coded few endpoints already which also gives you examples to work with.

<details>
<summary>Working with custom api request bases.</summary>

### What is `ApiRequestBase.cs` class and how can I use them?
It is an abstract class which gives you the ability to make guided and type safe request construction trough your IDE while you are coding your software.
We can say it is a "request frame" which defines how, when and where we have to put the data to make a successful call.

The implementation of `ApiRequestBase.cs` defines the following properties for a request:
- The request type Ex.: *GET, POST, PUT, DELETE* and so on...
- The relative path from the endpoint URL. Ex.: *We have a full URL -> `https://api.dmarket.com/exchange/v1/market/items?gameId=tf2&limit=50...`. Here we only take the PATH part which is the following -> `/exchange/v1/market/items` and make sure you don't include any of the query parameters!*
- The URL query parameters, especially for *GET* requests.
- The request body structure which will be serialized as JSON during request message construction.

### Implementing `ApiRequestBase.cs`
Let's create a new fesh class and extend it with `DMarketSharp/Helpers/ApiRequestBase`. After that make sure to implement all the abstract properties from `ApiRequestBase.cs`! There are optonal properties because the `UriQueryParams` and `BodyContent` are `null` by default. Override these if neccesary.
In this example I implement the balance API endpoint from the DMarket's official API documentation. This one is very simple, it only requires the request method and the relative path.
```csharp
using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

public class BalanceApiRequest : ApiRequestBase
{
	public override HttpMethod Method => HttpMethod.Get;
	public override string RelativePath => "/account/v1/balance";
}
```

This is cool right? Now what if we face with a more complex endpoint? Here I show you how you can make a query and a body definition.

**Query parameter definition:**
At this point we have two simple ways. You can choose which one is more familiar for you.
I will show the implementation of `https://api.dmarket.com/exchange/v1/market/items` in both cases.

> [!WARNING]
> Some endpoints has `.` in it's query key and the anonymous type can't handle that case. If you face with this problem just use the Dictionary version.

- Using anonymous types:
```csharp
using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

public class MarketItems(string gameId, string currencyCode) : ApiRequestBase
{
	// Required
	public string GameId { get; } = gameId;
	public string CurrencyCode { get; } = currencyCode;

	// Optional
	public string? Title { get; init; } = null;
	public int Limit { get; init; } = 50;
	public int Offset { get; init; } = 0;
	public string OrderBy { get; init; } = "title";
	public string OrderDirection { get; init; } = "desc";
	public string? TreeFilters { get; init; } = null;
	public int PriceFrom { get; init; } = 0;
	public int PriceTo { get; init; } = 0;
	public List<string>? Types { get; init; } = null;
	public string? Cursor { get; init; }

	// Inherited
	public override HttpMethod Method => HttpMethod.Get;
	public override string RelativePath => "/exchange/v1/market/items";

	public override object? UriQueryParams => new
	{
		gameId = GameId,
		title = Title,
		limit = Limit,
		offset = Offset,
		orderBy = OrderBy,
		orderDir = OrderDirection,
		treeFilters = TreeFilters,
		currency = CurrencyCode,
		priceFrom = PriceFrom,
		priceTo = PriceTo,
		types = Types == null ? null : string.Join(',', Types),
		cursor = Cursor
	};
}
```

- Using C# dictionary:
```csharp
using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

public class MarketItems(string gameId, string currencyCode) : ApiRequestBase
{
	// Required
	public string GameId { get; } = gameId;
	public string CurrencyCode { get; } = currencyCode;

	// Optional
	public string? Title { get; init; } = null;
	public int Limit { get; init; } = 50;
	public int Offset { get; init; } = 0;
	public string OrderBy { get; init; } = "title";
	public string OrderDirection { get; init; } = "desc";
	public string? TreeFilters { get; init; } = null;
	public int PriceFrom { get; init; } = 0;
	public int PriceTo { get; init; } = 0;
	public List<string>? Types { get; init; } = null;
	public string? Cursor { get; init; }

	// Inherited
	public override HttpMethod Method => HttpMethod.Get;
	public override string RelativePath => "/exchange/v1/market/items";

	public override object? UriQueryParams => new Dictionary<string, object?>()
	{
		{ "gameId", gameId },
		{ "title", Title },
		{ "limit", Limit },
		{ "offset", Offset },
		{ "orderBy", OrderBy },
		{ "orderDir", OrderDirection },
		{ "treeFilters", TreeFilters },
		{ "currency", CurrencyCode },
		{ "priceFrom", PriceFrom },
		{ "priceTo", PriceTo },
		{ "types", Types == null ? null : string.Join(',', Types) },
		{ "cursor", Cursor }
	};
}
```

**Body structure definition:**
You can use the same principle I show you at query construction.
I show you a way to implement the `https://api.dmarket.com/marketplace-api/v1/deposit-assets` endpoint.

```csharp
using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

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
```

Basically thats all! Now you should be more familiar with that simple framework works.
</details>

### Sending request to the API
This part is very easy. Now you should use the `DMarketApiClient` that we instantained before. It has syncronized and asynchronus methods.
Now I will use the synced version in the following example. Make sure to put the calling code in a try-catch block to handle the possibility of errors during requests and implement your error handling logic.

```csharp
using DMarketSharp;
using DMarketSharp.Endpoints;
using DMarketSharp.Helpers;

namespace Testing;

internal class Program
{
	private static void Main(string[] args)
	{
		var apiKey = new ApiKey("youPrivateApiKeyGoesHere");
		var client = new DMarketApiClient(apiKey);

		// Let's make a request payload using the pre-defined api bases.
		var balanceRequest = new BalanceApiRequest();

		try
		{
			var result = client.CallEndpoint(balanceRequest);
			var resultContent = result.Content.ReadAsStringAsync().Result;

			Console.WriteLine(resultContent);
		}
		catch (Exception ex)
		{
			Console.WriteLine("An error occurred during the API call!");
			Console.WriteLine(ex.ToString());
		}
	}
}
```