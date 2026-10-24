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

		public async Task<List<Book>> FindBooksAsync(string query)
		{
			var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}&limit=10&fields={Fields}";
			return await SearchBooksAsync(url);
		}

		public async Task<List<Book>> GetNewReleasesAsync(int count = 12)
		{
			var year = DateTime.Now.Year;
			var query = $"first_publish_year:[{year - 1} TO {year}] language:eng";
			var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}&sort=new&limit=60&fields={Fields}";

			var books = await SearchBooksAsync(url);

			return books
				.Where(b => b.BookCover != null)
				.DistinctBy(b => b.Key)
				.Take(count)
				.ToList();
		}

		public async Task<List<Book>> SearchBooksAsync(string query)
		{
			var response = await _httpClient.GetAsync($"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}&limit=10&fields=title,author_name,first_publish_year,cover_i");
			response.EnsureSuccessStatusCode();

			var jsonResponse = await response.Content.ReadAsStringAsync();
			var openLibraryResponse = JsonSerializer.Deserialize<OpenLibraryResponse>(jsonResponse);

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
						BookCover = doc.CoverId.HasValue ? $"https://covers.openlibrary.org/b/id/{doc.CoverId}-M.jpg" : null,
						Source = "Open Library"
					};
					books.Add(book);
				}
			}
			return books;
		}
	}
}
