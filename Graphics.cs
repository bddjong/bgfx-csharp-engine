using BgfxEngine.Platforms.MacOS;
using Silk.NET.Windowing;

namespace BgfxEngine;

public unsafe class Graphics
{
    private IWindow _window;
    
    public Graphics(IWindow window)
    {
        _window = window;
    }

    public void Initialize()
    {
        IntPtr windowHandler = _window.Handle;
        
        if(_window.Native?.Cocoa != null)
        {
            windowHandler = MetalNativeUtil.InitMetalLayer(_window.Native.Cocoa.Value);
        }
        
        var platformData = new bgfx.PlatformData()
        {
            nwh = (void*)windowHandler,
            ndt = (void*)IntPtr.Zero,
        };
        
        bgfx.Init bgfxInit = new bgfx.Init
        {
            platformData = platformData,
            type = bgfx.RendererType.Metal,
            resolution = new bgfx.Resolution()
            {
                width = (ushort)_window.Size.X,
                height = (ushort)_window.Size.Y
            },
            deviceId = 0,
            debug = 1
        };


        if (!bgfx.init(&bgfxInit))
        {
            Console.WriteLine("Failed to initialize bgfx");
        }
        else
        {
            Console.WriteLine("BGFX Initialized successfully");
            _window.Closing += bgfx.shutdown;
            _window.Render += Render;
        }
    }

    private void Render(double deltaTime)
    {
        ushort clearFlags = (ushort)(bgfx.ClearFlags.Color | bgfx.ClearFlags.Depth);
        uint clearColor = 0x903030ff;
        bgfx.set_view_clear(0, clearFlags, clearColor, 1.0f, 0);
        bgfx.set_view_rect(0, 0, 0, (ushort)_window.Size.X, (ushort)_window.Size.Y);
        bgfx.touch(0);
        bgfx.frame(false);
    }
}