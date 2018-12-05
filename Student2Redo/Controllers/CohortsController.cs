using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Student2Redo.Models;
using Student2Redo.Models.ViewModels;
//using StudentExercisesAPI.Data;

namespace Student2Redo.Controllers
{

    public class CohortsController : Controller
    {
        private readonly IConfiguration _config;

        public CohortsController(IConfiguration config)
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

        // GET: Cohort
        public async Task<ActionResult> Index()
        {
            using (IDbConnection conn = Connection)
            {

                IEnumerable<Cohort> cohorts = await conn.QueryAsync<Cohort>(@"
                    SELECT 
                        c.Id,
                        c.Name
         
                    FROM Cohort c
                ");
                return View(cohorts);
            }
        }

        // GET: Cohorts/Details/5
        public async Task<ActionResult> Details(int id)
        {
            string sql = $@"
            SELECT
                c.Id,
                c.Name,
            FROM Cohort c
            WHERE s.Id = {id}
            ";

            using (IDbConnection conn = Connection)
            {
                Cohort cohort = await conn.QueryFirstAsync<Cohort>(sql);
                return View(cohort);
            }
        }

        //// GET: Students/Create
        //public ActionResult Create()
        //{
        //    var model = new StudentCreateViewModel(_config);
        //    return View(model);
        //}

        //// POST: Students/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(StudentCreateViewModel model)
        //{
        //    string sql = $@"INSERT INTO Student 
        //    (FirstName, LastName, SlackHandle, CohortId)
        //    VALUES
        //    (
        //        '{model.student.FirstName}'
        //        ,'{model.student.LastName}'
        //        ,'{model.student.SlackHandle}'
        //        ,{model.student.CohortId}
        //    );";

        //    using (IDbConnection conn = Connection)
        //    {
        //        var newId = await conn.ExecuteAsync(sql);
        //        return RedirectToAction(nameof(Index));
        //    }

        //}

        //// GET: Students/Edit/5
        //[HttpGet]
        //public async Task<ActionResult> Edit(int id)
        //{
        //    string sql = $@"
        //    SELECT
        //        s.Id,
        //        s.FirstName,
        //        s.LastName,
        //        s.SlackHandle,
        //        s.CohortId
        //    FROM Student s
        //    WHERE s.Id = {id}
        //    ";

        //    using (IDbConnection conn = Connection)
        //    {
        //        Student student = await conn.QueryFirstAsync<Student>(sql);
        //        StudentEditViewModel model = new StudentEditViewModel(_config);
        //        model.student = student;
        //        return View(model);
        //    }
        //}

        //// POST: Students/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(int id, StudentEditViewModel model)
        //{
        //    try
        //    {
        //        Student student = model.student;

        //        // TODO: Add update logic here
        //        string sql = $@"
        //            UPDATE Student
        //            SET FirstName = '{student.FirstName}',
        //                LastName = '{student.LastName}',
        //                SlackHandle = '{student.SlackHandle}',
        //                CohortId = {student.CohortId}
        //            WHERE Id = {id}";

        //        using (IDbConnection conn = Connection)
        //        {
        //            int rowsAffected = await conn.ExecuteAsync(sql);
        //            if (rowsAffected > 0)
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
        //            return BadRequest();

        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Students/Delete/5
        //public async Task<ActionResult> DeleteConfirm(int id)
        //{
        //    string sql = $@"
        //    SELECT
        //        s.Id,
        //        s.FirstName,
        //        s.LastName,
        //        s.SlackHandle,
        //        s.CohortId
        //    FROM Student s
        //    WHERE s.Id = {id}
        //    ";

        //    using (IDbConnection conn = Connection)
        //    {
        //        Student student = await conn.QueryFirstAsync<Student>(sql);
        //        return View(student);
        //    }
        //}

        //// POST: Students/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    string sql = $@"DELETE FROM Student WHERE Id = {id}";

        //    using (IDbConnection conn = Connection)
        //    {
        //        int rowsAffected = await conn.ExecuteAsync(sql);
        //        if (rowsAffected > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //        throw new Exception("No rows affected");
        //    }
        }
    }
