module Fable.Dart.Environment

open Fable.Core
open Fable.Core.Dart

[<ImportMember "dart:core">]
type String =
    [<IsConst; NamedParams(fromIndex=1)>]
    static member fromEnvironment (key: string, [<OptionalArgument>] defaultValue: string) : string = nativeOnly

[<ImportMember "dart:core">]
type int =
    [<IsConst; NamedParams(fromIndex=1)>]
    static member fromEnvironment (key: string, [<OptionalArgument>] defaultValue: int) : int = nativeOnly

[<ImportMember "dart:core">]
type bool =
    [<IsConst>]
    static member hasEnvironment (key: string) : bool = nativeOnly
    [<IsConst; NamedParams(fromIndex=1)>]
    static member fromEnvironment (key: string, [<OptionalArgument>] defaultValue: bool) : bool = nativeOnly