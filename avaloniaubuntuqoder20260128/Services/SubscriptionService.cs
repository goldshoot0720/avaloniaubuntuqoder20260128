using Appwrite.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using avaloniaubuntuqoder20260128.Models;

namespace avaloniaubuntuqoder20260128.Services;

/// <summary>
/// 訂閱資料服務
/// </summary>
public class SubscriptionService
{
    private readonly AppwriteService _appwriteService;
    
    public SubscriptionService(AppwriteService appwriteService)
    {
        _appwriteService = appwriteService;
    }
    
    /// <summary>
    /// 取得所有訂閱資料
    /// </summary>
    public async Task<List<Subscription>> GetAllSubscriptionsAsync()
    {
        var documents = await _appwriteService.Databases.ListDocuments(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.SubscriptionCollectionId
        );
        
        return documents.Documents.Select(ConvertToSubscription).ToList();
    }
    
    /// <summary>
    /// 根據 ID 取得訂閱資料
    /// </summary>
    public async Task<Subscription> GetSubscriptionByIdAsync(string documentId)
    {
        var document = await _appwriteService.Databases.GetDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.SubscriptionCollectionId,
            documentId
        );
        
        return ConvertToSubscription(document);
    }
    
    /// <summary>
    /// 建立新的訂閱資料
    /// </summary>
    public async Task<Subscription> CreateSubscriptionAsync(Subscription subscription)
    {
        var data = new Dictionary<string, object>
        {
            { "name", subscription.Name },
            { "site", subscription.Site },
            { "price", subscription.Price },
            { "nextdate", subscription.NextDate },
            { "note", subscription.Note },
            { "account", subscription.Account }
        };
        
        var document = await _appwriteService.Databases.CreateDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.SubscriptionCollectionId,
            "unique()",
            data
        );
        
        return ConvertToSubscription(document);
    }
    
    /// <summary>
    /// 更新訂閱資料
    /// </summary>
    public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription)
    {
        var data = new Dictionary<string, object>
        {
            { "name", subscription.Name },
            { "site", subscription.Site },
            { "price", subscription.Price },
            { "nextdate", subscription.NextDate },
            { "note", subscription.Note },
            { "account", subscription.Account }
        };
        
        var document = await _appwriteService.Databases.UpdateDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.SubscriptionCollectionId,
            subscription.Id,
            data
        );
        
        return ConvertToSubscription(document);
    }
    
    /// <summary>
    /// 刪除訂閱資料
    /// </summary>
    public async Task DeleteSubscriptionAsync(string documentId)
    {
        await _appwriteService.Databases.DeleteDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.SubscriptionCollectionId,
            documentId
        );
    }
    
    /// <summary>
    /// 將 Appwrite Document 轉換為 Subscription 模型
    /// </summary>
    private Subscription ConvertToSubscription(Document document)
    {
        return new Subscription
        {
            Id = document.Id,
            Name = document.Data.TryGetValue("name", out var name) ? name?.ToString() ?? "" : "",
            Site = document.Data.TryGetValue("site", out var site) ? site?.ToString() ?? "" : "",
            Price = document.Data.TryGetValue("price", out var price) ? Convert.ToInt32(price) : 0,
            NextDate = document.Data.TryGetValue("nextdate", out var nextdate) ? DateTime.Parse(nextdate?.ToString() ?? DateTime.Now.ToString()) : DateTime.Now,
            Note = document.Data.TryGetValue("note", out var note) ? note?.ToString() ?? "" : "",
            Account = document.Data.TryGetValue("account", out var account) ? account?.ToString() ?? "" : "",
            CreatedAt = document.Data.TryGetValue("$createdAt", out var createdAt) ? DateTime.Parse(createdAt?.ToString() ?? DateTime.Now.ToString()) : DateTime.Now,
            UpdatedAt = document.Data.TryGetValue("$updatedAt", out var updatedAt) ? DateTime.Parse(updatedAt?.ToString() ?? DateTime.Now.ToString()) : DateTime.Now
        };
    }
}
