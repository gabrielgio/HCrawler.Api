module HCrawler.Core.Reddit

open FSharp.Data
open HCrawler.Core.Payloads
open System
open System.Text.RegularExpressions

type Post = JsonProvider<"ProviderData/reddit.json">

type UrlMethodType = Http=0 | Process=1 | Unknown=2

let redditUrl = "https://old.reddit.com"
let reddit = "reddit"

let gfycatRegex = "^.*gfycat.com.*$"
let reddJpegRegex = "^.*i\\.redd\\.it.*\\.(jpg|jpeg)$"
let imgurJpegRegex = "^.*i\\.imgur\\.com.*\\.(jpg|jpeg)$"
let redgifsJpegRegex = "^.*redgifs\\.com.*$"

let parsePost post =
    Post.Parse(post)

let getFullPermalink (root: Post.Root) =
    sprintf "%s%s" redditUrl root.Permalink

let getProfileUrl (root: Post.Root) =
    sprintf "%s/%s/" redditUrl root.SubredditNamePrefixed

let getPostDateTime (root: Post.Root) =
    let dateTimeOffset =
        int64 root.Created |> DateTimeOffset.FromUnixTimeSeconds
    dateTimeOffset.DateTime

let matchRegex input pattern =
    Regex.IsMatch(input, pattern)
    
let isHttp url =
    [|  reddJpegRegex; imgurJpegRegex |]
    |> Array.map (matchRegex (url))
    |> Array.reduce (fun x y -> x || y)
    
let isProcess url =
    [| redgifsJpegRegex; gfycatRegex |]
    |> Array.map (matchRegex (url))
    |> Array.reduce (fun x y -> x || y)

let isKnown (root: Post.Root) =
    if isHttp root.Url then
        UrlMethodType.Http
    elif isProcess root.Url then
        UrlMethodType.Process
    else
        UrlMethodType.Unknown

let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some() else None

let getPath (root: Post.Root) =
    match root.Url with
    | Regex reddJpegRegex -> sprintf "%s/%s.jpg" root.Subreddit.DisplayName root.Id
    | Regex imgurJpegRegex -> sprintf "%s/%s.jpg" root.Subreddit.DisplayName root.Id
    | Regex gfycatRegex -> sprintf "%s/%s.webm" root.Subreddit.DisplayName root.Id
    | Regex redgifsJpegRegex -> sprintf "%s/%s.webm" root.Subreddit.DisplayName root.Id

let getDownloadPost root =
    { Path = getPath root |> sprintf "%s/%s" reddit
      Url = root.Url }

let getPayload root =
    { ImagePath = getPath root
      ImageUrl = getFullPermalink root
      CreatedOn = getPostDateTime root
      ProfileName = root.Subreddit.DisplayName
      ProfileUrl = getProfileUrl root
      SourceName = reddit
      SourceUrl = redditUrl }
