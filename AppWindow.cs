using Silk.NET.Core.Contexts;
using Silk.NET.Windowing;

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
    }

    public void Run()
    {
        _window.Run();
    }
}