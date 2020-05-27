module HCrawler.Test.RedditTest

open System
open System.IO
open HCrawler.Core
open HCrawler.Core.Payloads
open FsUnit
open HCrawler.Core.Reddit
open Xunit


let loadPost postName =
    let filePath = sprintf "Data/Reddit/%s.json" postName

    filePath
    |> File.ReadAllText
    |> Reddit.parsePost

[<Theory>]
[<InlineData("redd_jpeg", "https://old.reddit.com/r/kpics/")>]
[<InlineData("gfycat", "https://old.reddit.com/r/kpopfap/")>]
[<InlineData("imgur_jpeg", "https://old.reddit.com/r/kpics/")>]
[<InlineData("redgifs", "https://old.reddit.com/r/kpopfap/")>]
let ``Get Profile Url`` name (url: string) =
    let post = loadPost name

    Reddit.getProfileUrl post
    |> should equal url
    
[<Theory>]
[<InlineData("redd_jpeg", "4/9/2020 4:05:56 PM")>]
[<InlineData("gfycat", "5/25/2020 11:13:49 PM")>]
[<InlineData("imgur_jpeg", "5/25/2020 5:17:49 PM")>]
[<InlineData("redgifs", "5/25/2020 11:10:03 PM")>]
let ``Get Post DateTime`` name dateTime =
    let post = loadPost name
    let expectedDateTime = DateTime.Parse  dateTime

    Reddit.getPostDateTime post
    |> should equal expectedDateTime
    
[<Theory>]
[<InlineData("redd_jpeg", "https://old.reddit.com/r/kpics/comments/fxogjy/xuanyi/")>]
[<InlineData("gfycat", "https://old.reddit.com/r/kpopfap/comments/gqcord/twice_jihyo/")>]
[<InlineData("imgur_jpeg", "https://old.reddit.com/r/kpics/comments/gq7v1p/yooa/")>]
[<InlineData("redgifs", "https://old.reddit.com/r/kpopfap/comments/gqcm6j/jo_jung_min_maxim_korea/")>]
let ``Get Full Permalink`` name (permalink: string) =
    let post = loadPost name

    Reddit.getFullPermalink post
    |> should equal permalink
  
[<Theory>]
[<InlineData("redd_jpeg", "kpics/fxogjy.jpg")>]
[<InlineData("gfycat", "kpopfap/gqcord.webm")>]
[<InlineData("imgur_jpeg", "kpics/gq7v1p.jpg")>]
[<InlineData("redgifs", "kpopfap/gqcm6j.webm")>]
let ``Get Path`` name (path: string) =
   let post = loadPost name
   
   Reddit.getPath post
   |> should equal path

[<Theory>]
[<InlineData("redd_jpeg", UrlMethodType.Http)>]
[<InlineData("gfycat", UrlMethodType.Process)>]
[<InlineData("imgur_jpeg", UrlMethodType.Http)>]
[<InlineData("redgifs", UrlMethodType.Process)>]
[<InlineData("unknown_url", UrlMethodType.Unknown)>]
let ``Is Known`` name (known: UrlMethodType) =
    let post = loadPost name

    Reddit.isKnown post
    |> should equal known
    

[<Theory>]
[<InlineData("redd_jpeg", "reddit/kpics/fxogjy.jpg", "https://i.redd.it/pjj1ll1b2rr41.jpg")>]
[<InlineData("gfycat", "reddit/kpopfap/gqcord.webm", "https://gfycat.com/presentdangerousdromedary")>]
[<InlineData("imgur_jpeg", "reddit/kpics/gq7v1p.jpg", "https://i.imgur.com/fXLMjfp.jpg")>]
[<InlineData("redgifs", "reddit/kpopfap/gqcm6j.webm", "https://redgifs.com/watch/ripesnivelingfiddlercrab")>]
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
[<InlineData(
    "gfycat",
    "kpopfap/gqcord.webm",
    "https://old.reddit.com/r/kpopfap/comments/gqcord/twice_jihyo/",
    "5/25/2020 11:13:49 PM",
    "kpopfap",
    "https://old.reddit.com/r/kpopfap/"
)>]
[<InlineData(
    "imgur_jpeg",
    "kpics/gq7v1p.jpg",
    "https://old.reddit.com/r/kpics/comments/gq7v1p/yooa/",
    "5/25/2020 5:17:49 PM",
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
