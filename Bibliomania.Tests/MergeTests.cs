using Bibliomania.Models;
using Bibliomania.Services;
using Xunit;

namespace Bibliomania.Tests
{
	public class MergeTests
	{
		[Fact]
		public void Merge_SameBookFomBothAPIs()
		{
			var fromOpenLibrary = new Book
			{
				BookTitle = "Dune",
				Authors = new List<string> { "Frank Herbert" },
				Published = 1965,
				BookCover = "https://example.com/cover.jpg",
				Source = "Open Library"
			};
			var fromGoogle = new Book
			{
				BookTitle = "dune",
				Authors = new List<string> { "Frank Herbert" },
				Blurb = "A desert planet story.",
				Pages = 412,
				Source = "Google Books"
			};

			var result = BookMerger.Merge(new[] { fromOpenLibrary, fromGoogle });

			var merged = Assert.Single(result);
			Assert.Equal(1965, merged.Published);
			Assert.Equal("https://example.com/cover.jpg", merged.BookCover);
			Assert.Equal("A desert planet story.", merged.Blurb);
			Assert.Equal(412, merged.Pages);
			Assert.Equal("Open Library + Google Books", merged.Source);
		}

		[Fact]
		public void Merge_SeperateBooks()
		{
			var a = new Book { BookTitle = "Dune", Authors = new List<string> { "Frank Herbert" }, Source = "Open Library" };
			var b = new Book { BookTitle = "Emma", Authors = new List<string> { "Jane Austen" }, Source = "Google Books" };

			var result = BookMerger.Merge(new[] { a, b });

			Assert.Equal(2, result.Count);
		}
	}
}
