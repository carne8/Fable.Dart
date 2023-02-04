namespace Fable.Dart

open Fable.Core
open System.Runtime.InteropServices

type IEncoding = interface end
type IUriData = interface end

/// https://api.dart.dev/dart-core/Uri-class.html
[<ImportMember "dart:core">]
type Uri [<NamedParams>]
    ([<Optional>] scheme: string,
     [<Optional>] userInfo: string,
     [<Optional>] host: string,
     [<Optional>] port: int,
     [<Optional>] path: string,
     [<Optional>] pathSegments: string seq,
     [<Optional>] query: string,
     [<Optional>] queryParameters: Map<string, _>,
     [<Optional>] fragment: string) =
    static member http(_host: string) : Uri = nativeOnly
    static member https(_host: string) : Uri = nativeOnly
    static member directory(_path: string) : Uri = nativeOnly
    static member file(_path: string) : Uri = nativeOnly
    /// Creates a data: URI containing an encoding of bytes.
    static member dataFromBytes(_bytes: int list, ?_mimeType: string, ?_parameters: Map<string, string>, ?_percentEncoded: bool) : Uri = nativeOnly
    /// Creates a data: URI containing the content string.
    static member dataFromString(_content: string, ?_mimeType: string, ?_encoding: IEncoding, ?_parameters: Map<string, string>, ?_base64: bool) : Uri = nativeOnly
    /// Like Uri.file except that a non-empty URI path ends in a slash.
    static member directory(_path: string, ?_windows: bool) : Uri = nativeOnly
    /// Creates a new file URI from an absolute or relative file path.
    static member file(_path: string, ?_windows: bool) : Uri = nativeOnly
    /// Creates a new http URI from authority, path and query.
    static member http(_authority: string, ?_unencodedPath: string, ?_queryParameters: Map<string, _>) : Uri = nativeOnly
    /// Creates a new https URI from authority, path and query.
    static member https(_authority: string, ?_unencodedPath: string, ?_queryParameters: Map<string, _>) : Uri = nativeOnly

    /// The authority component.
    member _.authority : string = nativeOnly
    /// Access the structure of a data: URI.
    member _.data : IUriData option = nativeOnly
    /// The fragment identifier component.
    member _.fragment : string = nativeOnly
    /// Whether the URI has an absolute path (starting with '/').
    member _.hasAbsolutePath : bool = nativeOnly
    /// Whether the URI has an authority component.
    member _.hasAuthority : bool = nativeOnly
    /// Whether the URI has an empty path.
    member _.hasEmptyPath : bool = nativeOnly
    /// Whether the URI has a fragment part.
    member _.hasFragment : bool = nativeOnly
    /// Returns a hash code computed as tostring().hashCode.
    member _.hashCode : int = nativeOnly
    /// Whether the URI has an explicit port.
    member _.hasPort : bool = nativeOnly
    /// Whether the URI has a query part.
    member _.hasQuery : bool = nativeOnly
    /// Whether the URI has a scheme component.
    member _.hasScheme : bool = nativeOnly
    /// The host part of the authority component.
    member _.host : string = nativeOnly
    /// Whether the URI is absolute.
    member _.isAbsolute : bool = nativeOnly
    /// Returns the origin of the URI in the form scheme://host:port for the schemes http and https.
    member _.origin : string = nativeOnly
    /// The path component.
    member _.path : string = nativeOnly
    /// The URI path split into its segments.
    member _.pathSegments : List<string> = nativeOnly
    /// The port part of the authority component.
    member _.port : int = nativeOnly
    /// The query component.
    member _.query : string = nativeOnly
    /// The URI query split into a map according to the rules specified for FORM post in the HTML 4.01 specification section 17.13.4.
    member _.queryParameters : Map<string, string> = nativeOnly
    /// Returns the URI query split into a map according to the rules specified for FORM post in the HTML 4.01 specification section 17.13.4.
    member _.queryParametersAll : Map<string, List<string>> = nativeOnly
    /// The scheme component of the URI.
    member _.scheme : string = nativeOnly
    /// The user info part of the authority component.
    member _.userInfo : string = nativeOnly