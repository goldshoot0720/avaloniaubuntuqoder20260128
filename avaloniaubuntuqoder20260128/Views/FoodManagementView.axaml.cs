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

public partial class FoodManagementView : UserControl
{
    private readonly FoodService _foodService;
    private List<Food> _allFoods = new();
    
    public FoodManagementView()
    {
        InitializeComponent();
        
        var appwriteService = new AppwriteService();
        _foodService = new FoodService(appwriteService);
        
        // 載入資料
        _ = LoadFoodsAsync();
    }
    
    /// <summary>
    /// 載入所有食品資料
    /// </summary>
    private async Task LoadFoodsAsync()
    {
        try
        {
            _allFoods = await _foodService.GetAllFoodsAsync();
            DisplayFoods(_allFoods);
        }
        catch (Exception ex)
        {
            ShowError($"載入失敗：{ex.Message}");
        }
    }
    
    /// <summary>
    /// 顯示食品清單
    /// </summary>
    private void DisplayFoods(List<Food> foods)
    {
        var panel = this.FindControl<StackPanel>("FoodListPanel");
        if (panel == null) return;
        
        panel.Children.Clear();
        
        if (foods.Count == 0)
        {
            var emptyText = new TextBlock
            {
                Text = "目前沒有食品資料",
                FontSize = 16,
                Foreground = new SolidColorBrush(Colors.Gray),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 50, 0, 0)
            };
            panel.Children.Add(emptyText);
            return;
        }
        
        foreach (var food in foods)
        {
            var card = CreateFoodCard(food);
            panel.Children.Add(card);
        }
    }
    
    /// <summary>
    /// 建立食品卡片
    /// </summary>
    private Border CreateFoodCard(Food food)
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
        
        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto")
        };
        
        // 左側資訊
        var infoStack = new StackPanel();
        
        var nameText = new TextBlock
        {
            Text = food.Name,
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            Margin = new Avalonia.Thickness(0, 0, 0, 10)
        };
        infoStack.Children.Add(nameText);
        
        var detailsGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto,*"),
            RowDefinitions = new RowDefinitions("Auto,Auto,Auto")
        };
        
        // 商店
        AddDetailLabel(detailsGrid, "商店：", 0, 0);
        AddDetailValue(detailsGrid, food.Shop, 1, 0);
        
        // 數量
        AddDetailLabel(detailsGrid, "數量：", 0, 1);
        AddDetailValue(detailsGrid, food.Amount.ToString(), 1, 1);
        
        // 單價
        AddDetailLabel(detailsGrid, "單價：", 2, 0);
        AddDetailValue(detailsGrid, $"${food.Price}", 3, 0);
        
        // 總價
        AddDetailLabel(detailsGrid, "總價：", 2, 1);
        AddDetailValue(detailsGrid, $"${food.TotalPrice}", 3, 1, true);
        
        // 日期
        AddDetailLabel(detailsGrid, "日期：", 0, 2);
        AddDetailValue(detailsGrid, food.ToDate.ToString("yyyy-MM-dd"), 1, 2);
        
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
            Tag = food
        };
        editButton.Click += OnEditFoodClick;
        buttonStack.Children.Add(editButton);
        
        var deleteButton = new Button
        {
            Content = "刪除",
            Background = new SolidColorBrush(Color.Parse("#E74C3C")),
            Foreground = new SolidColorBrush(Colors.White),
            Padding = new Avalonia.Thickness(20, 8),
            Tag = food
        };
        deleteButton.Click += OnDeleteFoodClick;
        buttonStack.Children.Add(deleteButton);
        
        Grid.SetColumn(buttonStack, 1);
        grid.Children.Add(buttonStack);
        
        border.Child = grid;
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
    
    private void AddDetailValue(Grid grid, string text, int column, int row, bool highlight = false)
    {
        var value = new TextBlock
        {
            Text = text,
            Margin = new Avalonia.Thickness(0, 5, 20, 5),
            FontWeight = highlight ? FontWeight.Bold : FontWeight.Normal,
            Foreground = highlight 
                ? new SolidColorBrush(Color.Parse("#27AE60")) 
                : new SolidColorBrush(Colors.Black)
        };
        Grid.SetColumn(value, column);
        Grid.SetRow(value, row);
        grid.Children.Add(value);
    }
    
    /// <summary>
    /// 新增食品
    /// </summary>
    private async void OnAddFoodClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new FoodEditDialog();
        var result = await dialog.ShowDialog<Food?>(GetParentWindow());
        
        if (result != null)
        {
            try
            {
                await _foodService.CreateFoodAsync(result);
                await LoadFoodsAsync();
            }
            catch (Exception ex)
            {
                ShowError($"新增失敗：{ex.Message}");
            }
        }
    }
    
    /// <summary>
    /// 編輯食品
    /// </summary>
    private async void OnEditFoodClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Food food)
        {
            var dialog = new FoodEditDialog(food);
            var result = await dialog.ShowDialog<Food?>(GetParentWindow());
            
            if (result != null)
            {
                try
                {
                    await _foodService.UpdateFoodAsync(result);
                    await LoadFoodsAsync();
                }
                catch (Exception ex)
                {
                    ShowError($"更新失敗：{ex.Message}");
                }
            }
        }
    }
    
    /// <summary>
    /// 刪除食品
    /// </summary>
    private async void OnDeleteFoodClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Food food)
        {
            // 確認對話框
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
                Text = $"確定要刪除「{food.Name}」嗎？",
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
                    await _foodService.DeleteFoodAsync(food.Id);
                    await LoadFoodsAsync();
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
        await LoadFoodsAsync();
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
                DisplayFoods(_allFoods);
            }
            else
            {
                var filtered = _allFoods.Where(f => 
                    f.Name.ToLower().Contains(searchText) || 
                    f.Shop.ToLower().Contains(searchText)
                ).ToList();
                
                DisplayFoods(filtered);
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
        // 簡單的錯誤顯示（實際專案中可以使用更好的對話框）
        System.Diagnostics.Debug.WriteLine($"錯誤：{message}");
    }
}
