module HCrawler.Core.Reddit

open FSharp.Data
open HCrawler.Core.Payloads
open System
open System.Text.RegularExpressions

type Post = JsonProvider<"ProviderData/reddit.json">

let redditUrl = "https://old.reddit.com"
let reddit = "reddit"

let gfycatRegex = "^.*gfycat.com.*$"
let reddJpegRegex = "^.*i\\.redd\\.it.*\\.(jpg|jpeg)$"
let imgurJpegRegex = "^.*i\\.imgur\\.com.*\\.(jpg|jpeg)$"

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

let isKnown (root: Post.Root) =
    [| gfycatRegex; reddJpegRegex; imgurJpegRegex |]
    |> Array.map (matchRegex (root.Url))
    |> Array.reduce (fun x y -> x || y)

let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some() else None

let getGfycatUrl (root: Post.Root) =
    root.Media.Oembed.ThumbnailUrl.Replace("thumbs", "giant").Replace("-size_restricted.gif", ".webm")

let getUrl (root: Post.Root) =
    match root.Url with
    | Regex reddJpegRegex -> root.Url
    | Regex imgurJpegRegex -> root.Url
    | Regex gfycatRegex -> getGfycatUrl root

let getPath (root: Post.Root) =
    match root.Url with
    | Regex reddJpegRegex -> sprintf "%s/%s.jpg" root.Subreddit.DisplayName root.Id
    | Regex imgurJpegRegex -> sprintf "%s/%s.jpg" root.Subreddit.DisplayName root.Id
    | Regex gfycatRegex -> sprintf "%s/%s.webm" root.Subreddit.DisplayName root.Id


let getDownloadPost root =
    { Path = getPath root |> sprintf "%s/%s" reddit
      Url = getUrl root }

let getPayload root =
    { ImagePath = getPath root
      ImageUrl = getFullPermalink root
      CreatedOn = getPostDateTime root
      ProfileName = root.Subreddit.DisplayName
      ProfileUrl = getProfileUrl root
      SourceName = reddit
      SourceUrl = redditUrl }
