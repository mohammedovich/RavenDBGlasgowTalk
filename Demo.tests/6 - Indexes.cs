using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Xunit;

namespace Demo.tests
{
    public class Indexes
    {
        public IDocumentStore Store { get; set; }

        public Indexes()
        {
            Store = new EmbeddableDocumentStore
                        {
                            Url = "http://localhost:8080",
                            DefaultDatabase = "DemoDB"
                        }.Initialize();
            IndexCreation.CreateIndexes(typeof(CompanyIndex).Assembly, Store);
        }

        [Fact]
        public void QueryUsingWithoutAnIndex()
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
        public void QueryUsingWithAnIndex()
        {
            using (var session = Store.OpenSession())
            {
                var companies = session.Query<Company, CompanyIndex>()
                    .Where(x => x.Name == "ItsMeRaven")
                    .Select(x => x)
                    .ToArray();
                
                Assert.Equal(1, companies.Length);
            }
        }

        public class CompanyIndex : AbstractIndexCreationTask<Company>
        {
            public CompanyIndex()
            {
                Map = docs => from doc in docs
                              select new
                                         {
                                             doc.Name
                                         };
            }
 
        }
    }
}