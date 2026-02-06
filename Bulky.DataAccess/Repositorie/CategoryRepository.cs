using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositorie.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repositorie
{
	public class CategoryRepository : Repository<Category>, ICategoriesRepository
	{
		private readonly ApplicationDbContext _db;

		public CategoryRepository(ApplicationDbContext db) : base(db) 
		{
			this._db = db;
		}
		
		public void Update(Category obj)
		{
			_db.Categories.Update(obj);
		}
	}
}
