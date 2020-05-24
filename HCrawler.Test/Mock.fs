module HCrawler.Test.Mock

open System.Collections.Generic
open HCrawler.Core
open HCrawler.Core.Payloads
open HCrawler.Core.Proxies
open Moq


let mockImage =
    Mock<IImageRepository>(MockBehavior.Strict)
    
let spawn (source:Mock<IImageRepository>) =
    source.Object

let mockGetAllAsync pageFilter (result:IEnumerable<Image>) (source:Mock<IImageRepository>) =
   source
       .Setup(fun x -> x.getAllAsync(pageFilter))
       .ReturnsAsync(result)
   |> ignore
   source
   
let mockProfileExistsAsync profileName (result:bool) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.profileExistsAsync(profileName))
        .ReturnsAsync result
    |> ignore
    source
   
let mockSourceExistsAsync sourceName (result:bool) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.sourceExistsAsync(sourceName))
        .ReturnsAsync result
    |> ignore
    source
    
let mockImageExistsAsync imagePath (result: bool) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.imageExistsAsync(imagePath))
        .ReturnsAsync result
    |> ignore
    source
    
let mockStoreProfileAsync (storeProfile: StoreProfile) (result: int) (source: Mock<IImageRepository>) =
    let name = storeProfile.Name
    let url = storeProfile.Url
    let sourceId = storeProfile.SourceId
    source
        .Setup(fun x -> x.storeProfileAsync(It.Is(fun (y: StoreProfile) ->
            y.Name = name &&
            y.Url = url &&
            y.SourceId = sourceId)))
        .ReturnsAsync result
     |> ignore
    source
    
let mockStoreSourceAsync (storeSource: StoreSource) (result: int) (source: Mock<IImageRepository>) =
    let name = storeSource.Name
    let url = storeSource.Url
    source
        .Setup(fun x -> x.storeSourceAsync(It.Is(fun (y: StoreSource) ->
            y.Name = name &&
            y.Url = url)))
        .ReturnsAsync result
    |> ignore
    source


let mockStoreImageAsync (storeImage: StoreImage) (result: int) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.storeImageAsync(It.Is(fun (y: StoreImage) ->
            y.Path = storeImage.Path &&
            y.ProfileId = storeImage.ProfileId &&
            y.CreatedOn = storeImage.CreatedOn &&
            y.Url = storeImage.Url)))
        .ReturnsAsync result
    |> ignore
    source
    
    
let mockGetProfileIdByNameAsync profileName (result: int) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.getProfileIdByNameAsync(profileName))
        .ReturnsAsync(result)
    |> ignore
    source
    
let mockGetSourceIdByNameAsync sourceName (result: int) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.getSourceIdByNameAsync(sourceName))
        .ReturnsAsync(result)
    |> ignore
    source
    
  