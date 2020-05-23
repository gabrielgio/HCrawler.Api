module HCrawler.Core.Instagram

open FSharp.Data
open Payloads
open System

type Post = JsonProvider<"ProviderData/instagram.json">

let instagram = "instagram"
let instagramUrl = "https://www.instagram.com"

let parsePost post =
    Post.Parse(post)

let getPostDateTime (root: Post.Root) =
    let dateTimeOffset =
        int64 root.Post.TakenAt |> DateTimeOffset.FromUnixTimeSeconds
    dateTimeOffset.DateTime

let getImagePath (root: Post.Root) =
    sprintf "%s/%s.jpeg" root.Post.User.Username root.Post.Id

let getVideoPath (root: Post.Root) =
    sprintf "%s/%s.mp4" root.Post.User.Username root.Post.Id
    
let getImageCarouselPath (user: Post.User) (media: Post.CarouselMedia) =
    sprintf "%s/%s.jpeg" user.Username media.Id

let getVideoCarouselPath (user: Post.User) (media: Post.CarouselMedia) =
    sprintf "%s/%s.mp4" user.Username media.Id

let getPostUrl (root: Post.Root) =
    sprintf "%s/p/%s" instagramUrl root.Post.Code

let getProfileUrl (root: Post.Root) =
    sprintf "%s/%s" instagramUrl root.Post.User.Username

let getImagePayload root =
    { ImagePath = getImagePath root
      ImageUrl = getPostUrl root
      CreatedOn = getPostDateTime root
      ProfileName = root.Post.User.Username
      ProfileUrl =  getProfileUrl root
      SourceName = instagram
      SourceUrl = instagramUrl }

let getVideoPayload root =
    { ImagePath = getVideoPath root
      ImageUrl = getPostUrl root
      CreatedOn = getPostDateTime root
      ProfileName = root.Post.User.Username
      ProfileUrl =  getProfileUrl root
      SourceName = instagram
      SourceUrl = instagramUrl }

let getSinglePayload  (root: Post.Root) (carouselMedia: Post.CarouselMedia) =
    let mediaType =  carouselMedia.MediaType
    
    let imagePath =
       match mediaType with
       | 1 -> getImageCarouselPath root.User carouselMedia
       | 2 -> getVideoCarouselPath root.User carouselMedia

    { ImagePath =  imagePath
      ImageUrl = getPostUrl root
      CreatedOn = getPostDateTime root
      ProfileName = root.Post.User.Username
      ProfileUrl = getProfileUrl root
      SourceName = instagram
      SourceUrl = instagramUrl }
    
let getCarouselPayload (root: Post.Root) =
    root.Post.CarouselMedia
    |> Seq.map(getSinglePayload(root))

let getPayload (root: Post.Root) =
    let mediaType = root.Post.MediaType

    match mediaType with
    | 1 -> seq { getImagePayload root }
    | 2 -> seq { getVideoPayload root }
    | _ -> getCarouselPayload root

let getImageDownload (root: Post.Root) =
    let url = root.Post.ImageVersions2.Candidates.[0].Url
    let path = getImagePath root
               |> sprintf "%s/%s" instagram
    
    { Path = path
      Url =  url }
    
let getVideoDownload (root: Post.Root) =
    let url = root.Post.VideoVersions.[0].Url
    let path = getVideoPath root
               |> sprintf "%s/%s" instagram
                
    { Path = path
      Url =  url }

let getImageCarouselDownload (root: Post.Root) (media: Post.CarouselMedia) =
    let url = media.ImageVersions2.Candidates.[0].Url
    let path = getImageCarouselPath root.User media
               |> sprintf "%s/%s" instagram
                
    { Path = path
      Url =  url }
    
let getVideoCarouselDownload (root: Post.Root) (media: Post.CarouselMedia) =
    let url = media.VideoVersions.[0].Url
    let path = getVideoCarouselPath root.User media
               |> sprintf "%s/%s" instagram
                
    { Path = path
      Url =  url }
    
let getCarouselDownload (root: Post.Root) (media: Post.CarouselMedia) =
    let mediaType = media.MediaType
    
    match mediaType with
    | 1 -> getImageCarouselDownload root media
    | 2 -> getVideoCarouselDownload root media
    
let getCarouselDownloads (root: Post.Root) =
    root.Post.CarouselMedia
    |> Array.toSeq
    |> Seq.map(getCarouselDownload(root))

let getDownloadPost (root: Post.Root) =
    let mediaType = root.Post.MediaType
    
    match mediaType with
    | 1 -> seq { getImageDownload root }
    | 2 -> seq { getVideoDownload root }
    | _ -> getCarouselDownloads root
