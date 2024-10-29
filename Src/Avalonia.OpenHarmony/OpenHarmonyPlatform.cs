using Avalonia.Input.Platform;
using Avalonia.OpenGL.Egl;
using Avalonia.Platform;
using Avalonia.Rendering.Composition;

namespace Avalonia.OpenHarmony;

public class OpenHarmonyPlatform
{
    public static Compositor? Compositor { get; private set; }

    public static void Initialize()
    {
        AvaloniaLocator.CurrentMutable.Bind<PlatformHotkeyConfiguration>().ToSingleton<PlatformHotkeyConfiguration>();
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
