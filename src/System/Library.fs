namespace SFX.System

open SFX.ROP
open SFX.System.Infrastructure

module Assembly =
    let private service = AssemblyService()
    let getExeFilePath() = service.GetExeFilePath() |> toResult

module Base64 =
    let private service = Base64Service()

    let toBase64 x = service.ToBase64String(x) |> toResult
    let fromBase64 x = service.FromBase64String(x) |> toResult

module IO =
    let private service = FileSystemService()

    type FilePath = {Value: string}
    let createFilePath x = {Value = x}
    let private fromFilePath (x: SFX.System.Model.FilePath) =
        {Value = x.Value}
    let private toFilePath x = 
        let result = SFX.System.Model.FilePath()
        result.Value <- x.Value
        result
    type FolderPath = {Value: string}
    let createFolderPath x = {Value = x}
    let private fromFolderPath (x: SFX.System.Model.FolderPath) =
        {Value = x.Value}
    let private toFolderPath x = 
        let result = SFX.System.Model.FolderPath()
        result.Value <- x.Value
        result

    let folderExists folder = service.FolderExists(folder |> toFolderPath) |> toResult
    let createRolder folder = 
        match service.CreateFolder(folder |> toFolderPath) |> toResult with
        | Success _ -> () |> succeed
        | Failure exn -> exn |> fail
    let clearFolder folder = 
        match service.ClearFolder(folder |> toFolderPath) |> toResult with
        | Success _ -> () |> succeed
        | Failure exn -> exn |> fail

    let fileExits file = service.FileExists(file |> toFilePath) |> toResult
    let getFiles folder = 
        match service.GetFiles(folder |> toFolderPath) |> toResult with
        | Success result -> result |> Seq.map fromFilePath |> succeed
        | Failure exn -> exn |> fail

    type Content =
    | B of byte array
    | S of string
    let createFile file content =
        let run() =
            match content with
            | B data -> service.CreateFile(file, data) |> toResult
            | S data -> service.CreateFile(file, data) |> toResult
        match run() with
        | Success _ -> () |> succeed
        | Failure exn -> exn |> fail
    let createFileAsync file content =
        let run() =
            match content with
            | B data -> service.CreateFileAsync(file, data) |> Async.AwaitTask |> Async.RunSynchronously |> toResult
            | S data -> service.CreateFileAsync(file, data) |> Async.AwaitTask |> Async.RunSynchronously |> toResult
        match run() with
        | Success _ -> () |> succeed
        | Failure exn -> exn |> fail

    let deleteFile file =
        match service.DeleteFile(file |> toFilePath) |> toResult with
        | Success _ -> () |> succeed
        | Failure exn -> exn |> fail

    let readBinaryContent file = service.ReadFileBinaryContent (file |> toFilePath) |> toResult
    let readStringContent file = service.ReadFileStringContent (file |> toFilePath) |> toResult
    let readBinaryContentAsync file = service.ReadFileBinaryContentAsync (file |> toFilePath) |> Async.AwaitTask |> Async.RunSynchronously |> toResult
    let readStringContentAsync file = service.ReadFileStringContentAsync (file |> toFilePath) |> Async.AwaitTask |> Async.RunSynchronously |> toResult

module SecureString =

    let service = SecureStringService()

    let toSecureString x = service.ToSecureString(x) |> toResult
    let toInsecureString x = service.ToInsecureString(x) |> toResult