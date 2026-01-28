using Appwrite.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using avaloniaubuntuqoder20260128.Models;

namespace avaloniaubuntuqoder20260128.Services;

/// <summary>
/// 銀行資料服務
/// </summary>
public class BankService
{
    private readonly AppwriteService _appwriteService;
    
    public BankService(AppwriteService appwriteService)
    {
        _appwriteService = appwriteService;
    }
    
    /// <summary>
    /// 取得所有銀行資料
    /// </summary>
    public async Task<List<Bank>> GetAllBanksAsync()
    {
        var documents = await _appwriteService.Databases.ListDocuments(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.BankCollectionId
        );
        
        return documents.Documents.Select(ConvertToBank).ToList();
    }
    
    /// <summary>
    /// 根據 ID 取得銀行資料
    /// </summary>
    public async Task<Bank> GetBankByIdAsync(string documentId)
    {
        var document = await _appwriteService.Databases.GetDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.BankCollectionId,
            documentId
        );
        
        return ConvertToBank(document);
    }
    
    /// <summary>
    /// 建立新的銀行資料
    /// </summary>
    public async Task<Bank> CreateBankAsync(Bank bank)
    {
        var data = new Dictionary<string, object>
        {
            { "name", bank.Name },
            { "deposit", bank.Deposit },
            { "site", bank.Site },
            { "address", bank.Address },
            { "withdrawals", bank.Withdrawals },
            { "transfer", bank.Transfer },
            { "activity", bank.Activity },
            { "card", bank.Card },
            { "account", bank.Account }
        };
        
        var document = await _appwriteService.Databases.CreateDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.BankCollectionId,
            "unique()",
            data
        );
        
        return ConvertToBank(document);
    }
    
    /// <summary>
    /// 更新銀行資料
    /// </summary>
    public async Task<Bank> UpdateBankAsync(Bank bank)
    {
        var data = new Dictionary<string, object>
        {
            { "name", bank.Name },
            { "deposit", bank.Deposit },
            { "site", bank.Site },
            { "address", bank.Address },
            { "withdrawals", bank.Withdrawals },
            { "transfer", bank.Transfer },
            { "activity", bank.Activity },
            { "card", bank.Card },
            { "account", bank.Account }
        };
        
        var document = await _appwriteService.Databases.UpdateDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.BankCollectionId,
            bank.Id,
            data
        );
        
        return ConvertToBank(document);
    }
    
    /// <summary>
    /// 刪除銀行資料
    /// </summary>
    public async Task DeleteBankAsync(string documentId)
    {
        await _appwriteService.Databases.DeleteDocument(
            AppwriteConfig.DatabaseId,
            AppwriteConfig.BankCollectionId,
            documentId
        );
    }
    
    /// <summary>
    /// 將 Appwrite Document 轉換為 Bank 模型
    /// </summary>
    private Bank ConvertToBank(Document document)
    {
        return new Bank
        {
            Id = document.Id,
            Name = document.Data.TryGetValue("name", out var name) ? name?.ToString() ?? "" : "",
            Deposit = document.Data.TryGetValue("deposit", out var deposit) ? Convert.ToInt32(deposit) : 0,
            Site = document.Data.TryGetValue("site", out var site) ? site?.ToString() ?? "" : "",
            Address = document.Data.TryGetValue("address", out var address) ? address?.ToString() ?? "" : "",
            Withdrawals = document.Data.TryGetValue("withdrawals", out var withdrawals) ? Convert.ToInt32(withdrawals) : 0,
            Transfer = document.Data.TryGetValue("transfer", out var transfer) ? Convert.ToInt32(transfer) : 0,
            Activity = document.Data.TryGetValue("activity", out var activity) ? activity?.ToString() ?? "" : "",
            Card = document.Data.TryGetValue("card", out var card) ? card?.ToString() ?? "" : "",
            Account = document.Data.TryGetValue("account", out var account) ? account?.ToString() ?? "" : "",
            CreatedAt = document.Data.TryGetValue("$createdAt", out var createdAt) ? DateTime.Parse(createdAt?.ToString() ?? DateTime.Now.ToString()) : DateTime.Now,
            UpdatedAt = document.Data.TryGetValue("$updatedAt", out var updatedAt) ? DateTime.Parse(updatedAt?.ToString() ?? DateTime.Now.ToString()) : DateTime.Now
        };
    }
}
