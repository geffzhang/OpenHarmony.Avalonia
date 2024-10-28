import { hilog } from '@kit.PerformanceAnalysisKit';

@Entry
@Component
struct Index {
  @State message: string = 'Hello World';

  build() {
    Row() {
      Column() {
        XComponent({libraryname: 'avalonianative',type:'Surface', id: 'avaloniaRootComponent' })
      }
      .width('100%')
    }
    .height('100%')
  }
}
