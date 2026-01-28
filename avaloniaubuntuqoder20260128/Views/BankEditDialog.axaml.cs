using Avalonia.Controls;
using Avalonia.Interactivity;
using avaloniaubuntuqoder20260128.Models;
using System;

namespace avaloniaubuntuqoder20260128.Views;

public partial class BankEditDialog : Window
{
    private readonly Bank? _editingBank;
    
    public BankEditDialog() : this(null)
    {
    }
    
    public BankEditDialog(Bank? bank)
    {
        InitializeComponent();
        _editingBank = bank;
        
        if (bank != null)
        {
            // 編輯模式 - 填入現有資料
            Title = "編輯銀行";
            LoadBankData(bank);
        }
        else
        {
            // 新增模式
            Title = "新增銀行";
        }
    }
    
    private void LoadBankData(Bank bank)
    {
        var nameTextBox = this.FindControl<TextBox>("NameTextBox");
        var accountTextBox = this.FindControl<TextBox>("AccountTextBox");
        var cardTextBox = this.FindControl<TextBox>("CardTextBox");
        var depositNumeric = this.FindControl<NumericUpDown>("DepositNumeric");
        var withdrawalsNumeric = this.FindControl<NumericUpDown>("WithdrawalsNumeric");
        var transferNumeric = this.FindControl<NumericUpDown>("TransferNumeric");
        var siteTextBox = this.FindControl<TextBox>("SiteTextBox");
        var activityTextBox = this.FindControl<TextBox>("ActivityTextBox");
        var addressTextBox = this.FindControl<TextBox>("AddressTextBox");
        
        if (nameTextBox != null) nameTextBox.Text = bank.Name;
        if (accountTextBox != null) accountTextBox.Text = bank.Account;
        if (cardTextBox != null) cardTextBox.Text = bank.Card;
        if (depositNumeric != null) depositNumeric.Value = bank.Deposit;
        if (withdrawalsNumeric != null) withdrawalsNumeric.Value = bank.Withdrawals;
        if (transferNumeric != null) transferNumeric.Value = bank.Transfer;
        if (siteTextBox != null) siteTextBox.Text = bank.Site;
        if (activityTextBox != null) activityTextBox.Text = bank.Activity;
        if (addressTextBox != null) addressTextBox.Text = bank.Address;
    }
    
    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }
    
    private void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        var nameTextBox = this.FindControl<TextBox>("NameTextBox");
        var accountTextBox = this.FindControl<TextBox>("AccountTextBox");
        var cardTextBox = this.FindControl<TextBox>("CardTextBox");
        var depositNumeric = this.FindControl<NumericUpDown>("DepositNumeric");
        var withdrawalsNumeric = this.FindControl<NumericUpDown>("WithdrawalsNumeric");
        var transferNumeric = this.FindControl<NumericUpDown>("TransferNumeric");
        var siteTextBox = this.FindControl<TextBox>("SiteTextBox");
        var activityTextBox = this.FindControl<TextBox>("ActivityTextBox");
        var addressTextBox = this.FindControl<TextBox>("AddressTextBox");
        
        // 驗證
        if (string.IsNullOrWhiteSpace(nameTextBox?.Text))
        {
            ShowError("請輸入銀行名稱");
            return;
        }
        
        // 建立或更新 Bank 物件
        var bank = new Bank
        {
            Id = _editingBank?.Id ?? string.Empty,
            Name = nameTextBox!.Text!,
            Account = accountTextBox?.Text ?? string.Empty,
            Card = cardTextBox?.Text ?? string.Empty,
            Deposit = (int)(depositNumeric?.Value ?? 0),
            Withdrawals = (int)(withdrawalsNumeric?.Value ?? 0),
            Transfer = (int)(transferNumeric?.Value ?? 0),
            Site = siteTextBox?.Text ?? string.Empty,
            Activity = activityTextBox?.Text ?? string.Empty,
            Address = addressTextBox?.Text ?? string.Empty
        };
        
        Close(bank);
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
