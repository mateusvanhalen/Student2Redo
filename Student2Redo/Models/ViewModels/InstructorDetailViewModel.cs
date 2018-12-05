using Dapper;
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
    public class InstructorDetailViewModel
        {

            public Cohort cohort { get; set; }
            public Instructor instructor { get; set; }

        public InstructorDetailViewModel() { }


    }
}
