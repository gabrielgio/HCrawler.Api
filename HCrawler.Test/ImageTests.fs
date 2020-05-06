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
        image.CreateSourceIfNotExistsAsync(CreateImage(SourceName = sourceName))
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
        image.CreateSourceIfNotExistsAsync(CreateImage(SourceName = sourceName, SourceUrl = sourceUrl))
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
        CreateImage(SourceName = sourceName, ProfileName = profileName, ProfileUrl = url)
        |> image.CreateProfileIfNotExistsAsync
        |> Async.AwaitTask
        |> Async.RunSynchronously

    newProfileId |> should equal profileId
    mock.VerifyAll()


[<Theory>]
[<InlineData("/home/", "demo", 2, "profile Name", 3, "http://localhost/")>]
let ``Create Image If Exists`` path sourceName sourceId profileName profileId url =
    let mock = mockImage
    let dateTime = DateTime.Now

    let imageRepo =
        mock
        |> mockProfileExistsAsync profileName true
        |> mockGetProfileIdByNameAsync profileName profileId
        |> mockImageExistsAsync path false
        |> mockImageSourceAsync {ProfileId=profileId; Path=path; Url=url; CreatedOn=dateTime} 0
        |> spawn

    let image = Image(imageRepo)

    CreateImage(ImagePath = path, CreatedOn = dateTime, SourceName = sourceName, ProfileName = profileName, ImageUrl = url)
    |> image.CreateImageIfNotExistsAsync
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> ignore

    mock.VerifyAll()
