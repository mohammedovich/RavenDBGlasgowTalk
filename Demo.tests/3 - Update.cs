using System;
using System.Collections.Generic;
using Raven.Client;
using Raven.Client.Embedded;
using Xunit;

namespace Demo.tests
{
    public class Update
    {
        public IDocumentStore Store { get; set; }

        public Update()
        {
            Store = new EmbeddableDocumentStore
                        {
                            Url = "http://localhost:8080",
                            DefaultDatabase = "DemoDB"
                        }.Initialize();
        }


        [Fact]
        public void UpdateEmployee()
        {
            string id;
            using (var session = Store.OpenSession())
            {
                var employee = new Employee
                                   {
                                       HourlyRate = 20,
                                       JoinAt = DateTime.Now,
                                       Name = "Lee Woo",
                                       Specialities = new[] {"C#"}
                                   };

                session.Store(employee);
                session.SaveChanges();
                id = employee.Id;
            }

            using (var session = Store.OpenSession())
            {
                var employee = session.Load<Employee>(id);
                employee.Name = "Kung Fu Panda";
                session.SaveChanges();
            }

            using (var session = Store.OpenSession())
            {
                var employee = session.Load<Employee>(id);
                Assert.Equal("Kung Fu Panda", employee.Name);
            }
        }

        [Fact]
        public void UpdateCompany()
        {
            string companyId;
            string employeeId;

            using (var session = Store.OpenSession())
            {
                var employee = new Employee
                {
                    HourlyRate = 30,
                    JoinAt = DateTime.Now,
                    Name = "Raven DBBBB",
                    Specialities = new[] { "NoSQL" }
                }; 

                session.Store(employee);
                employeeId = employee.Id;

                var company = new Company
                                  {
                                      Country = "Kuwait",
                                      Employees = new List<Employee>
                                                      {
                                                          employee
                                                      },
                                      Name = "CompO1"
                                  };

                session.Store(company);
                companyId = company.Id;

                session.SaveChanges();
            }

            using (var session = Store.OpenSession())
            {
                var employee = session.Load<Employee>(employeeId);
                var company = session.Load<Company>(companyId);

                employee.Name = "RavenDB";
                company.Name = "IT Info Company";

                //this will save all changes automatically
                // in a single request
                session.SaveChanges();
            }

            using (var session = Store.OpenSession())
            {
                var emp = session.Load<Employee>(employeeId);
                var comp = session.Load<Company>(companyId);
                Assert.Equal("RavenDB", emp.Name);
                Assert.Equal("IT Info Company", comp.Name);
            }
        }
    }
}
