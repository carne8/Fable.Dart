namespace Fable.Dart

open Fable.Core
open Fable.Core.Dart

[<ImportMember "dart:core">]
type MapEntry<'A, 'B>(_key: 'A, _value: 'B) = struct end

[<ImportMember "dart:core">]
type Map<'A, 'B> =
    static member inline fromEntries (mapEntries: seq<MapEntry<'A, 'B>>) : Map<'A, 'B> =
        emitExpr (import "Map" "dart:core", mapEntries) "$0.fromEntries($1)"