using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Student2Redo.Models
{
    public class Student {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "How to find me on Slack")]
        public string SlackHandle { get; set; }

        [Display(Name = "Durpity Durp Durp")]
        public string CohortId { get; set; }
        public Cohort Cohort { get; set; }
    }
}
