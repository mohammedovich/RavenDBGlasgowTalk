using System;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Xunit;

namespace Demo.tests
{
    public class Load
    {
        public IDocumentStore Store { get; set; }

        public Load()
        {
            Store = new DocumentStore
                        {
                            Url = "http://localhost:8080",
                            DefaultDatabase = "DemoDB"
                        }.Initialize();
        }

        [Fact]
        public void LoadEmployee()
        {
            string id;
            #region Create Employee
            using (var session = Store.OpenSession())
            {
                var employee = new Employee
                                   {
                                       HourlyRate = 20,
                                       JoinAt = DateTime.Now,
                                       Name = "Mohammed Ibrahim",
                                       Specialities = new[] {"RavenDB"}
                                   };

                session.Store(employee);
                id = employee.Id;
                session.SaveChanges();
            }
            #endregion

            using (var session = Store.OpenSession())
            {
                var emp = session.Load<Employee>(id);
                Assert.NotNull(emp);
            }
        }

        [Fact]
        public void DeleteEmployee()
        {
            string id;
            #region Create Employee
            using (var session = Store.OpenSession())
            {
                var employee = new Employee
                {
                    HourlyRate = 20,
                    JoinAt = DateTime.Now,
                    Name = "Mohammed Ibrahim -- To be deleted",
                    Specialities = new[] { "RavenDB" }
                };

                session.Store(employee);
                id = employee.Id;
                session.SaveChanges();
            }
            #endregion

            using (var session = Store.OpenSession())
            {
                var emp = session.Load<Employee>(id);
                Assert.NotNull(emp);

                session.Delete(emp);
                session.SaveChanges();

                var deletedEmployee = session.Load<Employee>(id);
                Assert.Null(deletedEmployee);
            }

        }
         
    }
}