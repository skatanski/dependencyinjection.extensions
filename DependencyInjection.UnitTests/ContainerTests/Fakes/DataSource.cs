namespace DependencyInjection.UnitTests.ContainerTests.Fakes
{
    public class DataSource : IDataSource
    {
        public string GetData()
        {
            return "DecoratedData";
        }
    }
}