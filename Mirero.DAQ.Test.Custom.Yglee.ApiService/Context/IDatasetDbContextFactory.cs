namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Context
{
    public interface IDatasetDbContextFactory
    {
        DatasetDbContext CreateContext(string datasetUri);
    }

    public class DatasetDbContextFactory : IDatasetDbContextFactory
    {
        public DatasetDbContext CreateContext(string datasetUri)
        {
            return DatasetDbContext.GetInstance(datasetUri);
        }
    }
}