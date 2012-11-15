using System.Linq;
using Demo.tests.Animalo;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Xunit;

namespace Demo.tests
{
    public class MultiMap
    {
        public IDocumentStore Store { get; set; }

        public MultiMap()
        {
            Store = new EmbeddableDocumentStore
                        {
                            Url = "http://localhost:8080",
                            DefaultDatabase = "DemoDB"
                        }.Initialize();
            IndexCreation.CreateIndexes(typeof(AnimalIndex).Assembly, Store);
        }

        [Fact]
        public void MuliMapDemo()
        {
            using (var session = Store.OpenSession())
            {
                var cat = new Cat
                              {
                                  Name = "Tom"
                              };
                var cat2 = new Cat
                              {
                                  Name = "Lolo"
                              };

                var dog = new Dog
                              {
                                  Name = "Jerry"
                              };

                var dog2 = new Dog
                                 {
                                     Name = "Soso"
                                 };

                session.Store(cat);
                session.Store(cat2);
                session.Store(dog);
                session.Store(dog2);
                session.SaveChanges();


                var animalsByName =
                    session.Query<Animal, AnimalIndex>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).Where(x => x.Name == "Tom").ToArray();

                Assert.True(animalsByName.Any());

            }

        }

        public class AnimalIndex : AbstractMultiMapIndexCreationTask
        {
            public AnimalIndex()
            {
                AddMap<Cat>(cats => from c in cats
                                        select new { c.Name });
                
                AddMap<Dog>(dogs => from d in dogs
                                        select new { d.Name });
            }
        }
    }
}