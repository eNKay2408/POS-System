using POSSystem.Models;
using POSSystem.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace POSSystem.ViewModels
{
    public class BrandViewModel : BaseViewModel
    {
        private readonly IBrandRepository _brandRepository;

        private List<Brand> _brands;
        public List<Brand> Brands
        {
            get => _brands;
            set
            {
                _brands = value;
                OnPropertyChanged();
            }
        }

        public BrandViewModel()
        {
            _brandRepository = new BrandRepository(ConnectionString);

            Brands = new List<Brand>();

            LoadBrands();
        }

        private async void LoadBrands()
        {
            var brands = await _brandRepository.GetAllBrands();

            foreach (var brand in brands) 
            {
                brand.Index = brands.IndexOf(brand) + 1;
            }

            Brands = brands;
        }

        public async void AddBrand(string brandName)
        {
            var brand = new Brand
            {
                Name = brandName
            };

            await _brandRepository.AddBrand(brand);

            LoadBrands();
        }

        public async void UpdateBrand(int brandId, string newName)
        {
            var brand = await _brandRepository.GetBrandById(brandId);

            brand.Name = newName;

            await _brandRepository.UpdateBrand(brand);

            LoadBrands();
        }

        public async void DeleteBrand(int brandId)
        {
            await _brandRepository.DeleteBrand(brandId);

            LoadBrands();
        }
    }
}
