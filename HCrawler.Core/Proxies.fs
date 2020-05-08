module HCrawler.Core.Proxies

open System

type DetailedSource =
    { Name: string
      Url: string }
    
type DetailedProfile =
    { Name: string
      Url: string
      DetailedSource: DetailedSource }
    
type DetailedImage =
    { Id: int
      Path: string
      CreatedOn: DateTime
      Url: string
      DetailedProfile: DetailedProfile }
    
