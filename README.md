# WpfDependencyInjection

Generic Host (汎用ホスト) のことが以前から気になっていましたが、**何を実現できるか** を理解できていないので、試しに WPF で 汎用ホスト を使用してみました。

ベースのソフトは YouTube動画 を参考にしました。[Dependency Injection in WPF in .NET 6 Including the Factory Pattern - YouTube](https://www.youtube.com/watch?v=dLR_D2IJE1M&ab_channel=IAmTimCorey)



## MS.Learn より

> ''*ホスト*'' とは、次のようなアプリのリソースと有効期間機能をカプセル化するオブジェクトです。
>
> - 依存関係の挿入 (DI)
> - ログの記録
> - 構成
> - アプリのシャットダウン
> - `IHostedService` の実装



## ソフト対応

App.csproj

```xaml
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net7.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>

    <!-- 以下を追加すると WPF がデフォルトの Program.Main() を自動生成しなくなります -->
    <EnableDefaultApplicationDefinition>false</EnableDefaultApplicationDefinition>
  </PropertyGroup>
```



## References

[.NET 汎用ホスト - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/generic-host)

[Dependency Injection in WPF in .NET 6 Including the Factory Pattern - YouTube](https://www.youtube.com/watch?v=dLR_D2IJE1M&ab_channel=IAmTimCorey)

[Dependency Injection, Generic Host, and WPF - YouTube](https://www.youtube.com/watch?v=j3pl2tkBM1A&ab_channel=KevinBost)

[WPFをGeneric Host上で実行するためのライブラリ「Wpf.Extensions.Hosting」をリリースしました](https://zenn.dev/nuits_jp/articles/wpf-extensions-hosting)

