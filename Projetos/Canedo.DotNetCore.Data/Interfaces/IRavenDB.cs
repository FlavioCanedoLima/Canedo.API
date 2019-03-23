using Raven.Client.Documents;

namespace Canedo.DotNetCore.Data.Interfaces
{
    public interface IRavenDB
    {   
        IDocumentStore DocumentStore();
    }
}
