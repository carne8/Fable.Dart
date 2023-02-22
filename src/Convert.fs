module Fable.Dart.Convert

open Fable.Core
open Fable.Dart

[<ImportMember "dart:convert">]
type json =
    static member decode (str: string) : Map<string, dynamic> = nativeOnly
    static member encode (value: _) : string = nativeOnly