using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Xunit;

namespace Demo.tests
{
    public class Accessing
    {
        [Fact]
        public void HeyyyICanConnectToRavenWeehaa()
        {
            IDocumentStore store = new DocumentStore
                                       {
                                           Url = "http://localhost:8080"
                                       }.Initialize();
            // per unit of work
            using (var session = store.OpenSession())
            {
                //Unit of work
            }
        }

        [Fact]
        public void ICanRunAnEmbeddedDatabase()
        {
            IDocumentStore store = new EmbeddableDocumentStore
                                       {
                                           DataDirectory = "data"
                                       }.Initialize();

            // per unit of work
            using (var session = store.OpenSession())
            {
                // unit of work
            }
        }

        [Fact]
        public void RunRavenDBInMemory()
        {
            IDocumentStore store = new EmbeddableDocumentStore
                                       {
                                           RunInMemory = true
                                       }.Initialize();

            using (var session = store.OpenSession())
            {
                // unit of work
            }
        }
    }
}
