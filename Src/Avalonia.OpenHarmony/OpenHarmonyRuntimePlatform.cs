using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.OpenHarmony;

public class OpenHarmonyRuntimePlatform : StandardRuntimePlatform
{
    private static readonly Lazy<RuntimePlatformInfo> Info = new(() =>
    {
        var isMobile = true;
        var isTv = false;
        var result = new RuntimePlatformInfo
        {
            IsMobile = isMobile && !isTv,
            IsDesktop = !isMobile && !isTv,
            IsTV = isTv
        };

        return result;
    });

    public override RuntimePlatformInfo GetRuntimeInfo() => Info.Value;
}
