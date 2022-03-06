using EzBuy.InputModels.AddEdit;
using EzBuy.Models;
using EzBuy.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Services.Contracts
{
    public interface IProductService
    {
        public List<ProductOnAllPageViewModel> GetAll(int currentPage);
        public Task<int> AddProductAsync(AddProductInputModel input, User user, string imgPath);
        public Task EditProductAsync(EditProductInputModel input, string imgPath);

        public ICollection<ProductOnAllPageViewModel> GetProductsByUserId(string username);
        public Task<FilledProductViewModel> GetFilledProductById(int productId);
        public Task DeleteProductImageByPathAsync(string path);

        public List<ProductOnAllPageViewModel> GetTopProducts();
    }
}
