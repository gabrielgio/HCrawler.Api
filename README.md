# HCrawler.Api

Provide a central application to aggregate multiples source of images 
and videos. It stores and organizes them.

The application will also provide a simple UI to display image already 
stored.


## Architecture

This project uses F#/C# with 
[aspnet core](https://github.com/dotnet/aspnetcore) plus
[Dapper](https://github.com/StackExchange/Dapper).

Dotnet is a really nice framework but it does not provide the best libs 
for everything so for to gather information from the sources it'll be 
using another langs. 


### Instagram:

To gather information from Instagram I'm using
[instabot](https://github.com/instagrambot/instabot) that I bundled 
into [hcrawler](https://gitlab.com/gabrielgio/hcrawler), it will only 
get JSON from post/stories and queue into RabbitMQ.

[hcrawler-runner](https://gitlab.com/gabrielgio/hcrawler-runner) will
dequeue, process it, save the file in the correct folder and then 
post information into this project. But `hcrawler-runner` will be 
deprecated and the logic will move into this project,to make easier to 
deploy and to have less moving parts.
