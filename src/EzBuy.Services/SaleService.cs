using EzBuy.Data;
using EzBuy.Models;
using EzBuy.Services.Contracts;
using EzBuy.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Services
{
    public class SaleService:ISaleService
    {
        private readonly EzBuyContext context;
        private const int productsOnPage = 6;
        public SaleService(EzBuyContext context)
        {
            this.context = context;
        }
        public void AddProductToSale(int productId, int saleId, int precentage)
        {
            if (precentage <= 0)
            {
                throw new ArgumentException("You must offer some sort of discount to participate in the sale.");
            }
            var product=this.context.Products.FirstOrDefault(x=>x.Id==productId);
            var sale = this.context.Sales.FirstOrDefault(x => x.Id == saleId);
            product.SaleOffPrecentage = precentage;
            if ((sale.Categorial && product.Category.Name == sale.CategoryName) || sale.Seasonal)
            {
                this.context.Update(product);
                this.context.SaveChanges();
                sale.Products.Add(product);
                this.context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Your product doesnt meet this sales requirments");
            }
        }
        public List<ProductOnSalePageViewModel> GetAll(int currentPage,Sale sale)
        {
            var pageCount = this.GetMaxPages(sale);
            if (currentPage <= 0 || currentPage > pageCount)
            {
                return null;
            }
            var products = sale.
                Products.
                OrderByDescending(x => x.SaleOffPrecentage).
                Select(x => new ProductOnSalePageViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SellerName = x.User.UserName,
                    Description = x.Description,
                    Price = x.Price,
                    PageCount = (int)pageCount,
                    CurrentPage = currentPage,
                    Cover = x.Images.Where(x => x.IsCover == true).FirstOrDefault()!.Url,
                    DiscountPrecentage=x.SaleOffPrecentage
                }).Skip((currentPage - 1) * productsOnPage).Take(productsOnPage).ToList();
            return products;
        }
        public decimal GetMaxPages(Sale sale) => Math.Ceiling((decimal)sale.Products.Count() / productsOnPage);
    }
}
