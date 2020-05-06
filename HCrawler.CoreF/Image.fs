module HCrawler.CoreF.Image

open System.Threading.Tasks
open HCrawler.CoreF.Payloads

type Image(imageRepo: IImageRepository) =
    member this.GetAllAsync pageFilter = imageRepo.GetAllAsync pageFilter

    member this.CreateSourceIfNotExistsAsync(createImage: CreateImage) =
        let sourceName = createImage.SourceName

        let exists =
            imageRepo.SourceExistsAsync sourceName
            |> Async.AwaitTask
            |> Async.RunSynchronously

        match exists with
        | true -> imageRepo.GetSourceIdByNameAsync sourceName
        | false ->
            imageRepo.StoreSourceAsync
                { Name = sourceName
                  Url = createImage.SourceUrl }

    member this.CreateProfileAsync(createImage: CreateImage) =
        let sourceId =
            this.CreateSourceIfNotExistsAsync createImage
            |> Async.AwaitTask
            |> Async.RunSynchronously

        imageRepo.StoreProfileAsync
            { SourceId = sourceId
              Name = createImage.ProfileName
              Url = createImage.ProfileUrl }

    member this.CreateProfileIfNotExistsAsync(createImage: CreateImage) =
        let profileName = createImage.ProfileName

        let exists =
            imageRepo.ProfileExistsAsync profileName
            |> Async.AwaitTask
            |> Async.RunSynchronously

        if exists
        then imageRepo.GetProfileIdByNameAsync profileName
        else this.CreateProfileAsync createImage

    member this.CreateImageAsync(createImage: CreateImage) =
        let profileId =
            this.CreateProfileIfNotExistsAsync createImage
            |> Async.AwaitTask
            |> Async.RunSynchronously

        imageRepo.StoreImageAsync
            { ProfileId = profileId
              Path = createImage.ImagePath
              Url = createImage.ImageUrl
              CreatedOn = createImage.CreatedOn }

    member this.CreateImageIfNotExistsAsync(createImage: CreateImage) =
        let imagePath = createImage.ImagePath

        let exists =
            imageRepo.ImageExistsAsync imagePath
            |> Async.AwaitTask
            |> Async.RunSynchronously

        if exists then Task.FromResult(0) else this.CreateImageAsync createImage
