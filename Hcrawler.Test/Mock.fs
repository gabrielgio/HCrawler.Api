module Hcrawler.TestF.Mock

open System.Collections.Generic
open HCrawler.Core.Repositories
open HCrawler.Core.Repositories.Models
open Moq


let mockImage =
    Mock<IImageRepository>(MockBehavior.Strict)
    
let spawn (source:Mock<IImageRepository>) =
    source.Object

let mockGetAllAsync pageFilter (result:IEnumerable<DetailedImage>) (source:Mock<IImageRepository>) =
   source
       .Setup(fun x -> x.GetAll(pageFilter))
       .ReturnsAsync(result)
   |> ignore
   source
   
let mockProfileExistsAsync profileName (result:bool) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.ProfileExistsAsync(profileName))
        .ReturnsAsync result
    |> ignore
    source
   
let mockSourceExistsAsync sourceName (result:bool) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.SourceExistsAsync(sourceName))
        .ReturnsAsync result
    |> ignore
    source
    
let mockImageExistsAsync imagePath (result: bool) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.ImageExistsAsync(imagePath))
        .ReturnsAsync result
    |> ignore
    source
    
let mockStoreProfileAsync (storeProfile: StoreProfile) (result: int) (source: Mock<IImageRepository>) =
    let name = storeProfile.Name
    let url = storeProfile.Url
    let sourceId = storeProfile.SourceId
    source
        .Setup(fun x -> x.StoreProfileAsync(It.Is(fun (y: StoreProfile) ->
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
        .Setup(fun x -> x.StoreSourceAsync(It.Is(fun (y: StoreSource) ->
            y.Name = name &&
            y.Url = url)))
        .ReturnsAsync result
    |> ignore
    source
    
let mockGetProfileIdByNameAsync profileName (result: int) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.GetProfileIdByNameAsync(profileName))
        .ReturnsAsync(result)
    |> ignore
    source
    
let mockGetSourceIdByNameAsync sourceName (result: int) (source: Mock<IImageRepository>) =
    source
        .Setup(fun x -> x.GetSourceIdByNameAsync(sourceName))
        .ReturnsAsync(result)
    |> ignore
    source
    
  