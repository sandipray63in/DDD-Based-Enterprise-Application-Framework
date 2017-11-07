
namespace Repository.BatchProcessing
{
    public class BatchConfiguration
    {
        public IBatchSeedSelector BatchSeedSelector { get; set; }

        public IBatchCommandProcesor BatchCommandProcesor { get; set; }
    }
}
