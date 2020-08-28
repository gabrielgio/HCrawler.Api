namespace HCrawler.Api

open System
open System.Collections.Generic
open System.Data
open System.Linq
open System.Threading.Tasks
open HCrawler.Core
open HCrawler.Core.Image
open HCrawler.DB.Repositories
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Npgsql

type Startup (configuration: IConfiguration)  =

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddControllers() |> ignore

        services.AddScoped<IImageRepository, ImageRepository>()
        |> ignore
        services.AddScoped<Image>() |> ignore
        
        services.AddScoped<IDbConnection>( fun  (t) ->
                let dbConnection = new NpgsqlConnection(configuration.["ConnectionString"])
                dbConnection.Open() |> ignore
                dbConnection :> IDbConnection
            ) |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment())
        then app.UseDeveloperExceptionPage() |> ignore

        app.UseRouting() |> ignore

        app.UseAuthorization() |> ignore

        app.UseEndpoints(fun endpoints -> endpoints.MapControllers() |> ignore)
        |> ignore

    member val Configuration: IConfiguration = null with get, set
