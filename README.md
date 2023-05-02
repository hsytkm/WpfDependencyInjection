# WpfDependencyInjection

Generic Host (汎用ホスト) のことが以前から気になっていましたが、**何を実現できるか** を理解できていないので、試しに WPF で 汎用ホスト を使用してみました。

ベースのソフトは YouTube動画 を参考にしました。[Dependency Injection in WPF in .NET 6 Including the Factory Pattern - YouTube](https://www.youtube.com/watch?v=dLR_D2IJE1M&ab_channel=IAmTimCorey)



## MS.Learn より

[.NET 汎用ホスト - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/generic-host)

> ''*ホスト*'' とは、次のようなアプリのリソースと有効期間機能をカプセル化するオブジェクトです。
>
> - 依存関係の挿入 (DI)
> - ログの記録
> - 構成
> - アプリのシャットダウン
> - `IHostedService` の実装



## 対応した内容

本リポジトリで対応した内容をまとめます。

#### 依存性の注入 (Dependency injection)

これは 既に知ってた内容。 いくつかのアプリで使用済み。

#### ログの記録 (Logging)



#### 構成 (Configuration)

#### アプリのシャットダウン (App shutdown)

#### IHostedService の実装 (IHostedService implementations)





## ソフト対応

1. App.csproj で `Program.Main()` の生成を止めます。

   ```xaml
   <PropertyGroup>
     <OutputType>WinExe</OutputType>
     <TargetFramework>net7.0-windows</TargetFramework>
     <UseWPF>true</UseWPF>
   
     <!-- 以下を追加すると WPF がデフォルトの Program.Main() を自動生成しなくなります -->
     <EnableDefaultApplicationDefinition>false</EnableDefaultApplicationDefinition>
   </PropertyGroup>
   ```

2. `Program.cs` を追加して `IHost` を生成します。(説明が雑)

   

## appsettings.json

あまり使用してなかったけど `App.config` は .NET Fw 時代らしく、.NET 最新トレンドで `appsettings.json` らしい。

[WPF アプリを 7 にアップグレードする.NET - .NET Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/porting/upgrade-assistant-wpf-framework#use-appsettingsjson-with-the-wpf-sample-app)

> .NET フレームワークでは、 *App.config* ファイルを使用して、接続文字列やログ プロバイダーなどのアプリの設定を読み込みます。 .NET では、アプリ設定に *appsettings.json* ファイルが使用されるようになりました。



## References

[.NET 汎用ホスト - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/generic-host)

[Dependency Injection in WPF in .NET 6 Including the Factory Pattern - YouTube](https://www.youtube.com/watch?v=dLR_D2IJE1M&ab_channel=IAmTimCorey)

[Dependency Injection, Generic Host, and WPF - YouTube](https://www.youtube.com/watch?v=j3pl2tkBM1A&ab_channel=KevinBost)

[WPFをGeneric Host上で実行するためのライブラリ「Wpf.Extensions.Hosting」をリリースしました](https://zenn.dev/nuits_jp/articles/wpf-extensions-hosting)

