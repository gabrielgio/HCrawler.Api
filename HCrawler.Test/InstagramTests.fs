module HCrawler.Test.InstagramTest

open System
open System.IO
open HCrawler.Core
open HCrawler.Core.Payloads
open FsUnit
open Xunit

let loadPost postName =
    let filePath = sprintf "Data/%s.json" postName

    filePath
    |> File.ReadAllText
    |> Instagram.parsePost

[<Theory>]
[<InlineData("image", "12/10/2019 5:28:26 PM")>]
[<InlineData("video", "4/28/2017 10:06:38 PM")>]
[<InlineData("carousel", "2/28/2020 2:42:05 PM")>]
let ``Get Post DateTime`` name dateTime =
    let post = loadPost name 
    let expectedDateTime = DateTime.Parse  dateTime

    Instagram.getPostDateTime post
    |> should equal expectedDateTime
    
[<Theory>]
[<InlineData("image", "maple.pepe_/2195960449254306067_21933169435.jpeg")>]
let ``Get Image Path`` name (path: string) =
   let post = loadPost name
   
   Instagram.getImagePath post
   |> should equal path
    
[<Theory>]
[<InlineData("video", "baesuicide/1503214870974369621_703072962.mp4")>]
let ``Get Video Path`` name (path: string) =
   let post = loadPost name
   
   Instagram.getVideoPath post
   |> should equal path
   
   
[<Theory>]
[<InlineData("carousel", 0, "gaeungbebe/2253858724269707438_873251263.jpeg")>]
[<InlineData("carousel", 1, "gaeungbebe/2253858773301191985_873251263.jpeg")>]
let ``Get Image Carousel Path`` name index (path: string) =
   let post = loadPost name
   
   Instagram.getImageCarouselPath post.User post.Post.CarouselMedia.[index]
   |> should equal path
   

[<Theory>]
[<InlineData("carousel", 0, "gaeungbebe/2253858724269707438_873251263.mp4")>]
[<InlineData("carousel", 1, "gaeungbebe/2253858773301191985_873251263.mp4")>]
let ``Get Video Carousel Path`` name index (path: string) =
   let post = loadPost name
   
   Instagram.getVideoCarouselPath post.User post.Post.CarouselMedia.[index]
   |> should equal path

   
[<Theory>]
[<InlineData("image", "https://www.instagram.com/p/B55nmTWnDET")>]
[<InlineData("video", "https://www.instagram.com/p/BTcffX1gw9V")>]
[<InlineData("carousel", "https://www.instagram.com/p/B9HUJ1lAc9b")>]
let ``Get Post Url`` name (url: string) =
    let post = loadPost name
    
    Instagram.getPostUrl post
    |> should equal url
    
[<Theory>]
[<InlineData("image", "https://www.instagram.com/maple.pepe_")>]
[<InlineData("video", "https://www.instagram.com/baesuicide")>]
[<InlineData("carousel", "https://www.instagram.com/gaeungbebe")>]
let ``Get Profile Url`` name (url: string) =
    let post = loadPost name
    
    Instagram.getProfileUrl post
    |> should equal url

[<Theory>]
[<InlineData(
    "image",
    "maple.pepe_/2195960449254306067_21933169435.jpeg",
    "https://www.instagram.com/p/B55nmTWnDET",
    "12/10/2019 5:28:26 PM",
    "maple.pepe_",
    "https://www.instagram.com/maple.pepe_"
)>]
let ``Get Image Payload`` name imagePath imageUrl dateTime profileName profileUrl =
    let post = loadPost name 
    
    let expectedPayload =
        { ImagePath = imagePath
          ImageUrl = imageUrl
          CreatedOn = DateTime.Parse dateTime
          ProfileName = profileName
          ProfileUrl =  profileUrl
          SourceName = "instagram"
          SourceUrl = "https://www.instagram.com" }
    
    Instagram.getImagePayload post
    |> should equal expectedPayload

[<Theory>]
[<InlineData(
    "video",
    "baesuicide/1503214870974369621_703072962.mp4",
    "https://www.instagram.com/p/BTcffX1gw9V",
    "4/28/2017 10:06:38 PM",
    "baesuicide",
    "https://www.instagram.com/baesuicide"
)>]
let ``Get Video Payload`` name imagePath imageUrl dateTime profileName profileUrl =
    let post = loadPost name 
    
    let expectedPayload =
        { ImagePath = imagePath
          ImageUrl = imageUrl
          CreatedOn = DateTime.Parse dateTime
          ProfileName = profileName
          ProfileUrl =  profileUrl
          SourceName = "instagram"
          SourceUrl = "https://www.instagram.com" }
    
    Instagram.getVideoPayload post
    |> should equal expectedPayload
    
    

[<Theory>]
[<InlineData(
    "carousel",
    0,
    "gaeungbebe/2253858724269707438_873251263.mp4",
    "https://www.instagram.com/p/B9HUJ1lAc9b",
    "2/28/2020 2:42:05 PM",
    "gaeungbebe",
    "https://www.instagram.com/gaeungbebe"
)>]
[<InlineData(
    "carousel",
    1,
    "gaeungbebe/2253858773301191985_873251263.jpeg",
    "https://www.instagram.com/p/B9HUJ1lAc9b",
    "2/28/2020 2:42:05 PM",
    "gaeungbebe",
    "https://www.instagram.com/gaeungbebe"
)>]
let ``Get Single Payload`` name index imagePath imageUrl dateTime profileName profileUrl =
    let post = loadPost name 
    
    let expectedPayload =
        { ImagePath = imagePath
          ImageUrl = imageUrl
          CreatedOn = DateTime.Parse dateTime
          ProfileName = profileName
          ProfileUrl =  profileUrl
          SourceName = "instagram"
          SourceUrl = "https://www.instagram.com" }
    
    Instagram.getSinglePayload post post.Post.CarouselMedia.[index]
    |> should equal expectedPayload
