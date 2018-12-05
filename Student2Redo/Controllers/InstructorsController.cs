using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Student2Redo.Models;
using Student2Redo.Models.ViewModels;


namespace Student2Redo.Controllers
{
    public class InstructorsController : Controller
    {
            private readonly IConfiguration _config;

            public InstructorsController(IConfiguration config)
            {
                _config = config;
            }

            public IDbConnection Connection
            {
                get
                {
                    return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                }
            }

            // GET: Instructor
            public async Task<ActionResult> Index()
            {
                using (IDbConnection conn = Connection)
                {

                    IEnumerable<Instructor> instructors = await conn.QueryAsync<Instructor>(@"
                    SELECT 
                        i.Id,
                        i.FirstName,
                        i.LastName,
                        i.SlackHandle,
                        i.Specialty,
                        i.CohortId
         
                    FROM Instructor i
                ");
                    return View(instructors);
                }
            }

        // GET: Instructor/Details/5

        public async Task<ActionResult> Details(int id)
        {
            string sql = $@"
                   SELECT
                        i.Id,
                        i.FirstName,
                        i.LastName,
                        i.SlackHandle,
                        i.Specialty,
                        i.CohortId,
                        c.Id,
                        c.Name
            FROM Instructor i
                JOIN Cohort c ON c.Id = i.CohortId
            WHERE i.Id = {id}
                ";
                
                
       using (IDbConnection conn = Connection)
            {
                InstructorDetailViewModel model = new InstructorDetailViewModel();
                conn.Query<Instructor, Cohort, Instructor>(sql, (generatedInstructor, generatedCohort) =>
                {
                    generatedInstructor.Cohort = generatedCohort;
               
                model.instructor = generatedInstructor;
                    return generatedInstructor;
                });
                return View(model);
            }
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            var model = new CreateInstructorViewModel(_config);
            return View(model);
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInstructorViewModel viewmodel)
        {
           
                string sql = $@"INSERT INTO Instructor (
                                    FirstName, LastName, SlackHandle, CohortId, Specialty
                                ) VALUES (
                                    '{viewmodel.instructor.FirstName}', 
                                    '{viewmodel.instructor.LastName}',
                                    '{viewmodel.instructor.SlackHandle}', 
                                    '{viewmodel.instructor.CohortId}',
                                    '{viewmodel.instructor.Specialty}'
                                );";

            using (IDbConnection conn = Connection)
            {

              
                var newId = await conn.ExecuteAsync(sql);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //// POST: Instructors/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Instructors/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Instructors/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        private async Task<List<Cohort>> GetAllCohorts()
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $@"SELECT Id, Name
                                FROM Cohort";

                IEnumerable<Cohort> cohorts = await conn.QueryAsync<Cohort>(sql);
                return cohorts.ToList();
            }
        }

    }
}