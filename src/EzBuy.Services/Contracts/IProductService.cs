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
        public void EditProduct(string product, EditProductInputModel input, User user);
    }
}
