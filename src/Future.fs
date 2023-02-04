namespace rec Fable.Dart

#nowarn "40"

open Fable.Core
open Fable.Core.Dart

/// https://api.dart.dev/dart-async/Future-class.html
[<ImportMember "dart:async">]
type Future<'T> =
    interface Dart.Future<'T>
    member _.``then``(_: 'T -> 'A) : Future<'A> = nativeOnly
    member _.catchError(_: _ -> _) = nativeOnly

/// https://api.dart.dev/dart-async/Future-class.html
module Future =
    let inline map (f: 'T -> 'A) (future: Future<'T>) : Future<'A> = emitExpr (future, f) "$0.then($1)"
    let inline bind (f: 'T -> Future<'A>) (future: Future<'T>) : Future<'A> = emitExpr (future, f) "$0.then($1)"
    let inline delayed (duration: System.TimeSpan) (future: unit -> 'A) : Future<'A> = emitExpr (import "Future" "dart:async", duration, future) "$0.delayed($1, $2)"
    let future = new FutureBuilder()

type FutureBuilder() =
    member _.Bind(p: Future<'T1>, f: 'T1 -> Future<'T2>): Future<'T2> = p |> Future.bind f
    member _.For(p: Future<'T1>, f: 'T1 -> Future<'T2>): Future<'T2> = p |> Future.bind f
    [<Emit("$1.then(() => $2)")>]
    member _.Combine(p1: Future<unit>, p2: Future<'T>): Future<'T> = nativeOnly
    [<Emit("return $1")>]
    member _.Return(a: 'T): Future<'T> = nativeOnly
    [<Emit("$1")>]
    member _.ReturnFrom(p: Future<'T>): Future<'T> = nativeOnly
    member _.Zero(): Future<unit> = emitExpr (import "Future" "dart:async") "$0<void>.value()"
    [<Emit("$1.catchError($2)")>]
    member _.TryWith(p: Future<'T>, catchHandler: exn -> Future<'T>): Future<'T> = nativeOnly
    member _.Run(p:Future<'T>): Future<'T> = p.``then``(id)
    member _.BindReturn(y: Future<'T1>, f) = Future.map f y