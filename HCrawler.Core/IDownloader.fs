namespace HCrawler.Core

open System.Threading.Tasks
open HCrawler.Core.Payloads

type IDownloader =
    abstract downloadHttp: Download -> Task
    abstract downloadProcess: Download -> Task
