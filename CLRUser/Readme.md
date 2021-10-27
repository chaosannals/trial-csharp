# clr

- CLRNative 是个纯 VC++ 非托管项目，模拟被调用的第三方 dll 库。
- CLRWrapper 是个 混合模式 VC++ 托管项目，封装了对 CLRNative 的调用。
- CLRUser 是个 C# .net 项目，使用者。

因为 VS C++ 和 C# 生成路径不一致，所以 CLRUser 改过生成路径，不然报找不到 DLL。

注：VC2019 下 CLRWrapper 指定 .NET 框架版本 3.5 会报错（ C2337 TargetFrameworkAttribute）。只有 4.0 以上才不会。

## CLRNative

需要 DllMain 定义。

## CLRWrapper

托管 不需要 DllMain 定义。

## CLRUser

