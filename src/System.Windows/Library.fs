module SFX.System.Windows

open SFX.ROP
open SFX.System.Infrastructure

module Encryption =
    let private service = EncryptionService(SecureStringService())

    /// Represents salt for encryption
    type Salt = {Value: byte array}
    /// Create Salt
    let createSalt x = {Value = x}
    let private toSalt x = 
        let result = SFX.System.Model.Salt()
        result.Value <- x.Value
        result

    /// Encrypts the provided binary content utilizing the salt
    let encrypt x salt = service.Encrypt(x, salt |> toSalt) |> toResult
    /// Encrypts the provided string utilizing the salt
    let encryptString x salt = service.EncryptString(x, salt |> toSalt) |> toResult
    /// Encrypts the provided SecureString utilizing the salt
    let encryptSecureString x salt = service.EncryptSecureString(x, salt |> toSalt) |> toResult

    /// Decrypts the provided emcrypted binary content utilizing the salt
    let decrypt x salt = service.Decrypt(x, salt |> toSalt) |> toResult
    /// Decrypts the provided encrypted string utilizing the salt
    let decryptString x salt = service.DecryptString(x, salt |> toSalt) |> toResult
    /// Decrypts the provided encrypted SecureString utilizing the salt
    let decryptSecureString x salt = service.DecryptSecureString(x, salt |> toSalt) |> toResult

module Registry =
    let internal service = RegistryReader()

    /// Reads a string value from the registry
    let getStringValue rootKey path name = 
        service.ReadStringValue(rootKey, path, name) |> toResult
    /// Reads a blob value from the registry
    let getBlobValue rootKey path name =
        service.ReadBlobValue(rootKey, path, name) |> toResult

    /// Reads a string value from the registry
    let readClassesRootStringValue path name =
        service.ReadClassesRootStringValue(path, name) |> toResult
    /// Reads a blob value from the registry
    let readClassesRootBlobValue path name =
        service.ReadClassesRootBlobValue(path, name) |> toResult

    /// Reads a string value from the registry
    let readCurrentConfigStringValue path name =
        service.ReadCurrentConfigStringValue(path, name) |> toResult
    /// Reads a blob value from the registry
    let readCurrentConfigBlobValue path name =
        service.ReadCurrentConfigBlobValue(path, name) |> toResult

    /// Reads a string value from the registry
    let readCurrentUserStringValue path name =
        service.ReadCurrentUserStringValue(path, name) |> toResult
    /// Reads a blob value from the registry
    let readCurrentUserBlobValue path name =
        service.ReadCurrentUserBlobValue(path, name) |> toResult

    /// Reads a string value from the registry
    let readLocalMachineStringValue path name =
        service.ReadLocalMachineStringValue(path, name) |> toResult
    /// Reads a blob value from the registry
    let readLocalMachineBlobValue path name =
        service.ReadLocalMachineBlobValue(path, name) |> toResult

    /// Reads a string value from the registry
    let readPerformanceDataStringValue path name =
        service.ReadPerformanceDataStringValue(path, name) |> toResult
    /// Reads a blob value from the registry
    let readPerformanceDataBlobValue path name =
        service.ReadPerformanceDataBlobValue(path, name) |> toResult

    /// Reads a string value from the registry
    let readUsersStringValue path name =
        service.ReadUsersStringValue(path, name) |> toResult
    /// Reads a blob value from the registry
    let readUsersBlobValue path name =
        service.ReadUsersBlobValue(path, name) |> toResult

module Machine =
    let private machineGuidReader = MachineGuidReader(Registry.service)
    do machineGuidReader.Initialize()
    let private machineKeyReader = MachineKeyReader(machineGuidReader)

    /// Reads the machine Guid
    let getMachineGuid() = machineGuidReader.GetMachineGuid() |> toResult
    /// Reads the machine key
    let getMachineKey() = machineKeyReader.GetMachineKey() |> toResult

module Product =
    let private service = ProductIdReader(Registry.service) :> IProductIdReader
    do service.Initialize()

    /// Reads the product id
    let getProductId() = service.GetProductId() |> toResult
    /// Reads the digital product id
    let getDigitalProductId() = service.GetDigitalProductId() |> toResult

