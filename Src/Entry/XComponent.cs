using Avalonia.OpenHarmony;
using Avalonia;
using AvaloniaExample;
using OpenHarmony.Sdk.Native;
using Avalonia.Controls.Embedding;

namespace Entry;

public class XComponent
{
    public IntPtr XComponentHandle {get; private set;}
    public IntPtr WindowHandle { get; private set; }
    public EmbeddableControlRoot Root { get; private set; }
    public TopLevelImpl TopLevelImpl { get; private set; }

    public XComponent(IntPtr XComponentHandle, IntPtr WindowHandle)
    {
        var rsa = System.Security.Cryptography.RSA.Create();


        this.XComponentHandle = XComponentHandle;
        this.WindowHandle = WindowHandle;
    }

    public unsafe Size GetSize()
    {
        ulong Width = 0;
        ulong Height = 0;
        ace_ndk.OH_NativeXComponent_GetXComponentSize((OH_NativeXComponent*)XComponentHandle, (void*)WindowHandle, &Width, &Height);
        return new Size(Width, Height); 
    }

    public void OnSurfaceCreated()
    {
        Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "OnSurfaceCreated");
        var SingleViewLifetime = new SingleViewLifetime();
        
        AppBuilder.Configure<App>().UseOpenHarmony().SetupWithLifetime(SingleViewLifetime);
        TopLevelImpl = new TopLevelImpl(XComponentHandle, WindowHandle);
        Root = new EmbeddableControlRoot(TopLevelImpl);
        SingleViewLifetime.Root = Root;
        Root.Prepare();
        Root.StartRendering();

    }

    public void OnSurfaceDestroyed() 
    { 
    }


    public void OnSurfaceRendered(ulong timestamp, ulong targetTimestamp)
    {
        TopLevelImpl.Paint(new Rect(GetSize()));
    }

    public void OnSurfaceChanged()
    {

    }

    public void DispatchTouchEvent()
    {

    }

}
