# docker 项目

## 原镜像

### 开发容器

```sh
docker run -itd -v /host/path:/dnc -p 13080:13080 -w /dnc --name dotnetcore --entrypoint /bin/bash mcr.microsoft.com/dotnet/core/sdk
```

### 部署容器

```sh
docker run -d -v /host/path:/dnc -p 13080:13080 -w /dnc --name dotnetcore --entrypoint dnc mcr.microsoft.com/dotnet/core/aspnet
```
