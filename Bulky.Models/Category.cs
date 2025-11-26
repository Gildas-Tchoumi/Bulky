using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
	public class Category
	{
		public int categoryId { get; set; }
		[Required]
		[DisplayName("Name")]
		[MaxLength(20)]
		public string Name { get; set; }

		[Range(1,100)]
		[DisplayName("DisplayOrder")]
		public int DisplayOrder { get; set; }
	}
}
