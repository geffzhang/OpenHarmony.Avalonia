using Avalonia;
using Avalonia.OpenHarmony;
using AvaloniaExample;
using OpenHarmony.Sdk.Native;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Entry;

public unsafe static class XComponentEntry
{
    public static Dictionary<IntPtr, XComponent> XComponents = [];
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void OnSurfaceCreated(OH_NativeXComponent* component, void* window)
    {
        Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "OnSurfaceCreated111");
        ace_ndk.OH_NativeXComponent_RegisterOnFrameCallback(component, &OnSurfaceRendered);

        if (XComponents.TryGetValue((nint)component, out XComponent? xComponent) == true)
            return;
        Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "OnSurfaceCreated222");
        xComponent = new XComponent((nint)component, (nint)window);
        XComponents.Add((nint)component, xComponent);
        xComponent.OnSurfaceCreated();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void OnSurfaceRendered(OH_NativeXComponent* component, ulong timestamp, ulong targetTimestamp)
    {
        if (XComponents.TryGetValue((nint)component, out XComponent? xComponent) == false)
            return;
        xComponent.OnSurfaceRendered(timestamp, targetTimestamp);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void OnSurfaceChanged(OH_NativeXComponent* component, void* window)
    {
        if (XComponents.TryGetValue((nint)component, out XComponent? xComponent) == false)
            return;
        xComponent.OnSurfaceChanged();
    }
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void OnSurfaceDestroyed(OH_NativeXComponent* component, void* window)
    {
        if (XComponents.TryGetValue((nint)component, out XComponent? xComponent) == false)
            return;
        xComponent.OnSurfaceDestroyed();
        XComponents.Remove((nint)component);

    }
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void DispatchTouchEvent(OH_NativeXComponent* component, void* window)
    {
        if (XComponents.TryGetValue((nint)component, out XComponent? xComponent) == false)
            return;
        xComponent.DispatchTouchEvent();
    }
}
