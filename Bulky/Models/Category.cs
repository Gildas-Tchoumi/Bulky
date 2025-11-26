using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
	public class Category
	{
		public int categoryId { get; set; }
		[Required]
		public string Name { get; set; }
		public int DisplayOrder { get; set; }
	}
}
