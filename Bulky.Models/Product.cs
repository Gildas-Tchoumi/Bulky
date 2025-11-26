using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models
{
	public class Product
	{
		public int Id { get; set; }
		[Required]
		[DisplayName("Title")]
		[MaxLength(20)]
		public string Title { get; set; }

		public string description { get; set; }
		[Required]
		public string ISBN { get; set; }
		[Required]
		public string Author { get; set; }

		[Required]
		[DisplayName("ListePrice")]
		[Range(1,1000)]
		public double ListePrice { get; set; }

		[Required]
		[DisplayName("Price for 1-50")]
		[Range(1, 1000)]
		public double Price { get; set; }

		[Required]
		[DisplayName("Price for 50+")]
		[Range(1, 1000)]
		public double Price50 { get; set; }

		[Required]
		[DisplayName("Price for 100+")]
		[Range(1, 1000)]
		public double Price100 { get; set; }
	}
}
