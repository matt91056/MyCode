using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DemoWeb.Data;
namespace DemoWeb.Repositories
{
    public class BaseRepository<TEntity>:IRepository<TEntity> where TEntity:class
    {
        protected BaseContext _contex = null;
        public BaseRepository(BaseContext context){

            this._contex = context;
        }
        public void Create(TEntity instant){
          _contex.Set<TEntity>().Add(instant);
          this.SaveChanges();
        }
        public void Update(TEntity instant)
        {
             if(instant == null){
                  throw new ArgumentNullException("instance");
             }
             else{
                   this._contex.Entry<TEntity>(instant).State = EntityState.Modified;
                   this.SaveChanges();
             }
        }
        public  TEntity GetOne(Expression<Func<TEntity, bool>> predicate){
           return  _contex.Set<TEntity>().FirstOrDefault(predicate);
        }
        public void Delete (TEntity instance)
        {
            if(instance == null){
                 throw new ArgumentNullException("instance");
            }
            else{
                 this._contex.Entry(instance).State = EntityState.Deleted;
                 this.SaveChanges();
            }
        }
        public IQueryable<TEntity> GetAll(){
            return  _contex.Set<TEntity>();
        }
        public void SaveChanges(){

            _contex.SaveChanges();
        }
    }
}