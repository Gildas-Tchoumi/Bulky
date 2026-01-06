using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositorie.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repositorie
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		private readonly DbSet<T> dbset;

		public Repository(ApplicationDbContext db)
		{
			_db = db;
			this.dbset = _db.Set<T>();
		}
		public void Add(T entity)
		{
			 dbset.Add(entity);
		}

		public T Get(Expression<Func<T, bool>> filter)
		{
			//preparer une requete
			IQueryable<T> query = dbset;
			//ajouter un filtre ou une condition 
			query = query.Where(filter);
			// executer la requete et retourner le premier element trouver
			return query.FirstOrDefault();
		}

		public IEnumerable<T> GetAll()
		{
			//preparer une requete
			IQueryable<T> query = dbset;
			// executer la requete et retourner le premier element trouver
			return query.ToList();
		}

		public void Remove(T entity)
		{
			dbset.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entity)
		{
			dbset.RemoveRange(entity);
		}
	}
}
