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

Now the code is ready to send requests to the API. Let's continue our journey.

The following section is only important for you if you want to implement an API endpoint by yourself.
For default I pre-coded few endpoints already which also gives you examples to work with.

<details>
<summary>Working with custom api request bases.</summary>

### What is ApiRequestBase.cs Class and How Can I Use It?
The ApiRequestBase.cs class serves as an abstract framework for constructing guided and type-safe requests within your IDE as you develop your software. Think of it as a "request framework" that dictates how, when, and where you should provide data to ensure a successful API call.

When using `ApiRequestBase.cs`, you have to define the following properties for a valid request:
- ***Request Type:*** Specifies the HTTP method (e.g., GET, POST, PUT, DELETE).
- ***Relative Path:*** Refers to the path portion of the endpoint URL. For instance, if the full URL is `https://api.dmarket.com/exchange/v1/market/items?gameId=tf2&limit=50`, the relative path would be `/exchange/v1/market/items`. It's essential to exclude any query parameters.
- ***URL Query Parameters:*** Especially relevant for GET requests, these parameters are included in the URL.
- ***Request Body Structure:*** This structure, serialized to JSON during request message construction, defines the content of the request body.

By utilizing ApiRequestBase.cs, you streamline the process of crafting requests, ensuring consistency and adherence to API specifications throughout your development workflow.

### Implementing `ApiRequestBase.cs`
1. ***Create a New Class:*** Start by creating a new class and extend it with `DMarketSharp/Helpers/ApiRequestBase`.
2. ***Implement Abstract Properties:*** Ensure to implement all the abstract properties defined in `ApiRequestBase.cs`. Note that `UriQueryParams` and `BodyContent` are optional properties, defaulting to null. Override them if necessary.

***Example: Balance API Endpoint:*** As an example, let's implement the balance API endpoint from DMarket's official API documentation. This endpoint is straightforward, requiring only the request method and the relative path.

```csharp
using DMarketSharp.Helpers;

namespace MySuperProgram
{
    public class BalanceApiRequest : ApiRequestBase
    {
        public override string RequestMethod => "GET";
        
        public override string RelativePath => "/path/to/balance/endpoint";

        // Optionally override UriQueryParams and BodyContent properties if needed
    }
}
```

This is indeed an excellent approach! But what if we encounter a more complex endpoint that requires both query parameters and a request body? Let's explore how to define both.

**Query Parameter Definition:**
When it comes to defining query parameters, we have two straightforward options.
You can choose the one that feels more familiar to you. Below, I'll demonstrate the implementation of the `https://api.dmarket.com/exchange/v1/market/items` endpoint using both approaches.

> [!WARNING]
> Some endpoints contain periods `(.)` in their query keys, which anonymous types cannot handle. If you encounter this issue, utilize the dictionary version.

- Using C#'s anonymous types:
```csharp
using DMarketSharp.Helpers;

namespace MySuperProgram;

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

- Using C# native Dictionary:
```csharp
using DMarketSharp.Helpers;

namespace MySuperProgram;

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

**Body Structure Definition:**
Defining the structure of the request body follows a similar principle to query construction.
Below, I'll demonstrate how to implement the body structure for the `https://api.dmarket.com/marketplace-api/v1/deposit-assets` endpoint.

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

At this stage, you should be more comfortable with this lightweight system. Feel free to continue reading the remaining documentation, where we'll delve into how to make an actual request to the API.


</details>

### Sending Requests to the API
This step is straightforward. Utilize the `DMarketApiClient` instance we instantiated earlier, which provides both synchronous and asynchronous methods.
In the following example, I'll demonstrate using the synchronous version. Remember to wrap your calling code in a try-catch block to handle potential errors during requests, and implement your error-handling logic accordingly.

```csharp
using DMarketSharp;
using DMarketSharp.Endpoints;
using DMarketSharp.Helpers;

namespace HiReader;

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
			Console.WriteLine("An unexpected error occurred during API call!");
			Console.WriteLine(ex.ToString());
		}
	}
}
```

> ### Disclaimer: Use of this Library
> Before proceeding, it's important to note that this library is provided for free and comes with no warranties. While every effort has been made to ensure its accuracy and functionality, it's possible that issues may arise with both the code and the accompanying documentation.
> Users are encouraged to utilize this software at their own discretion and risk. It's recommended to thoroughly review the code and documentation, and to test it in a controlled environment before deploying it in production or mission-critical systems.
> Please be aware that by using this library, you accept full responsibility for any consequences that may arise from its use, including but not limited to errors, data loss, or system instability.
> Thank you for your understanding and caution.