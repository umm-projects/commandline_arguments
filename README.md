# What?

* Unity に渡されたコマンドライン引数をパースし、スクリプトで受け取れるようにするライブラリです。

# Why?

* バッチビルドなどの際に、Jenkins などの CI ツールから引数を貰うことが多いので、共通化しました。

# Install

```shell
$ npm install github:umm-projects/commandline_argument
```

# Usage

```csharp
using UnityEngine;

public class Sample : MonoBehaviour {

    public void Start() {
        // 引数が hoge fuga -a --bbb BBB の場合
        Debug.Log(UnityModule.CommandLine.Arguments.GetMainArgumentList()[1]); // fuga
        Debug.Log(UnityModule.CommandLine.Arguments.GetSwitch("a"));           // true
        Debug.Log(UnityModule.CommandLine.Arguments.GetOption("bbb"));         // BBB
    }

}
```

* ハイフンの数が1&#xff5e;2個の場合に引数と見なします。
* `GetSwitch()`, `GetOption()` は第一引数に `IEnumerable<string>` を取るコトができます。
  * その場合、渡されたキーの中から最初にヒットした値を返します。
* `GetOptionString()`, `GetOptionInt()`, `GetOptionBool()` は値をよしなにキャストして返します。
* `GetOption***()` は第二引数にデフォルト値を取るコトができます。

# License

Copyright (c) 2017-2018 Tetsuya Mori

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)

