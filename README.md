# BoardGamer.BoardGameGeek

A .NET library for interacting with the BGG XML API2 made available on boardgamegeek.com.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development. 
Since I don't know your specific use cases, I have tried to return full fidelity responses that contain all
the information provided by boardgamegeek.com. Hopefully this makes it easy for you to pick and choose the
data elements that you need for your own project.


### Installing 

You should install [BoardGamer.BoardGameGeek with NuGet](https://www.nuget.org/packages/BoardGamer.BoardGameGeek/):

```
Install-Package BoardGamer.BoardGameGeek
```

Or use the .NET Core command line interface:

```
dotnet add package BoardGamer.BoardGameGeek
```

## Setup

This library was intended to be used in conjunction with HttpClientFactory as a Typed client within an ASP.NET Core web appllication.

To configure this for use within ASP.NET Core just add the following line to `ConfigureServices` within your `Startup.cs` file:

```
services.AddHttpClient<IBoardGameGeekXmlApi2Client, BoardGameGeekXmlApi2Client>();
```

Then just define this as a dependency in a service or controller class.

```
[Route("api/[controller]")]
public class GeekController : Controller
{
	private readonly IBoardGameGeekXmlApi2Client bgg;

	public GeekController(IBoardGameGeekXmlApi2Client bggClient) 
	{
		this.bgg = bggClient;
	}
}
```

But you can use this outside of ASP.NET Core by providing your own instance of `System.Net.Http.HttpClient`:

```
IBoardGameGeekXmlApi2Client bgg = new BoardGameGeekXmlApi2Client(new HttpClient());
```

## Usage

Here are some common use cases that this library can help you fulfill. 

### Get a user's profile

This example makes a user request to get the user's profile information.

```
UserRequest request = new UserRequest("jakefromstatefarm");
UserResponse response = await bgg.GetUserAsync(request);
UserResponse.User user = response.Result;
```

### Get a user's profile including all their geek buddies

This example makes a user request and includes the buddies argument to get the user
profile information and includes the username and id of all their buddies.

```
UserRequest request = new UserRequest("jakefromstatefarm", buddies: true);
UserResponse response = await bgg.GetUserAsync(request);
List<UserResponse.Buddy> buddies = response.Result.Buddies;
```

### Get a user's game collection

This example makes a collection request to get all of a board games in a user's collection.

```
CollectionRequest request = new CollectionRequest("jakefromstatefarm");
CollectionResponse response = await bgg.GetCollectionAsync(request);
CollectionResponse.ItemCollection collection = response.Result;

foreach (CollectionResponse.Item item in collection) {
	// do something which each item in the collection
}
```

### Get a user's game collection including stats for each game

This example makes a collection request that includes the stats for each game and uses it
to filter the collection to the games that play 2 to 4 people.

```
CollectionRequest request = new CollectionRequest("jakefromstatefarm", stats: true);
CollectionResponse response = await bgg.GetCollectionAsync(request);
CollectionResponse.ItemCollection collection = response.Result;

Func<CollectionResponse.Item,bool> TwoToFourPeopleCanPlay = i => i.Stats.MinPlayers >= 2 && i.Stats.MaxPlayers <= 4;

IEnumerable<CollectionResponse.Item> games = collection.Where(TwoToFourPeopleCanPlay);
```

### Get details about a board game

This example makes a thing request to get detailed information about board game 161936 (i.e. Pandemic Legacy: Season 1).

```
ThingRequest request = new ThingRequest(new [] { 161936 });
ThingResponse response = await bgg.GetThingAsync(request);
ThingResponse.Item pandemicLegacySeason1 = response.Result.GetFirstOrDefault();
```

### Get details about multiple board games

This example makes a thing request with multiple ids to get detail information about multiple board games.

```
ThingRequest request = new ThingRequest(new [] { 161936, 221107 });
ThingResponse response = await bgg.GetThingAsync(request);
ThingResponse.Item pandemicLegacySeason1 = response.Result[0];
ThingResponse.Item pandemicLegacySeason2 = response.Result[1];
```

### Get logged play information for a player

This examples gets the most recent 100 logged plays for a user.

```
PlaysRequest request = new PlaysRequest("jakefromstatefarm");
PlaysResponse response = await bgg.GetPlaysAsync(request);
PlaysResponse.PlaysCollection collection = response.Result;

foreach (PlaysResponse.Play loggedPlay in collection.Plays)
{
    // do something interesting here
}
```

### Get all logged plays for a player

This example makes multiple requests to get all the logged plays in batches of 100.

```
PlaysRequest request = new PlaysRequest("jakefromstatefarm");
PlaysResponse response = await bgg.GetPlaysAsync(request);
PlaysResponse.PlaysCollection collection = response.Result;

int pages = (int)(Math.Ceiling(collection.Total / (double)100);

List<PlaysResponse.Play> allPlays = new List<PlaysResponse.Play>();
allPlays.AddRange(collection.Plays);

// Okay we've got page 1 containing the first 100 logged plays
for (int i=2; i <= pages; i++) 
{
    request = new PlaysRequest("jakefromstatefarm", page: i);
    // should probably delay for a bit of time to prevent slamming the bgg server with requests    
    response = await bgg.GetPlaysAsync(request);
    allPlays.AddRange(response.Collection.Plays);
}

foreach (PlaysResponse.Play play in allPlays)
{
    // do something interesting here
}
```

### Get all logged plays of a game

This example gets the most recent 100 logged plays of Carcassonne (#822). This gets all logged plays 
regardless of user, and could possibly be used to compute game wide statistics.

```
PlaysRequest request = new PlaysRequest(id: 822);
PlaysResponse response = await bgg.GetPlaysAsync(request);
PlaysResponse.PlaysCollection collection = response.Result;
```

### Get all logged plays of a game for a player

This example gets the most recent 100 logged plays of Carcassonne (#822) for a user.

```
PlaysRequest request = new PlaysRequest("jakefromstatefarm", 822);
PlaysResponse response = await bgg.GetPlaysAsync(request);
PlaysResponse.PlaysCollection collection = response.Result;
```


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details