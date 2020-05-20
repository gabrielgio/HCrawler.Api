namespace HCrawler.Core

open System.Threading.Tasks
open HCrawler.Core.Payloads

type IDownloader =
    abstract download: Download -> Task