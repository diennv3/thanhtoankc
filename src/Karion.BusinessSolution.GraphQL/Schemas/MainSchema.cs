using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using Karion.BusinessSolution.Queries.Container;

namespace Karion.BusinessSolution.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}