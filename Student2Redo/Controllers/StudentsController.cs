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
using Student2Redo.Models.ViewModels;

namespace Student2Redo.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IConfiguration _config;

        public StudentsController(IConfiguration config)
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

                IEnumerable<Student> students = await conn.QueryAsync<Student>(@"
                    SELECT 
                        s.Id,
                        s.FirstName,
                        s.LastName,
                        s.SlackHandle,
                        s.CohortId
         
                    FROM Student s
                ");
                return View(students);
            }
        }


        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            string sql = $@"
                   SELECT
                        s.Id,
                        s.FirstName,
                        s.LastName,
                        s.SlackHandle,
                        s.CohortId,
                        c.Id,
                        c.Name
            FROM Student s
                JOIN Cohort c ON c.Id = s.CohortId
            WHERE s.Id = {id}
                ";

            using (IDbConnection conn = Connection)
            {
                StudentDetailViewModel model = new StudentDetailViewModel();
                conn.Query<Student, Cohort, Student>(sql, (generatedStudent, generatedCohort) =>
                {
                    generatedStudent.Cohort = generatedCohort;

                    model.student = generatedStudent;
                    return generatedStudent;
                });
                return View(model);
            }
        }

        // GET: Students/Create
        public ActionResult Create()
        {
                var model = new CreateStudentViewModel(_config);
                return View(model);
            }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentViewModel viewmodel)
        {

            string sql = $@"INSERT INTO Student (
                                    FirstName, LastName, SlackHandle, CohortId 
                                ) VALUES (
                                    '{viewmodel.student.FirstName}', 
                                    '{viewmodel.student.LastName}',
                                    '{viewmodel.student.SlackHandle}', 
                                    '{viewmodel.student.CohortId}'
                                );";

            using (IDbConnection conn = Connection)
            {


                var newId = await conn.ExecuteAsync(sql);
                return RedirectToAction(nameof(Index));
            }
        }
        // GET: Students/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            string sql = $@"
            SELECT
                s.Id,
                s.FirstName,
                s.LastName,
                s.SlackHandle,
                s.CohortId
            FROM Student s
            WHERE s.Id = {id}
            ";

            using (IDbConnection conn = Connection)
            {
                Student student = await conn.QueryFirstAsync<Student>(sql);
                StudentEditViewModel model = new StudentEditViewModel(_config);
                model.student = student;
                return View(model);
            }
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, StudentEditViewModel model)
        {
            try
            {
                Student student = model.student;

                // TODO: Add update logic here
                string sql = $@"
                    UPDATE Student
                    SET FirstName = '{student.FirstName}',
                        LastName = '{student.LastName}',
                        SlackHandle = '{student.SlackHandle}',
                        CohortId = {student.CohortId}
                    WHERE Id = {id}";

                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    if (rowsAffected > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return BadRequest();

                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}