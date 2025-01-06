using POSSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.Repositories
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllBrands();
        Task<Brand> GetBrandById(int id);
        Task AddBrand(Brand brand);
        Task UpdateBrand(Brand brand);
        Task DeleteBrand(int id);

        Task<bool> HasReferencingProducts(int brandId);
    }
}
