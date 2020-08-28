module HCrawler.Core.Image

open System.Threading.Tasks
open HCrawler.Core.Payloads
open FSharp.Control.Tasks.V2

type Image(imageRepo: IImageRepository) =
    member this.getAllAsync pageFilter = imageRepo.getAllAsync pageFilter

    member this.createSourceIfNotExistsAsync(createImage: CreateImage) =
        task {
            let sourceName = createImage.SourceName

            let! exists = imageRepo.sourceExistsAsync sourceName

            return! match exists with
                    | true -> imageRepo.getSourceIdByNameAsync sourceName
                    | false ->
                        imageRepo.storeSourceAsync
                            { Name = sourceName
                              Url = createImage.SourceUrl }
        }

    member this.createProfileAsync(createImage: CreateImage) =
        task {
            let! sourceId = this.createSourceIfNotExistsAsync createImage

            return! imageRepo.storeProfileAsync
                        { SourceId = sourceId
                          Name = createImage.ProfileName
                          Url = createImage.ProfileUrl }
        }

    member this.createProfileIfNotExistsAsync(createImage: CreateImage) =
        task {
            let profileName = createImage.ProfileName

            let! exists = imageRepo.profileExistsAsync profileName

            return! if exists
                    then imageRepo.getProfileIdByNameAsync profileName
                    else this.createProfileAsync createImage
        }

    member this.createImageAsync(createImage: CreateImage) =
        task {
            let! profileId = this.createProfileIfNotExistsAsync createImage

            return! imageRepo.storeImageAsync
                        { ProfileId = profileId
                          Path = createImage.ImagePath
                          Url = createImage.ImageUrl
                          CreatedOn = createImage.CreatedOn }
        }

    member this.createImageIfNotExistsAsync(createImage: CreateImage) =
        task {
            let imagePath = createImage.ImagePath

            let! exists = imageRepo.imageExistsAsync imagePath

            return! if exists then Task.FromResult(0) else this.createImageAsync createImage
        }
