# BoardGamer.BoardGameGeek

A .NET library for interacting with the boardgamegeek.com XML API2.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Installing 

You should install [BoardGamer.BoardGameGeek with NuGet](https://www.nuget.org/packages/BoardGamer.BoardGameGeek/):

```
Install-Package BoardGamer.BoardGameGeek
```

Or by using the .NET Core command line interface:

```
dotnet add package BoardGamer.BoardGameGeek
```

## Usage

This library was designed to be used in conjunction with HttpClientFactory as a Typed client.

To configure this for use within ASP.Net Core just add the following line to `ConfigureServices` within your `Startup.cs` file:

```
services.AddHttpClient<IBoardGameGeekXmlApi2Client, BoardGameGeekXmlApi2Client>();
```

Then just define this as a dependency in a service or controller class.

```
[Route("api/[controller]")]
public class GeekController : Controller
{
	private readonly IBoardGameGeekXmlApi2Client bggClient;

	public GeekController(IBoardGameGeekXmlApi2Client bggClient) 
	{
		this.bggClient = bggClient;
	}
}
```

Or you can easily create your own instance by supplying a `System.Net.Http.HttpClient` and doing some work:

```
IBoardGameGeekXmlApi2Client bgg = new BoardGameGeekXmlApi2Client(new HttpClient());
UserResponse response = bgg.GetUserAsync(new UserRequest("jakefromstatefarm"));
User user = response.User;
```

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details