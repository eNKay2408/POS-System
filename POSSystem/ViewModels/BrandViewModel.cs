using POSSystem.Models;
using POSSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            _brandRepository = new BrandRepository();

            Brands = new List<Brand>();

            _ = LoadBrands();
        }

        // Constructor for unit testing
        public BrandViewModel(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task LoadBrands()
        {
            var brands = await _brandRepository.GetAllBrands();

            foreach (var brand in brands)
            {
                brand.Index = brands.IndexOf(brand) + 1;
            }

            Brands = brands;
        }

        public async Task AddBrand(string brandName)
        {
            if (string.IsNullOrWhiteSpace(brandName))
            {
                throw new ArgumentException("Brand name cannot be empty.");
            }

            try
            {
                var brand = new Brand
                {
                    Name = brandName
                };

                await _brandRepository.AddBrand(brand);

                await LoadBrands();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateBrand(int brandId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Brand name cannot be empty.");
            }

            var brand = await _brandRepository.GetBrandById(brandId);

            brand.Name = newName;

            await _brandRepository.UpdateBrand(brand);

            await LoadBrands();
        }

        public async Task DeleteBrand(int brandId)
        {
            await _brandRepository.DeleteBrand(brandId);

            await LoadBrands();
        }
    }
}
