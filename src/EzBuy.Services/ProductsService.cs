using EzBuy.Data;
using EzBuy.InputModels.AddEdit;
using EzBuy.Models;
using EzBuy.Services.Contracts;
using EzBuy.ViewModels.Products;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public Manufacturer FindManufacturer(string manufacturerName)
        {
            var manufacturer = context.Manufacturers.FirstOrDefault(x => x.Name == manufacturerName);
            return manufacturer;
        }
        public ICollection<Tag> FindTags(string tagString)
        {
            var tags = tagString.Split(",").ToList();
            var tagsCollection = context.Tags.Where(x => tags.Contains(x.Name)).ToList();
            return tagsCollection;
        }

        public int AddProduct(AddProductInputModel input, User user)
        {
            AddProductComponents(input);
            var productId = this.GetBiggestId<Product>() + 1;
            var newProduct = new Product
            {
                
                Id = productId,
                Name = input.Name,
                Price=input.Price,
                Manufacturer=FindManufacturer(input.Manufacturer),
                //Category = input.Category, tdl drop down menu or check or smthng idk
                User = user,
            };
            context.Products.Add(newProduct);
            context.SaveChanges();
            newProduct=context.Products.FirstOrDefault(x=>x.Name==input.Name);
            AddTagsToProduct(FindTags(input.Tags), newProduct);
            return newProduct.Id;
        }
        public void AddTagsToProduct(ICollection<Tag> tags, Product product)
        {
            foreach (var tag in tags)
            {
                context.ProductTags.Add(new ProductTags(product.Id, tag.Id));
            }
            context.SaveChanges();
        }
        public void DeleteProduct(string productName)
        {
            var product = context.Products.FirstOrDefault(x => x.Name == productName);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("No product with this name exists", (productName));
            }
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

        public void EditProduct(string productName, EditProductInputModel input, User user)
        {
            if (CheckIfEntityExists<Product>(productName))
            {
                var product = context.Products.FirstOrDefault(x => x.Name == productName);
                if (input.NewName != null)
                {
                    product.Name = input.NewName;
                }
                if (input.NewPrice != 0)
                {
                    product.Price = input.NewPrice;
                }
                if(input.NewDescription != null)
                {
                    product.Description = input.NewDescription;
                }
                context.Update(product);
                context.SaveChanges();

                if (input.NewTags != null)
                {
                    AddNonexistentTags(input.NewTags);
                    AddTagsToProduct(FindTags(input.NewTags),product);
                }
                if (input.RemoveTags != null)
                {
                    RemoveTags(FindTags(input.RemoveTags), product);
                }
            }
            else {
            throw new ArgumentException("No product with this name exists", (productName));
            }
            
        }
        public void RemoveTags(ICollection<Tag>tags,Product product)
        {
            foreach (var tag in tags)
            {
                ProductTags connection = (ProductTags)context.ProductTags.Where(x => x.ProductId == product.Id && x.TagId == tag.Id);
                context.ProductTags.Remove(connection);
            }
            context.SaveChanges();
        }
        public ICollection<ProductTags> FindProductTags(Product product)
        {
            var connections = context.ProductTags.Where(x => x.ProductId==product.Id).ToList();
            return connections;
        }
        public void AddNonexistentTags(string tagString)
        {
                var tags = new List<string>();
                tags = tagString.Split(",").ToList();
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
