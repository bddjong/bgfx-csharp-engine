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
        
        BGFX.PlatformData platformData = new BGFX.PlatformData()
        {
            nwh = (void*)windowHandler,
            ndt = (void*)IntPtr.Zero,
        };
        
        BGFX.Init bgfxInit = new BGFX.Init
        {
            platformData = platformData,
            type = BGFX.RendererType.Metal,
            resolution = new BGFX.Resolution()
            {
                width = (ushort)_window.Size.X,
                height = (ushort)_window.Size.Y,
                reset = (uint)BGFX.ResetFlags.Vsync,
                format = BGFX.TextureFormat.BGRA8
            },
            deviceId = 0,
            debug = 1,
        };

        if (!BGFX.init(&bgfxInit))
        {
            Console.WriteLine("Failed to initialize bgfx");
        }
        else
        {
            Console.WriteLine("BGFX Initialized successfully");
            _window.Closing += BGFX.shutdown;
            _window.Render += Render;
            _window.Resize += (_) => { ResetWindow(); };
            _window.Move += (_) => { ResetWindow(); };
        }
    }

    private void ResetWindow()
    {
        if(_window.Native?.Cocoa != null)
        {
            MetalUtil.LinkMetalLayer(_window.Native.Cocoa.Value, _metalLayer.NativePtr);
        }
        
        BGFX.reset((ushort)_window.Size.X, (ushort)_window.Size.Y, (uint)BGFX.ResetFlags.Vsync, BGFX.TextureFormat.BGRA8);
    }

    private void Render(double deltaTime)
    {
        BGFX.set_view_rect(0, 0, 0, (ushort)_window.Size.X, (ushort)_window.Size.Y);
        BGFX.set_view_clear(0, (ushort)(BGFX.ClearFlags.Color | BGFX.ClearFlags.Depth), 0x803030ff, 1.0f, 0);
        BGFX.touch(0);
        BGFX.frame(false);
    }

    private void CreateVertexLayout()
    {
        BGFX.VertexLayout vertexLayout;
    }
}