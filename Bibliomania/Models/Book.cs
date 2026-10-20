namespace Bibliomania.Models
{
	public class Book
	{
		public string BookTitle { get; set; }
		public List<string> Authors { get; set; }
		public int? Published { get; set; }
		public string? BookCover { get; set; }
	}
}
