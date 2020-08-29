namespace HCrawler.Api.Controllers

open HCrawler.Core.Image
open HCrawler.Core.Payloads
open Microsoft.AspNetCore.Mvc
open FSharp.Control.Tasks.V2

[<ApiController>]
[<Route("[controller]")>]
type ImagesController(image: Image) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.Get([<FromQuery>] filter: PageFilter) = image.getAllAsync filter
