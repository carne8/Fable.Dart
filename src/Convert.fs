module Fable.Dart.Convert

open Fable.Core
open Fable.Core.Dart

type [<Global>] dynamic = interface end
let inline (?) (a: dynamic) b : _ = emitExpr (a, b) "$0[$1]"

[<ImportMember "dart:convert">]
type json =
    static member decode (str: string) : dynamic = nativeOnly
    static member encode (value: _) : string = nativeOnly