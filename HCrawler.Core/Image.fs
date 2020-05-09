module HCrawler.Core.Image

open System.Threading.Tasks
open HCrawler.Core.Payloads

type Image(imageRepo: IImageRepository) =
    member this.getAllAsync pageFilter = imageRepo.getAllAsync pageFilter

    member this.createSourceIfNotExistsAsync(createImage: CreateImage) =
        let sourceName = createImage.SourceName

        let exists =
            imageRepo.sourceExistsAsync sourceName
            |> Async.AwaitTask
            |> Async.RunSynchronously

        match exists with
        | true -> imageRepo.getSourceIdByNameAsync sourceName
        | false ->
            imageRepo.storeSourceAsync
                { Name = sourceName
                  Url = createImage.SourceUrl }

    member this.createProfileAsync(createImage: CreateImage) =
        let sourceId =
            this.createSourceIfNotExistsAsync createImage
            |> Async.AwaitTask
            |> Async.RunSynchronously

        imageRepo.storeProfileAsync
            { SourceId = sourceId
              Name = createImage.ProfileName
              Url = createImage.ProfileUrl }

    member this.createProfileIfNotExistsAsync(createImage: CreateImage) =
        let profileName = createImage.ProfileName

        let exists =
            imageRepo.profileExistsAsync profileName
            |> Async.AwaitTask
            |> Async.RunSynchronously

        if exists
        then imageRepo.getProfileIdByNameAsync profileName
        else this.createProfileAsync createImage

    member this.createImageAsync(createImage: CreateImage) =
        let profileId =
            this.createProfileIfNotExistsAsync createImage
            |> Async.AwaitTask
            |> Async.RunSynchronously

        imageRepo.storeImageAsync
            { ProfileId = profileId
              Path = createImage.ImagePath
              Url = createImage.ImageUrl
              CreatedOn = createImage.CreatedOn }

    member this.createImageIfNotExistsAsync(createImage: CreateImage) =
        let imagePath = createImage.ImagePath

        let exists =
            imageRepo.imageExistsAsync imagePath
            |> Async.AwaitTask
            |> Async.RunSynchronously

        if exists then Task.FromResult(0) else this.createImageAsync createImage
