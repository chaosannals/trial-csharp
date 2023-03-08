# [trial-csharp](https://github.com/chaosannals/trial-csharp)

```bash
# 打 nuget 包
dotnet pack -c Release
```

## vs 模板

```bat
@rem 安装当前项目作为模板
dotnet new install ./
@rem 或
dotnet new -i ./

@rem 卸载当前项目作为模板
dotnet new uninstall ./
```

网上使用 Template.csproj 打包，亲测不行。
通过上面直接用 dotnet new install 源码安装可以。
