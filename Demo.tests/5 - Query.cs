using System.Linq;
using Raven.Client;
using Raven.Client.Linq;
using Raven.Client.Embedded;
using Xunit;

namespace Demo.tests
{
    public class Query
    {
        public IDocumentStore Store { get; set; }

        public Query()
        {
            Store = new EmbeddableDocumentStore
                        {
                            Url = "http://localhost:8080",
                            DefaultDatabase = "DemoDB"
                        }.Initialize();
        }

        [Fact]
        public void QueryRavenWithoutAnIndex()
        {
            using (var session = Store.OpenSession())
            {
                var employees = (from employee in session.Query<Employee>()
                                where employee.Name == "mohammed ibrahim"
                                select employee).ToArray();

                Assert.True(employees.Any());
            }
        }

        [Fact]
        public void QueryRavenUseAny()
        {
            using (var session = Store.OpenSession())
            {
                var companies = (from company in session.Query<Company>()
                                where company.Employees.Any(x => x.Name == "john smith")
                                select company).ToList();

                Assert.True(companies.Any());
                Assert.True(companies.Count == 1);
            }
        }

        [Fact]
        public void QueryRavenUseIn()
        {
            using (var session = Store.OpenSession())
            {
                var companies = (from company in session.Query<Company>()
                                 where company.Country.In(new[] {"Kuwait", "United Kingdom"})
                                 select company).ToList();

                Assert.Equal(2, companies.Count);
            }
        }
    }
}