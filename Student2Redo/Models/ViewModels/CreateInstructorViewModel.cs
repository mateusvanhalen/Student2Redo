﻿using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Student2Redo.Models.ViewModels
{
    public class CreateInstructorViewModel
    {
            private readonly IConfiguration _config;

            public List<SelectListItem> Cohorts { get; set; }
            public Instructor instructor { get; set; }

            public CreateInstructorViewModel() { }

            public CreateInstructorViewModel(IConfiguration config)
            {

                using (IDbConnection conn = new SqlConnection(config.GetConnectionString("DefaultConnection")))
                {
                    Cohorts = conn.Query<Cohort>(@"
                    SELECT Id, 
                           Name 
                            FROM Cohort;
                ")
                    .AsEnumerable()
                    .Select(li => new SelectListItem
                    {
                        Text = li.Name,
                        Value = li.Id.ToString()
                    }).ToList();
                    ;
                }

                Cohorts.Insert(0, new SelectListItem
                {
                    Text = "Choose cohort...",
                    Value = "0"
                });
            }
        }
    }