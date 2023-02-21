namespace FsToolkit.ErrorHandling

open System

[<RequireQualifiedAccess>]
module Result =

    let map
        (mapper: 'okInput -> 'okOutput)
        (input: Result<'okInput, 'error>)
        : Result<'okOutput, 'error> =
        match input with
        | Ok x -> Ok(mapper x)
        | Error e -> Error e

    let mapError
        (errorMapper: 'errorInput -> 'errorOutput)
        (input: Result<'ok, 'errorInput>)
        : Result<'ok, 'errorOutput> =
        match input with
        | Ok x -> Ok x
        | Error e -> Error(errorMapper e)

    let bind
        (binder: 'okInput -> Result<'okOutput, 'error>)
        (input: Result<'okInput, 'error>)
        : Result<'okOutput, 'error> =
        match input with
        | Ok x -> binder x
        | Error e -> Error e

    let isOk (value: Result<'ok, 'error>) : bool =
        match value with
        | Ok _ -> true
        | Error _ -> false

    let isError (value: Result<'ok, 'error>) : bool =
        match value with
        | Ok _ -> false
        | Error _ -> true

    let either
        (onOk: 'okInput -> 'output)
        (onError: 'errorInput -> 'output)
        (input: Result<'okInput, 'errorInput>)
        : 'output =
        match input with
        | Ok x -> onOk x
        | Error err -> onError err

    let eitherMap
        (onOk: 'okInput -> 'okOutput)
        (onError: 'errorInput -> 'errorOutput)
        (input: Result<'okInput, 'errorInput>)
        : Result<'okOutput, 'errorOutput> =
        match input with
        | Ok x -> Ok(onOk x)
        | Error err -> Error(onError err)

    let apply
        (applier: Result<'okInput -> 'okOutput, 'error>)
        (input: Result<'okInput, 'error>)
        : Result<'okOutput, 'error> =
        match (applier, input) with
        | Ok f, Ok x -> Ok(f x)
        | Error e, _
        | _, Error e -> Error e

    let map2
        (mapper: 'okInput1 -> 'okInput2 -> 'okOutput)
        (input1: Result<'okInput1, 'error>)
        (input2: Result<'okInput2, 'error>)
        : Result<'okOutput, 'error> =
        match (input1, input2) with
        | Ok x, Ok y -> Ok(mapper x y)
        | Error e, _
        | _, Error e -> Error e


    let map3
        (mapper: 'okInput1 -> 'okInput2 -> 'okInput3 -> 'okOutput)
        (input1: Result<'okInput1, 'error>)
        (input2: Result<'okInput2, 'error>)
        (input3: Result<'okInput3, 'error>)
        : Result<'okOutput, 'error> =
        match (input1, input2, input3) with
        | Ok x, Ok y, Ok z -> Ok(mapper x y z)
        | Error e, _, _
        | _, Error e, _
        | _, _, Error e -> Error e

    let ofChoice (input: Choice<'ok, 'error>) : Result<'ok, 'error> =
        match input with
        | Choice1Of2 x -> Ok x
        | Choice2Of2 e -> Error e

    let inline tryCreate (fieldName: string) (x: 'a) : Result< ^b, (string * 'c) > =
        let tryCreate' x = (^b: (static member TryCreate: 'a -> Result< ^b, 'c >) x)

        tryCreate' x
        |> mapError (fun z -> (fieldName, z))


    /// <summary>
    /// Returns <paramref name="result"/> if it is <c>Ok</c>, otherwise returns <paramref name="ifError"/>
    /// </summary>
    /// <param name="ifError">The value to use if <paramref name="result"/> is <c>Error</c></param>
    /// <param name="result">The input result.</param>
    /// <remarks>
    /// </remarks>
    /// <example>
    /// <code>
    ///     Error ("First") |> Result.orElse (Error ("Second")) // evaluates to Error ("Second")
    ///     Error ("First") |> Result.orElseWith (Ok ("Second")) // evaluates to Ok ("Second")
    ///     Ok ("First") |> Result.orElseWith (Error ("Second")) // evaluates to Ok ("First")
    ///     Ok ("First") |> Result.orElseWith (Ok ("Second")) // evaluates to Ok ("First")
    /// </code>
    /// </example>
    /// <returns>
    /// The result if the result is Ok, else returns <paramref name="ifError"/>.
    /// </returns>
    let orElse
        (ifError: Result<'ok, 'errorOutput>)
        (result: Result<'ok, 'error>)
        : Result<'ok, 'errorOutput> =
        match result with
        | Ok x -> Ok x
        | Error e -> ifError


    /// <summary>
    /// Returns <paramref name="result"/> if it is <c>Ok</c>, otherwise executes <paramref name="ifErrorFunc"/> and returns the result.
    /// </summary>
    /// <param name="ifErrorFunc">A function that provides an alternate result when evaluated.</param>
    /// <param name="result">The input result.</param>
    /// <remarks>
    /// <paramref name="ifErrorFunc"/>  is not executed unless <paramref name="result"/> is an <c>Error</c>.
    /// </remarks>
    /// <example>
    /// <code>
    ///     Error ("First") |> Result.orElseWith (fun _ -> Error ("Second")) // evaluates to Error ("Second")
    ///     Error ("First") |> Result.orElseWith (fun _ -> Ok ("Second")) // evaluates to Ok ("Second")
    ///     Ok ("First") |> Result.orElseWith (fun _ -> Error ("Second")) // evaluates to Ok ("First")
    ///     Ok ("First") |> Result.orElseWith (fun _ -> Ok ("Second")) // evaluates to Ok ("First")
    /// </code>
    /// </example>
    /// <returns>
    /// The result if the result is Ok, else the result of executing <paramref name="ifErrorFunc"/>.
    /// </returns>
    let orElseWith
        (ifErrorFunc: 'error -> Result<'ok, 'errorOutput>)
        (result: Result<'ok, 'error>)
        : Result<'ok, 'errorOutput> =
        match result with
        | Ok x -> Ok x
        | Error e -> ifErrorFunc e

    /// Replaces the wrapped value with unit
    let ignore<'ok, 'error> (result: Result<'ok, 'error>) : Result<unit, 'error> =
        match result with
        | Ok _ -> Ok()
        | Error e -> Error e

    /// Returns the specified error if the value is false.
    let requireTrue (error: 'error) (value: bool) : Result<unit, 'error> =
        if value then Ok() else Error error

    /// Returns the specified error if the value is true.
    let requireFalse (error: 'error) (value: bool) : Result<unit, 'error> =
        if not value then Ok() else Error error

    /// Converts an Option to a Result, using the given error if None.
    let requireSome (error: 'error) (option: 'ok option) : Result<'ok, 'error> =
        match option with
        | Some x -> Ok x
        | None -> Error error

    /// Converts an Option to a Result, using the given error if Some.
    let requireNone (error: 'error) (option: 'value option) : Result<unit, 'error> =
        match option with
        | Some _ -> Error error
        | None -> Ok()

    /// Converts a nullable value into a Result, using the given error if null
    let requireNotNull (error: 'error) (value: 'ok) : Result<'ok, 'error> =
        match value with
        | null -> Error error
        | nonnull -> Ok nonnull

    /// Returns Ok if the two values are equal, or the specified error if not.
    /// Same as requireEqual, but with a signature that fits piping better than
    /// normal function application.
    let requireEqualTo
        (other: 'value)
        (error: 'error)
        (this: 'value)
        : Result<unit, 'error> =
        if this = other then Ok() else Error error

    /// Returns Ok if the two values are equal, or the specified error if not.
    /// Same as requireEqualTo, but with a signature that fits normal function
    /// application better than piping.
    let requireEqual (x1: 'value) (x2: 'value) (error: 'error) : Result<unit, 'error> =
        if x1 = x2 then Ok() else Error error

    /// Returns Ok if the sequence is empty, or the specified error if not.
    let requireEmpty (error: 'error) (xs: #seq<'value>) : Result<unit, 'error> =
        if Seq.isEmpty xs then Ok() else Error error

    /// Returns the specified error if the sequence is empty, or Ok if not.
    let requireNotEmpty (error: 'error) (xs: #seq<'value>) : Result<unit, 'error> =
        if Seq.isEmpty xs then Error error else Ok()

    /// Returns the first item of the sequence if it exists, or the specified
    /// error if the sequence is empty
    let requireHead (error: 'error) (xs: #seq<'ok>) : Result<'ok, 'error> =
        match Seq.tryHead xs with
        | Some x -> Ok x
        | None -> Error error

    /// Replaces an error value with a custom error value.
    let setError (error: 'error) (result: Result<'ok, 'errorIgnored>) : Result<'ok, 'error> =
        result
        |> mapError (fun _ -> error)

    /// Replaces a unit error value with a custom error value. Safer than setError
    /// since you're not losing any information.
    let inline withError (error: 'error) (result: Result<'ok, unit>) : Result<'ok, 'error> =
        result |> mapError (fun () -> error)

    /// Returns the contained value if Ok, otherwise returns ifError.
    let defaultValue (ifError: 'ok) (result: Result<'ok, 'error>) : 'ok =
        match result with
        | Ok x -> x
        | Error _ -> ifError

    // Returns the contained value if Error, otherwise returns ifOk.
    let defaultError (ifOk: 'error) (result: Result<'ok, 'error>) : 'error =
        match result with
        | Error error -> error
        | Ok _ -> ifOk

    /// Returns the contained value if Ok, otherwise evaluates ifErrorThunk and
    /// returns the result.
    let defaultWith
        (ifErrorThunk: 'error -> 'ok)
        (result: Result<'ok, 'error>)
        : 'ok =
        match result with
        | Ok x -> x
        | Error e -> ifErrorThunk e

    /// Same as defaultValue for a result where the Ok value is unit. The name
    /// describes better what is actually happening in this case.
    let ignoreError<'error> (result: Result<unit, 'error>) : unit = defaultValue () result

    /// If the result is Ok and the predicate returns true, executes the function
    /// on the Ok value. Passes through the input value.
    let teeIf
        (predicate: 'ok -> bool)
        (inspector: 'ok -> unit)
        (result: Result<'ok, 'error>)
        : Result<'ok, 'error> =
        match result with
        | Ok x ->
            if predicate x then
                inspector x
        | Error _ -> ()

        result

    /// If the result is Error and the predicate returns true, executes the
    /// function on the Error value. Passes through the input value.
    let teeErrorIf
        (predicate: 'error -> bool)
        (inspector: 'error -> unit)
        (result: Result<'ok, 'error>)
        : Result<'ok, 'error> =
        match result with
        | Ok _ -> ()
        | Error x ->
            if predicate x then
                inspector x

        result

    /// If the result is Ok, executes the function on the Ok value. Passes through
    /// the input value.
    let tee
        (inspector: 'ok -> unit)
        (result: Result<'ok, 'error>)
        : Result<'ok, 'error> =
        teeIf (fun _ -> true) inspector result

    /// If the result is Error, executes the function on the Error value. Passes
    /// through the input value.
    let teeError
        (inspector: 'error -> unit)
        (result: Result<'ok, 'error>)
        : Result<'ok, 'error> =
        teeErrorIf (fun _ -> true) inspector result

    // /// Converts a Result<Async<_>,_> to an Async<Result<_,_>>
    // let sequenceAsync (resAsync: Result<Async<'ok>, 'error>) : Async<Result<'ok, 'error>> = async {
    //     match resAsync with
    //     | Ok asnc ->
    //         let! x = asnc
    //         return Ok x
    //     | Error err -> return Error err
    // }

    // ///
    // let traverseAsync
    //     (f: 'okInput -> Async<'okOutput>)
    //     (res: Result<'okInput, 'error>)
    //     : Async<Result<'okOutput, 'error>> =
    //     sequenceAsync ((map f) res)


    /// Returns the Ok value or runs the specified function over the error value.
    let valueOr (f: 'error -> 'ok) (res: Result<'ok, 'error>) : 'ok =
        match res with
        | Ok x -> x
        | Error x -> f x

    /// Takes two results and returns a tuple of the pair
    let zip
        (left: Result<'leftOk, 'error>)
        (right: Result<'rightOk, 'error>)
        : Result<'leftOk * 'rightOk, 'error> =
        match left, right with
        | Ok x1res, Ok x2res -> Ok(x1res, x2res)
        | Error e, _ -> Error e
        | _, Error e -> Error e

    /// Takes two results and returns a tuple of the error pair
    let zipError
        (left: Result<'ok, 'leftError>)
        (right: Result<'ok, 'rightError>)
        : Result<'ok, 'leftError * 'rightError> =
        match left, right with
        | Error x1res, Error x2res -> Error(x1res, x2res)
        | Ok e, _ -> Ok e
        | _, Ok e -> Ok e

[<AutoOpen>]
module ResultCE =

    type ResultBuilder() =
        member _.Return(value: 'ok) : Result<'ok, 'error> = Ok value

        member _.ReturnFrom(result: Result<'ok, 'error>) : Result<'ok, 'error> = result

        member _.Zero() : Result<unit, 'error> = Ok ()

        member _.Bind
            (
                input: Result<'okInput, 'error>,
                binder: 'okInput -> Result<'okOutput, 'error>
            ) : Result<'okOutput, 'error> =
            Result.bind binder input

        member _.Delay
            (generator: unit -> Result<'ok, 'error>)
            : unit -> Result<'ok, 'error> =
            generator

        member _.Run
            (generator: unit -> Result<'ok, 'error>)
            : Result<'ok, 'error> =
            generator ()

        member _.Combine
            (
                result: Result<unit, 'error>,
                binder: unit -> Result<'ok, 'error>
            ) : Result<'ok, 'error> =
            match result with
            | Error e -> Error e
            | Ok _ -> binder ()

        member this.TryWith
            (
                generator: unit -> Result<'T, 'TError>,
                handler: exn -> Result<'T, 'TError>
            ) : Result<'T, 'TError> =
            try
                this.Run generator
            with e ->
                handler e

        member this.TryFinally
            (
                generator: unit -> Result<'ok, 'error>,
                compensation: unit -> unit
            ) : Result<'ok, 'error> =
            try
                this.Run generator
            finally
                compensation ()

        member this.Using
            (
                resource: 'disposable :> IDisposable,
                binder: 'disposable -> Result<'ok, 'error>
            ) : Result<'ok, 'error> =
            this.TryFinally(
                (fun () -> binder resource),
                (fun () ->
                    if not (obj.ReferenceEquals(resource, null)) then
                        resource.Dispose()
                )
            )

        member this.While
            (
                guard: unit -> bool,
                generator: unit -> Result<unit, 'error>
            ) : Result<unit, 'error> =

            let mutable doContinue = true
            let mutable result = Ok()

            while doContinue
                  && guard () do
                match generator () with
                | Ok () -> ()
                | Error e ->
                    doContinue <- false
                    result <- Error e

            result

        // member this.For
        //     (
        //         sequence: #seq<'T>,
        //         binder: 'T -> Result<unit, 'TError>
        //     ) : Result<unit, 'TError> =
        //     this.Using(
        //         sequence.GetEnumerator(),
        //         fun enum ->
        //             this.While(
        //                 (fun () -> enum.MoveNext()),
        //                 this.Delay(fun () -> binder enum.Current)
        //             )
        //     )

        member _.BindReturn
            (
                x: Result<'okInput, 'error>,
                f: 'okInput -> 'okOutput
            ) : Result<'okOutput, 'error> =
            Result.map f x

        member _.MergeSources
            (
                left: Result<'left, 'error>,
                right: Result<'right, 'error>
            ) : Result<'left * 'right, 'error> =
            match left, right with
            | Ok x1res, Ok x2res -> Ok(x1res, x2res)
            | Error e, _ -> Error e
            | _, Error e -> Error e

        /// <summary>
        /// Method lets us transform data types into our internal representation.  This is the identity method to recognize the self type.
        ///
        /// See https://stackoverflow.com/questions/35286541/why-would-you-use-builder-source-in-a-custom-computation-expression-builder
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        member _.Source(result: Result<'ok, 'error>) : Result<'ok, 'error> = result

    let result = ResultBuilder()

[<AutoOpen>]
module ResultCEExtensions =

    type ResultBuilder with

        /// <summary>
        /// Needed to allow `for..in` and `for..do` functionality
        /// </summary>
        member _.Source(s: #seq<_>) : #seq<_> = s


// Having Choice<_> members as extensions gives them lower priority in
// overload resolution and allows skipping more type annotations.
[<AutoOpen>]
module ResultCEChoiceExtensions =
    type ResultBuilder with

        /// <summary>
        /// Method lets us transform data types into our internal representation.
        /// </summary>
        /// <returns></returns>
        member _.Source(choice: Choice<'ok, 'error>) : Result<'ok, 'error> =
            match choice with
            | Choice1Of2 ok -> Ok ok
            | Choice2Of2 error -> Error error