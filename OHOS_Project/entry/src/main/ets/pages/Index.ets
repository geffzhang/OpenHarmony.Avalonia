import { hilog } from '@kit.PerformanceAnalysisKit';
import testNapi from 'libentry.so';

@Entry
@Component
struct Index {
  @State message: string = 'Hello World';

  build() {
    Row() {
      Column() {
        XComponent({ type: XComponentType.SURFACE, id: 'AvaloniaRootComponent', libraryname: "entry" })
      }
      .width('100%')
    }
    .height('100%')
  }
}
