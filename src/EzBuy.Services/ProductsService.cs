using EzBuy.Data;
using EzBuy.InputModels.AddDelete;
using EzBuy.Models;
using EzBuy.Services.Contracts;
using EzBuy.ViewModels.Products;
using Microsoft.EntityFrameworkCore;

namespace EzBuy.Services
{
    public class ProductsService : IProductService
    {
        private readonly EzBuyContext context;
        private const int productsOnPage = 6;
        public ProductsService(EzBuyContext context)
        {
            this.context = context;
        }

        public List<ProductOnAllPageViewModel> GetAll(int currentPage)
        {
            var pageCount=this.GetMaxPages();
            if (currentPage <= 0 || currentPage > pageCount)
            {
                return null;
            }
            var products=context.
                Products.
                OrderByDescending(x=>x.DateListed).
                Select(x => new ProductOnAllPageViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SellerName=x.User.UserName,
                    Description = x.Description,
                    Price = x.Price,
                    PageCount = (int)pageCount,
                    CurrentPage = currentPage
                }).Skip((currentPage-1)*productsOnPage).Take(productsOnPage).ToList();
            return products;
                //Cover=x.CoverImage,}
        }

        public decimal GetMaxPages() => Math.Ceiling((decimal)context.Products.Count() / productsOnPage);

        public void AddProductComponents (AddProductInputModel input)
        {
            if (input.Name==null ||input.Price==0||input.Description==null||input.Category==null)
            {
                throw new ArgumentException("Dont be shy put some more");
            }
            //category shoud be with a dropmenu
            if (input.Manufacturer != null)
            {
                string manufacturerName = input.Manufacturer;
                if (!CheckIfEntityExists<Manufacturer>(manufacturerName))
                {
                    context.Manufacturers.Add(new Manufacturer
                    {
                        Id = GetBiggestId<Manufacturer>() + 1,
                        Name = manufacturerName
                    });
                    context.SaveChanges();
                }
                if (input.Tags != null)
                {
                    var tags = new List<string>();
                    tags = input.Tags.Split(",").ToList();
                    foreach (var tag in tags)
                    {
                        if (!CheckIfEntityExists<Tag>(tag))
                        {
                            context.Tags.Add(new Tag
                            {
                                Id = GetBiggestId<Tag>() + 1,
                                Name = tag
                            });
                        }
                        context.SaveChanges();
                    }
                }
            }
        }
        public int AddProduct(AddProductInputModel input, User user, string basePath)
        {
            throw new NotImplementedException();
        }
        public bool CheckIfEntityExists<T>(string name) where T : EntityName
        {
            return this.context.Set<T>().Any(x => x.Name.ToLower() == name.ToLower());
        }
        public int GetBiggestId<T>() where T : MainEntity
        {
            if (this.context.Set<T>().Any())
            {
                return this.context.Set<T>().AsNoTracking().OrderByDescending(x => x.Id).FirstOrDefault().Id;
            }

            return 0;
        }
    }
}