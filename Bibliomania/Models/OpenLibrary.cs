using System.Text.Json.Serialization;

namespace Bibliomania.Models
{
	public class OpenLibrary
	{
		[JsonPropertyName("docs")]
		public List<OpenLibraryDoc> Docs { get; set; }
	}

	public class OpenLibraryDoc
	{
		[JsonPropertyName("title")]
		public string? Title { get; set; }
		[JsonPropertyName("author_name")]
		public List<string>? AuthorName { get; set; }
		[JsonPropertyName("publishment_year")]
		public int? FirstPublishYear { get; set; }
		[JsonPropertyName("cover")]
		public int? CoverId { get; set; }
	}
}
