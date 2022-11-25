using ShredStore.Factory.Interface;
using ShredStore.Models;
using ShredStore.Models.Utility;

namespace ShredStore.Factory.ConcreteFactory
{
    public class ConcreteProductFactory : IProductFactory
    {
        private readonly MiscellaneousUtilityClass _utilityClass;
        public ConcreteProductFactory(MiscellaneousUtilityClass _utilityClass)
        {
            this._utilityClass = _utilityClass;
        }

        public async Task<ProductViewModel> createProduct(ProductViewModel productInfo)
        {
            ProductViewModel newProduct = new ProductViewModel();
            if(productInfo.Id > 0)
            {
                newProduct.Id = productInfo.Id;
                if (productInfo.ImageFile != null)
                {
                    string res = _utilityClass.DeleteImage(productInfo.ImageName);
                    newProduct.ImageName = await _utilityClass.UploadImage(productInfo.ImageFile);
                    newProduct.ImageFile = productInfo.ImageFile;
                }
                newProduct.ImageName = productInfo.ImageName;
            }
            else
            {
                newProduct.ImageName = await _utilityClass.UploadImage(productInfo.ImageFile);
            }
            newProduct.Name = productInfo.Name;
            newProduct.Brand = productInfo.Brand;
            newProduct.Description = productInfo.Description;
            newProduct.Category = productInfo.Category;
            newProduct.UserId = productInfo.UserId;
            newProduct.Price = productInfo.Price;
            
            return newProduct;
        }


        


    }
}
