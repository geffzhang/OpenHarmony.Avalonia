#include <cassert>
#include <dlfcn.h>
#include <stdlib.h>
#include <stdint.h>

extern "C" __attribute__((constructor)) void RegisterAvaloniaNativeModule(void)
{
    setenv("ICU_DATA", "/system/usr/ohos_icu", 0);
    auto handle = dlopen("libavalonia.so", RTLD_NOW);
    assert(handle != nullptr);
    
    auto pRegisterEntryModule = (void(*)())dlsym(handle, "RegisterEntryModule");
    assert(pRegisterEntryModule != nullptr);
    
    pRegisterEntryModule();
}
