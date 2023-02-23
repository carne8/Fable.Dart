module Fable.Dart.IO

open Fable.Core
open Fable.Dart
open Fable.Dart.Future

type [<ImportMember "dart:io">] FileSystemEntity() =
    member _.delete (recursive: bool) : Future<FileSystemEntity> = nativeOnly
    member _.deleteSync (recursive: bool) : unit = nativeOnly
    member _.exists () : Future<bool> = nativeOnly
    member _.existsSync () : bool = nativeOnly
    member _.absolute: FileSystemEntity = nativeOnly
    member _.isAbsolute: bool = nativeOnly
    member _.parent: FileSystemEntity = nativeOnly
    member _.path: string = nativeOnly
    member _.uri: Uri = nativeOnly

type [<ImportMember "dart:io">] Directory(path: string) =
    inherit FileSystemEntity()

    static member fromRawPath (path: byte seq) : Directory = nativeOnly
    static member fromUri (uri: Uri) : Directory = nativeOnly
    member _.absolute: Directory = nativeOnly
    member _.parent: Directory = nativeOnly
    [<NamedParams>] member _.create (recursive: bool) : Future<Directory> = nativeOnly
    [<NamedParams>] member _.createSync (recursive: bool) : unit = nativeOnly
    [<NamedParams>] member _.createTemp ([<OptionalArgument>] prefix: string) : Future<Directory> = nativeOnly
    [<NamedParams>] member _.createTempSync ([<OptionalArgument>] prefix: string) : Directory = nativeOnly
    [<NamedParams>] member _.listSync ([<OptionalArgument>] recursive: bool, [<OptionalArgument>] followLinks: bool) : FileSystemEntity array = nativeOnly
    [<NamedParams>] member _.rename (newPath: string) : Future<Directory> = nativeOnly
    [<NamedParams>] member _.renameSync (newPath: string) : Directory = nativeOnly

type [<ImportMember "dart:io">] File(path: string) =
    inherit FileSystemEntity()

    static member fromRawPath (path: byte seq) : File = nativeOnly
    static member fromUri (uri: Uri) : File = nativeOnly
    member _.absolute: File = nativeOnly
    member _.parent: File = nativeOnly

    /// Copies this file.
    member _.copy (newPath: string) : Future<File> = nativeOnly
    /// Synchronously copies this file.
    member _.copySync (newPath: string) : File = nativeOnly
    /// Creates the file.
    [<NamedParams>] member _.create ([<OptionalArgument>] recursive: bool, [<OptionalArgument>] exclusive: bool) : Future<File> = nativeOnly
    /// Synchronously creates the file.
    [<NamedParams>] member _.createSync ([<OptionalArgument>] recursive: bool, [<OptionalArgument>] exclusive: bool) : unit = nativeOnly
    /// The last-accessed time of the file.
    member _.lastAccessed () : Future<System.DateTime> = nativeOnly
    /// The last-accessed time of the file.
    member _.lastAccessedSync () : System.DateTime = nativeOnly
    /// Get the last-modified time of the file.
    member _.lastModified () : Future<System.DateTime> = nativeOnly
    /// Get the last-modified time of the file.
    member _.lastModifiedSync () : System.DateTime = nativeOnly
    /// The length of the file.
    member _.length () : Future<int> = nativeOnly
    /// The length of the file provided synchronously.
    member _.lengthSync () : int = nativeOnly
    /// Opens the file for random access operations.
//     member _.open ({FileMode mode = FileMode.read}) : Future<RandomAccessFile> = nativeOnly
//     /// Creates a new independent Stream for the contents of this file.
//     member _.openRead ([int? start, int? end]) : Stream<List<int>> = nativeOnly
//     /// Synchronously opens the file for random access operations.
//     member _.openSync ({FileMode mode = FileMode.read}) : RandomAccessFile = nativeOnly
//     /// Creates a new independent IOSink for the file.
//     member _.openWrite ({FileMode mode = FileMode.write, Encoding encoding = utf8}) : IOSink = nativeOnly
//     /// Reads the entire file contents as a list of bytes.
//     member _.readAsBytes () : Future<Uint8List> = nativeOnly
//     /// Synchronously reads the entire file contents as a list of bytes.
//     member _.readAsBytesSync () : Uint8List = nativeOnly
//     /// Reads the entire file contents as lines of text using the given Encoding.
//     member _.readAsLines ({Encoding encoding = utf8}) : Future<List<String>> = nativeOnly
//     /// Synchronously reads the entire file contents as lines of text using the given Encoding.
//     member _.readAsLinesSync ({Encoding encoding = utf8}) : List<String> = nativeOnly
//     /// Reads the entire file contents as a string using the given Encoding.
//     member _.readAsString ({Encoding encoding = utf8}) : Future<String> = nativeOnly
//     /// Synchronously reads the entire file contents as a string using the given Encoding.
//     member _.readAsStringSync ({Encoding encoding = utf8}) : String = nativeOnly
//     /// Renames this file.
//     member _.rename (String newPath) : Future<File> = nativeOnly
// // override
//     /// Synchronously renames this file.
//     member _.renameSync (String newPath) : File = nativeOnly
// // override
//     /// Resolves the path of a file system object relative to the current working directory.
//     member _.resolveSymbolicLinks () : Future<String> = nativeOnly
// // inherited
//     /// Resolves the path of a file system object relative to the current working directory.
//     member _.resolveSymbolicLinksSync () : String = nativeOnly
// // inherited
//     /// Modifies the time the file was last accessed.
//     member _.setLastAccessed (System.DateTime time) : Future = nativeOnly
//     /// Synchronously modifies the time the file was last accessed.
//     member _.setLastAccessedSync (System.DateTime time) : void = nativeOnly
//     /// Modifies the time the file was last modified.
//     member _.setLastModified (System.DateTime time) : Future = nativeOnly
//     /// Synchronously modifies the time the file was last modified.
//     member _.setLastModifiedSync (System.DateTime time) : void = nativeOnly
//     /// Calls the operating system's stat() function on path.
//     member _.stat () : Future<FileStat> = nativeOnly
// // inherited
//     /// Synchronously calls the operating system's stat() function on path.
//     member _.statSync () : FileStat = nativeOnly
// // inherited
//     /// A string representation of this object.
//     member _.toString () : String = nativeOnly
// // inherited
//     /// Start watching the FileSystemEntity for changes.
//     member _.watch ({int events = FileSystemEvent.all, bool recursive = false}) : Stream<FileSystemEvent> = nativeOnly
// // inherited
//     /// Writes a list of bytes to a file.
//     member _.writeAsBytes (List<int> bytes, {FileMode mode = FileMode.write, bool flush = false}) : Future<File> = nativeOnly
//     /// Synchronously writes a list of bytes to a file.
//     member _.writeAsBytesSync (List<int> bytes, {FileMode mode = FileMode.write, bool flush = false}) : void = nativeOnly
//     /// Writes a string to a file.
//     member _.writeAsString (String contents, {FileMode mode = FileMode.write, Encoding encoding = utf8, bool flush = false}) : Future<File> = nativeOnly
//     /// Synchronously writes a string to a file.
//     member _.writeAsStringSync (String contents, {FileMode mode = FileMode.write, Encoding encoding = utf8, bool flush = false}) : void = nativeOnly