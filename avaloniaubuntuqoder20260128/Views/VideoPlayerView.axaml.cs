using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using LibVLCSharp.Shared;
using System;
using System.Diagnostics;

namespace avaloniaubuntuqoder20260128.Views;

public partial class VideoPlayerView : UserControl
{
    private LibVLC? _libVLC;
    private MediaPlayer? _mediaPlayer;
    private DispatcherTimer? _timer;
    private bool _isUserDraggingSlider = false;
    
    public event EventHandler? CloseRequested;
    
    public VideoPlayerView()
    {
        InitializeComponent();
        InitializeVLC();
        SetupEventHandlers();
    }
    
    private void InitializeVLC()
    {
        try
        {
            Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            
            var videoView = this.FindControl<LibVLCSharp.Avalonia.VideoView>("VideoView");
            if (videoView != null)
            {
                videoView.MediaPlayer = _mediaPlayer;
            }
            
            // Setup timer for progress updates
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _timer.Tick += Timer_Tick;
            
            Debug.WriteLine("VLC initialized successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to initialize VLC: {ex.Message}");
        }
    }
    
    private void SetupEventHandlers()
    {
        var progressSlider = this.FindControl<Slider>("ProgressSlider");
        var volumeSlider = this.FindControl<Slider>("VolumeSlider");
        
        if (progressSlider != null)
        {
            progressSlider.PointerPressed += (s, e) => _isUserDraggingSlider = true;
            progressSlider.PointerReleased += (s, e) =>
            {
                _isUserDraggingSlider = false;
                if (_mediaPlayer != null && _mediaPlayer.IsSeekable)
                {
                    _mediaPlayer.Position = (float)(progressSlider.Value / 100.0);
                }
            };
        }
        
        if (volumeSlider != null)
        {
            volumeSlider.PropertyChanged += (s, e) =>
            {
                if (e.Property.Name == nameof(Slider.Value) && _mediaPlayer != null)
                {
                    _mediaPlayer.Volume = (int)volumeSlider.Value;
                }
            };
        }
    }
    
    public void LoadMedia(string url, string title = "Video Player")
    {
        try
        {
            if (_mediaPlayer == null || _libVLC == null)
            {
                Debug.WriteLine("MediaPlayer not initialized");
                return;
            }
            
            var titleText = this.FindControl<TextBlock>("TitleText");
            if (titleText != null)
            {
                titleText.Text = title;
            }
            
            var media = new Media(_libVLC, new Uri(url));
            _mediaPlayer.Play(media);
            _timer?.Start();
            
            Debug.WriteLine($"Loading media: {url}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to load media: {ex.Message}");
        }
    }
    
    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_mediaPlayer == null || _isUserDraggingSlider) return;
        
        try
        {
            var progressSlider = this.FindControl<Slider>("ProgressSlider");
            var currentTimeText = this.FindControl<TextBlock>("CurrentTimeText");
            var durationText = this.FindControl<TextBlock>("DurationText");
            
            if (progressSlider != null && _mediaPlayer.Length > 0)
            {
                progressSlider.Value = _mediaPlayer.Position * 100;
            }
            
            if (currentTimeText != null)
            {
                currentTimeText.Text = TimeSpan.FromMilliseconds(_mediaPlayer.Time).ToString(@"mm\:ss");
            }
            
            if (durationText != null)
            {
                durationText.Text = TimeSpan.FromMilliseconds(_mediaPlayer.Length).ToString(@"mm\:ss");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Timer error: {ex.Message}");
        }
    }
    
    private void OnPlayClick(object? sender, RoutedEventArgs e)
    {
        _mediaPlayer?.Play();
    }
    
    private void OnPauseClick(object? sender, RoutedEventArgs e)
    {
        _mediaPlayer?.Pause();
    }
    
    private void OnStopClick(object? sender, RoutedEventArgs e)
    {
        _mediaPlayer?.Stop();
        _timer?.Stop();
        
        var progressSlider = this.FindControl<Slider>("ProgressSlider");
        var currentTimeText = this.FindControl<TextBlock>("CurrentTimeText");
        
        if (progressSlider != null)
        {
            progressSlider.Value = 0;
        }
        
        if (currentTimeText != null)
        {
            currentTimeText.Text = "00:00";
        }
    }
    
    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        Cleanup();
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
    
    private void Cleanup()
    {
        try
        {
            _timer?.Stop();
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            _libVLC?.Dispose();
            
            _timer = null;
            _mediaPlayer = null;
            _libVLC = null;
            
            Debug.WriteLine("VLC cleanup completed");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Cleanup error: {ex.Message}");
        }
    }
}
