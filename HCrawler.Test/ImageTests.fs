module HCrawler.Test.ImageTests

open System
open HCrawler.Core.Image
open HCrawler.Core.Payloads
open Xunit
open HCrawler.Test.Mock
open FsUnit
open Xunit


[<Theory>]
[<InlineData("demo", 1)>]
let ``Create Source If Exist`` sourceName sourceId =
    let mock = mockImage

    let imageRepo =
        mock
        |> mockSourceExistsAsync sourceName true
        |> mockGetSourceIdByNameAsync sourceName sourceId
        |> spawn

    let image = Image(imageRepo)

    let newSourceId =
        { ImagePath = String.Empty
          CreatedOn = DateTime.Now
          SourceName = sourceName
          ProfileName = String.Empty
          ImageUrl = String.Empty
          ProfileUrl = String.Empty
          SourceUrl = String.Empty }
        |> image.createSourceIfNotExistsAsync
        |> Async.AwaitTask
        |> Async.RunSynchronously

    newSourceId |> should equal sourceId
    mock.VerifyAll()

[<Theory>]
[<InlineData("demo", 1, "http://localhost/")>]
let ``Create Source If Not Exists`` sourceName sourceId sourceUrl =
    let mock = mockImage

    let storeSource =
        { Name = sourceName
          Url = sourceUrl }

    let imageRepo =
        mock
        |> mockSourceExistsAsync sourceName false
        |> mockStoreSourceAsync storeSource sourceId
        |> spawn

    let image = Image(imageRepo)

    let newSourceId =
        { ImagePath = String.Empty
          CreatedOn = DateTime.Now
          SourceName = sourceName
          ProfileName = String.Empty
          ImageUrl = String.Empty
          ProfileUrl = String.Empty
          SourceUrl = sourceUrl }
        |> image.createSourceIfNotExistsAsync
        |> Async.AwaitTask
        |> Async.RunSynchronously

    newSourceId |> should equal sourceId
    mock.VerifyAll()

[<Theory>]
[<InlineData("source name", 1, "http://localhost/", "profile name", 2)>]
let ``Create Profile If Not Exists`` sourceName sourceId url profileName profileId =
    let mock = mockImage

    let storeProfile =
        { SourceId = sourceId
          Name = profileName
          Url = url }

    let imageRepo =
        mock
        |> mockSourceExistsAsync sourceName true
        |> mockGetSourceIdByNameAsync sourceName sourceId
        |> mockProfileExistsAsync profileName false
        |> mockStoreProfileAsync storeProfile profileId
        |> spawn

    let image = Image(imageRepo)

    let newProfileId =

        { ImagePath = String.Empty
          CreatedOn = DateTime.Now
          SourceName = sourceName
          ProfileName = profileName 
          ImageUrl = url
          ProfileUrl = url
          SourceUrl = url }
        |> image.createProfileIfNotExistsAsync
        |> Async.AwaitTask
        |> Async.RunSynchronously

    newProfileId |> should equal profileId
    mock.VerifyAll()


[<Theory>]
[<InlineData("source name", 1, "http://localhost/", "profile name", 2)>]
let ``Create Profile If Exists`` sourceName sourceId url profileName profileId =
    let mock = mockImage

    let storeProfile =
        { SourceId = sourceId
          Name = profileName
          Url = url }

    let imageRepo =
        mock
        |> mockProfileExistsAsync profileName true
        |> mockGetProfileIdByNameAsync profileName profileId
        |> spawn

    let image = Image(imageRepo)

    let newProfileId =
        { ImagePath = String.Empty
          CreatedOn = DateTime.Now
          SourceName = sourceName
          ProfileName = profileName
          ImageUrl = url
          ProfileUrl = url
          SourceUrl = url }
        |> image.createProfileIfNotExistsAsync
        |> Async.AwaitTask
        |> Async.RunSynchronously

    newProfileId |> should equal profileId
    mock.VerifyAll()


[<Theory>]
[<InlineData("/home/", "demo", 2, "profile Name", 3, "http://localhost/")>]
let ``Create Image If Not Exists`` path sourceName sourceId profileName profileId url =
    let mock = mockImage
    let dateTime = DateTime.Now

    let imageRepo =
        mock
        |> mockProfileExistsAsync profileName true
        |> mockGetProfileIdByNameAsync profileName profileId
        |> mockImageExistsAsync path false
        |> mockStoreImageAsync
            { ProfileId = profileId
              Path = path
              Url = url
              CreatedOn = dateTime } 0
        |> spawn

    let image = Image(imageRepo)

    { ImagePath = path
      CreatedOn = dateTime
      SourceName = sourceName
      ProfileName = profileName
      ImageUrl = url
      ProfileUrl = url
      SourceUrl = url }
    |> image.createImageIfNotExistsAsync
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> ignore

    mock.VerifyAll()

[<Theory>]
[<InlineData("/home/", "demo", 2, "profile Name", 3, "http://localhost/")>]
let ``Create Image If Exists`` path sourceName sourceId profileName profileId url =
    let mock = mockImage
    let dateTime = DateTime.Now

    let imageRepo =
        mock
        |> mockImageExistsAsync path true
        |> spawn

    let image = Image(imageRepo)

    { ImagePath = path
      CreatedOn = dateTime
      SourceName = sourceName
      ProfileName = profileName
      ImageUrl = url
      ProfileUrl = url
      SourceUrl = url }
    |> image.createImageIfNotExistsAsync
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> ignore

    mock.VerifyAll()
