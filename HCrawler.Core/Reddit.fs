module HCrawler.Core.Reddit

open FSharp.Data
open HCrawler.Core.Payloads
open System
open System.Text.RegularExpressions

type Post = JsonProvider<"ProviderData/reddit.json">

let redditUrl = "https://old.reddit.com"
let reddit = "reddit"

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

let getImagePath (root: Post.Root) =
    sprintf "%s/%s.jpg" root.Subreddit.DisplayName root.Id

let isKnown (root: Post.Root) =
    Regex.IsMatch(root.Url, ".*i\\.redd\\.it.*\\.(jpg|jpeg)")

let getDownloadPost root =
    let path =
        getImagePath root |> sprintf "%s/%s" reddit

    { Path = path
      Url = root.Url }

let getPayload root =
    { ImagePath = getImagePath root
      ImageUrl = getFullPermalink root
      CreatedOn = getPostDateTime root
      ProfileName = root.Subreddit.DisplayName
      ProfileUrl = getProfileUrl root
      SourceName = reddit
      SourceUrl = redditUrl }
