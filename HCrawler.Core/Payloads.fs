module HCrawler.Core.Payloads

open System

[<CLIMutable>]
type CreateImage =
    { ImagePath: string
      ImageUrl: string
      CreatedOn: DateTime
      ProfileName: string
      ProfileUrl: string
      SourceName: string
      SourceUrl: string }

type StoreSource = { Name: string; Url: string }

type StoreProfile =
    { SourceId: int
      Name: string
      Url: string }

type StoreImage =
    { ProfileId: int
      Path: string
      Url: string
      CreatedOn: DateTime }

[<CLIMutable>]
type PageFilter =
    { Size: int
      Checkpoint: Nullable<DateTime>
      Source: Nullable<int>
      Profile: Nullable<int> }

type Download = { Path: string; Url: string }
