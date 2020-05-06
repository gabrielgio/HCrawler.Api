namespace HCrawler.CoreF

open System.Collections.Generic
open System.Threading.Tasks
open HCrawler.CoreF.Payloads
open HCrawler.CoreF.Proxies

type IImageRepository =
    abstract GetAllAsync: PageFilter -> Task<IEnumerable<DetailedImage>>
    abstract ProfileExistsAsync: string -> Task<bool>
    abstract SourceExistsAsync: string -> Task<bool>
    abstract ImageExistsAsync: string -> Task<bool>
    abstract StoreProfileAsync: StoreProfile -> Task<int>
    abstract StoreSourceAsync: StoreSource -> Task<int>
    abstract StoreImageAsync: StoreImage -> Task<int>
    abstract GetProfileIdByNameAsync: string -> Task<int>
    abstract GetSourceIdByNameAsync: string -> Task<int>