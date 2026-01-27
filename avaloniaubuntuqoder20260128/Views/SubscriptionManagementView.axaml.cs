using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using avaloniaubuntuqoder20260128.Models;
using avaloniaubuntuqoder20260128.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace avaloniaubuntuqoder20260128.Views;

public partial class SubscriptionManagementView : UserControl
{
    private readonly SubscriptionService _subscriptionService;
    private List<Subscription> _allSubscriptions = new();
    
    public SubscriptionManagementView()
    {
        InitializeComponent();
        
        var appwriteService = new AppwriteService();
        _subscriptionService = new SubscriptionService(appwriteService);
        
        // 載入資料
        _ = LoadSubscriptionsAsync();
    }
    
    /// <summary>
    /// 載入所有訂閱資料
    /// </summary>
    private async Task LoadSubscriptionsAsync()
    {
        try
        {
            _allSubscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            // 依下次續訂日期排序
            _allSubscriptions = _allSubscriptions.OrderBy(s => s.NextDate).ToList();
            DisplaySubscriptions(_allSubscriptions);
        }
        catch (Exception ex)
        {
            ShowError($"載入失敗：{ex.Message}");
        }
    }
    
    /// <summary>
    /// 顯示訂閱清單
    /// </summary>
    private void DisplaySubscriptions(List<Subscription> subscriptions)
    {
        var panel = this.FindControl<StackPanel>("SubscriptionListPanel");
        if (panel == null) return;
        
        panel.Children.Clear();
        
        if (subscriptions.Count == 0)
        {
            var emptyText = new TextBlock
            {
                Text = "目前沒有訂閱資料",
                FontSize = 16,
                Foreground = new SolidColorBrush(Colors.Gray),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 50, 0, 0)
            };
            panel.Children.Add(emptyText);
            return;
        }
        
        foreach (var subscription in subscriptions)
        {
            var card = CreateSubscriptionCard(subscription);
            panel.Children.Add(card);
        }
    }
    
    /// <summary>
    /// 建立訂閱卡片
    /// </summary>
    private Border CreateSubscriptionCard(Subscription subscription)
    {
        // 根據狀態設定顏色
        Color borderColor;
        if (subscription.IsOverdue)
            borderColor = Color.Parse("#E74C3C"); // 紅色 - 已過期
        else if (subscription.IsDueSoon)
            borderColor = Color.Parse("#F39C12"); // 橘色 - 即將到期
        else
            borderColor = Colors.White;
        
        var border = new Border
        {
            Background = new SolidColorBrush(Colors.White),
            BorderBrush = new SolidColorBrush(borderColor),
            BorderThickness = new Avalonia.Thickness(0, 0, 0, 4),
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
        
        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto")
        };
        
        // 左側資訊
        var infoStack = new StackPanel();
        
        // 標題列（名稱 + 狀態標籤）
        var titleStack = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Spacing = 10,
            Margin = new Avalonia.Thickness(0, 0, 0, 10)
        };
        
        var nameText = new TextBlock
        {
            Text = subscription.Name,
            FontSize = 20,
            FontWeight = FontWeight.Bold
        };
        titleStack.Children.Add(nameText);
        
        // 狀態標籤
        if (subscription.IsOverdue)
        {
            titleStack.Children.Add(CreateStatusBadge("已過期", "#E74C3C"));
        }
        else if (subscription.IsDueSoon)
        {
            titleStack.Children.Add(CreateStatusBadge("即將到期", "#F39C12"));
        }
        
        infoStack.Children.Add(titleStack);
        
        var detailsGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto,*"),
            RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto")
        };
        
        // 網站
        AddDetailLabel(detailsGrid, "網站：", 0, 0);
        AddDetailValue(detailsGrid, subscription.Site, 1, 0, isLink: true);
        
        // 帳號
        AddDetailLabel(detailsGrid, "帳號：", 2, 0);
        AddDetailValue(detailsGrid, subscription.Account, 3, 0);
        
        // 價格
        AddDetailLabel(detailsGrid, "價格：", 0, 1);
        AddDetailValue(detailsGrid, $"${subscription.Price}", 1, 1);
        
        // 下次續訂
        AddDetailLabel(detailsGrid, "下次續訂：", 2, 1);
        var nextDateColor = subscription.IsOverdue ? "#E74C3C" : (subscription.IsDueSoon ? "#F39C12" : "#27AE60");
        AddDetailValue(detailsGrid, subscription.NextDate.ToString("yyyy-MM-dd"), 3, 1, highlight: true, color: nextDateColor);
        
        // 剩餘天數
        AddDetailLabel(detailsGrid, "剩餘天數：", 0, 2);
        var daysText = subscription.IsOverdue ? $"逾期 {Math.Abs(subscription.DaysUntilNext)} 天" : $"{subscription.DaysUntilNext} 天";
        AddDetailValue(detailsGrid, daysText, 1, 2, highlight: true, color: nextDateColor);
        
        // 備註
        if (!string.IsNullOrWhiteSpace(subscription.Note))
        {
            AddDetailLabel(detailsGrid, "備註：", 0, 3);
            var noteText = new TextBlock
            {
                Text = subscription.Note,
                Margin = new Avalonia.Thickness(0, 5, 20, 5),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Foreground = new SolidColorBrush(Color.Parse("#7F8C8D"))
            };
            Grid.SetColumn(noteText, 1);
            Grid.SetRow(noteText, 3);
            Grid.SetColumnSpan(noteText, 3);
            detailsGrid.Children.Add(noteText);
        }
        
        infoStack.Children.Add(detailsGrid);
        
        Grid.SetColumn(infoStack, 0);
        grid.Children.Add(infoStack);
        
        // 右側按鈕
        var buttonStack = new StackPanel
        {
            Spacing = 10,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };
        
        var editButton = new Button
        {
            Content = "編輯",
            Background = new SolidColorBrush(Color.Parse("#3498DB")),
            Foreground = new SolidColorBrush(Colors.White),
            Padding = new Avalonia.Thickness(20, 8),
            Tag = subscription
        };
        editButton.Click += OnEditSubscriptionClick;
        buttonStack.Children.Add(editButton);
        
        var deleteButton = new Button
        {
            Content = "刪除",
            Background = new SolidColorBrush(Color.Parse("#E74C3C")),
            Foreground = new SolidColorBrush(Colors.White),
            Padding = new Avalonia.Thickness(20, 8),
            Tag = subscription
        };
        deleteButton.Click += OnDeleteSubscriptionClick;
        buttonStack.Children.Add(deleteButton);
        
        Grid.SetColumn(buttonStack, 1);
        grid.Children.Add(buttonStack);
        
        border.Child = grid;
        return border;
    }
    
    private Border CreateStatusBadge(string text, string color)
    {
        return new Border
        {
            Background = new SolidColorBrush(Color.Parse(color)),
            CornerRadius = new Avalonia.CornerRadius(3),
            Padding = new Avalonia.Thickness(8, 4),
            Child = new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 12,
                FontWeight = FontWeight.Bold
            }
        };
    }
    
    private void AddDetailLabel(Grid grid, string text, int column, int row)
    {
        var label = new TextBlock
        {
            Text = text,
            FontWeight = FontWeight.SemiBold,
            Margin = new Avalonia.Thickness(0, 5, 10, 5),
            Foreground = new SolidColorBrush(Color.Parse("#7F8C8D"))
        };
        Grid.SetColumn(label, column);
        Grid.SetRow(label, row);
        grid.Children.Add(label);
    }
    
    private void AddDetailValue(Grid grid, string text, int column, int row, bool highlight = false, bool isLink = false, string? color = null)
    {
        var value = new TextBlock
        {
            Text = text,
            Margin = new Avalonia.Thickness(0, 5, 20, 5),
            FontWeight = highlight ? FontWeight.Bold : FontWeight.Normal,
            Foreground = color != null 
                ? new SolidColorBrush(Color.Parse(color))
                : (isLink ? new SolidColorBrush(Color.Parse("#3498DB")) : new SolidColorBrush(Colors.Black)),
            TextDecorations = isLink ? Avalonia.Media.TextDecorations.Underline : null,
            Cursor = isLink ? new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand) : null
        };
        Grid.SetColumn(value, column);
        Grid.SetRow(value, row);
        grid.Children.Add(value);
    }
    
    /// <summary>
    /// 新增訂閱
    /// </summary>
    private async void OnAddSubscriptionClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new SubscriptionEditDialog();
        var result = await dialog.ShowDialog<Subscription?>(GetParentWindow());
        
        if (result != null)
        {
            try
            {
                await _subscriptionService.CreateSubscriptionAsync(result);
                await LoadSubscriptionsAsync();
            }
            catch (Exception ex)
            {
                ShowError($"新增失敗：{ex.Message}");
            }
        }
    }
    
    /// <summary>
    /// 編輯訂閱
    /// </summary>
    private async void OnEditSubscriptionClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Subscription subscription)
        {
            var dialog = new SubscriptionEditDialog(subscription);
            var result = await dialog.ShowDialog<Subscription?>(GetParentWindow());
            
            if (result != null)
            {
                try
                {
                    await _subscriptionService.UpdateSubscriptionAsync(result);
                    await LoadSubscriptionsAsync();
                }
                catch (Exception ex)
                {
                    ShowError($"更新失敗：{ex.Message}");
                }
            }
        }
    }
    
    /// <summary>
    /// 刪除訂閱
    /// </summary>
    private async void OnDeleteSubscriptionClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Subscription subscription)
        {
            var confirmDialog = new Window
            {
                Title = "確認刪除",
                Width = 350,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            
            var stack = new StackPanel
            {
                Margin = new Avalonia.Thickness(20)
            };
            
            stack.Children.Add(new TextBlock
            {
                Text = $"確定要刪除「{subscription.Name}」嗎？",
                FontSize = 14,
                Margin = new Avalonia.Thickness(0, 0, 0, 20)
            });
            
            var buttonPanel = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Spacing = 10
            };
            
            var cancelButton = new Button
            {
                Content = "取消",
                Padding = new Avalonia.Thickness(20, 8)
            };
            cancelButton.Click += (s, args) => confirmDialog.Close(false);
            
            var confirmButton = new Button
            {
                Content = "確定刪除",
                Background = new SolidColorBrush(Color.Parse("#E74C3C")),
                Foreground = new SolidColorBrush(Colors.White),
                Padding = new Avalonia.Thickness(20, 8)
            };
            confirmButton.Click += (s, args) => confirmDialog.Close(true);
            
            buttonPanel.Children.Add(cancelButton);
            buttonPanel.Children.Add(confirmButton);
            stack.Children.Add(buttonPanel);
            
            confirmDialog.Content = stack;
            
            var confirmed = await confirmDialog.ShowDialog<bool>(GetParentWindow());
            
            if (confirmed)
            {
                try
                {
                    await _subscriptionService.DeleteSubscriptionAsync(subscription.Id);
                    await LoadSubscriptionsAsync();
                }
                catch (Exception ex)
                {
                    ShowError($"刪除失敗：{ex.Message}");
                }
            }
        }
    }
    
    /// <summary>
    /// 重新整理
    /// </summary>
    private async void OnRefreshClick(object? sender, RoutedEventArgs e)
    {
        await LoadSubscriptionsAsync();
    }
    
    /// <summary>
    /// 搜尋文字變更
    /// </summary>
    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var searchText = textBox.Text?.ToLower() ?? "";
            
            if (string.IsNullOrWhiteSpace(searchText))
            {
                DisplaySubscriptions(_allSubscriptions);
            }
            else
            {
                var filtered = _allSubscriptions.Where(s => 
                    s.Name.ToLower().Contains(searchText) || 
                    s.Site.ToLower().Contains(searchText) ||
                    s.Account.ToLower().Contains(searchText)
                ).ToList();
                
                DisplaySubscriptions(filtered);
            }
        }
    }
    
    private Window GetParentWindow()
    {
        var parent = this.Parent;
        while (parent != null && parent is not Window)
        {
            parent = parent.Parent;
        }
        return (Window?)parent ?? throw new InvalidOperationException("無法找到父視窗");
    }
    
    private void ShowError(string message)
    {
        System.Diagnostics.Debug.WriteLine($"錯誤：{message}");
    }
}
