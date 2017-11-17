namespace DependencyInjection.UnitTests.ContainerTests.Fakes
{
    public class DataSource2 : IDataSource
    {
        private readonly IDataSource _decorated;

        public DataSource2(IDataSource decorated)
        {
            _decorated = decorated;
        }

        public string GetData()
        {
            return _decorated.GetData();
        }
    }
}