using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleManager.Models
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Person> Children { get; set; }

        public override string ToString()
        {
            if (Age < 0) 
                return "";

            return Name + " (" + Age + ")";
        }
    }
}
