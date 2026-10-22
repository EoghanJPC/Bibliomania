namespace Bibliomania.Models
{
	public class ClubReview
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string BookKey { get; set; };
		public string BookTitle { get; set; };
		public string MemberName { get; set; };
		public int StarRating { get; set; };
		public string? ReviewText { get; set; };
		public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
	}
}
