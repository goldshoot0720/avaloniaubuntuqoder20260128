using Avalonia.Controls;
using Avalonia.Interactivity;
using avaloniaubuntuqoder20260128.Models;
using System;

namespace avaloniaubuntuqoder20260128.Views;

public partial class FoodEditDialog : Window
{
    private readonly Food? _editingFood;
    
    public FoodEditDialog() : this(null)
    {
    }
    
    public FoodEditDialog(Food? food)
    {
        InitializeComponent();
        _editingFood = food;
        
        if (food != null)
        {
            // 編輯模式 - 填入現有資料
            Title = "編輯食品";
            LoadFoodData(food);
        }
        else
        {
            // 新增模式
            Title = "新增食品";
            var datePicker = this.FindControl<DatePicker>("DatePicker");
            if (datePicker != null)
            {
                datePicker.SelectedDate = DateTimeOffset.Now;
            }
        }
    }
    
    private void LoadFoodData(Food food)
    {
        var nameTextBox = this.FindControl<TextBox>("NameTextBox");
        var shopTextBox = this.FindControl<TextBox>("ShopTextBox");
        var amountNumeric = this.FindControl<NumericUpDown>("AmountNumeric");
        var priceNumeric = this.FindControl<NumericUpDown>("PriceNumeric");
        var datePicker = this.FindControl<DatePicker>("DatePicker");
        var photoTextBox = this.FindControl<TextBox>("PhotoTextBox");
        var photoHashTextBox = this.FindControl<TextBox>("PhotoHashTextBox");
        
        if (nameTextBox != null) nameTextBox.Text = food.Name;
        if (shopTextBox != null) shopTextBox.Text = food.Shop;
        if (amountNumeric != null) amountNumeric.Value = food.Amount;
        if (priceNumeric != null) priceNumeric.Value = food.Price;
        if (datePicker != null) datePicker.SelectedDate = new DateTimeOffset(food.ToDate);
        if (photoTextBox != null) photoTextBox.Text = food.Photo;
        if (photoHashTextBox != null) photoHashTextBox.Text = food.PhotoHash;
    }
    
    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }
    
    private void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        var nameTextBox = this.FindControl<TextBox>("NameTextBox");
        var shopTextBox = this.FindControl<TextBox>("ShopTextBox");
        var amountNumeric = this.FindControl<NumericUpDown>("AmountNumeric");
        var priceNumeric = this.FindControl<NumericUpDown>("PriceNumeric");
        var datePicker = this.FindControl<DatePicker>("DatePicker");
        var photoTextBox = this.FindControl<TextBox>("PhotoTextBox");
        var photoHashTextBox = this.FindControl<TextBox>("PhotoHashTextBox");
        
        // 驗證
        if (string.IsNullOrWhiteSpace(nameTextBox?.Text))
        {
            ShowError("請輸入食品名稱");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(shopTextBox?.Text))
        {
            ShowError("請輸入商店名稱");
            return;
        }
        
        if (datePicker?.SelectedDate == null)
        {
            ShowError("請選擇日期");
            return;
        }
        
        // 建立或更新 Food 物件
        var food = new Food
        {
            Id = _editingFood?.Id ?? string.Empty,
            Name = nameTextBox!.Text!,
            Shop = shopTextBox!.Text!,
            Amount = (int)(amountNumeric?.Value ?? 1),
            Price = (int)(priceNumeric?.Value ?? 0),
            ToDate = datePicker.SelectedDate.Value.DateTime,
            Photo = photoTextBox?.Text ?? string.Empty,
            PhotoHash = photoHashTextBox?.Text ?? string.Empty
        };
        
        Close(food);
    }
    
    private async void ShowError(string message)
    {
        var dialog = new Window
        {
            Title = "錯誤",
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        
        var stack = new StackPanel
        {
            Margin = new Avalonia.Thickness(20),
            Spacing = 20
        };
        
        stack.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        });
        
        var button = new Button
        {
            Content = "確定",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Padding = new Avalonia.Thickness(20, 8)
        };
        button.Click += (s, e) => dialog.Close();
        
        stack.Children.Add(button);
        dialog.Content = stack;
        
        await dialog.ShowDialog(this);
    }
}
