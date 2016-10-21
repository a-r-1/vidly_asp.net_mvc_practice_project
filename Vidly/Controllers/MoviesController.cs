﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!", Id = 3, Duration = 1.5 };

            var customers = new List<Customer>
            {
                new Customer { Name = "customer 1" },
                new Customer { Name = "customer 2" }
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel); //return a View
            //return View(movie);

            //return Content("hello world!"); //return the content, similar to Laravel's return $request->all();
            //return HttpNotFound(); //return 404
            //return new EmptyResult(); //return empty page
            //return RedirectToAction("Index", "Home", new { page = 1, sortBy = "name" } ); //specify <ActionName, Controller, [{args to pass as URL query string}]> to redirect to
        }

        #region ActionResult with 1 param
        //public ActionResult Edit(int id)
        //{
        //    //cannot use movieId as param even if that is the name of the arg b/c default param as specified in RouteConfig is {id}
        //    //return Content(string.Format("id = {0}", movieId)); 

        //    //must use id as route param
        //    return Content(string.Format("id = {0}", id));
        //}
        #endregion


        #region ActionResult with nullable and optional params
        //public ActionResult Index(int? pageIndex, string sortBy)
        //{
        //    if (!pageIndex.HasValue)
        //        pageIndex = 1;

        //    if (String.IsNullOrWhiteSpace(sortBy))
        //        sortBy = "Name";

        //    return Content(string.Format("pageIndex = {0}, sortBy = {1}", pageIndex, sortBy));
        //}
        #endregion

        #region GET: movies/released/{year}/{month} as defined in our custom MapRoute in RouteConfig
        //[Route("movies/released/{year:regex(\\d{4}):range(2012,2016)}/{month:regex(\\d{2}):range(1,12)}")] //attr route with constraints
        //public ActionResult ByReleaseDate(int year, int month)
        //{
        //    return Content(month + "/" + year);
        //}
        #endregion

        public ActionResult New()
        {
            var genres = _context.Genres.ToList();

            var viewModel = new MovieFormViewModel
            {
                Genres = genres
            };

            return View("MovieForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);


            if (movie == null)
            {
                return HttpNotFound();
            }
            else
            {
                var viewModel = new MovieFormViewModel
                {
                    Movie = movie,
                    Genres = _context.Genres.ToList()
                };
                return View("MovieForm", viewModel); //specify the view you want to return
            }
        }

        public ActionResult Save(Movie movie)
        {
            if(movie.Id == 0)
            {
                _context.Movies.Add(movie);
            }
            else
            {
                var movieInDb = _context.Movies.Single(mov => mov.Id == movie.Id);

                movieInDb.Name = movie.Name;
                movieInDb.Duration = movie.Duration;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.NumInStock = movie.NumInStock;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }


        #region GET: movies/index
        public ActionResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre).ToList();

            return View(movies);
        }
        #endregion

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(movie);
            }
        }

        //private IEnumerable<Movie> GetMovies()
        //{
        //    return new List<Movie>
        //    {
        //        new Movie { Id = 1, Name = "Rent", Duration = 2 },
        //        new Movie { Id = 2, Name = "Cats", Duration = 3 }
        //    };
        //}

    }
}