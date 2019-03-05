namespace MemeEconomy.Insights
{
    public interface IContextProvider<T>
    {
        T Get();
    }
}
