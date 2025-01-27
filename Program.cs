// See https://aka.ms/new-console-template for more information

namespace BgfxEngine;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("ENGINE: Start");
        
        Console.WriteLine("ENGINE: Instantiating AppWindow");
        var appWindow = new AppWindow();
        
        Console.WriteLine("ENGINE: Instantiating Graphics");
        var graphics = new Graphics(appWindow.SilkWindow);
        
        Console.WriteLine("ENGINE: Initializing Graphics");
        graphics.Initialize();
        
        Console.WriteLine("ENGINE: Running AppWindow");
        appWindow.Run();
    }
}