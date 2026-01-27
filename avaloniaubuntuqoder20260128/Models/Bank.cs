using System;

namespace avaloniaubuntuqoder20260128.Models;

/// <summary>
/// 銀行資料模型
/// </summary>
public class Bank
{
    /// <summary>
    /// 文件 ID
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// 銀行名稱
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 存款金額
    /// </summary>
    public int Deposit { get; set; }
    
    /// <summary>
    /// 網站
    /// </summary>
    public string Site { get; set; } = string.Empty;
    
    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; set; } = string.Empty;
    
    /// <summary>
    /// 提款金額
    /// </summary>
    public int Withdrawals { get; set; }
    
    /// <summary>
    /// 轉帳金額
    /// </summary>
    public int Transfer { get; set; }
    
    /// <summary>
    /// 活動網址
    /// </summary>
    public string Activity { get; set; } = string.Empty;
    
    /// <summary>
    /// 卡片資訊
    /// </summary>
    public string Card { get; set; } = string.Empty;
    
    /// <summary>
    /// 帳號
    /// </summary>
    public string Account { get; set; } = string.Empty;
    
    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// 淨餘額（存款 - 提款 - 轉帳）
    /// </summary>
    public int NetBalance => Deposit - Withdrawals - Transfer;
    
    /// <summary>
    /// 總支出（提款 + 轉帳）
    /// </summary>
    public int TotalExpenditure => Withdrawals + Transfer;
}
