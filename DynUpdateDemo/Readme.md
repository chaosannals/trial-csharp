# DynUpdate

旧版本 .net 使用 AppDomain 来管理程序集。
加载的时候  新建一个 AppDomain 然后在此 AppDomain 下生成一个远程对象 MarshalByRefObject。
在 该对象的 方法里面执行的加载的程序集会在新 AppDomain 里面。
该对象的执行空间也是在新的 AppDomain 里，这就是为什么这个对象必须是 MarshalByRefObject。

注：不要试图在 主 AppDomain 里面 使用 新 AppDomain 实例去创建和执行要独立开的程序集方法。
这样会在主 AppDomain 引入欲分离的程序集。
