module Tests

open System
open HCrawler.Core
open HCrawler.Core.Repositories
open HCrawler.Core.Repositories.Models
open Xunit
open Hcrawler.TestF.Mock
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
        image.CreateSourceIfNotExists(CreateImage(SourceName = sourceName))
        |> Async.AwaitTask
        |> Async.RunSynchronously

    newSourceId |> should equal sourceId
    mock.VerifyAll()

[<Theory>]
[<InlineData("demo", 1, "http://localhost/")>]
let ``Create Source If Not Exists`` sourceName sourceId sourceUrl =
    let mock = mockImage
    let storeSource = StoreSource (sourceName, sourceUrl)
    
    let imageRepo =
        mock
        |> mockSourceExistsAsync sourceName false
        |> mockStoreSourceAsync storeSource sourceId
        |> spawn
    
    let image = Image(imageRepo)
    
    let newSourceId =
        image.CreateSourceIfNotExists(CreateImage(SourceName = sourceName, SourceUrl= sourceUrl))
        |> Async.AwaitTask
        |> Async.RunSynchronously
        
    newSourceId |> should equal sourceId
    mock.VerifyAll()

[<Theory>]
[<InlineData("source name", 1, "http://localhost/", "profile name", 2)>]
let ``Create Profile If Not Exists`` sourceName sourceId url profileName profileId =
   let mock = mockImage
   let storeProfile = StoreProfile(sourceId, profileName, url)
   
   let imageRepo =
       mock
       |> mockSourceExistsAsync sourceName true
       |> mockGetSourceIdByNameAsync sourceName sourceId
       |> mockProfileExistsAsync profileName false
       |> mockStoreProfileAsync storeProfile profileId
       |> spawn
       
   let image = Image(imageRepo)
  
   let newProfileId =
       CreateImage(SourceName=sourceName, ProfileName=profileName, ProfileUrl = url)
       |> image.CreateProfileIfNotExistsAsync
       |> Async.AwaitTask
       |> Async.RunSynchronously
   
   newProfileId |> should equal profileId
   mock.VerifyAll()
   

[<Theory>]
[<InlineData("/home/", "demo", 2, "profile Name", 3)>]
let ``Create Image If Exists`` path sourceName sourceId profileName profileId =
    let mock = mockImage
    
    let imageRepo =
        mock
        |> mockSourceExistsAsync sourceName true
        |> mockGetSourceIdByNameAsync sourceName sourceId
        |> mockProfileExistsAsync profileName true
        |> mockGetProfileIdByNameAsync profileName profileId
        |> mockImageExistsAsync path true
        |> spawn
        
    let image = Image(imageRepo)
    
    CreateImage(ImagePath=path, CreatedOn=DateTime.Now, SourceName=sourceName, ProfileName=profileName)
    |> image.CreateImageIfNotExistsAsync
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> ignore
    
    mock.VerifyAll()
