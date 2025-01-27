namespace BgfxEngine.Platforms.MacOS;

public class MetalNativeUtil
{
    public static IntPtr InitMetalLayer(IntPtr cocoaHandle)
    {
        CAMetalLayer metalLayer = CAMetalLayer.New();
        NSWindow nsWindow = new NSWindow(cocoaHandle);
        NSView contentView = nsWindow.contentView;
        contentView.wantsLayer = true;
        contentView.layer = metalLayer.NativePtr;

        return metalLayer.NativePtr;
    }
}