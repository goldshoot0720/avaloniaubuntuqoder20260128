using Appwrite;
using Appwrite.Services;

namespace avaloniaubuntuqoder20260128.Services;

/// <summary>
/// Appwrite 客戶端服務
/// </summary>
public class AppwriteService
{
    private readonly Client _client;
    private readonly Databases _databases;
    
    public AppwriteService()
    {
        // 初始化 Appwrite 客戶端
        _client = new Client()
            .SetEndpoint(AppwriteConfig.Endpoint)
            .SetProject(AppwriteConfig.ProjectId);
        
        // 初始化資料庫服務
        _databases = new Databases(_client);
    }
    
    /// <summary>
    /// 取得資料庫服務
    /// </summary>
    public Databases Databases => _databases;
    
    /// <summary>
    /// 取得客戶端
    /// </summary>
    public Client Client => _client;
    
    /// <summary>
    /// 設定會話金鑰（用於伺服器端操作）
    /// </summary>
    public AppwriteService SetSession(string session)
    {
        _client.SetSession(session);
        return this;
    }
    
    /// <summary>
    /// 設定 API 金鑰（用於伺服器端操作）
    /// </summary>
    public AppwriteService SetKey(string key)
    {
        _client.SetKey(key);
        return this;
    }
}
