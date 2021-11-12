# IL

目前跨平台的 .net 没有开发完整的 IL 工具，无法做字节码工程。
只有 .net framework 保留了原来的 ILGenerator 完整功能，只支持 windows。

```bash
# 指定微软的原添加 预览版 的。 这个是生成 本地机器码 的编译器。
dotnet add package Microsoft.Dotnet.ILCompiler -s https://dotnetfeed.blob.core.windows.net/dotnet-core/index.json --prerelease
```