using Avalonia.Controls.Platform.Surfaces;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.OpenHarmony;

public class FramebufferManager : IFramebufferPlatformSurface
{

    TopLevelImpl TopLevelImpl;
    public FramebufferManager(TopLevelImpl topLevelImpl) 
    {
        TopLevelImpl = topLevelImpl;
    }
    public ILockedFramebuffer Lock() => new OpenHarmonyFramebuffer(TopLevelImpl);
    public IFramebufferRenderTarget CreateFramebufferRenderTarget() => new FuncFramebufferRenderTarget(Lock);
}
