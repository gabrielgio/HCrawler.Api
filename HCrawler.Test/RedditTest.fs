module HCrawler.Test.RedditTest

open System
open System.IO
open HCrawler.Core
open HCrawler.Core.Payloads
open FsUnit
open Xunit


let loadPost postName =
    let filePath = sprintf "Data/Reddit/%s.json" postName

    filePath
    |> File.ReadAllText
    |> Reddit.parsePost

[<Theory>]
[<InlineData("redd_jpeg", "https://old.reddit.com/r/kpics/")>]
let ``Get Profile Url`` name (url: string) =
    let post = loadPost name

    Reddit.getProfileUrl post
    |> should equal url
    
[<Theory>]
[<InlineData("redd_jpeg", "4/9/2020 4:05:56 PM")>]
let ``Get Post DateTime`` name dateTime =
    let post = loadPost name
    let expectedDateTime = DateTime.Parse  dateTime

    Reddit.getPostDateTime post
    |> should equal expectedDateTime
    
[<Theory>]
[<InlineData("redd_jpeg", "https://old.reddit.com/r/kpics/comments/fxogjy/xuanyi/")>]
let ``Get Full Permalink`` name (permalink: string) =
    let post = loadPost name

    Reddit.getFullPermalink post
    |> should equal permalink
  
[<Theory>]
[<InlineData("redd_jpeg", "kpics/fxogjy.jpg")>]
let ``Get Image Path`` name (path: string) =
   let post = loadPost name
   
   Reddit.getImagePath post
   |> should equal path

[<Theory>]
[<InlineData("redd_jpeg", true)>]
let ``Is Known`` name known =
    let post = loadPost name

    Reddit.isKnown post
    |> should equal known
    

[<Theory>]
[<InlineData("redd_jpeg", "reddit/kpics/fxogjy.jpg", "https://i.redd.it/pjj1ll1b2rr41.jpg")>]
let ``Get Download Post`` name path url =
    let post = loadPost name
    let expectedDownload =
            { Path = path
              Url = url }
    

    Reddit.getDownloadPost post
    |> should equal expectedDownload
    
[<Theory>]
[<InlineData(
    "redd_jpeg",
    "kpics/fxogjy.jpg",
    "https://old.reddit.com/r/kpics/comments/fxogjy/xuanyi/",
    "4/9/2020 4:05:56 PM",
    "kpics",
    "https://old.reddit.com/r/kpics/"
)>]
let ``Get Payload`` name imagePath imageUrl dateTime profileName profileUrl =
    let post = loadPost name

    let expectedPayload =
        { ImagePath = imagePath
          ImageUrl = imageUrl
          CreatedOn = DateTime.Parse dateTime
          ProfileName = profileName
          ProfileUrl =  profileUrl
          SourceName = "reddit"
          SourceUrl = "https://old.reddit.com" }
        
    Reddit.getPayload post
    |> should equal expectedPayload
