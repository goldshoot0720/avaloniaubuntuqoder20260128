using System;

namespace avaloniaubuntuqoder20260128.Models;

/// <summary>
/// 食品資料模型
/// </summary>
public class Food
{
    /// <summary>
    /// 文件 ID
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// 食品名稱
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 數量
    /// </summary>
    public int Amount { get; set; }
    
    /// <summary>
    /// 價格
    /// </summary>
    public int Price { get; set; }
    
    /// <summary>
    /// 商店
    /// </summary>
    public string Shop { get; set; } = string.Empty;
    
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime ToDate { get; set; }
    
    /// <summary>
    /// 照片網址
    /// </summary>
    public string Photo { get; set; } = string.Empty;
    
    /// <summary>
    /// 照片雜湊值
    /// </summary>
    public string PhotoHash { get; set; } = string.Empty;
    
    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// 總價（計算屬性）
    /// </summary>
    public int TotalPrice => Amount * Price;
}
