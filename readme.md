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

注：templates 目录是必须。

上次通过网上使用 Template.csproj 打包，亲测不行。是因为没有放 templates 目录被改。
通过上面直接用 dotnet new install 源码安装可以。

注：使用 VSIX Project 模板的只支持 .net framework 。而且我目前用 VS2022 会有 该项目中不存在目标“TemplateProjectOutputGroup” 的错误。看文档说支持 VS2017。 大概率只有老版本 VS 可行。不想尝试。就这样吧。
