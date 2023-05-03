# WpfDependencyInjection

[.NET Generic Host (汎用ホスト)](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/generic-host) のことが以前から気になっていましたが、**何を実現できるか** を理解できていないので、試しに WPF で 汎用ホスト を使用してみました。

ベースのソフトは YouTube動画 を参考にしました。[Dependency Injection in WPF in .NET 6 Including the Factory Pattern - YouTube](https://www.youtube.com/watch?v=dLR_D2IJE1M&ab_channel=IAmTimCorey)



## 成果まとめ

汎用ホスト を試したことで新たに実現できたことをまとめます。

- ログ記録（Serilog の使用）
- `appsettings.json` の読み込み（構成プロバイダー）
- コマンドライン引数 の読み込み（構成プロバイダー）



## リポジトリ対応

以降に本リポジトリで対応した内容をまとめていきます。

### 基本のコード

1. `Microsoft.Extensions.Hosting` をインストールします。 本調査時の最新は `7.0.1` でした。

2. App.csproj で `Program.Main()` の生成を止めます。

   ```xaml
   <PropertyGroup>
     <OutputType>WinExe</OutputType>
     <TargetFramework>net7.0-windows</TargetFramework>
     <UseWPF>true</UseWPF>
   
     <!-- 以下を追加すると WPF がデフォルトの Program.Main() を自動生成しなくなります -->
     <EnableDefaultApplicationDefinition>false</EnableDefaultApplicationDefinition>
   </PropertyGroup>
   ```

3. `Program.cs` を追加して `Main` メソッド内で `IHost` を生成し `App.Run()` を実行します。(説明が雑)

   

### 依存性の注入 (Dependency injection)

[依存関係の挿入 - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection)

DI はいくつかのアプリで使用済みですが、新たに知った内容もありました。

- `IDisposable` なインスタンスは `Transient` で登録しない。 有効期間を制御したければ `Scoped` を使用してとのこと。 [依存関係の挿入のガイドライン - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection-guidelines#general-idisposable-guidelines)
- 複数のコンストラクタが定義されてる場合、DIが解決できるインスタンスが最も多いコンストラクタが使用されるらしい。（そんな意地悪な実装にはせんけども…）



### ログの記録 (Logging)

[C# でのログ記録 - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line)

Serilog を導入して以下に対応した。

- ファイルへのログ記録
- 出力→デバッグ へのログ出力
- appsettings.json からのログレベル指定



#### Serilog

以下は Serilog のログレベル一覧です。 出力レベルを指定しなかった場合は `Information` で動作するそうです。

```cs
logger.LogCritical("Fatal");            // 致命的　※名称異なる
logger.LogError("Error");               // エラー
logger.LogWarning("Warning");           // 警告
logger.LogInformation("Information");   // 一般的な情報
logger.LogDebug("Debug");               // デバッグ
logger.LogTrace("Verbose");             // 最低　　※名称異なる
```



#### ログ メッセージ テンプレート

文字列は専用の記法があるようで、`$` を使った 文字列補完 だと waring [CA2254](https://learn.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/quality-rules/ca2254) が表示されます。 ヒープ削減の目的と思われます。[C# でのログ記録 - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-message-template)

```cs
_logger.LogTrace("Button was clicked. Counter={Cnt}, Date={Date:yyMMdd}", ButtonCounter, DateTime.Now);
```



### 構成 (Configuration)

[構成 - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/configuration)

- `appsettings.json` からの設定読み込み
- コマンドライン引数



#### appsettings.json

プログラム開始時にパラメータを読み出す外部ファイルです。 `App.config` は .NET Fw 時代の仕組みらしく、.NET 最新トレンドは `appsettings.json` らしいです。

もともとの `App.Config` に明るくないですが、 `appsettings.json` では汎用ホストが良い感じに `IOptions<T>` にデシリアライズしてくれるので便利に扱えそうです。

[WPF アプリを 7 にアップグレードする.NET - .NET Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/porting/upgrade-assistant-wpf-framework#use-appsettingsjson-with-the-wpf-sample-app)

> .NET フレームワークでは、 *App.config* ファイルを使用して、接続文字列やログ プロバイダーなどのアプリの設定を読み込みます。 .NET では、アプリ設定に *appsettings.json* ファイルが使用されるようになりました。

**optional フラグ**

`optional` フラグが `false` だと、`appsettings.json` が削除された状態で アプリ.exe を実行してもウィンドウが表示されません。 余計な障害報告を減らすため、`true` にしておき、デフォルト設定をメンテしておくと良さげです。

```cs
// jsonファイルが存在しなくてもOK 
config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
```



#### コマンドライン引数

`appsettings.json` と同様に exe のコマンドライン引数を  `IOptions<T>` にデシリアライズしてもらえるので良い感じに扱えそうです。

コマンドライン引数の指定方法は 3種類あるようです。 [コマンド ライン引数 - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/configuration-providers#command-line-arguments)

```ps
dotnet run SecretKey="key"
dotnet run /SecretKey "key"
dotnet run --SecretKey "key"
```



## References

[.NET 汎用ホスト - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/generic-host)

[Dependency Injection in WPF in .NET 6 Including the Factory Pattern - YouTube](https://www.youtube.com/watch?v=dLR_D2IJE1M&ab_channel=IAmTimCorey)

[Dependency Injection, Generic Host, and WPF - YouTube](https://www.youtube.com/watch?v=j3pl2tkBM1A&ab_channel=KevinBost)

[WPFをGeneric Host上で実行するためのライブラリ「Wpf.Extensions.Hosting」をリリースしました](https://zenn.dev/nuits_jp/articles/wpf-extensions-hosting)

