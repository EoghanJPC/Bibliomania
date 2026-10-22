namespace Bibliomania.Models
{
	public class Book
	{
		public string BookTitle { get; set; }
		public List<string> Authors { get; set; }
		public int? Published { get; set; }
		public string? BookCover { get; set; }

		public string? Blurb { get; set; }
		public int? Pages { get; set; }
		public string Source { get; set; }

		public string Key => $"{BookTitle}|{Authors.FirstOrDefault()}".ToLowerInvariant;
	}
}
