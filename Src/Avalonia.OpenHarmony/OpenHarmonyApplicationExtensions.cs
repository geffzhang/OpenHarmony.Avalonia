using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.OpenHarmony;

public static class OpenHarmonyApplicationExtensions
{
    public static AppBuilder UseOpenHarmony(this AppBuilder builder)
    {
        return builder
            .UseStandardRuntimePlatformSubsystem()
            .UseWindowingSubsystem(OpenHarmonyPlatform.Initialize, "OpenHarmony")
            .UseSkia();
    }
}
