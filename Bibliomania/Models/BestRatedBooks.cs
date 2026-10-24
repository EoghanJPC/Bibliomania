namespace Bibliomania.Models
{
	public class BestRatedBooks
	{
		public string BookKey { get; set; }
		public string BookTitle { get; set; }
		public string Authors { get; set; }
		public string? BookCover { get; set; }
		public double AverageStars { get; set; }
		public int ReviewCount { get; set; }
	}
}
