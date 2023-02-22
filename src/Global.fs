namespace Fable.Dart

open Fable.Core
open Fable.Core.Dart

[<AutoOpen>]
module Global =
    let inline (?) (a: obj) b : 'a = emitExpr (a, b) "$0[$1]"
    type [<Global>] dynamic =
        static member op_Explicit(source: dynamic) : _ = source |> unbox
