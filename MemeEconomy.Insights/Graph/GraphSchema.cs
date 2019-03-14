using GraphQL;
using GraphQL.Types;

namespace MemeEconomy.Insights.Graph
{
    public class GraphSchema : Schema
    {
        public GraphSchema(IDependencyResolver resolver)
        {
            Query = resolver.Resolve<GraphQuery>();
            //Subscription = resolver.Resolve<Subscription>();
        }
    }
}
