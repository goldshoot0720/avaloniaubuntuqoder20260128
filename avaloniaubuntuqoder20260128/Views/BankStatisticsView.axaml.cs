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

public partial class BankStatisticsView : UserControl
{
    private readonly BankService _bankService;
    private List<Bank> _allBanks = new();
    
    public BankStatisticsView()
    {
        InitializeComponent();
        
        var appwriteService = new AppwriteService();
        _bankService = new BankService(appwriteService);
        
        // 載入資料
        _ = LoadBanksAsync();
    }
    
    /// <summary>
    /// 載入所有銀行資料
    /// </summary>
    private async Task LoadBanksAsync()
    {
        try
        {
            _allBanks = await _bankService.GetAllBanksAsync();
            DisplayBanks(_allBanks);
            UpdateStatistics(_allBanks);
        }
        catch (Exception ex)
        {
            ShowError($"載入失敗：{ex.Message}");
        }
    }
    
    /// <summary>
    /// 更新統計資訊
    /// </summary>
    private void UpdateStatistics(List<Bank> banks)
    {
        var totalDeposit = banks.Sum(b => b.Deposit);
        var totalWithdrawals = banks.Sum(b => b.Withdrawals);
        var totalTransfer = banks.Sum(b => b.Transfer);
        var netBalance = totalDeposit - totalWithdrawals - totalTransfer;
        
        var totalDepositText = this.FindControl<TextBlock>("TotalDepositText");
        var totalWithdrawalsText = this.FindControl<TextBlock>("TotalWithdrawalsText");
        var totalTransferText = this.FindControl<TextBlock>("TotalTransferText");
        var netBalanceText = this.FindControl<TextBlock>("NetBalanceText");
        
        if (totalDepositText != null) totalDepositText.Text = $"${totalDeposit:N0}";
        if (totalWithdrawalsText != null) totalWithdrawalsText.Text = $"${totalWithdrawals:N0}";
        if (totalTransferText != null) totalTransferText.Text = $"${totalTransfer:N0}";
        if (netBalanceText != null) netBalanceText.Text = $"${netBalance:N0}";
    }
    
    /// <summary>
    /// 顯示銀行清單
    /// </summary>
    private void DisplayBanks(List<Bank> banks)
    {
        var panel = this.FindControl<StackPanel>("BankListPanel");
        if (panel == null) return;
        
        panel.Children.Clear();
        
        if (banks.Count == 0)
        {
            var emptyText = new TextBlock
            {
                Text = "目前沒有銀行資料",
                FontSize = 16,
                Foreground = new SolidColorBrush(Colors.Gray),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 50, 0, 0)
            };
            panel.Children.Add(emptyText);
            return;
        }
        
        foreach (var bank in banks)
        {
            var card = CreateBankCard(bank);
            panel.Children.Add(card);
        }
    }
    
    /// <summary>
    /// 建立銀行卡片
    /// </summary>
    private Border CreateBankCard(Bank bank)
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
        
        var mainGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto")
        };
        
        // 左側資訊
        var infoStack = new StackPanel();
        
        // 銀行名稱
        var nameText = new TextBlock
        {
            Text = bank.Name,
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            Margin = new Avalonia.Thickness(0, 0, 0, 10)
        };
        infoStack.Children.Add(nameText);
        
        // 詳細資訊網格
        var detailsGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto,*"),
            RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto,Auto")
        };
        
        int row = 0;
        
        // 帳號
        AddDetailLabel(detailsGrid, "帳號：", 0, row);
        AddDetailValue(detailsGrid, bank.Account, 1, row);
        
        // 卡片
        AddDetailLabel(detailsGrid, "卡片：", 2, row);
        AddDetailValue(detailsGrid, bank.Card, 3, row);
        row++;
        
        // 存款
        AddDetailLabel(detailsGrid, "存款：", 0, row);
        AddDetailValue(detailsGrid, $"${bank.Deposit:N0}", 1, row, color: "#27AE60");
        
        // 提款
        AddDetailLabel(detailsGrid, "提款：", 2, row);
        AddDetailValue(detailsGrid, $"${bank.Withdrawals:N0}", 3, row, color: "#E74C3C");
        row++;
        
        // 轉帳
        AddDetailLabel(detailsGrid, "轉帳：", 0, row);
        AddDetailValue(detailsGrid, $"${bank.Transfer:N0}", 1, row, color: "#F39C12");
        
        // 淨餘額
        AddDetailLabel(detailsGrid, "淨餘額：", 2, row);
        var balanceColor = bank.NetBalance >= 0 ? "#27AE60" : "#E74C3C";
        AddDetailValue(detailsGrid, $"${bank.NetBalance:N0}", 3, row, highlight: true, color: balanceColor);
        row++;
        
        // 地址
        if (!string.IsNullOrWhiteSpace(bank.Address))
        {
            AddDetailLabel(detailsGrid, "地址：", 0, row);
            var addressText = new TextBlock
            {
                Text = bank.Address,
                Margin = new Avalonia.Thickness(0, 5, 20, 5),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            };
            Grid.SetColumn(addressText, 1);
            Grid.SetRow(addressText, row);
            Grid.SetColumnSpan(addressText, 3);
            detailsGrid.Children.Add(addressText);
            row++;
        }
        
        // 網站和活動連結
        var linksPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Spacing = 15,
            Margin = new Avalonia.Thickness(0, 5, 0, 5)
        };
        
        if (!string.IsNullOrWhiteSpace(bank.Site))
        {
            var siteText = new TextBlock
            {
                Text = "🌐 官網",
                Foreground = new SolidColorBrush(Color.Parse("#3498DB")),
                TextDecorations = Avalonia.Media.TextDecorations.Underline,
                Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
            };
            linksPanel.Children.Add(siteText);
        }
        
        if (!string.IsNullOrWhiteSpace(bank.Activity))
        {
            var activityText = new TextBlock
            {
                Text = "🎁 活動",
                Foreground = new SolidColorBrush(Color.Parse("#E67E22")),
                TextDecorations = Avalonia.Media.TextDecorations.Underline,
                Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
            };
            linksPanel.Children.Add(activityText);
        }
        
        if (linksPanel.Children.Count > 0)
        {
            Grid.SetColumn(linksPanel, 0);
            Grid.SetRow(linksPanel, row);
            Grid.SetColumnSpan(linksPanel, 4);
            detailsGrid.Children.Add(linksPanel);
        }
        
        infoStack.Children.Add(detailsGrid);
        
        Grid.SetColumn(infoStack, 0);
        mainGrid.Children.Add(infoStack);
        
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
            Tag = bank
        };
        editButton.Click += OnEditBankClick;
        buttonStack.Children.Add(editButton);
        
        var deleteButton = new Button
        {
            Content = "刪除",
            Background = new SolidColorBrush(Color.Parse("#E74C3C")),
            Foreground = new SolidColorBrush(Colors.White),
            Padding = new Avalonia.Thickness(20, 8),
            Tag = bank
        };
        deleteButton.Click += OnDeleteBankClick;
        buttonStack.Children.Add(deleteButton);
        
        Grid.SetColumn(buttonStack, 1);
        mainGrid.Children.Add(buttonStack);
        
        border.Child = mainGrid;
        return border;
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
    
    private void AddDetailValue(Grid grid, string text, int column, int row, bool highlight = false, string? color = null)
    {
        var value = new TextBlock
        {
            Text = text,
            Margin = new Avalonia.Thickness(0, 5, 20, 5),
            FontWeight = highlight ? FontWeight.Bold : FontWeight.Normal,
            Foreground = color != null 
                ? new SolidColorBrush(Color.Parse(color))
                : new SolidColorBrush(Colors.Black)
        };
        Grid.SetColumn(value, column);
        Grid.SetRow(value, row);
        grid.Children.Add(value);
    }
    
    /// <summary>
    /// 新增銀行
    /// </summary>
    private async void OnAddBankClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new BankEditDialog();
        var result = await dialog.ShowDialog<Bank?>(GetParentWindow());
        
        if (result != null)
        {
            try
            {
                await _bankService.CreateBankAsync(result);
                await LoadBanksAsync();
            }
            catch (Exception ex)
            {
                ShowError($"新增失敗：{ex.Message}");
            }
        }
    }
    
    /// <summary>
    /// 編輯銀行
    /// </summary>
    private async void OnEditBankClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Bank bank)
        {
            var dialog = new BankEditDialog(bank);
            var result = await dialog.ShowDialog<Bank?>(GetParentWindow());
            
            if (result != null)
            {
                try
                {
                    await _bankService.UpdateBankAsync(result);
                    await LoadBanksAsync();
                }
                catch (Exception ex)
                {
                    ShowError($"更新失敗：{ex.Message}");
                }
            }
        }
    }
    
    /// <summary>
    /// 刪除銀行
    /// </summary>
    private async void OnDeleteBankClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Bank bank)
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
                Text = $"確定要刪除「{bank.Name}」嗎？",
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
                    await _bankService.DeleteBankAsync(bank.Id);
                    await LoadBanksAsync();
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
        await LoadBanksAsync();
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
                DisplayBanks(_allBanks);
                UpdateStatistics(_allBanks);
            }
            else
            {
                var filtered = _allBanks.Where(b => 
                    b.Name.ToLower().Contains(searchText) || 
                    b.Account.ToLower().Contains(searchText) ||
                    b.Card.ToLower().Contains(searchText)
                ).ToList();
                
                DisplayBanks(filtered);
                UpdateStatistics(filtered);
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
