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
			//appliquons la logie de fonctionnement de inclide avec notre context et ajoute cela a la methode getall

			_db.Products.Include(u => u.Category);
			// definir les inclusions par defaut pour chaque entite
			//_db.Products.Include(u => u.Category).Include(u => u.Category).Include();
		}
		public void Add(T entity)
		{
			 dbset.Add(entity);
		}

		public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
		{
			//preparer une requete
			IQueryable<T> query = dbset;

			//ajouter les proprietees a inclure
			if (!string.IsNullOrEmpty(includeProperties))
			{
				//parcourir les proprietees a inclure ,les separer par une virgule et les ajouter a la requete
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}
			//ajouter un filtre ou une condition 
			query = query.Where(filter);
			// executer la requete et retourner le premier element trouver
			return query.FirstOrDefault();
		}

		//NB: ajouter aussi la proprietee includeProperties dans l'interface IRepository
		public IEnumerable<T> GetAll(string? includeProperties = null)
		{
			//preparer une requete
			IQueryable<T> query = dbset;
			//ajouter les proprietees a inclure
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}
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
