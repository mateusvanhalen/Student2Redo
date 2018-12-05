using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student2Redo.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }
        public string Speciailty { get; set; }
        public string CohortId { get; set; }
    }
}
