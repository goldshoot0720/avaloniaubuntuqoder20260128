using Avalonia.Controls;
using Avalonia.Interactivity;
using avaloniaubuntuqoder20260128.Models;
using System;

namespace avaloniaubuntuqoder20260128.Views;

public partial class SubscriptionEditDialog : Window
{
    private readonly Subscription? _editingSubscription;
    
    public SubscriptionEditDialog() : this(null)
    {
    }
    
    public SubscriptionEditDialog(Subscription? subscription)
    {
        InitializeComponent();
        _editingSubscription = subscription;
        
        if (subscription != null)
        {
            // 編輯模式 - 填入現有資料
            Title = "編輯訂閱";
            LoadSubscriptionData(subscription);
        }
        else
        {
            // 新增模式
            Title = "新增訂閱";
            var nextDatePicker = this.FindControl<DatePicker>("NextDatePicker");
            if (nextDatePicker != null)
            {
                // 預設下次續訂日期為一個月後
                nextDatePicker.SelectedDate = DateTimeOffset.Now.AddMonths(1);
            }
        }
    }
    
    private void LoadSubscriptionData(Subscription subscription)
    {
        var nameTextBox = this.FindControl<TextBox>("NameTextBox");
        var siteTextBox = this.FindControl<TextBox>("SiteTextBox");
        var accountTextBox = this.FindControl<TextBox>("AccountTextBox");
        var priceNumeric = this.FindControl<NumericUpDown>("PriceNumeric");
        var nextDatePicker = this.FindControl<DatePicker>("NextDatePicker");
        var noteTextBox = this.FindControl<TextBox>("NoteTextBox");
        
        if (nameTextBox != null) nameTextBox.Text = subscription.Name;
        if (siteTextBox != null) siteTextBox.Text = subscription.Site;
        if (accountTextBox != null) accountTextBox.Text = subscription.Account;
        if (priceNumeric != null) priceNumeric.Value = subscription.Price;
        if (nextDatePicker != null) nextDatePicker.SelectedDate = new DateTimeOffset(subscription.NextDate);
        if (noteTextBox != null) noteTextBox.Text = subscription.Note;
    }
    
    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }
    
    private void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        var nameTextBox = this.FindControl<TextBox>("NameTextBox");
        var siteTextBox = this.FindControl<TextBox>("SiteTextBox");
        var accountTextBox = this.FindControl<TextBox>("AccountTextBox");
        var priceNumeric = this.FindControl<NumericUpDown>("PriceNumeric");
        var nextDatePicker = this.FindControl<DatePicker>("NextDatePicker");
        var noteTextBox = this.FindControl<TextBox>("NoteTextBox");
        
        // 驗證
        if (string.IsNullOrWhiteSpace(nameTextBox?.Text))
        {
            ShowError("請輸入訂閱名稱");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(siteTextBox?.Text))
        {
            ShowError("請輸入網站");
            return;
        }
        
        if (nextDatePicker?.SelectedDate == null)
        {
            ShowError("請選擇下次續訂日期");
            return;
        }
        
        // 建立或更新 Subscription 物件
        var subscription = new Subscription
        {
            Id = _editingSubscription?.Id ?? string.Empty,
            Name = nameTextBox!.Text!,
            Site = siteTextBox!.Text!,
            Account = accountTextBox?.Text ?? string.Empty,
            Price = (int)(priceNumeric?.Value ?? 0),
            NextDate = nextDatePicker.SelectedDate.Value.DateTime,
            Note = noteTextBox?.Text ?? string.Empty
        };
        
        Close(subscription);
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
