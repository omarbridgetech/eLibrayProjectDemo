using System.ComponentModel.DataAnnotations;

namespace eLibrayProjectDemo.Models
{
    public class Book
    {
        [Key]
        public int EbookId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = "Book-Title";

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = "Book-Author";

        [StringLength(100)]
        public string Publisher { get; set; } = "Book-Publisher";

        [Required]
        public string Formats { get; set; } = "pdf";

        [Required]
        public decimal PriceBorrow { get; set; }

        [Required]
        public decimal PriceBuy { get; set; }

        public int PublicationYear { get; set; }

        [Required]
        public int AgeRating { get; set; }

        [Required]
        public int AvailableCopies { get; set; }

        [Required]
        public bool IsBuyOnly { get; set; }

        [Required]
        [StringLength(255)]
        public string CoverImagePath { get; set; } = "";

        [Required]
        public string Category { get; set; } = "Book-Category";

        public string Description { get; set; } = "Book-Description";

        public DateTime AddedAt { get; set; }
    }
}
