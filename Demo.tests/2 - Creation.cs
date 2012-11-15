using System;
using System.Collections.Generic;
using Raven.Client;
using Raven.Client.Embedded;
using Xunit;

namespace Demo.tests
{
    public class Creation
    {
        public IDocumentStore Store { get; set; }

        public Creation()
        {
            Store = new EmbeddableDocumentStore
                        {
                            Url = "http://localhost:8080",
                            DefaultDatabase = "DemoDB"
                        }.Initialize();
        }

        [Fact]
        public void CreateNewEmployee()
        {
            using (var session = Store.OpenSession())
            {
                var employee = new Employee
                                   {
                                       HourlyRate = 50,
                                       JoinAt = DateTime.Today.Date,
                                       Name = "Mohammed Ibrahim",
                                       Specialities = new[] {"C#", "RavenDB", "Knockoutjs"}
                                   };

                Assert.Null(employee.Id);
                session.Store(employee);
                Assert.NotNull(employee.Id);

                // nothing sent to the server yet
                session.SaveChanges();
            }
        }

        [Fact]
        public void CreateNewCompany()
        {
            using (var session = Store.OpenSession())
            {
                var employee = new Employee
                {
                    HourlyRate = 50,
                    JoinAt = DateTime.Today.Date,
                    Name = "John Smith",
                    Specialities = new[] { "C#" }
                };

                var employee2 = new Employee
                {
                    HourlyRate = 50,
                    JoinAt = DateTime.Today.Date,
                    Name = "Smith Lee",
                    Specialities = new[] { "RavenDB", "SQL" }
                };

                var company = new Company
                                  {
                                      Country = "United Kingdom",
                                      Name = "ItsMeRaven",
                                      Employees = new List<Employee>
                                                      {
                                                          employee,
                                                          employee2
                                                      }
                                  };

                // serializing the whole document into one big JSON blob
                session.Store(company);
                Assert.NotNull(company.Id);

                // nothing sent to the server yet
                session.SaveChanges();
            }
        }
    }
}