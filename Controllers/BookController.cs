using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using DemoWeb.Repositories;
using DemoWeb.Data;
using DemoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWeb.Controllers
{
    
    public class BookController : Controller
    {
        private IRepository<BookModel> rep = null;
        public BookController(BaseContext context){
            rep = new BaseRepository<BookModel>(context);
        }
        [HttpGet]
        public  IActionResult Index(string sortOrder,string searchStr){
            ViewData["NameSortParm"] =  string.IsNullOrEmpty(sortOrder)  ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "price_desc" ? "price_asc" : "price_desc";
            ViewBag.SearchTxt = string.IsNullOrEmpty(searchStr) ? "" : searchStr;
            var books = rep.GetAll();
            if(!string.IsNullOrEmpty(searchStr))
            {
                books = books.Where(x=>x.BookName.Contains(searchStr));
            }
            books = BooksSort(books,sortOrder);
            return View(books);
        }
        [HttpPost]
        public IActionResult Index(string searchStr){
            var books = rep.GetAll();
            ViewData["NameSortParm"] =  "";
            ViewData["PriceSortParm"] = "";
            ViewBag.SearchTxt =searchStr;
            if(!string.IsNullOrEmpty(searchStr))
            {
                books = books.Where(x=>x.BookName.Contains(searchStr));
            }
            return View(books);
        }
        [HttpGet]
        public IActionResult Create(){

            return View();
        }
        [HttpPost]
        public IActionResult Create(BookModel model){
             try{
                rep.Create(model);
                return RedirectToAction("Index");
             }
             catch(Exception e)
             {
                ViewBag.error = "N";
                return View(model);
             }
        }
        [HttpGet]
        public IActionResult Update(int id){
           BookModel book = rep.GetOne(x=>x.Id == id);
           if(book!=null)
           {
              return View(book);
           }
           else{
             return RedirectToAction("Index");
           }
        }
        [HttpPost]
        public IActionResult Update(BookModel model){
           try{
             rep.Update(model);
             return RedirectToAction("Index");
           }
           catch(Exception e)
           {
              ViewBag.error = "N";
              return View(model);
           }
        }
        [HttpPost]
        public IActionResult Delete(int id){
            try{
                BookModel book  =  rep.GetOne(x=>x.Id == id);
                rep.Delete(book);
                return Content("Y");
            }
            catch(Exception e){
                return Content("N");
            }
        }
        [NonAction] 
        private IQueryable<BookModel> BooksSort(IQueryable<BookModel> books , string sortOrder){
            string sortCol = "name";
            string sort = "desc";
            if(!string.IsNullOrEmpty(sortOrder))
            {
                sortCol = sortOrder.Split('_')[0];
                sort = sortOrder.Split('_')[1];
            }
            Expression<Func<BookModel,string>> expressionStr = null;
            Expression<Func<BookModel,int>> expressionInt = null;
            switch(sortCol){
              case "name":
              default:
                 expressionStr =  x => x.BookName;
                 if(sort == "asc"){
                     books = this.SortAsc(books,expressionStr);
                 }
                 else{
                     books = this.SortDesc(books.AsQueryable(),expressionStr);
                     ViewData["NameSort"] = "asc";
                 }
                  break;
              case "price":
                   expressionInt = x=>x.Price;
                     if(sort == "asc"){
                     books = this.SortAsc(books.AsQueryable(),expressionInt);
                    }
                    else{
                        books = this.SortDesc(books.AsQueryable(),expressionInt);
                    }
               break;
            }
            return  books;
        } 
        [NonAction]
        private IQueryable<TEntity> SortAsc<TEntity,TOrderBy>(IQueryable<TEntity> source, Expression<Func<TEntity,TOrderBy>> expression) where TEntity:class
        {
            return source.OrderBy(expression);
        }
        private IQueryable<TEntity> SortDesc<TEntity,TOrderBy>(IQueryable<TEntity> source, Expression<Func<TEntity,TOrderBy>> expression) where TEntity:class
        {
             return source.OrderByDescending(expression);
        }
    }
}