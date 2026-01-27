using Appwrite.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using avaloniaubuntuqoder20260128.Models;

namespace avaloniaubuntuqoder20260128.Services;

/// <summary>
/// 食物資料服務
/// </summary>
public class FoodService
{
    private readonly AppwriteService _appwriteService;
    
    public FoodService(AppwriteService appwriteService)
    {
        _appwriteService = appwriteService;
    }
    
    /// <summary>
    /// 取得所有食物資料
    /// </summary>
    public async Task<List<Food>> GetAllFoodsAsync()
    {
        var documents = await _appwriteService.Databases.ListDocuments(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.FoodCollectionId
        );
        
        return documents.Documents.Select(ConvertToFood).ToList();
    }
    
    /// <summary>
    /// 根據 ID 取得食物資料
    /// </summary>
    public async Task<Food> GetFoodByIdAsync(string documentId)
    {
        var document = await _appwriteService.Databases.GetDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.FoodCollectionId,
            documentId
        );
        
        return ConvertToFood(document);
    }
    
    /// <summary>
    /// 建立新的食物資料
    /// </summary>
    public async Task<Food> CreateFoodAsync(Food food)
    {
        var data = new Dictionary<string, object>
        {
            { "name", food.Name },
            { "amount", food.Amount },
            { "price", food.Price },
            { "shop", food.Shop },
            { "todate", food.ToDate },
            { "photo", food.Photo },
            { "photohash", food.PhotoHash }
        };
        
        var document = await _appwriteService.Databases.CreateDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.FoodCollectionId,
            "unique()",
            data
        );
        
        return ConvertToFood(document);
    }
    
    /// <summary>
    /// 更新食物資料
    /// </summary>
    public async Task<Food> UpdateFoodAsync(Food food)
    {
        var data = new Dictionary<string, object>
        {
            { "name", food.Name },
            { "amount", food.Amount },
            { "price", food.Price },
            { "shop", food.Shop },
            { "todate", food.ToDate },
            { "photo", food.Photo },
            { "photohash", food.PhotoHash }
        };
        
        var document = await _appwriteService.Databases.UpdateDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.FoodCollectionId,
            food.Id,
            data
        );
        
        return ConvertToFood(document);
    }
    
    /// <summary>
    /// 刪除食物資料
    /// </summary>
    public async Task DeleteFoodAsync(string documentId)
    {
        await _appwriteService.Databases.DeleteDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.FoodCollectionId,
            documentId
        );
    }
    
    /// <summary>
    /// 將 Appwrite Document 轉換為 Food 模型
    /// </summary>
    private Food ConvertToFood(Document document)
    {
        return new Food
        {
            Id = document.Id,
            Name = document.Data.TryGetValue("name", out var name) ? name?.ToString() ?? "" : "",
            Amount = document.Data.TryGetValue("amount", out var amount) ? Convert.ToInt32(amount) : 0,
            Price = document.Data.TryGetValue("price", out var price) ? Convert.ToInt32(price) : 0,
            Shop = document.Data.TryGetValue("shop", out var shop) ? shop?.ToString() ?? "" : "",
            ToDate = document.Data.TryGetValue("todate", out var todate) ? DateTime.Parse(todate?.ToString() ?? DateTime.Now.ToString()) : DateTime.Now,
            Photo = document.Data.TryGetValue("photo", out var photo) ? photo?.ToString() ?? "" : "",
            PhotoHash = document.Data.TryGetValue("photohash", out var photohash) ? photohash?.ToString() ?? "" : "",
            CreatedAt = document.Data.TryGetValue("$createdAt", out var createdAt) ? DateTime.Parse(createdAt?.ToString() ?? DateTime.Now.ToString()) : DateTime.Now,
            UpdatedAt = document.Data.TryGetValue("$updatedAt", out var updatedAt) ? DateTime.Parse(updatedAt?.ToString() ?? DateTime.Now.ToString()) : DateTime.Now
        };
    }
}
