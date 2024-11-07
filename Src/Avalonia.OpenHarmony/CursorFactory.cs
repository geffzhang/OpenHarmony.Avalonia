using Avalonia.Input;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.OpenHarmony;

public class CursorFactory : ICursorFactory
{
    public ICursorImpl CreateCursor(IBitmapImpl cursor, PixelPoint hotSpot) => CursorImpl.ZeroCursor;

    public ICursorImpl GetCursor(StandardCursorType cursorType) => CursorImpl.ZeroCursor;

    private sealed class CursorImpl : ICursorImpl
    {
        public static CursorImpl ZeroCursor { get; } = new CursorImpl();

        private CursorImpl() { }

        public void Dispose() { }
    }
}
