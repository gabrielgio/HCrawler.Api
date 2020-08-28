module HCrawler.Core.Proxies

open System

type Source =
    { Id: int
      Name: string
      Url: string }
    
type Profile =
    { Id: int
      Name: string
      Url: string
      DetailedSource: Source }
    
[<CLIMutable>]
type Image =
    { Id: int
      Path: string
      CreatedOn: DateTime
      Url: string
      DetailedProfile: Profile }
    
