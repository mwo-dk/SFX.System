module SFX.System.Windows

open SFX.ROP
open SFX.System.Infrastructure

module Encryption =
    let private service = EncryptionService(SecureStringService())

    type Salt = {Value: byte array}
    let createSalt x = {Value = x}
    let private fromSalt (x: SFX.System.Model.Salt) =
        {Value = x.Value}
    let private toSalt x = 
        let result = SFX.System.Model.Salt()
        result.Value <- x.Value
        result

    let encrypt x salt = service.Encrypt(x, salt |> toSalt) |> toResult
    let encryptString x salt = service.EncryptString(x, salt |> toSalt) |> toResult
    let encryptSecureString x salt = service.EncryptSecureString(x, salt |> toSalt) |> toResult

    let decrypt x salt = service.Decrypt(x, salt |> toSalt) |> toResult
    let decryptString x salt = service.DecryptString(x, salt |> toSalt) |> toResult
    let decryptSecureString x salt = service.DecryptSecureString(x, salt |> toSalt) |> toResult

module Registry =
    let internal service = RegistryReader()

    let getStringValue rootKey path name = 
        service.ReadStringValue(rootKey, path, name) |> toResult
    let getBlobValue rootKey path name =
        service.ReadBlobValue(rootKey, path, name) |> toResult

    let readClassesRootStringValue path name =
        service.ReadClassesRootStringValue(path, name) |> toResult
    let readClassesRootBlobValue path name =
        service.ReadClassesRootBlobValue(path, name) |> toResult

    let readCurrentConfigStringValue path name =
        service.ReadCurrentConfigStringValue(path, name) |> toResult
    let readCurrentConfigBlobValue path name =
        service.ReadCurrentConfigBlobValue(path, name) |> toResult

    let readCurrentUserStringValue path name =
        service.ReadCurrentUserStringValue(path, name) |> toResult
    let readCurrentUserBlobValue path name =
        service.ReadCurrentUserBlobValue(path, name) |> toResult

    let readLocalMachineStringValue path name =
        service.ReadLocalMachineStringValue(path, name) |> toResult
    let readLocalMachineBlobValue path name =
        service.ReadLocalMachineBlobValue(path, name) |> toResult

    let readPerformanceDataStringValue path name =
        service.ReadPerformanceDataStringValue(path, name) |> toResult
    let readPerformanceDataBlobValue path name =
        service.ReadPerformanceDataBlobValue(path, name) |> toResult

    let readUsersStringValue path name =
        service.ReadUsersStringValue(path, name) |> toResult
    let readUsersBlobValue path name =
        service.ReadUsersBlobValue(path, name) |> toResult

module Machine =
    let private machineGuidReader = MachineGuidReader(Registry.service)
    do machineGuidReader.Initialize()
    let private machineKeyReader = MachineKeyReader(machineGuidReader)

    let getMachineGuid() = machineGuidReader.GetMachineGuid() |> toResult
    let getMachineKey() = machineKeyReader.GetMachineKey() |> toResult

module Product =
    let private service = ProductIdReader(Registry.service) :> IProductIdReader
    do service.Initialize()

    let getProductId() = service.GetProductId() |> toResult
    let getDigitalProductId() = service.GetDigitalProductId() |> toResult

