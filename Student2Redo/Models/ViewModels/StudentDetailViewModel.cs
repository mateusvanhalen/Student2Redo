using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student2Redo.Models.ViewModels
{
    public class StudentDetailViewModel
    {

        public Cohort cohort { get; set; }
        public Student student { get; set; }

        public StudentDetailViewModel() { }


    }
}