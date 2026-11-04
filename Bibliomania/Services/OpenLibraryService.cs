using System.Text.Json;
using Bibliomania.Models;

namespace Bibliomania.Services
{
	public class OpenLibraryService
	{
		private readonly HttpClient _httpClient;
		private const string Fields = "title,author_name,first_publish_year,cover_i";

		public OpenLibraryService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<Book>> SearchBooksAsync(string query)
		{
			var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}&limit=10&fields={Fields}";
			return await FetchBooksAsync(url);
		}

		public async Task<List<Book>> GetNewReleasesAsync(int count = 12)
		{
			var year = DateTime.Now.Year;

			foreach (var yearsBack in new[] { 1, 3, 10 })
			{
				var query = $"first_publish_year:[{year - yearsBack} TO {year}]";
				var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}&sort=new&limit=100&fields={Fields}";

				var books = await FetchBooksAsync(url);

				var withCovers = books
					.Where(b => b.BookCover != null)
					.DistinctBy(b => b.Key)
					.Take(count)
					.ToList();

				if (withCovers.Count > 0)
				{
					return withCovers;
				}
			}

			return new List<Book>();
		}

		private async Task<List<Book>> FetchBooksAsync(string url)
		{
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();

			var jsonResponse = await response.Content.ReadAsStringAsync();
			var openLibraryResponse = JsonSerializer.Deserialize<OpenLibraryResponse>(jsonResponse);

			var books = new List<Book>();

			if (openLibraryResponse?.Docs != null)
			{
				foreach (var doc in openLibraryResponse.Docs)
				{
					books.Add(new Book
					{
						BookTitle = doc.Title ?? "Unknown Title",
						Authors = doc.AuthorName ?? new List<string> { "Unknown Author" },
						Published = doc.FirstPublishYear,
						BookCover = doc.CoverId.HasValue ? $"https://covers.openlibrary.org/b/id/{doc.CoverId}-M.jpg" : null,
						Source = "Open Library"
					});
				}
			}
			return books;
		}
	}
}
