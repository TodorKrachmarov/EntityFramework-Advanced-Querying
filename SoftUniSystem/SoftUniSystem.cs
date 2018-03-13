using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftUniSystem
{
    class SoftUniSystem
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();


            EmployeesMaximumSalaries(context);
        }

        private static void EmployeesMaximumSalaries(SoftUniContext context)
        {
            var dep = context.Departments.Where(d => d.Employees.Max(e => e.Salary) > 70000 || d.Employees.Max(e => e.Salary) < 30000);

            foreach (var d in dep)
            {
                Console.WriteLine($"{d.Name} - {d.Employees.Max(e => e.Salary):F2}");
            }
        }
    }
}
