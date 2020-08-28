namespace HCrawler.Api.Controllers

open HCrawler.Core.Image
open HCrawler.Core.Payloads
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open FSharp.Control.Tasks.V2

[<ApiController>]
[<Route("[controller]")>]
type ImagesController(logger: ILogger<ImagesController>, image: Image) =
    inherit ControllerBase()


    [<HttpGet>]
    member __.Get(filter: PageFilter) =
        task {
            let! images = image.getAllAsync filter
            return images
        }
