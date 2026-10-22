using System.Text.Json.Serialization;

namespace Bibliomania.Models
{
	public class GoogleBooksResponse
	{
		[JsonPropertyName("items")]
		public List<GoogleBooksItem>? Items { get; set; };
	}
	public class GoogleBooksItem
	{
		[JsonPropertyName("volumeInfo")]
		public GoogleBooksVolumeInfo? VolumeInfo { get; set; }
	}
	public class GoogleBooksVolumeInfo
	{
		[JsonPropertyName("title")]
		public string? Title { get; set; }
		[JsonPropertyName("authors")]
		public List<string>? Authors { get; set; }
		[JsonPropertyName("publishedDate")]
		public string? PublishedDate { get; set; }
		[JsonPropertyName("description")]
		public string? Description { get; set; }
		[JsonPropertyName("pageCount")]
		public int? PageCount { get; set; }
		[JsonPropertyName("imageLinks")]
		public GoogleBooksImageLinks? ImageLinks { get; set; }
	}
	public class GoogleBooksImageLinks
	{
		[JsonPropertyName("thumbnail")]
		public string? Thumbnail { get; set; }
	}
}
