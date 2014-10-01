
using System.Threading.Tasks;
namespace SeriesManager.UILogic.Repositories
{
    public interface IBannerRepository
    {
        Task<byte[]> Get(string remotePath);
    }
}
