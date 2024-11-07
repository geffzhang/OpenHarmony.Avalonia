using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.OpenGL.Egl;
using Avalonia.OpenGL.Surfaces;
using Avalonia.Platform;
using Avalonia.Rendering.Composition;
using OpenHarmony.Sdk;
using OpenHarmony.Sdk.Native;

namespace Avalonia.OpenHarmony;

public class TopLevelImpl : ITopLevelImpl, EglGlPlatformSurface.IEglWindowGlPlatformSurfaceInfo
{
    public IntPtr Window {  get; private set; }
    public IntPtr XComponent { get; private set; }
    public nint Handle => Window;

    public PixelSize Size { get; private set; }

    public double Scaling { get; private set; }

    public IGlPlatformSurface _gl;
    public unsafe TopLevelImpl(IntPtr xcomponent, IntPtr window)
    {
        Window = window;
        XComponent = xcomponent;
        ulong width = 0, height = 0;
        ace_ndk.OH_NativeXComponent_GetXComponentSize((OH_NativeXComponent*)xcomponent, (void*)window, &width, &height);
        float density = 1;
        display_manager.OH_NativeDisplayManager_GetDefaultDisplayScaledDensity(&density);
        Size = new PixelSize((int)width, (int)height);
        Scaling = density;
        _gl = new EglGlPlatformSurface(this);
        Surfaces = [_gl];
    }


    public Size ClientSize => new Size(Size.Width, Size.Height) / Scaling;

    public Size? FrameSize => null;

    public double RenderScaling => Scaling;

    public IEnumerable<object> Surfaces { get; }

    public Action<RawInputEventArgs>? Input { get; set; }
    public Action<Rect>? Paint { get; set; }
    public Action<Size, WindowResizeReason>? Resized { get; set; }
    public Action<double>? ScalingChanged { get; set; }
    public Action<WindowTransparencyLevel>? TransparencyLevelChanged { get; set; }

    public Compositor Compositor => OpenHarmonyPlatform.Compositor ?? throw new InvalidOperationException("Android backend wasn't initialized. Make sure .UseAndroid() was executed.");

    public Action? Closed  { get; set; }
    public Action? LostFocus { get; set; }

    private WindowTransparencyLevel _transparencyLevel;
    public WindowTransparencyLevel TransparencyLevel 
    {
        get => _transparencyLevel;
        private set
        {
            if (_transparencyLevel != value)
            {
                _transparencyLevel = value;
                TransparencyLevelChanged?.Invoke(value);
            }
        }
    }

    public AcrylicPlatformCompensationLevels AcrylicCompensationLevels => new AcrylicPlatformCompensationLevels(1, 1, 1);


    public IInputRoot? InputRoot { get; private set; }
    public void SetInputRoot(IInputRoot inputRoot)
    {
        InputRoot = inputRoot;
    }

    public Point PointToClient(PixelPoint point)
    {
        return point.ToPoint(RenderScaling);
    }

    public PixelPoint PointToScreen(Point point)
    {
        return PixelPoint.FromPoint(point, RenderScaling);
    }

    public void SetCursor(ICursorImpl? cursor)
    {
    }

    public IPopupImpl? CreatePopup() => null;

    public void SetTransparencyLevelHint(IReadOnlyList<WindowTransparencyLevel> transparencyLevels)
    {
        // todo
        return;
    }

    public void SetFrameThemeVariant(PlatformThemeVariant themeVariant)
    {
        // todo
        return;
    }

    public object? TryGetFeature(Type featureType)
    {
        // todo
        return null;
    }

    public void Dispose()
    {
    }
}
