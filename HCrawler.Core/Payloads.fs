module HCrawler.Core.Payloads

open System

// TODO: remove this from here
// this is done so aspnet is able to create object from request
type CreateImage() =
    member val ImagePath = "" with get, set
    member val ImageUrl = "" with get, set
    member val CreatedOn = DateTime.Now with get, set
    member val ProfileName = "" with get, set
    member val ProfileUrl = "" with get, set
    member val SourceName = "" with get, set
    member val SourceUrl = "" with get, set

type StoreSource =
    { Name: string
      Url: string }

type StoreProfile =
    { SourceId: int
      Name: string
      Url: string }

type StoreImage =
    { ProfileId: int
      Path: string
      Url: string
      CreatedOn: DateTime }

type PageFilter() =
    member val Size = 30 with get, set
    member val Checkpoint: Nullable<DateTime> = Nullable() with get, set
    member val Name: string = null with get, set
