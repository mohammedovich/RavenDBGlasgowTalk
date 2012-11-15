using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using System.Linq;
using Xunit;

namespace Demo.tests
{
    public class MapReduce
    {
        public IDocumentStore Store { get; set; }

        public MapReduce()
        {
            Store = new EmbeddableDocumentStore
                        {
                            Url = "http://localhost:8080",
                            DefaultDatabase = "DemoDB"
                        }.Initialize();
            IndexCreation.CreateIndexes(typeof(ReducedCompanyMapIndex).Assembly, Store);
        }

        [Fact]
        public void BeforeReduce()
        {
            using (var session = Store.OpenSession())
            {
                var results = session.Query<Employee, ReducedCompanyMapIndex>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).ToList();
                Assert.NotNull(results);
            }
        }

        public class ReducedCompanyMapIndex : AbstractIndexCreationTask<Employee, SpecialitiesCount>
        {
            public ReducedCompanyMapIndex()
            {
                Map = docs => from doc in docs
                              from tag in doc.Specialities
                              select new
                                         {
                                             Tag = tag,
                                             Count = 1
                                         };
               
                Reduce = results => from result in results
                                    group result by result.Tag
                                    into g
                                    select new
                                               {
                                                   Tag = g.Key,
                                                   Count = g.Sum(x => x.Count)
                                               };
            }
        }
    }

    public class SpecialitiesCount
    {
        public string Tag { get; set; }

        public int Count { get; set; }
    }
}