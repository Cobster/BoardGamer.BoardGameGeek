using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public partial class ThingResponse
    {
        public ThingResponse(IEnumerable<Item> things)
        {
            this.Succeeded = true;
            this.Items = new List<Item>(things);
        }

        public bool Succeeded { get; }
        public List<Item> Items { get; }

        public class Item
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Thumbnail { get; set; }
            public string Image { get; set; }
            public string Name { get; set; }
            public List<string> AlternateNames { get; set; }
            public string Description { get; set; }
            public int? YearPublished { get; set; }
            public int MaxPlayers { get; set; }
            public int MinPlayers { get; set; }
            public int? PlayingTime { get; set; }
            public int? MinPlayingTime { get; set; }
            public int? MaxPlayingTime { get; set; }
            public int? MinAge { get; set; }
            public DateTime? ReleaseDate { get; set; }
            public List<Poll> Polls { get; set; }
            public List<Link> Links { get; set; }
            public List<Version> Versions { get; set; }
            public VideoCollection Videos { get; set; }
        }

        public class Poll
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public int TotalVotes { get; set; }
            public List<PollResults> Results { get; set; }
        }

        public class PollResults
        {
            public int? NumPlayers { get; set; }
            public List<PollResult> Results { get; set; }
        }

        public class PollResult
        {
            public string Value { get; set; }
            public int NumVotes { get; set; }
        }

        public class Link
        {
            public string Type { get; set; }
            public int Id { get; set; }
            public string Value { get; set; }
        }

        public class Version
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Thumbnail { get; set; }
            public string Image { get; set; }
        }

        public class BoardGameVersion : Version
        {
            public string Name { get; set; }
            public int YearPublished { get; set; }
            public string ProductCode { get; set; }
            public double? Width { get; set; }
            public double? Length { get; set; }
            public double? Depth { get; set; }
            public double? Weight { get; set; }
            public List<Link> Links { get; set; }
        }

        public class RpgItemVersion : Version
        {
            public string Name { get; set; }
            public int YearPublished { get; set; }
            public string Format { get; set; }
            public string ProductCode { get; set; }
            public int? PageCount { get; set; }
            public string Isbn10 { get; set; }
            public string Isbn13 { get; set; }
            public double? Width { get; set; }
            public double? Height { get; set; }
            public double? Weight { get; set; }
            public string Description { get; set; }
            public List<Link> Links { get; set; }
        }

        public class VideoGameVersion : Version
        {
            public string Name { get; set; }
            public DateTime ReleaseDate { get; set; }
            public List<Link> Links { get; set; }
        }

        public class VideoGameCharacterVersion : Version
        {

        }


        public class VideoCollection : List<Video>
        {
            public VideoCollection() { }
            public VideoCollection(IEnumerable<Video> videos, int? total)
                : base(videos)
            {
                Videos = new List<Video>(videos);
                Total = total;
            }

            public int? Total { get; set; }
            public List<Video> Videos { get; set; }
        }

        public class Video
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Category { get; set; }
            public string Language { get; set; }
            public string Link { get; set; }
            public string Username { get; set; }
            public int UserId { get; set; }
            public DateTime PostDate { get; set; }
        }
    }
}
