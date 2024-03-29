# [WPF] 汎用ホストの調査

2023.4.29 (GW) ～

[.NET Generic Host (汎用ホスト)](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/generic-host) のことが以前から気になっていましたが、**何を実現できるか** を理解できていないので、試しに WPF で 汎用ホスト を使用してみました。



## そもそも汎用ホスト (Generic Host) とは

- 汎用ホストは .NET アプリケーションの実行時に、アプリケーションの構成、DI、ログなどを統合的に扱うための仕組みです。
- 元々 ASP.NET Core で Web Host と呼ばれていたものから Web 固有の機能を分離されて作られたそうです。
- 現在新しくリリースされる.NET向けの素晴らしいライブラリの多くは、Generic Hostを前提として提供されるようになってきていると思います。（nuitsjp さん）



## 成果まとめ

汎用ホスト を試した成果をまとめます。

**実現できたこと**

- ファイルへのログ記録（[Serilog](https://serilog.net/) の使用）
- `appsettings.json` の読み込み（構成プロバイダー）
- コマンドライン引数 の読み込み（構成プロバイダー）

**存在を知ったけど試せていない**

- 常駐サービス（WPFベースじゃなさそうなので）



## リポジトリ対応

以降に本リポジトリで対応した内容をまとめていきます。

### ベース対応

以下では `Program.Main()` を同期メソッドで対応しました。 非同期メソッドにすることもできましたが優位性が分からなかったので避けました。 非同期の方がカッコイイのは間違いないです。

1. `Microsoft.Extensions.Hosting` をインストールします。 本調査時の最新は `7.0.1` でした。

2. `App.xaml` から `StartupUri="MainWindow.xaml"` を削除します。

3. `*.csproj` で `Program.Main()` の自動生成を止めます。

   ```xml
   <PropertyGroup>
     <OutputType>WinExe</OutputType>
     <TargetFramework>net7.0-windows</TargetFramework>
     <UseWPF>true</UseWPF>
   
     <!-- 以下を追加すると WPF がデフォルトの Program.Main() を自動生成しなくなります -->
     <EnableDefaultApplicationDefinition>false</EnableDefaultApplicationDefinition>
   </PropertyGroup>
   ```

4. 新たに `Program.cs` を追加して `Main` メソッド内で `IHost` を生成し `App.Run()` を実行します。(説明が雑。詳細はリポジトリソースを参照してください。)

   

### 依存性の注入 (Dependency injection)

[依存関係の挿入 - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection)

DI はいくつかのアプリで使用実績がありますが、新たに知ることができた内容を箇条書きにします。

- `IDisposable` なインスタンスは `Transient` で登録しない。 有効期間を制御したければ `Scoped` を使用して とのこと。 [依存関係の挿入のガイドライン - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection-guidelines#general-idisposable-guidelines)

- 複数のコンストラクタが定義されてる場合、DIが解決できるインスタンスが最も多いコンストラクタが使用されるらしいです。（そんな意地悪な実装にはしませんが…）

- `Scoped` で登録したクラスを Scope を区切っていない状態でインスタンス取得した場合は Singleton として発行されるそうです。

- 複数の interface を登録した場合、 `IEnumerable<T>` で複数のインスタンスを取得できるようです。 使うか分かりませんが知らなかったので驚きました。（後登録勝ち と思っていました）

- `IServiceProvider` からインスタンスを取り出すメソッドは 2 つ用意されており挙動が異なるそうです。 `GetRequiredService()` の内部で `GetService()` をコールしているようなので、こだわりがなければ `GetService()` を使っておけばよさそうです。

  | メソッド             | 未登録クラスの取得を試みた場合           |
  | -------------------- | ---------------------------------------- |
  | GetService()         | null が返されます。                      |
  | GetRequiredService() | InvalidOperationException が発生します。 |

  

### 構成 (Configuration)

[構成 - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/configuration)

- `appsettings.json` からの設定読み込み
- コマンドライン引数 からの設定読み込み

#### appsettings.json

プログラム開始時にパラメータを読み出す外部ファイルです。 `App.config` は .NET Fw 時代の仕組みらしく、.NET 最新トレンドは `appsettings.json` らしいです。 もともとの `App.Config` に明るくないですが、 `appsettings.json` では汎用ホストが良い感じに `IOptions<T>` にデシリアライズしてくれるので便利に扱えそうです。

[WPF アプリを 7 にアップグレードする.NET - .NET Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/porting/upgrade-assistant-wpf-framework#use-appsettingsjson-with-the-wpf-sample-app)

> .NET フレームワークでは、 *App.config* ファイルを使用して、接続文字列やログ プロバイダーなどのアプリの設定を読み込みます。 .NET では、アプリ設定に *appsettings.json* ファイルが使用されるようになりました。

**optional フラグ**

`optional` フラグが `false` だと、`appsettings.json` が削除された状態で アプリ.exe を実行してもウィンドウが表示されません。 余計な障害報告を減らすため `true` にしておき、デフォルト設定をメンテしておくと良さげです。

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



### ログの記録 (Logging)

[C# でのログ記録 - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line)

[Serilog](https://serilog.net/) を導入して以下に対応しました。

- ファイルへのログ記録
- 出力→デバッグ ウィンドウへのログ出力
- `appsettings.json` からのログレベル指定

#### Serilog

.NET 向けの非常に強力なロギングフレームワークで、Console や File だけでなく、AWS, Azure, Email などにもログを出力できます。他のロギングライブラリと比較した特徴として、構造化イベントデータ を念頭に置いて構築されているそうです。

##### ログレベル

以下は Serilog のログレベル一覧です。 出力レベルを指定しなかった場合は `Information` で動作するそうです。

```cs
logger.LogCritical("Fatal");            // 致命的　※名称異なる
logger.LogError("Error");               // エラー
logger.LogWarning("Warning");           // 警告
logger.LogInformation("Information");   // 一般的な情報
logger.LogDebug("Debug");               // デバッグ
logger.LogTrace("Verbose");             // 最低　　※名称異なる
```

ログレベルの使い分けが分からなかったので自分で勝手に考えた分類をメモっておきます。

| ログレベル       | 分類     | 個人的な使い分け基準         |
| ---------------- | -------- | ---------------------------- |
| Fatal (Critical) | 致命的   | ここまで重篤なものはないはず |
| Error            | エラー   | ヤバいことが発生済み         |
| Warning          | 警告     | ヤバいことが起こりそう       |
| Information      | 情報     | ユーザ操作など               |
| Debug            | デバッグ | 処理開始/終了など            |
| Verbose (Trace)  | トレース | コミットしない。ローカル用途 |

##### ログ メッセージ テンプレート

文字列は専用の記法があるようで、`$` を使った 文字列補完 だと waring [CA2254](https://learn.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/quality-rules/ca2254) が表示されます。 ヒープ削減の目的と思われます。[C# でのログ記録 - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logging?tabs=command-line#log-message-template)

```cs
logger.LogTrace("Button was clicked. Counter={Cnt}, Date={Date:yyMMdd}", ButtonCounter, DateTime.Now);
```

##### 分解 (Destructuring)

以下はログで `@` を使用した場合の動作例です。 `@` なし なら `ToString()` が出力され、`@` あり なら オブジェクトをシリアライズした 文字列が出力されます。

```cs
C c = new();
logger.LogInformation("1: {C}", c);
logger.LogInformation("2: {@C}", c);

class C
{
    public int Value1 { get; }
    public override string ToString() => "C.ToString()";
}
// 13:57:24:663	[INF] 1: "C.ToString()"
// 13:57:24:663	[INF] 2: C { Value1: 0 }
```



## References

[.NET 汎用ホスト - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/generic-host)

[Dependency Injection in WPF in .NET 6 Including the Factory Pattern - YouTube](https://www.youtube.com/watch?v=dLR_D2IJE1M&ab_channel=IAmTimCorey)

[WPFをGeneric Host上で実行するためのライブラリ「Wpf.Extensions.Hosting」をリリースしました](https://zenn.dev/nuits_jp/articles/wpf-extensions-hosting)

