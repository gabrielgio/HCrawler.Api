namespace HCrawler.Core

open System.Collections.Generic
open System.Threading.Tasks
open HCrawler.Core.Payloads
open HCrawler.Core.Proxies

type IImageRepository =
    abstract getAllAsync: PageFilter -> Task<IEnumerable<DetailedImage>>
    abstract profileExistsAsync: string -> Task<bool>
    abstract sourceExistsAsync: string -> Task<bool>
    abstract imageExistsAsync: string -> Task<bool>
    abstract storeProfileAsync: StoreProfile -> Task<int>
    abstract storeSourceAsync: StoreSource -> Task<int>
    abstract storeImageAsync: StoreImage -> Task<int>
    abstract getProfileIdByNameAsync: string -> Task<int>
    abstract getSourceIdByNameAsync: string -> Task<int>