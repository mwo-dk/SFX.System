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

    let folderExists folder = service.FolderExists(folder) |> toResult
    let createRolder folder = 
        match service.CreateFolder(folder) |> toResult with
        | Success _ -> () |> succeed
        | Failure exn -> exn |> fail
    let clearFolder folder = 
        match service.ClearFolder(folder) |> toResult with
        | Success _ -> () |> succeed
        | Failure exn -> exn |> fail

    let fileExits file = service.FileExists(file) |> toResult
    let getFiles folder = service.GetFiles(folder) |> toResult

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
        match service.DeleteFile(file) |> toResult with
        | Success _ -> () |> succeed
        | Failure exn -> exn |> fail

    let readBinaryContent file = service.ReadFileBinaryContent file |> toResult
    let readStringContent file = service.ReadFileStringContent file |> toResult
    let readBinaryContentAsync file = service.ReadFileBinaryContentAsync file |> Async.AwaitTask |> Async.RunSynchronously |> toResult
    let readStringContentAsync file = service.ReadFileStringContentAsync file |> Async.AwaitTask |> Async.RunSynchronously |> toResult

module SecureString =

    let service = SecureStringService()

    let toSecureString x = service.ToSecureString(x) |> toResult
    let toInsecureString x = service.ToInsecureString(x) |> toResult