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
        Debug.Log(UnityModule.CommandLine.Arguments.GetString("string_key"));
        Debug.Log(UnityModule.CommandLine.Arguments.GetBool("bool_key"));
    }

}
```

* 上記のコードの場合 `--string_key hogehoge`, `--bool_key` といった引数を解釈できます。
* ハイフンの数が1&#xff5e;2個の場合に引数と見なします。
* `GetBool` は値を持たない引数が渡されている場合は真を、引数が存在しない場合に偽を返します

# License

Copyright (c) 2017 Tetsuya Mori

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)

