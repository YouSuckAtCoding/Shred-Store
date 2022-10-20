﻿using DataLibrary.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public ProductRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }

        public Task<IEnumerable<ProductModel>> GetProducts() =>
            sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { });

        public Task<IEnumerable<ProductModel>> GetProductsByCategory(string Category) =>
            sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetByCategory", new { Category = Category});

        public async Task<ProductModel?> GetProduct(int id)
        {
            var result = await sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = id });

            return result.FirstOrDefault();
        }
        public async Task<IEnumerable<ProductModel>> GetProductsByUserId(int userId)
        {
            var result = await sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetByUserId", new { UserId = userId });

            return result;
        }

        public Task InsertProduct(ProductModel product) =>
            sqlDataAccess.SaveData("dbo.spProduct_Insert", new { product.Name, product.UserId, product.Price, product.Description, product.Category, product.ImageName });

        public Task UpdateProduct(ProductModel product) =>
            sqlDataAccess.SaveData("dbo.spProduct_Update", new { product.Id, product.Name, product.UserId, product.Price, product.Description, product.Category, product.ImageName });

        public Task DeleteProduct(int id) => sqlDataAccess.SaveData("dbo.spProduct_Delete", new { Id = id });
    }
}
