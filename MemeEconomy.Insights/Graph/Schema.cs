using GraphQL;

namespace MemeEconomy.Insights.Graph
{
    public class Schema : GraphQL.Types.Schema
    {
        public Schema(IDependencyResolver resolver)
        {
            Query = resolver.Resolve<Query>();
            Subscription = resolver.Resolve<Subscription>();
        }
    }
}
