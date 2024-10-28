#include <dlfcn.h>

extern "C" __attribute__((constructor)) void RegisterAvaloniaNativeModule(void)
{
    auto handle = dlopen("libavalonia.so", RTLD_NOW);
    if (handle == nullptr)
        return;
    auto fun = (void(*)())dlsym(handle, "RegisterEntryModule");
    if (fun == nullptr)
        return;
    fun();
}
