using System.Collections.Generic;

namespace Demo.tests
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
        public string Country { get; set; }
    }
}