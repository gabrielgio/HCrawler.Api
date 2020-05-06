module HCrawler.CoreF.Proxies

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
      DetailedProfile: DetailedProfile }
    
