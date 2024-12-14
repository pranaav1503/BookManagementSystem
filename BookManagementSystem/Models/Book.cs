using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Author { get; set; }

        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }

        [Required]
        [Range(1000, 9999, ErrorMessage = "Please enter a valid year.")]
        public int PublishedYear { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
        public double DiscountPercentage { get; set; }

        public String? ImageUrl { get; set; }
        public double FinalPrice => (double)Price - ((double)Price * DiscountPercentage / 100);
    }
}
