using Avalonia.Controls;
using Avalonia.Controls.Embedding;
using Avalonia.OpenGL.Egl;
using OpenHarmony.Sdk.Native;
using Silk.NET.OpenGLES;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Avalonia.OpenHarmony;

public class AvaloniaXComponent<TApp> : XComponent where TApp : Application, new()
{
    public SingleViewLifetime SingleViewLifetime;
    public EmbeddableControlRoot Root;
    public TopLevelImpl TopLevelImpl;

    public bool UseSoftRenderer = true;
    GL gl;
    nint display;
    nint surface;
    EglInterface egl;

    public AvaloniaXComponent(nint XComponentHandle, nint WindowHandle) : base(XComponentHandle, WindowHandle)
    {
    }

    public unsafe void InitOpenGlEnv()
    {
        egl = new EglInterface("libEGL.so");

        display = egl.GetDisplay(0);
        if (egl.Initialize(display, out var major, out var minor) == false)
        {
            Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "egl.Initialize fail");
            return;
        }
        int[] attributes = [0x3033, 0x0004, 0x3024, 8, 0x3023, 8, 0x3022, 8, 0x3021, 8, 0x3040, 0x0004, 0x3038];
        if (egl.ChooseConfig(display, attributes, out var configs, 1, out var choosenConfig) == false)
        {
            Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "egl.ChooseConfig fail");
            return;
        }
        int[] winAttribs = [0x309D, 0x3089, 0x3038];
        surface = egl.CreateWindowSurface(display, configs, WindowHandle, winAttribs);
        if (surface == 0)
        {
            Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "egl.CreateWindowSurface fail");
            return;
        }
        int[] attrib3_list = [0x3098, 2, 0x3038];
        int sharedEglContext = 0;
        var context = egl.CreateContext(display, configs, sharedEglContext, attrib3_list);
        if (egl.MakeCurrent(display, surface, surface, context) == false)
        {
            Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "egl.MakeCurrent fail");
            return;
        }

        Hilog.OH_LOG_DEBUG(LogType.LOG_APP, "CSharp", "egl init success");
        gl = GL.GetApi(name =>
        {
            var ptr = Marshal.StringToHGlobalAnsi(name);
            var fun = egl.GetProcAddress(ptr);
            Marshal.FreeHGlobal(ptr);
            return fun;
        });
    }

    public override void OnSurfaceCreated()
    {
        base.OnSurfaceCreated();
        if (UseSoftRenderer == true)
        {
            InitOpenGlEnv();
        }
        var builder = CreateAppBuilder();
        if (UseSoftRenderer)
        {
            builder.UseSoftwareRenderer();
            AvaloniaLocator.CurrentMutable.Bind<GL>().ToConstant(gl);
        }
        SingleViewLifetime = new SingleViewLifetime();
        builder.AfterApplicationSetup(CreateView).SetupWithLifetime(SingleViewLifetime);
        Root.StartRendering();
    }

    public override void OnSurfaceRendered(ulong timestamp, ulong targetTimestamp)
    {
        base.OnSurfaceRendered(timestamp, targetTimestamp);
        TopLevelImpl.Render();
        if (UseSoftRenderer == true)
        {
            egl.SwapBuffers(display, surface);
        }
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
