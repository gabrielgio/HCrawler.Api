module HCrawler.Test.InstagramTest

open System
open System.IO
open HCrawler.Core
open HCrawler.Core.Payloads
open FsUnit
open Xunit

let loadPost postName =
    let filePath = sprintf "Data/Instagram/%s.json" postName

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
    
    
[<Theory>]
[<InlineData(
    "image",
    "instagram/maple.pepe_/2195960449254306067_21933169435.jpeg",
    "https://scontent-dus1-1.cdninstagram.com/v/t51.2885-15/e35/79437981_571401397020124_5748988122873669093_n.jpg?_nc_ht=scontent-dus1-1.cdninstagram.com&_nc_cat=104&_nc_ohc=w4looW95wtsAX_8LFOm&se=8&oh=4e0ee3c0f8ceaeadfcf8dc75d55b7e17&oe=5EA876FB&ig_cache_key=MjE5NTk2MDQ0OTI1NDMwNjA2Nw%3D%3D.2" 
)>]
let ``Get Image Download`` name path url =
    let post = loadPost name
    let expectedDownload =
        { Path = path
          Url = url }
    
    Instagram.getImageDownload post
    |> should equal expectedDownload
         
[<Theory>]
[<InlineData(
    "video",
    "instagram/baesuicide/1503214870974369621_703072962.mp4",
    "https://scontent-dus1-1.cdninstagram.com/v/t50.2886-16/18320084_249828945490982_1543373000051523584_n.mp4?_nc_ht=scontent-dus1-1.cdninstagram.com&_nc_cat=102&_nc_ohc=H5Yjaw5urIEAX-gAN2H&oe=5E08CCA1&oh=65accf2cf047136a277bbcd851f99d74" 
)>]
let ``Get Video Download`` name path url =
    let post = loadPost name
    let expectedDownload =
        { Path = path
          Url = url }
    
    Instagram.getVideoDownload post
    |> should equal expectedDownload
    

[<Theory>]
[<InlineData(
    "carousel",
    1,
    "instagram/gaeungbebe/2253858773301191985_873251263.jpeg",
    "https://scontent-frt3-1.cdninstagram.com/v/t51.2885-15/e35/87652998_237147804113804_8218851372993129774_n.jpg?_nc_ht=scontent-frt3-1.cdninstagram.com&_nc_cat=108&_nc_ohc=PVRYfpoSzHsAX9Qojwb&se=8&oh=ee24861162fd25420e0ecfd3d00050fe&oe=5EEBF60C&ig_cache_key=MjI1Mzg1ODc3MzMwMTE5MTk4NQ%3D%3D.2" 
)>]
let ``Get Carousel Image Download`` name index path url =
    let post = loadPost name
    let expectedDownload =
        { Path = path
          Url = url }
    
    Instagram.getImageCarouselDownload post post.Post.CarouselMedia.[index]
    |> should equal expectedDownload
    
    
[<Theory>]
[<InlineData(
    "carousel",
    0,
    "instagram/gaeungbebe/2253858724269707438_873251263.mp4",
    "https://scontent-frt3-1.cdninstagram.com/v/t50.2886-16/87952835_562165107718988_1303781504874987105_n.mp4?efg=eyJ2ZW5jb2RlX3RhZyI6InZ0c192b2RfdXJsZ2VuLjcyMC5jYXJvdXNlbF9pdGVtIn0&_nc_ht=scontent-frt3-1.cdninstagram.com&_nc_cat=108&_nc_ohc=_QoYyZ07T4gAX_Rh9aE&vs=17910260968408611_1881153279&_nc_vs=HBksFQAYJEdNTU5QZ1ZNXzR4RFNmOEJBR0VfelBaZTloY1Nia1lMQUFBRhUAAsgBABUAGCRHSi1sT0FXM0E4a1F5VUlLQUJLOTFIRkhrOHhfYmtZTEFBQUYVAgLIAQAoABgAGwGIB3VzZV9vaWwBMBUAABgAFsa5wciy0tA%2FFQIoAkMzLBdAJyHKwIMSbxgSZGFzaF9iYXNlbGluZV8xX3YxEQB17gcA&_nc_rid=9cd9ec0a81&oe=5EC52711&oh=742347fb280579eba41f0f745182b7ce" 
)>]
let ``Get Carousel Video Download`` name index path url =
    let post = loadPost name
    let expectedDownload =
        { Path = path
          Url = url }
    
    Instagram.getVideoCarouselDownload post post.Post.CarouselMedia.[index]
    |> should equal expectedDownload
    
    
[<Theory>]
[<InlineData(
    "carousel",
    0,
    "instagram/gaeungbebe/2253858724269707438_873251263.mp4",
    "https://scontent-frt3-1.cdninstagram.com/v/t50.2886-16/87952835_562165107718988_1303781504874987105_n.mp4?efg=eyJ2ZW5jb2RlX3RhZyI6InZ0c192b2RfdXJsZ2VuLjcyMC5jYXJvdXNlbF9pdGVtIn0&_nc_ht=scontent-frt3-1.cdninstagram.com&_nc_cat=108&_nc_ohc=_QoYyZ07T4gAX_Rh9aE&vs=17910260968408611_1881153279&_nc_vs=HBksFQAYJEdNTU5QZ1ZNXzR4RFNmOEJBR0VfelBaZTloY1Nia1lMQUFBRhUAAsgBABUAGCRHSi1sT0FXM0E4a1F5VUlLQUJLOTFIRkhrOHhfYmtZTEFBQUYVAgLIAQAoABgAGwGIB3VzZV9vaWwBMBUAABgAFsa5wciy0tA%2FFQIoAkMzLBdAJyHKwIMSbxgSZGFzaF9iYXNlbGluZV8xX3YxEQB17gcA&_nc_rid=9cd9ec0a81&oe=5EC52711&oh=742347fb280579eba41f0f745182b7ce" 
)>]
[<InlineData(
    "carousel",
    1,
    "instagram/gaeungbebe/2253858773301191985_873251263.jpeg",
    "https://scontent-frt3-1.cdninstagram.com/v/t51.2885-15/e35/87652998_237147804113804_8218851372993129774_n.jpg?_nc_ht=scontent-frt3-1.cdninstagram.com&_nc_cat=108&_nc_ohc=PVRYfpoSzHsAX9Qojwb&se=8&oh=ee24861162fd25420e0ecfd3d00050fe&oe=5EEBF60C&ig_cache_key=MjI1Mzg1ODc3MzMwMTE5MTk4NQ%3D%3D.2" 
)>]
let ``Get Carousel Download`` name index path url =
    let post = loadPost name
    let expectedDownload =
        { Path = path
          Url = url }
    
    Instagram.getCarouselDownload post post.Post.CarouselMedia.[index]
    |> should equal expectedDownload
    

[<Theory>]
[<InlineData(
    "carousel",
    "instagram/gaeungbebe/2253858724269707438_873251263.mp4",
    "https://scontent-frt3-1.cdninstagram.com/v/t50.2886-16/87952835_562165107718988_1303781504874987105_n.mp4?efg=eyJ2ZW5jb2RlX3RhZyI6InZ0c192b2RfdXJsZ2VuLjcyMC5jYXJvdXNlbF9pdGVtIn0&_nc_ht=scontent-frt3-1.cdninstagram.com&_nc_cat=108&_nc_ohc=_QoYyZ07T4gAX_Rh9aE&vs=17910260968408611_1881153279&_nc_vs=HBksFQAYJEdNTU5QZ1ZNXzR4RFNmOEJBR0VfelBaZTloY1Nia1lMQUFBRhUAAsgBABUAGCRHSi1sT0FXM0E4a1F5VUlLQUJLOTFIRkhrOHhfYmtZTEFBQUYVAgLIAQAoABgAGwGIB3VzZV9vaWwBMBUAABgAFsa5wciy0tA%2FFQIoAkMzLBdAJyHKwIMSbxgSZGFzaF9iYXNlbGluZV8xX3YxEQB17gcA&_nc_rid=9cd9ec0a81&oe=5EC52711&oh=742347fb280579eba41f0f745182b7ce",
    "instagram/gaeungbebe/2253858773301191985_873251263.jpeg",
    "https://scontent-frt3-1.cdninstagram.com/v/t51.2885-15/e35/87652998_237147804113804_8218851372993129774_n.jpg?_nc_ht=scontent-frt3-1.cdninstagram.com&_nc_cat=108&_nc_ohc=PVRYfpoSzHsAX9Qojwb&se=8&oh=ee24861162fd25420e0ecfd3d00050fe&oe=5EEBF60C&ig_cache_key=MjI1Mzg1ODc3MzMwMTE5MTk4NQ%3D%3D.2" 
)>]
let ``Get Carousel Downloads?`` name pathFirst urlFirst pathLast urlLast =
    let post = loadPost name
    let expectedDownload0 =
        { Path = pathFirst
          Url = urlFirst }
    
    let expectedDownload1 =
        { Path = pathLast
          Url = urlLast }
    
    let downloads = Instagram.getCarouselDownloads post
    
    
    downloads
    |> Seq.length
    |> should equal 2
    
    downloads
    |> Seq.head
    |> should equal expectedDownload0
    
    downloads
    |> Seq.rev
    |> Seq.head
    |> should equal expectedDownload1

[<Theory>]
[<InlineData(
    "image",
    "instagram/maple.pepe_/2195960449254306067_21933169435.jpeg",
    "https://scontent-dus1-1.cdninstagram.com/v/t51.2885-15/e35/79437981_571401397020124_5748988122873669093_n.jpg?_nc_ht=scontent-dus1-1.cdninstagram.com&_nc_cat=104&_nc_ohc=w4looW95wtsAX_8LFOm&se=8&oh=4e0ee3c0f8ceaeadfcf8dc75d55b7e17&oe=5EA876FB&ig_cache_key=MjE5NTk2MDQ0OTI1NDMwNjA2Nw%3D%3D.2" 
)>]
let ``Get Download Post for image`` name path url =
    let post = loadPost name
    let expectedDownload =
        { Path = path
          Url = url }
    
    let downloads = Instagram.getDownloadPost post
    
    downloads
    |> Seq.length
    |> should equal 1
   
    downloads
    |> Seq.head
    |> should equal expectedDownload
         
[<Theory>]
[<InlineData(
    "video",
    "instagram/baesuicide/1503214870974369621_703072962.mp4",
    "https://scontent-dus1-1.cdninstagram.com/v/t50.2886-16/18320084_249828945490982_1543373000051523584_n.mp4?_nc_ht=scontent-dus1-1.cdninstagram.com&_nc_cat=102&_nc_ohc=H5Yjaw5urIEAX-gAN2H&oe=5E08CCA1&oh=65accf2cf047136a277bbcd851f99d74" 
)>]
let ``Get Download Post for Video`` name path url =
    let post = loadPost name
    let expectedDownload =
        { Path = path
          Url = url }
    
    let downloads = Instagram.getDownloadPost post
    
    downloads
    |> Seq.length
    |> should equal 1
   
    downloads
    |> Seq.head
    |> should equal expectedDownload

[<Theory>]
[<InlineData(
    "carousel",
    "instagram/gaeungbebe/2253858724269707438_873251263.mp4",
    "https://scontent-frt3-1.cdninstagram.com/v/t50.2886-16/87952835_562165107718988_1303781504874987105_n.mp4?efg=eyJ2ZW5jb2RlX3RhZyI6InZ0c192b2RfdXJsZ2VuLjcyMC5jYXJvdXNlbF9pdGVtIn0&_nc_ht=scontent-frt3-1.cdninstagram.com&_nc_cat=108&_nc_ohc=_QoYyZ07T4gAX_Rh9aE&vs=17910260968408611_1881153279&_nc_vs=HBksFQAYJEdNTU5QZ1ZNXzR4RFNmOEJBR0VfelBaZTloY1Nia1lMQUFBRhUAAsgBABUAGCRHSi1sT0FXM0E4a1F5VUlLQUJLOTFIRkhrOHhfYmtZTEFBQUYVAgLIAQAoABgAGwGIB3VzZV9vaWwBMBUAABgAFsa5wciy0tA%2FFQIoAkMzLBdAJyHKwIMSbxgSZGFzaF9iYXNlbGluZV8xX3YxEQB17gcA&_nc_rid=9cd9ec0a81&oe=5EC52711&oh=742347fb280579eba41f0f745182b7ce",
    "instagram/gaeungbebe/2253858773301191985_873251263.jpeg",
    "https://scontent-frt3-1.cdninstagram.com/v/t51.2885-15/e35/87652998_237147804113804_8218851372993129774_n.jpg?_nc_ht=scontent-frt3-1.cdninstagram.com&_nc_cat=108&_nc_ohc=PVRYfpoSzHsAX9Qojwb&se=8&oh=ee24861162fd25420e0ecfd3d00050fe&oe=5EEBF60C&ig_cache_key=MjI1Mzg1ODc3MzMwMTE5MTk4NQ%3D%3D.2" 
)>]
let ``Get Downloads Post for Carousel`` name pathFirst urlFirst pathLast urlLast =
    let post = loadPost name
    let expectedDownload0 =
        { Path = pathFirst
          Url = urlFirst }
    
    let expectedDownload1 =
        { Path = pathLast
          Url = urlLast }
    
    let downloads = Instagram.getDownloadPost post
    
    downloads
    |> Seq.length
    |> should equal 2
    
    downloads
    |> Seq.head
    |> should equal expectedDownload0
    
    downloads
    |> Seq.rev
    |> Seq.head
    |> should equal expectedDownload1
    
