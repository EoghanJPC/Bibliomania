using Bibliomania.Models;
using Bibliomania.Services;	
using Xunit;

namespace Bibliomania.Tests
{
	public class ReviewTests
	{
		private static string NewTempPath() =>
			Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

		private static ClubReview MakeReview(string key = "dune|frank herbert", string name = "Sam", int stars = 4) =>
			new ClubReview { BookKey = key, BookTitle = key, MemberName = name, Stars = stars };


		[Theory]
		[InlineData(0)]
		[InlineData(6)]
		[InlineData(-1)]
		public void NewReview_InvalidStars(int stars)
		{
			var service = new ReviewService(NewTempPath());

			Assert.Throws<ArgumentException>(() => service.AddReview(MakeReview(stars: stars)));
		}

		[Fact]
		public void Review_ReturnsAverageRating()
		{
			var service = new ReviewService(NewTempPath());
			service.AddReview(MakeReview(name: "Sam", stars: 4));
			service.AddReview(MakeReview(name: "Alex", stars: 2));

			Assert.Equal(3.0, service.GetAverageStars("dune|frank herbert"));
		}

		[Fact]
		public void Review_NoReviews()
		{
			var service = new ReviewService(NewTempPath());

			Assert.Null(service.GetAverageStars("unknown book"));
		}

		[Fact]
		public void Review_GetTopRated()
		{
			var service = new ReviewService(NewTempPath());
			service.AddReview(new ClubReview { BookKey = "low", BookTitle = "Low Book", MemberName = "Sam", Stars = 2 });
			service.AddReview(new ClubReview { BookKey = "high", BookTitle = "High Book", MemberName = "Sam", Stars = 5 });

			var top = service.getTopRated();

			Assert.Equal("High Book", top[0].BookTitle);
		}


	}		
}
	
