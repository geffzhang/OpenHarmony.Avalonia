using Avalonia.Controls.Embedding;
using OpenHarmony.Sdk.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.OpenHarmony;

public class AvaloniaXComponent<TApp> : XComponent where TApp : Application, new()
{
    public SingleViewLifetime SingleViewLifetime;
    public EmbeddableControlRoot Root;
    public TopLevelImpl TopLevelImpl;
    public AvaloniaXComponent(nint XComponentHandle, nint WindowHandle) : base(XComponentHandle, WindowHandle)
    {

    }

    public override void OnSurfaceCreated()
    {
        Hilog.OH_LOG_INFO(LogType.LOG_APP, "CSharp", "OnSurfaceCreated Enter");
        base.OnSurfaceCreated();
        var builder = CreateAppBuilder();
        SingleViewLifetime = new SingleViewLifetime();
        builder.AfterApplicationSetup(CreateView).SetupWithLifetime(SingleViewLifetime);
        Root.StartRendering();
        Hilog.OH_LOG_INFO(LogType.LOG_APP, "CSharp", "OnSurfaceCreated Exit");
    }

    public override void OnSurfaceRendered(ulong timestamp, ulong targetTimestamp)
    {
        base.OnSurfaceRendered(timestamp, targetTimestamp);
        OpenHarmonyRenderTimer.Instance.Render();
        TopLevelImpl.Paint.Invoke(new Rect(GetSize()));
    }
    private void CreateView(AppBuilder appBuilder)
    {
        TopLevelImpl = new TopLevelImpl(XComponentHandle, WindowHandle);
        Root = new EmbeddableControlRoot(TopLevelImpl);
        SingleViewLifetime.Root = Root;
        Root.Prepare();

    }

    private AppBuilder CreateAppBuilder() => AppBuilder.Configure<TApp>().UseOpenHarmony();

}
