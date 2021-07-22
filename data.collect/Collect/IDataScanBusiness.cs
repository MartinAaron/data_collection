using System.Threading;
using System.Threading.Tasks;

namespace data.collect.Collect
{
    public interface IDataScanBusiness
    {
        Task ScanYJ(CancellationToken stoppingToken);
        Task ScanZL(CancellationToken stoppingToken);
    }
}