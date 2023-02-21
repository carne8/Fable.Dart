namespace Fable.Dart.Future

#nowarn "59"

open FsToolkit.ErrorHandling

[<RequireQualifiedAccess>]
module FutureResult =
    let map f tr = Future.map (Result.map f) tr
    let mapError f tr = Future.map (Result.mapError f) tr
    let tap f tr = Future.map (Result.map (fun r -> r |> f |> ignore; r)) tr
    let tapError f tr = Future.map (Result.mapError (fun r -> r |> f |> ignore; r)) tr
    let bind f (tr : Future<_>) = future {
        let! result = tr
        let t =
            match result with
            | Ok x -> f x
            | Error e -> future { return Error e }
        return! t
    }
    let catchError (future: Future<'ok>) =
        future
        |> Future.map (Ok >> unbox<Result<'ok, obj>>)
        |> Future.catchError (fun e -> e |> Error :> Result<'ok, obj> |> unbox<Result<'ok, obj>>)
    let retn x = Ok x |> Future.singleton
    let returnError x = Error x |> Future.singleton
    /// Replaces the wrapped value with unit
    let ignore tr = tr |> map ignore
    /// Returns the specified error if the task-wrapped value is false.
    let requireTrue error value = value |> Future.map (Result.requireTrue error)
    /// Returns the specified error if the task-wrapped value is true.
    let requireFalse error value = value |> Future.map (Result.requireFalse error)
    // Converts an task-wrapped Option to a Result, using the given error if None.
    let requireSome error option = option |> Future.map (Result.requireSome error)
    // Converts an task-wrapped Option to a Result, using the given error if Some.
    let requireNone error option = option |> Future.map (Result.requireNone error)
    /// Returns Ok if the task-wrapped value and the provided value are equal, or the specified error if not.
    let requireEqual x1 x2 error = x2 |> Future.map (fun x2' -> Result.requireEqual x1 x2' error)
    /// Returns Ok if the two values are equal, or the specified error if not.
    let requireEqualTo other error this = this |> Future.map (Result.requireEqualTo other error)
    /// Returns Ok if the task-wrapped sequence is empty, or the specified error if not.
    let requireEmpty error xs = xs |> Future.map (Result.requireEmpty error)
    /// Returns Ok if the task-wrapped sequence is not-empty, or the specified error if not.
    let requireNotEmpty error xs = xs |> Future.map (Result.requireNotEmpty error)
    /// Returns the first item of the task-wrapped sequence if it exists, or the specified
    /// error if the sequence is empty
    let requireHead error xs = xs |> Future.map (Result.requireHead error)
    /// Replaces an error value of an task-wrapped result with a custom error
    /// value.
    let setError error taskResult = taskResult |> Future.map (Result.setError error)
    /// Replaces a unit error value of an task-wrapped result with a custom
    /// error value. Safer than setError since you're not losing any information.
    let inline withError error taskResult = taskResult |> Future.map (Result.withError error)
    /// Extracts the contained value of an task-wrapped result if Ok, otherwise
    /// uses ifError.
    let defaultValue ifError taskResult = taskResult |> Future.map (Result.defaultValue ifError)
    /// Extracts the contained value of an task-wrapped result if Ok, otherwise
    /// evaluates ifErrorThunk and uses the result.
    let defaultWith ifErrorThunk taskResult = taskResult |> Future.map (Result.defaultWith ifErrorThunk)
    /// Same as defaultValue for a result where the Ok value is unit. The name
    /// describes better what is actually happening in this case.
    let ignoreError taskResult = defaultValue () taskResult
    /// If the task-wrapped result is Ok, executes the function on the Ok value.
    /// Passes through the input value.
    let tee f taskResult = taskResult |> Future.map (Result.tee f)
    /// If the task-wrapped result is Ok and the predicate returns true, executes
    /// the function on the Ok value. Passes through the input value.
    let teeIf predicate f taskResult = taskResult |> Future.map (Result.teeIf predicate f)
    /// If the task-wrapped result is Error, executes the function on the Error
    /// value. Passes through the input value.
    let teeError f taskResult = taskResult |> Future.map (Result.teeError f)
    /// If the task-wrapped result is Error and the predicate returns true,
    /// executes the function on the Error value. Passes through the input value.
    let teeErrorIf predicate f taskResult = taskResult |> Future.map (Result.teeErrorIf predicate f)
    /// Takes two results and returns a tuple of the pair
    let zip x1 x2 = Future.zip x1 x2 |> Future.map(fun (r1, r2) -> Result.zip r1 r2)


[<AutoOpen>]
module ResultComputationExpression =
    type FutureResultBuilder() =
        member _.Return(value: 'T) : Future<Result<'T, 'TError>> = Future.singleton (Ok value)
        member _.ReturnFrom(futureResult: Future<Result<'T, 'TError>>) : Future<Result<'T, 'TError>> = futureResult
        member _.Zero() : Future<Result<unit, 'TError>> = Future.singleton (Ok ())
        member _.Bind
            (
                futureResult: Future<Result<'T, 'TError>>,
                binder: 'T -> Future<Result<'U, 'TError>>
            ) : Future<Result<'U, 'TError>> =
            future {
                match! futureResult with
                | Ok x -> return! binder x
                | Error x -> return Error x
            }
        member _.Delay(generator: unit -> Future<Result<'T, 'TError>>) = generator
        member _.Combine
            (
                computation1: Future<Result<unit, 'TError>>,
                computation2: unit -> Future<Result<'U, 'TError>>
            ) : Future<Result<'U, 'TError>> =
            future {
                match! computation1 with
                | Error e -> return Error e
                | Ok _ -> return! computation2 ()
            }
        member this.BindReturn(x: Future<Result<'T, 'U>>, f) = this.Bind(x, (fun x -> this.Return(f x)))
        member _.MergeSources(t1: Future<Result<'T, 'U>>, t2: Future<Result<'T1, 'U>>) = FutureResult.zip t1 t2
        member _.Run(f: unit -> Future<'m>) = f ()
        member inline _.Source(future: Future<Result<_, _>>) : Future<Result<_, _>> = future

    [<AutoOpen>]
    module PromiseResultCEExtensions =
        type FutureResultBuilder with
            member inline _.Source(future : Future<_>) : Future<Result<_,_>> = future |> Future.map Ok
            member inline _.Source(result : Result<_,_>) : Future<Result<_,_>> = Future.singleton result

    let futureResult = FutureResultBuilder()

module List =
    let rec private traverseResultM' (state: Result<_, _>) (f: _ -> Result<_, _>) xs =
        match xs with
        | [] -> state |> Result.map List.rev
        | x :: xs ->
            let r =
                result {
                    let! y = f x
                    let! ys = state
                    return y :: ys
                }

            match r with
            | Ok _ -> traverseResultM' r f xs
            | Error _ -> r

    let traverseResultM f xs = traverseResultM' (Ok []) f xs
    let sequenceResultM xs = traverseResultM id xs

    let rec private traverseResultA' state f xs =
        match xs with
        | [] -> state |> Result.map List.rev
        | x :: xs ->
            let fR = f x |> Result.mapError List.singleton

            match state, fR with
            | Ok ys, Ok y -> traverseResultA' (Ok(y :: ys)) f xs
            | Error errs, Error e -> traverseResultA' (Error(errs @ e)) f xs
            | Ok _, Error e
            | Error e, Ok _ -> traverseResultA' (Error e) f xs

    let rec private traverseFutureResultA' state f xs =
        match xs with
        | [] -> state |> FutureResult.map List.rev
        | x :: xs ->
            future {
                let! s = state
                let! fR = f x |> FutureResult.mapError List.singleton

                match s, fR with
                | Ok ys, Ok y -> return! traverseFutureResultA' (FutureResult.retn (y :: ys)) f xs
                | Error errs, Error e -> return! traverseFutureResultA' (FutureResult.returnError (errs @ e)) f xs
                | Ok _, Error e
                | Error e, Ok _ -> return! traverseFutureResultA' (FutureResult.returnError e) f xs
            }

    let traverseResultA f xs = traverseResultA' (Ok []) f xs
    let sequenceResultA xs = traverseResultA id xs
    let rec traverseValidationA' state f xs =
        match xs with
        | [] -> state |> Result.map List.rev
        | x :: xs ->
            let fR = f x

            match state, fR with
            | Ok ys, Ok y -> traverseValidationA' (Ok(y :: ys)) f xs
            | Error errs1, Error errs2 -> traverseValidationA' (Error(errs2 @ errs1)) f xs
            | Ok _, Error errs
            | Error errs, Ok _ -> traverseValidationA' (Error errs) f xs

    let traverseValidationA f xs = traverseValidationA' (Ok []) f xs
    let sequenceValidationA xs = traverseValidationA id xs

    let traverseFutureResultA f xs =
        traverseFutureResultA' (FutureResult.retn []) f xs

    let sequenceFutureResultA xs = traverseFutureResultA id xs
