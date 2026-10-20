using System.Text.Json;
using Bibliomania.Models;

namespace Bibliomania.Services
{
	public class OpenLibraryService
	{
		private readonly HttpClient _httpClient;

		public OpenLibraryService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<Book>> SearchBooksAsync(string query)
		{
			var response = await _httpClient.GetAsync($"https://openlibrary.org/search.json?q={query}");
			response.EnsureSuccessStatusCode();

			var jsonResponse = await response.Content.ReadAsStringAsync();
			var openLibraryResponse = JsonSerializer.Deserialize<OpenLibrary>(jsonResponse);

			var books = new List<Book>();

			if (openLibraryResponse?.Docs != null)
			{
				foreach (var doc in openLibraryResponse.Docs)
				{
					var book = new Book
					{
						BookTitle = doc.Title ?? "Unknown Title",
						Authors = doc.AuthorName ?? new List<string> { "Unknown Author" },
						Published = doc.FirstPublishYear,
						BookCover = doc.CoverId.HasValue ? $"https://covers.openlibrary.org/b/id/{doc.CoverId}-M.jpg" : null
					};
					books.Add(book);
				}
			}
			return books;
		}
	}
}
