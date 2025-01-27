namespace BgfxEngine.Platforms.MacOS;

public class MetalUtil
{
    public static void LinkMetalLayer(IntPtr cocoaHandle, IntPtr metalLayerHandle)
    {
        NSWindow nsWindow = new NSWindow(cocoaHandle);
        NSView contentView = nsWindow.contentView;
        contentView.wantsLayer = true;
        contentView.layer = metalLayerHandle;
    }
}