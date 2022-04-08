using EzBuy.Models;
using EzBuy.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Services.Contracts
{
    public interface ISaleService
    {
        public void AddProductToSale(int productId, int saleId, int precentage);
        public List<ProductOnSalePageViewModel> GetAll(int currentPage, Sale sale);

    }
}
