using Avalonia.Controls.Platform;
using Avalonia.Input.Platform;
using Avalonia.Media;
using Avalonia.OpenGL.Egl;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Rendering.Composition;
using Avalonia.Skia;
using Avalonia.Threading;

namespace Avalonia.OpenHarmony;

public class OpenHarmonyPlatform
{
    public static Compositor? Compositor { get; private set; }
    public static OpenHarmonyPlatformThreading? OpenHarmonyPlatformThreading { get; private set; }
    public static void Initialize()
    {
        OpenHarmonyPlatformThreading = new OpenHarmonyPlatformThreading();
        AvaloniaLocator.CurrentMutable
            .Bind<PlatformHotkeyConfiguration>().ToSingleton<PlatformHotkeyConfiguration>()
            .Bind<IFontManagerImpl>().ToConstant(new CustomFontManagerImpl())
            .Bind<IRuntimePlatform>().ToSingleton<OpenHarmonyRuntimePlatform>()
            .Bind<IRenderTimer>().ToConstant(OpenHarmonyRenderTimer.Instance)
            .Bind<ICursorFactory>().ToSingleton<CursorFactory>()
            .Bind<IPlatformThreadingInterface>().ToConstant(OpenHarmonyPlatformThreading);

        var platformGraphics = InitializeGraphics();
        if (platformGraphics is not null)
        {
            AvaloniaLocator.CurrentMutable.Bind<IPlatformGraphics>().ToConstant(platformGraphics);
        }
        Compositor = new Compositor(platformGraphics);
        AvaloniaLocator.CurrentMutable.Bind<Compositor>().ToConstant(Compositor);
    }

    private static IPlatformGraphics? InitializeGraphics()
    {
        return EglPlatformGraphics.TryCreate(() =>
        {
            return new EglDisplay(new EglDisplayCreationOptions
            {
                Egl = new EglInterface("libEGL.so"),
                SupportsMultipleContexts = true,
                SupportsContextSharing = true
            });
        });
    }
}
