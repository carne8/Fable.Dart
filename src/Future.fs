namespace Fable.Dart.Future

open Fable.Core
open Fable.Core.Dart
open FsToolkit.ErrorHandling

/// https://api.dart.dev/dart-async/Future-class.html
[<ImportMember "dart:async">]
type Future<'T> =
    interface Dart.Future<'T>
    member _.``then``(_: 'T -> 'A) : Future<'A> = nativeOnly
    member _.catchError(_: obj -> 'A) : Future<'A> = nativeOnly

/// https://api.dart.dev/dart-future/Future-class.html
[<RequireQualifiedAccess>]
module Future =
    let inline map (f: 'T -> 'A) (future: Future<'T>) : Future<'A> = emitExpr (future, f) "$0.then($1)"
    let inline catchError (f: obj -> 'A) (future: Future<'T>) : Future<'A> = emitExpr (future, f) "$0.catchError($1)"
    let inline bind (f: 'T -> Future<'A>) (future: Future<'T>) : Future<'A> = emitExpr (future, f) "$0.then($1)"
    let inline delayed (duration: System.TimeSpan) (future: unit -> 'A) : Future<'A> = emitExpr (import "Future" "dart:async", duration, future) "$0.delayed($1, $2)"
    let inline value (x: 'A) : Future<'A> = emitExpr (import "Future" "dart:async", x) "$0.value($1)"
    let inline singleton (x: 'A) : Future<'A> = value x
    /// Takes two futures and returns a tuple of the pair
    let zip (a1: Future<'T1>) (a2: Future<'T2>) : ('T1 * 'T2) Future =
        a1 |> bind (fun r1 -> a2 |> map (fun r2 -> r1, r2))

[<AutoOpen>]
module ComputationExpression =
    type FutureBuilder() =
        member _.Bind(p: Future<'T1>, f: 'T1 -> Future<'T2>): Future<'T2> = p |> Future.bind f
        member _.For(p: Future<'T1>, f: 'T1 -> Future<'T2>): Future<'T2> = p |> Future.bind f
        [<Emit("$1.then(() => $2)")>]
        member _.Combine(p1: Future<unit>, p2: Future<'T>): Future<'T> = nativeOnly
        member _.Return(a: 'T): Future<'T> = Future.singleton a
        member _.ReturnFrom(p: Future<'T>): Future<'T> = p
        member _.Zero(): Future<unit> = emitExpr (import "Future" "dart:async") "$0<void>.value()"
        [<Emit("$1.catchError($2)")>]
        member _.fryWith(p: Future<'T>, catchHandler: exn -> Future<'T>): Future<'T> = nativeOnly
        member _.Run(p:Future<'T>): Future<'T> = p.``then``(id)
        member _.BindReturn(y: Future<'T1>, f) = Future.map f y

    let future : FutureBuilder = FutureBuilder()