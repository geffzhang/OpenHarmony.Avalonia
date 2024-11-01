#include <cassert>
#include <dlfcn.h>
#include <stdlib.h>
#include <stdint.h>
#define _TEST_ICU_ false

void InitICU();

extern "C" __attribute__((constructor)) void RegisterAvaloniaNativeModule(void)
{
    InitICU();
    
    auto handle = dlopen("libavalonia.so", RTLD_NOW);
    assert(handle != nullptr);
    
    auto pRegisterEntryModule = (void(*)())dlsym(handle, "RegisterEntryModule");
    assert(pRegisterEntryModule != nullptr);
    
    pRegisterEntryModule();
}

#define U_MAX_VERSION_LENGTH 4
typedef uint8_t UVersionInfo[U_MAX_VERSION_LENGTH];

void InitICU()
{
    setenv("ICU_DATA", "/system/usr/ohos_icu", 0);
    // test code
    if (_TEST_ICU_){
        auto handle = dlopen("libicui18n.so.76", RTLD_LAZY);
        assert(handle != nullptr);
        
        auto pUlocdata_getCLDRVersion = (void(*)(UVersionInfo, int32_t*))dlsym(handle, "ulocdata_getCLDRVersion_76");
        assert(pUlocdata_getCLDRVersion != nullptr);
        
        UVersionInfo version;
        int32_t errorCode;
        
        pUlocdata_getCLDRVersion(version, &errorCode);
        assert(errorCode == 0);
    }
}