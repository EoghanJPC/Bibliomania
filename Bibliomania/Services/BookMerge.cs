using Bibliomania.Models;

namespace Bibliomania.Services
{
	public static class BookMerger
	{
	public static List<Book> Merge(IEnumerable<Book> books)
		{
			return books
				.GroupBy(b => b.Key)
				.Select(g => new Book
				{
					BookTitle = g.First().BookTitle,
					Authors = g.First().Authors,
					Published = g.Select(b => b.Published).FirstOrDefault(p => p != null),
					BookCover = g.Select(b => b.BookCover).FirstOrDefault(c => c != null),
					Blurb = g.Select(b => b.Blurb).FirstOrDefault(d => !string.IsNullOrWhiteSpace(d)),
					Pages = g.Select(b => b.Pages).FirstOrDefault(p => p != null),
					Source = string.Join(" + ", g.Select(b => b.Source).Distinct())
				})
				.ToList();
		}
	}
}
