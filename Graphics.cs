using BgfxEngine.Platforms.MacOS;
using Silk.NET.Windowing;

namespace BgfxEngine;

public unsafe class Graphics
{
    private IWindow _window;
    private CAMetalLayer _metalLayer;
    
    public Graphics(IWindow window)
    {
        _window = window;
    }

    public void Initialize()
    {
        IntPtr windowHandler = _window.Handle;
        
        if(_window.Native?.Cocoa != null)
        {
            _metalLayer = CAMetalLayer.New();
            MetalUtil.LinkMetalLayer(_window.Native.Cocoa.Value, _metalLayer.NativePtr);
            
            windowHandler = _metalLayer.NativePtr;
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
                height = (ushort)_window.Size.Y,
                reset = (uint)bgfx.ResetFlags.Vsync,
                format = bgfx.TextureFormat.BGRA8
            },
            deviceId = 0,
            debug = 1,
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
            _window.Resize += (_) => { Console.WriteLine("Resized"); };
            _window.Move += (_) => { ResetWindow(); };
        }
    }

    private void ResetWindow()
    {
        if(_window.Native?.Cocoa != null)
        {
            MetalUtil.LinkMetalLayer(_window.Native.Cocoa.Value, _metalLayer.NativePtr);
        }
        
        bgfx.reset((ushort)_window.Size.X, (ushort)_window.Size.Y, (uint)bgfx.ResetFlags.Vsync, bgfx.TextureFormat.BGRA8);
    }

    private void Render(double deltaTime)
    {
        bgfx.set_view_rect(0, 0, 0, (ushort)_window.Size.X, (ushort)_window.Size.Y);
        ushort clearFlags = (ushort)(bgfx.ClearFlags.Color | bgfx.ClearFlags.Depth);
        uint clearColor = 0x803030ff;
        bgfx.set_view_clear(0, clearFlags, clearColor, 1.0f, 0);
        bgfx.touch(0);
        bgfx.frame(false);
    }
}