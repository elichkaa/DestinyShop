using CloudinaryDotNet;
using EzBuy.Data;
using EzBuy.InputModels.AddEdit;
using EzBuy.InputModels.Search;
using EzBuy.Models;
using EzBuy.Services.Contracts;
using EzBuy.ViewModels.Products;
using EzBuy.ViewModels.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EzBuy.Services
{
    public class ProductsService : IProductService
    {
        private readonly EzBuyContext context;
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;
        private const int productsOnPage = 9;
        public ProductsService(EzBuyContext context, ICloudinaryService cloudinaryService, Cloudinary cloudinary)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
            this.cloudinary = cloudinary;
        }

        public List<ProductOnAllPageViewModel> GetAll(int currentPage, string[] categories)
        {
            var pageCount = this.GetMaxPages();
            if (currentPage <= 0 || currentPage > pageCount)
            {
                return null;
            }
            var products = new List<ProductOnAllPageViewModel>();
            if (categories == null || categories.Length == 0)
            {
                products = context.
                Products.
                OrderByDescending(x => x.DateListed).
                Select(x => new ProductOnAllPageViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SellerName = x.User.UserName,
                    Description = x.Description,
                    Price = x.Price,
                    Category = x.Category.Name,
                    PageCount = (int)pageCount,
                    CurrentPage = currentPage,
                    Cover = x.Images.Where(x => x.IsCover == true).FirstOrDefault()!.Url
                }).Skip((currentPage - 1) * productsOnPage).Take(productsOnPage).ToList();

                return products;
            }
            else
            {
                var filtered = new List<ProductOnAllPageViewModel>();
                foreach (var category in categories)
                {
                    filtered.Add(products.Where(x => x.Category == category).FirstOrDefault());
                }
                return filtered;
            }
            
        }

        public decimal GetMaxPages() => Math.Ceiling((decimal)context.Products.Count() / productsOnPage);

        public void AddProductComponents(AddProductInputModel input)
        {
            if (input.Name == null || input.Price == 0 || input.Description == null || input.Category == null)
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
                        //Id = GetBiggestId<Manufacturer>() + 1,
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
                                //Id = GetBiggestId<Tag>() + 1,
                                Name = tag
                            });
                        }
                    }
                    context.SaveChanges();
                }
            }
        }
        public Manufacturer FindManufacturer(string manufacturerName)
        {
            var manufacturer = context.Manufacturers.FirstOrDefault(x => x.Name == manufacturerName);
            return manufacturer;
        }

        public Category GetCategory(int categoryId)
        {
            return this.context.Categories.FirstOrDefault(x => x.Id == categoryId);
        }

        public ICollection<Tag> FindTags(string tagString)
        {
            var tags = tagString.Split(",").ToList();
            var tagsCollection = context.Tags.Where(x => tags.Contains(x.Name)).ToList();
            return tagsCollection;
        }

        public async Task<int> AddProductAsync(AddProductInputModel input, User user, string imgPath)
        {
            AddProductComponents(input);

            input.Images.Add(input.Cover);
            var uploadedImages = await UploadPicturesToCloudinary(input.Images, imgPath);
            uploadedImages.Last().IsCover = true;

            var newProduct = new Product
            {
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                Manufacturer = FindManufacturer(input.Manufacturer),
                Category = GetCategory(input.Category),
                DateListed=DateTime.Now,
                User = user,
                Images = uploadedImages.ToList()
            };
            context.Products.Add(newProduct);
            context.SaveChanges();
            newProduct = context.Products.FirstOrDefault(x => x.Name == input.Name);
            var tagsToAdd = FindTags(input.Tags);
            AddTagsToProduct(tagsToAdd, newProduct);
            return newProduct.Id;
        }

        public async Task<List<Image>> UploadPicturesToCloudinary(ICollection<IFormFile> images, string imgPath)
        {
            var uploadedImages = images != null ? await this.cloudinaryService.UploadAsync(images, imgPath) : null;
            return uploadedImages.ToList();
        }

        public void AddTagsToProduct(ICollection<Tag> tags, Product product)
        {
            foreach (var tag in tags)
            {
                context.ProductTags.Add(new ProductTags(product.Id, tag.Id));
            }
            context.SaveChanges();
        }
        public async Task DeleteProduct(int productId)
        {
            var product = context.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == productId);
            var productImages = product.Images.ToList();
            foreach (var image in productImages)
            {
                await this.DeleteProductImageByPathAsync(image.Url);
            }
            context.Products.Remove(product);
            context.SaveChanges();
        }
        public bool CheckIfEntityExists<T>(string name) where T : EntityName
        {
            return this.context.Set<T>().Any(x => x.Name.ToLower() == name.ToLower());
        }

        //public int GetBiggestId<T>() where T : MainEntity
        //{
        //    if (this.context.Set<T>().Any())
        //    {
        //        return this.context.Set<T>().AsNoTracking().OrderByDescending(x => x.Id).FirstOrDefault().Id;
        //    }
        //    return 0;
        //}

        public async Task EditProductAsync(EditProductInputModel input, string imgPath)
        {
            var product = context.Products.Include(x => x.Category).Include(x => x.Images).Include(x => x.Tags).FirstOrDefault(x => x.Id == input.Id);
            product.Name = input.Name != product.Name ? input.Name : product.Name;
            product.Description = input.Description != product.Description ? input.Description : product.Description;
            product.Price = input.Price != product.Price ? input.Price : product.Price;
            product.Category = product.Category.Id != input.Category ? GetCategory(input.Category) : product.Category;

            if (input.Cover != null)
            {
                if (product.Images.Where(x => x.IsCover).FirstOrDefault() != null)
                {
                    await this.DeleteProductImageByPathAsync(product.Images.Where(x => x.IsCover).FirstOrDefault().Url);
                }
                var uploadedCover = (await UploadPicturesToCloudinary(new List<IFormFile>() { input.Cover }, imgPath)).FirstOrDefault();
                uploadedCover.IsCover = true;
                product.Images.Add(uploadedCover);
            }

            if (input.Images != null)
            {
                var uploadedImages = await UploadPicturesToCloudinary(input.Images, imgPath);
                foreach (var image in uploadedImages)
                {
                    product.Images.Add(image);
                }
            }

            context.Update(product);
            context.SaveChanges();

            if (input.NewTags != null)
            {
                AddNonexistentTags(input.NewTags);
                AddTagsToProduct(FindTags(input.NewTags), product);
            }
            if (input.RemoveTags != null)
            {
                RemoveTags(FindTags(input.RemoveTags), product);
            }
        }
        public void RemoveTags(ICollection<Tag> tags, Product product)
        {
            //slow but works
            foreach (var tag in tags)
            {
                foreach (var connection in product.Tags)
                {
                    if (connection.TagId == tag.Id)
                    {
                        this.context.Remove(connection);
                    }
                }
            }
            context.SaveChanges();
        }
        public ICollection<ProductTags> FindProductTags(Product product)
        {
            var connections = context.ProductTags.Where(x => x.ProductId == product.Id).ToList();
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
                        Name = tag
                    });
                }
                context.SaveChanges();
            }
        }

        public ICollection<ProductOnAllPageViewModel> GetProductsByUserId(string username)
        {
            var products = context.Products
                .Where(x => x.User.UserName == username)
                .Select(x => new ProductOnAllPageViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SellerName = x.User.UserName,
                    Description = x.Description,
                    Price = x.Price,
                    Cover = x.Images.Where(x => x.IsCover == true).FirstOrDefault()!.Url,
                    Category = x.Category.Name
                }).ToList();
            return products;
        }

        public async Task<FilledProductViewModel> GetFilledProductById(int productId)
        {
            var result = await this.context
                .Products
                .Where(x => x.Id == productId)
                .Select(x => new FilledProductViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Manufacturer = x.Manufacturer.Name,
                    Category = x.Category.Id,
                    Cover = x.Images.Where(x => x.IsCover == true).FirstOrDefault()!,
                    Images = x.Images,
                    Tags = string.Join(", ", x.Tags.Where(t => t.ProductId == x.Id).Select(t => t.Tag.Name))
                }).FirstOrDefaultAsync();

            var tags = this.context.ProductTags.Where(x => x.ProductId == productId).ToList();

            return result;
        }

        public async Task DeleteProductImageByPathAsync(string path)
        {
            await this.cloudinaryService.DeleteImageAsync(cloudinary, path);
        }

        public List<ProductOnAllPageViewModel> GetTopProducts()
        {
            var products = this.context.Products.OrderByDescending(x => x.Price)
                .Select(x => new ProductOnAllPageViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category.Name,
                    Price = x.Price,
                    Cover = x.Images.Where(x => x.IsCover == true).FirstOrDefault()!.Url
                }).ToList();
            return products;
        }
        public ICollection<SearchProductViewModel> SearchProducts(SearchProductInputModel input)
        {
            if (input.Name == null)
            {
                throw new ArgumentException("Product name cannot be null");
            }
            var products = context
                .Products
                .Where(x => x.Name.ToLower().Contains(input.Name.ToLower()))
                .Select(x => new SearchProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SellerName = x.User.UserName,
                    ReleaseDate = ((DateTime)x.DateListed).ToString("MM/dd/yyyy"),
                    Category = x.Category.Name,
                })
                .ToList();

            if (input.SellerName != null)
            {
                products= products.Where(x => x.SellerName.ToLower().Contains(input.SellerName.ToLower())).ToList();
            }
            if (input.Category != null)
            {
                products = products.Where(x => x.Category.ToLower().Contains(input.Category.ToLower())).ToList();
            }
            
            //List< ProductOnAllPageViewModel> products=new List< ProductOnAllPageViewModel>();
            //if (input.Name != null)
            //{
                

            //         products = context
            //        .Products
            //        .Where(x => x.Name.ToLower().Contains(input.Name.ToLower()))
            //        .Select(x => new SearchProductViewModel
            //        {
            //            Id = x.Id,
            //            Name = x.Name,
            //            SellerName = x.User.UserName,
            //            ReleaseDate = ((DateTime)x.DateListed).ToString("MM/dd/yyyy"),
            //            Category = x.Category.Name,
            //        })
            //        .ToList();

            //    if (input.SellerName != null)
            //    {
            //        products = products.Where(x => x.SellerName.ToLower().Contains(input.SellerName.ToLower())).ToList();
            //    }
            //    if (input.Category != null)
            //    {
            //        products = products.Where(x => x.Category.ToLower().Contains(input.Category.ToLower())).ToList();
            //    }
            //}else if(input.SellerName != null)
            //{
            //        products = context
            //       .Products
            //       .Where(x => x.User.UserName.ToLower().Contains(input.Name.ToLower()))
            //       .Select(x => new SearchProductViewModel
            //       {
            //           Id = x.Id,
            //           Name = x.Name,
            //           SellerName = x.User.UserName,
            //           ReleaseDate = ((DateTime)x.DateListed).ToString("MM/dd/yyyy"),
            //           Category = x.Category.Name,
            //       })
            //       .ToList();
            //    if (input.Category != null)
            //    {
            //        products = products.Where(x => x.Category.ToLower().Contains(input.Category.ToLower())).ToList();
            //    }
            //}
            //else if(input.Category!=null){
            //        products = context
            //       .Products
            //       .Where(x => x.Category.Name.ToLower().Contains(input.Name.ToLower()))
            //       .Select(x => new SearchProductViewModel
            //       {
            //           Id = x.Id,
            //           Name = x.Name,
            //           SellerName = x.User.UserName,
            //           ReleaseDate = ((DateTime)x.DateListed).ToString("MM/dd/yyyy"),
            //           Category = x.Category.Name,
            //       })
            //       .ToList();
            //}else
            //{
            //    throw new ArgumentException("Everything cannot be null");
            //}


            return products;
        }
    }
}
