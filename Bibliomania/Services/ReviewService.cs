using System.Text.Json;
using Bibliomania.Models;

namespace Bibliomania.Services
{
	public class ReviewService
	{
		private readonly string _filePath;
		private List<ClubReview> _reviews = new ();

		public ReviewService(string filePath)
		{
			_filePath = filePath;
			Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
			Load();
		}

		public void AddReview(ClubReview review)
		{
			if (review.Stars < 1 || review.Stars > 5)
				throw new ArgumentException("Rate This Book Between 1 and 5 Stars:");
			if (string.IsNullOrWhiteSpace(review.MemberName))
				throw new ArgumentException("Enter Your Name Here:");

			_reviews.Add(review);
			Save();
		}

		public List<ClubReview> GetReviews(string bookKey)
		{
			return _reviews
				.Where(r => r.BookKey == bookKey)
				.OrderByDescending(r => r.MemberName)
				.ToList();
		}

		public double? GetAverageStars(string bookKey)
		{
			var matching = _reviews.Where(r => r.BookKey == bookKey).ToList();

			if (matching.Count == 0)
			{
				return null;
			}

			return matching.Average(r => r.Stars);
		}

		private void Load()
		{
			if (!File.Exists(_filePath)) 
			{
				return;
			}

			try
			{
				var json = File.ReadAllText(_filePath);
				_reviews = JsonSerializer.Deserialize<List<ClubReview>>(json) ?? new();
			}
			catch (JsonException)
			{
				_reviews = new();
			}
		}

		private void Save()
		{
			var json = JsonSerializer.Serialize(_reviews, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_filePath, json);
		}
	}
}
