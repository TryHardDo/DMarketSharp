namespace DMarketSharp.Utils;

/// <summary>
///     This class contains constant values used by the DMarket API.
/// </summary>
public class SiteConstants
{
	/// <summary>
	///     Constant values for game types used by DMarket.
	/// </summary>
	public class GameTypes
	{
		public const string TF2 = "tf2";
		public const string CS2 = "a8db";
		public const string Dota2 = "9a92";
		public const string Rust = "rust";
	}

	/// <summary>
	///     Constant values for currency types used by DMarket.
	/// </summary>
	public class CurrencyTypes
	{
		public const string UnitedStatesDollar = "USD";
		public const string Euro = "EUR";
		public const string TurkishLira = "TRY";
		public const string PolishZloty = "PLN";
		public const string BritishPound = "GBP";
		public const string UkrainianHryvnia = "UAH";
		public const string ChineseYuanRenminbi = "CNY";
		public const string SouthKoreanWon = "KRW";
		public const string BrazilianReal = "BRL";
	}
}