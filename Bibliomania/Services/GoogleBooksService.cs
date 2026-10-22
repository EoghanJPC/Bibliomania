using System.Text.Json;
using Bibliomania.Models;

namespace Bibliomania.Services
{
	public class GoogleBooksService
	{
		private readonly HttpClient _httpClient;
		private readonly string? _apiKey;
		public GoogleBooksService(HttpClient httpClient, IConfiguration? configuration)
		{
			_httpClient = httpClient;
			_apiKey = configuration["GoogleBooks:ApiKey"];
		}
		public async Task<List<Book>> SearchBooksAsync(string query)
		{
			var url = $"volumes?q={Uri.EscapeDataString(query)}&maxResults=10&fields=items(volumeInfo(title,authors,publishedDate,description,pageCount,imageLinks/thumbnail))";
			if(!string.IsNullOrEmpty(_apiKey))
			{
				url += $"&key={_apiKey}";
			}
			var response = await _httpClient.GetAsync($"https://www.googleapis.com/books/v1/{url}");
			response.EnsureSuccessStatusCode();

			var jsonResponse = await response.Content.ReadAsStringAsync();
			var googleBooksResponse = JsonSerializer.Deserialize<GoogleBooksResponse>(jsonResponse);

			var books = new List<Book>();
			if (googleBooksResponse?.Items != null)
			{
				foreach (var item in googleBooksResponse.Items)
				{
					var volumeInfo = item.VolumeInfo;
					if (volumeInfo != null)
					{
						var book = new Book
						{
							BookTitle = volumeInfo.Title ?? "Unknown Title",
							Authors = volumeInfo.Authors ?? new List<string> { "Unknown Author" },
							Published = ParseYear(volumeInfo.PublishedDate),
							Blurb = volumeInfo.Description,
							Pages = volumeInfo.PageCount,
							BookCover = volumeInfo.ImageLinks?.Thumbnail,
							Source = "Google Books"
						};
						books.Add(book);
					}
				}
			}
			return books;
		}

		private static int? ParseYear(string? date)
		{
			if(date != null && date.Length >= 4 && int.TryParse(date.Substring(0,4), out var year))
			{
				return year;
			}
			return null;
		}
	}
}
