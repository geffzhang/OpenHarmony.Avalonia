using OpenHarmony.Sdk.Native;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AvaloniaEntry;

public unsafe static class XComponent
{
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void OnSurfaceCreated(OH_NativeXComponent* component, void* window)
    {
        ace_ndk.OH_NativeXComponent_RegisterOnFrameCallback(component, &OnSurfaceRendered);
        Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "OnSurfaceCreated");
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void OnSurfaceRendered(OH_NativeXComponent* component, ulong timestamp, ulong targetTimestamp)
    {
        Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "OnRender");
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void OnSurfaceChanged(OH_NativeXComponent* component, void* window)
    {

    }
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void OnSurfaceDestroyed(OH_NativeXComponent* component, void* window)
    {

    }
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void DispatchTouchEvent(OH_NativeXComponent* component, void* window)
    {

    }
}
