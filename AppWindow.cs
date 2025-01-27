using Silk.NET.Core.Contexts;
using Silk.NET.Windowing;
using Monitor = Silk.NET.Windowing.Monitor;

namespace BgfxEngine;

public class AppWindow
{
    private IWindow _window;
    
    public Action<IWindow> OnLoad { get; set; }
    
    public IWindow SilkWindow => _window;
    public INativeWindow NativeWindow => _window.Native!;
    
    public AppWindow()
    {
        WindowOptions options = WindowOptions.Default;
        options.Title = "BGFX Engine";
        options.Size = new(1280, 720);
        options.VSync = true;
        
        _window = Window.Create(options);
        _window.Load += () => OnLoad?.Invoke(_window);
        
        _window.Initialize();
        
        _window.Monitor = Monitor.GetMonitors(null).Last();
        _window.Position = _window.Monitor.Bounds.Center - _window.Size / 2;
    }

    public void Run()
    {
        _window.Run();
    }
}