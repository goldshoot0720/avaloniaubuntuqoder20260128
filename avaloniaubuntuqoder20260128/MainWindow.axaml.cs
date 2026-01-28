using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using avaloniaubuntuqoder20260128.Services;
using avaloniaubuntuqoder20260128.Views;
using System;
using System.Threading.Tasks;

namespace avaloniaubuntuqoder20260128;

public partial class MainWindow : Window
{
    private readonly AppwriteService _appwriteService;
    private readonly FoodService _foodService;
    private readonly SubscriptionService _subscriptionService;
    private readonly BankService _bankService;
    private string _musicUrl = "https://fra.cloud.appwrite.io/v1/storage/buckets/6867c5280021205ba9c0/files/6979070d00375eecdfd0/view?project=680c76af0037a7d23e44&mode=admin";
    private string _videoUrl = "https://fra.cloud.appwrite.io/v1/storage/buckets/6867c5280021205ba9c0/files/697907f10026bb583f30/view?project=680c76af0037a7d23e44&mode=admin";
    
    public MainWindow()
    {
        InitializeComponent();
        
        // 初始化 Appwrite 服務
        _appwriteService = new AppwriteService();
        _foodService = new FoodService(_appwriteService);
        _subscriptionService = new SubscriptionService(_appwriteService);
        _bankService = new BankService(_appwriteService);
    }
    
    private void OnHomeClick(object? sender, RoutedEventArgs e)
    {
        ShowHomeView();
    }
    
    private void OnDashboardClick(object? sender, RoutedEventArgs e)
    {
        ShowDashboardView();
    }
    
    private void OnSubscriptionManagementClick(object? sender, RoutedEventArgs e)
    {
        ShowSubscriptionManagementView();
    }
    
    private void OnFoodManagementClick(object? sender, RoutedEventArgs e)
    {
        ShowFoodManagementView();
    }
    
    private void OnVideoIntroClick(object? sender, RoutedEventArgs e)
    {
        ShowVideoIntroView();
    }
    
    private void OnMusicLyricsClick(object? sender, RoutedEventArgs e)
    {
        ShowMusicLyricsView();
    }
    
    private void OnBankStatisticsClick(object? sender, RoutedEventArgs e)
    {
        ShowBankStatisticsView();
    }
    
    private void OnAboutClick(object? sender, RoutedEventArgs e)
    {
        ShowAboutView();
    }
    
    private void UpdateContent(string title, string description)
    {
        var contentPanel = this.FindControl<StackPanel>("ContentPanel");
        if (contentPanel != null)
        {
            contentPanel.Children.Clear();
            
            // Title
            var titleBlock = new TextBlock
            {
                Text = title,
                FontSize = 24,
                FontWeight = FontWeight.Bold,
                Margin = new Avalonia.Thickness(0, 0, 0, 10)
            };
            contentPanel.Children.Add(titleBlock);
            
            // Description
            AddTextBlock(contentPanel, description);
            
            // Sample content cards
            for (int i = 1; i <= 3; i++)
            {
                var border = new Border
                {
                    Background = new SolidColorBrush(Colors.White),
                    Padding = new Avalonia.Thickness(15),
                    Margin = new Avalonia.Thickness(0, 10),
                    CornerRadius = new Avalonia.CornerRadius(5)
                };
                
                var cardStack = new StackPanel();
                
                var cardTitle = new TextBlock
                {
                    Text = $"{title} - 項目 {i}",
                    FontSize = 16,
                    FontWeight = FontWeight.Bold,
                    Margin = new Avalonia.Thickness(0, 0, 0, 10)
                };
                
                var cardContent = new TextBlock
                {
                    Text = $"項目 {i} 的內容。",
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap
                };
                
                cardStack.Children.Add(cardTitle);
                cardStack.Children.Add(cardContent);
                border.Child = cardStack;
                
                contentPanel.Children.Add(border);
            }
        }
    }
    
    private void AddTextBlock(StackPanel parent, string text)
    {
        var textBlock = new TextBlock
        {
            Text = text,
            FontSize = 14,
            Margin = new Avalonia.Thickness(0, 0, 0, 5),
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        };
        parent.Children.Add(textBlock);
    }
    
    // Appwrite 服務使用範例
    
    /// <summary>
    /// 載入儀表板資料的範例
    /// </summary>
    private async Task LoadDashboardDataAsync()
    {
        try
        {
            // 取得所有食物資料
            var foods = await _foodService.GetAllFoodsAsync();
            
            // 取得所有訂閱資料
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            
            // 取得所有銀行資料
            var banks = await _bankService.GetAllBanksAsync();
            
            // 處理資料並更新 UI
            // ...
        }
        catch (Exception ex)
        {
            // 錯誤處理
            System.Diagnostics.Debug.WriteLine($"載入資料時發生錯誤: {ex.Message}");
        }
    }
    
    /// <summary>
    /// 建立新食物資料的範例
    /// </summary>
    private async Task CreateFoodExampleAsync()
    {
        try
        {
            var foodData = new
            {
                name = "範例食物",
                description = "這是一個範例食物描述",
                price = 100
            };
            
            var result = await _foodService.CreateFoodAsync(
                new Models.Food
                {
                    Name = "範例食物",
                    Amount = 1,
                    Price = 100,
                    Shop = "範例商店",
                    ToDate = DateTime.Now
                }
            );
            
            // 處理結果
            System.Diagnostics.Debug.WriteLine($"建立成功: {result.Id}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"建立失敗: {ex.Message}");
        }
    }
    
    // UI 視圖切換方法
    
    private void ShowHomeView()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl != null)
        {
            // 顯示預設的歡迎畫面（已在 XAML 中定義）
            UpdateContent("歡迎使用", "這是一個功能強大的管理應用程式，包含食品管理、訂閱管理和銀行統計功能。");
        }
    }
    
    private void ShowDashboardView()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl == null) return;
        
        var scrollViewer = new ScrollViewer
        {
            Padding = new Avalonia.Thickness(20)
        };
        
        var panel = new StackPanel();
        
        // 標題
        panel.Children.Add(new TextBlock
        {
            Text = "儀表板",
            FontSize = 24,
            FontWeight = FontWeight.Bold,
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        });
        
        // 統計卡片區域
        var statsGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,*,*"),
            Margin = new Avalonia.Thickness(0, 0, 0, 30)
        };
        
        // 食品統計
        var foodCard = CreateStatCard("食品管理", "15", "筆資料", "#27AE60");
        Grid.SetColumn(foodCard, 0);
        statsGrid.Children.Add(foodCard);
        
        // 訂閱統計
        var subCard = CreateStatCard("訂閱管理", "8", "個訂閱", "#3498DB");
        Grid.SetColumn(subCard, 1);
        statsGrid.Children.Add(subCard);
        
        // 銀行統計
        var bankCard = CreateStatCard("銀行統計", "5", "個帳戶", "#E67E22");
        Grid.SetColumn(bankCard, 2);
        statsGrid.Children.Add(bankCard);
        
        panel.Children.Add(statsGrid);
        
        // 說明區域
        panel.Children.Add(CreateInfoCard(
            "歡迎使用儀表板",
            "這裡顯示您的所有資料統計。您可以從左側選單中選擇不同的功能模塊，進行詳細的資料管理。"
        ));
        
        panel.Children.Add(CreateInfoCard(
            "主要功能",
            "● 食品管理：追蹤食品購買記錄\n● 訂閱管理：管理各種訂閱服務\n● 銀行統計：財務資料統計分析"
        ));
        
        scrollViewer.Content = panel;
        contentControl.Content = scrollViewer;
    }
    
    private void ShowVideoIntroView()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl == null) return;
            
        var scrollViewer = new ScrollViewer
        {
            Padding = new Avalonia.Thickness(20)
        };
            
        var panel = new StackPanel();
            
        // 標題
        panel.Children.Add(new TextBlock
        {
            Text = "影片介紹",
            FontSize = 24,
            FontWeight = FontWeight.Bold,
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        });
            
        // 特色推薦
        var featuredBorder = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#E74C3C")),
            CornerRadius = new Avalonia.CornerRadius(8),
            Padding = new Avalonia.Thickness(20),
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        };
            
        var featuredStack = new StackPanel();
        featuredStack.Children.Add(new TextBlock
        {
            Text = "🔥 特色推薦",
            FontSize = 18,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Colors.White),
            Margin = new Avalonia.Thickness(0, 0, 0, 10)
        });
            
        featuredStack.Children.Add(new TextBlock
        {
            Text = "鋒兄進化 Show🔥",
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Colors.White),
            Margin = new Avalonia.Thickness(0, 0, 0, 10)
        });
            
        var videoInfo = new TextBlock
        {
            Text = "🎬 鋒兄進化 Show🔥影片保留十五年.mp4",
            FontSize = 14,
            Foreground = new SolidColorBrush(Colors.White),
            Opacity = 0.9,
            Margin = new Avalonia.Thickness(0, 0, 0, 10)
        };
        featuredStack.Children.Add(videoInfo);
            
        var urlText = new TextBlock
        {
            Text = "影片來源：Appwrite 儲存空間",
            FontSize = 12,
            Foreground = new SolidColorBrush(Colors.White),
            Opacity = 0.8,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        };
        featuredStack.Children.Add(urlText);
            
        // 播放按鈕
        var playButton = new Button
        {
            Content = "▶ 播放影片",
            Background = new SolidColorBrush(Colors.White),
            Foreground = new SolidColorBrush(Color.Parse("#E74C3C")),
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            Padding = new Avalonia.Thickness(30, 10),
            Margin = new Avalonia.Thickness(0, 15, 0, 5),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left
        };
        playButton.Click += (s, e) => OpenVideoPlayer();
        featuredStack.Children.Add(playButton);
            
        featuredBorder.Child = featuredStack;
        panel.Children.Add(featuredBorder);
            
        // 影片清單標題
        panel.Children.Add(new TextBlock
        {
            Text = "🎥 更多影片",
            FontSize = 18,
            FontWeight = FontWeight.Bold,
            Margin = new Avalonia.Thickness(0, 10, 0, 15)
        });
            
        // 影片清單
        var videos = new[]
        {
            new { Title = "鋒兄進化 Show🔥", Duration = "保留十五年", Description = "精彩的進化歷程，值得珍藏十五年的經典影片", Featured = true },
            new { Title = "應用程式介紹", Duration = "5:30", Description = "了解如何使用本應用程式的各項功能", Featured = false },
            new { Title = "食品管理教學", Duration = "8:15", Description = "學習如何有效管理您的食品購買記錄", Featured = false },
            new { Title = "訂閱管理技巧", Duration = "6:45", Description = "掌握訂閱服務管理的實用技巧", Featured = false },
            new { Title = "銀行統計分析", Duration = "10:20", Description = "如何讀懂您的財務統計報表", Featured = false },
            new { Title = "進階功能介紹", Duration = "12:00", Description = "探索更多進階功能和使用訣竅", Featured = false }
        };
            
        foreach (var video in videos)
        {
            panel.Children.Add(CreateVideoCard(video.Title, video.Duration, video.Description, video.Featured));
        }
            
        scrollViewer.Content = panel;
        contentControl.Content = scrollViewer;
    }
    
    private void ShowMusicLyricsView()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl == null) return;
        
        var scrollViewer = new ScrollViewer
        {
            Padding = new Avalonia.Thickness(20)
        };
        
        var panel = new StackPanel();
        
        // 標題
        panel.Children.Add(new TextBlock
        {
            Text = "鋒兄音樂歌詞",
            FontSize = 24,
            FontWeight = FontWeight.Bold,
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        });
        
        // 特色推薦
        var featuredBorder = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#E74C3C")),
            CornerRadius = new Avalonia.CornerRadius(8),
            Padding = new Avalonia.Thickness(20),
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        };
        
        var featuredStack = new StackPanel();
        featuredStack.Children.Add(new TextBlock
        {
            Text = "🔥 特色推薦",
            FontSize = 18,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Colors.White),
            Margin = new Avalonia.Thickness(0, 0, 0, 10)
        });
        
        featuredStack.Children.Add(new TextBlock
        {
            Text = "鋒兄進化Show🔥",
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Colors.White),
            Margin = new Avalonia.Thickness(0, 0, 0, 10)
        });
        
        var audioInfo = new TextBlock
        {
            Text = "🎵 鋒兄進化Show🔥.mp3",
            FontSize = 14,
            Foreground = new SolidColorBrush(Colors.White),
            Opacity = 0.9,
            Margin = new Avalonia.Thickness(0, 0, 0, 10)
        };
        featuredStack.Children.Add(audioInfo);
        
        var urlText = new TextBlock
        {
            Text = "音訊來源：Appwrite 儲存空間",
            FontSize = 12,
            Foreground = new SolidColorBrush(Colors.White),
            Opacity = 0.8,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        };
        featuredStack.Children.Add(urlText);
        
        // 播放按鈕
        var playButton = new Button
        {
            Content = "▶ 播放音樂",
            Background = new SolidColorBrush(Colors.White),
            Foreground = new SolidColorBrush(Color.Parse("#E74C3C")),
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            Padding = new Avalonia.Thickness(30, 10),
            Margin = new Avalonia.Thickness(0, 15, 0, 5),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left
        };
        playButton.Click += (s, e) => OpenMusicPlayer();
        featuredStack.Children.Add(playButton);
        
        featuredBorder.Child = featuredStack;
        panel.Children.Add(featuredBorder);
        
        // 歌曲清單標題
        panel.Children.Add(new TextBlock
        {
            Text = "🎼 更多作品",
            FontSize = 18,
            FontWeight = FontWeight.Bold,
            Margin = new Avalonia.Thickness(0, 10, 0, 15)
        });
        
        // 歌曲清單
        var songs = new[]
        {
            new { Title = "鋒兄進化Show🔥", Artist = "鋒兄", Album = "精選作品", Year = "2024", Featured = true },
            new { Title = "夢想追尋", Artist = "鋒兄", Album = "音樂創作集 Vol.1", Year = "2023", Featured = false },
            new { Title = "時光之歌", Artist = "鋒兄", Album = "音樂創作集 Vol.1", Year = "2023", Featured = false },
            new { Title = "星空下的約定", Artist = "鋒兄", Album = "音樂創作集 Vol.2", Year = "2024", Featured = false },
            new { Title = "勇氣前進", Artist = "鋒兄", Album = "音樂創作集 Vol.2", Year = "2024", Featured = false },
            new { Title = "心中的光", Artist = "鋒兄", Album = "單曲", Year = "2024", Featured = false }
        };
        
        foreach (var song in songs)
        {
            panel.Children.Add(CreateSongCard(song.Title, song.Artist, song.Album, song.Year, song.Featured));
        }
        
        scrollViewer.Content = panel;
        contentControl.Content = scrollViewer;
    }
    
    private void ShowAboutView()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl == null) return;
        
        var scrollViewer = new ScrollViewer
        {
            Padding = new Avalonia.Thickness(20)
        };
        
        var panel = new StackPanel();
        
        // 標題
        panel.Children.Add(new TextBlock
        {
            Text = "關於我們",
            FontSize = 24,
            FontWeight = FontWeight.Bold,
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        });
        
        // 應用程式資訊
        panel.Children.Add(CreateInfoCard(
            "應用程式資訊",
            "名稱：生活管理助手\n版本：1.0.0\n開發框架：Avalonia UI + .NET 10.0\n後端服務：Appwrite"
        ));
        
        panel.Children.Add(CreateInfoCard(
            "功能特色",
            "● 直覺的使用者介面\n● 即時資料同步\n● 強大的搜尋功能\n● 詳細的統計分析\n● 跨平台支援"
        ));
        
        panel.Children.Add(CreateInfoCard(
            "聯絡資訊",
            "📧 電子郵件：info@example.com\n🌐 官方網站：https://example.com\n📱 社交媒體：@example"
        ));
        
        panel.Children.Add(CreateInfoCard(
            "版權聲明",
            "© 2024-2026 鋒兄工作室. 保留所有權利.\n\n本應用程式仅供個人使用，不得用於商業目的。"
        ));
        
        scrollViewer.Content = panel;
        contentControl.Content = scrollViewer;
    }
    
    private void ShowFoodManagementView()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl != null)
        {
            contentControl.Content = new FoodManagementView();
        }
    }
    
    private void ShowSubscriptionManagementView()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl != null)
        {
            contentControl.Content = new SubscriptionManagementView();
        }
    }
    
    private void ShowBankStatisticsView()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl != null)
        {
            contentControl.Content = new BankStatisticsView();
        }
    }
    
    // 卡片建立輔助方法
    
    private Border CreateStatCard(string title, string value, string unit, string color)
    {
        return new Border
        {
            Background = new SolidColorBrush(Color.Parse(color)),
            CornerRadius = new Avalonia.CornerRadius(8),
            Padding = new Avalonia.Thickness(20),
            Margin = new Avalonia.Thickness(5),
            Child = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = title,
                        Foreground = new SolidColorBrush(Colors.White),
                        FontSize = 14,
                        Margin = new Avalonia.Thickness(0, 0, 0, 10)
                    },
                    new TextBlock
                    {
                        Text = value,
                        Foreground = new SolidColorBrush(Colors.White),
                        FontSize = 32,
                        FontWeight = FontWeight.Bold
                    },
                    new TextBlock
                    {
                        Text = unit,
                        Foreground = new SolidColorBrush(Colors.White),
                        FontSize = 14,
                        Opacity = 0.8
                    }
                }
            }
        };
    }
    
    private Border CreateInfoCard(string title, string content)
    {
        return new Border
        {
            Background = new SolidColorBrush(Colors.White),
            Padding = new Avalonia.Thickness(20),
            Margin = new Avalonia.Thickness(0, 0, 0, 15),
            CornerRadius = new Avalonia.CornerRadius(8),
            BoxShadow = new Avalonia.Media.BoxShadows(
                new Avalonia.Media.BoxShadow
                {
                    OffsetX = 0,
                    OffsetY = 2,
                    Blur = 8,
                    Color = Color.FromArgb(40, 0, 0, 0)
                }
            ),
            Child = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = title,
                        FontSize = 18,
                        FontWeight = FontWeight.Bold,
                        Margin = new Avalonia.Thickness(0, 0, 0, 10)
                    },
                    new TextBlock
                    {
                        Text = content,
                        FontSize = 14,
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Foreground = new SolidColorBrush(Color.Parse("#7F8C8D"))
                    }
                }
            }
        };
    }
    
    private Border CreateVideoCard(string title, string duration, string description, bool featured = false)
    {
        var border = new Border
        {
            Background = new SolidColorBrush(Colors.White),
            Padding = new Avalonia.Thickness(20),
            Margin = new Avalonia.Thickness(0, 0, 0, 15),
            CornerRadius = new Avalonia.CornerRadius(8),
            BoxShadow = new Avalonia.Media.BoxShadows(
                new Avalonia.Media.BoxShadow
                {
                    OffsetX = 0,
                    OffsetY = 2,
                    Blur = 8,
                    Color = Color.FromArgb(40, 0, 0, 0)
                }
            )
        };
        
        if (featured)
        {
            border.BorderBrush = new SolidColorBrush(Color.Parse("#E74C3C"));
            border.BorderThickness = new Avalonia.Thickness(2);
        }
        
        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto")
        };
        
        var leftStack = new StackPanel();
        
        var titleStack = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Spacing = 10
        };
        
        titleStack.Children.Add(new TextBlock
        {
            Text = $"🎥 {title}",
            FontSize = 16,
            FontWeight = FontWeight.Bold
        });
        
        if (featured)
        {
            titleStack.Children.Add(new Border
            {
                Background = new SolidColorBrush(Color.Parse("#E74C3C")),
                CornerRadius = new Avalonia.CornerRadius(3),
                Padding = new Avalonia.Thickness(8, 4),
                Child = new TextBlock
                {
                    Text = "🔥 特色",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 12,
                    FontWeight = FontWeight.Bold
                }
            });
        }
        
        leftStack.Children.Add(titleStack);
        leftStack.Children.Add(new TextBlock
        {
            Text = description,
            FontSize = 14,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Foreground = new SolidColorBrush(Color.Parse("#7F8C8D")),
            Margin = new Avalonia.Thickness(0, 5, 0, 0)
        });
        
        Grid.SetColumn(leftStack, 0);
        grid.Children.Add(leftStack);
        
        var durationText = new TextBlock
        {
            Text = duration,
            FontSize = 14,
            FontWeight = FontWeight.SemiBold,
            Foreground = new SolidColorBrush(Color.Parse("#3498DB")),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };
        Grid.SetColumn(durationText, 1);
        grid.Children.Add(durationText);
        
        border.Child = grid;
        return border;
    }
    
    private Border CreateSongCard(string title, string artist, string album, string year, bool featured = false)
    {
        var border = new Border
        {
            Background = new SolidColorBrush(Colors.White),
            Padding = new Avalonia.Thickness(20),
            Margin = new Avalonia.Thickness(0, 0, 0, 15),
            CornerRadius = new Avalonia.CornerRadius(8),
            BoxShadow = new Avalonia.Media.BoxShadows(
                new Avalonia.Media.BoxShadow
                {
                    OffsetX = 0,
                    OffsetY = 2,
                    Blur = 8,
                    Color = Color.FromArgb(40, 0, 0, 0)
                }
            )
        };
        
        if (featured)
        {
            border.BorderBrush = new SolidColorBrush(Color.Parse("#E74C3C"));
            border.BorderThickness = new Avalonia.Thickness(2);
        }
        
        var stack = new StackPanel();
        
        var titleStack = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Spacing = 10
        };
        
        titleStack.Children.Add(new TextBlock
        {
            Text = $"🎵 {title}",
            FontSize = 18,
            FontWeight = FontWeight.Bold
        });
        
        if (featured)
        {
            titleStack.Children.Add(new Border
            {
                Background = new SolidColorBrush(Color.Parse("#E74C3C")),
                CornerRadius = new Avalonia.CornerRadius(3),
                Padding = new Avalonia.Thickness(8, 4),
                Child = new TextBlock
                {
                    Text = "🔥 特色",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 12,
                    FontWeight = FontWeight.Bold
                }
            });
        }
        
        stack.Children.Add(titleStack);
        stack.Children.Add(new Separator { Margin = new Avalonia.Thickness(0, 10, 0, 10) });
        
        var infoGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto,*"),
            RowDefinitions = new RowDefinitions("Auto,Auto")
        };
        
        // 歌手
        var artistLabel = new TextBlock
        {
            Text = "歌手：",
            FontSize = 14,
            Foreground = new SolidColorBrush(Color.Parse("#7F8C8D"))
        };
        Grid.SetColumn(artistLabel, 0);
        Grid.SetRow(artistLabel, 0);
        infoGrid.Children.Add(artistLabel);
        
        var artistValue = new TextBlock
        {
            Text = artist,
            FontSize = 14,
            Margin = new Avalonia.Thickness(5, 0, 20, 0)
        };
        Grid.SetColumn(artistValue, 1);
        Grid.SetRow(artistValue, 0);
        infoGrid.Children.Add(artistValue);
        
        // 專輯
        var albumLabel = new TextBlock
        {
            Text = "專輯：",
            FontSize = 14,
            Foreground = new SolidColorBrush(Color.Parse("#7F8C8D"))
        };
        Grid.SetColumn(albumLabel, 2);
        Grid.SetRow(albumLabel, 0);
        infoGrid.Children.Add(albumLabel);
        
        var albumValue = new TextBlock
        {
            Text = album,
            FontSize = 14,
            Margin = new Avalonia.Thickness(5, 0, 0, 0)
        };
        Grid.SetColumn(albumValue, 3);
        Grid.SetRow(albumValue, 0);
        infoGrid.Children.Add(albumValue);
        
        // 年份
        var yearText = new TextBlock
        {
            Text = $"發行年份：{year}",
            FontSize = 12,
            Foreground = new SolidColorBrush(Color.Parse("#95A5A6")),
            Margin = new Avalonia.Thickness(0, 5, 0, 0)
        };
        Grid.SetColumn(yearText, 0);
        Grid.SetRow(yearText, 1);
        Grid.SetColumnSpan(yearText, 4);
        infoGrid.Children.Add(yearText);
        
        stack.Children.Add(infoGrid);
        border.Child = stack;
        
        return border;
    }
    
    private void OpenMusicUrl(string url)
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"開啟音樂連結失敗：{ex.Message}");
        }
    }
    
    private void OpenVideoPlayer()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl == null) return;
        
        var videoPlayerView = new VideoPlayerView();
        videoPlayerView.CloseRequested += (s, e) =>
        {
            // Return to video intro view
            ShowVideoIntroView();
        };
        
        contentControl.Content = videoPlayerView;
        videoPlayerView.LoadMedia(_videoUrl, "鋒兄進化 Show🔥");
    }
    
    private void OpenMusicPlayer()
    {
        var contentControl = this.FindControl<ContentControl>("MainContentControl");
        if (contentControl == null) return;
        
        var musicPlayerView = new MusicPlayerView();
        musicPlayerView.CloseRequested += (s, e) =>
        {
            // Return to music lyrics view
            ShowMusicLyricsView();
        };
        
        contentControl.Content = musicPlayerView;
        musicPlayerView.LoadMusic(_musicUrl, "鋒兄進化Show🔥", "鋒兄");
    }
}