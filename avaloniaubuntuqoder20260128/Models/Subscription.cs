using System;

namespace avaloniaubuntuqoder20260128.Models;

/// <summary>
/// 訂閱資料模型
/// </summary>
public class Subscription
{
    /// <summary>
    /// 文件 ID
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// 訂閱名稱
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 網站
    /// </summary>
    public string Site { get; set; } = string.Empty;
    
    /// <summary>
    /// 價格
    /// </summary>
    public int Price { get; set; }
    
    /// <summary>
    /// 下次續訂日期
    /// </summary>
    public DateTime NextDate { get; set; }
    
    /// <summary>
    /// 備註
    /// </summary>
    public string Note { get; set; } = string.Empty;
    
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
    /// 距離下次續訂天數（計算屬性）
    /// </summary>
    public int DaysUntilNext => (NextDate - DateTime.Now).Days;
    
    /// <summary>
    /// 是否即將到期（30天內）
    /// </summary>
    public bool IsDueSoon => DaysUntilNext <= 30 && DaysUntilNext >= 0;
    
    /// <summary>
    /// 是否已過期
    /// </summary>
    public bool IsOverdue => DaysUntilNext < 0;
}
