using System;
using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.LocalIntros.Configuration;

public class IntroPluginConfiguration : BasePluginConfiguration
{
    public string Local { get; set; } = string.Empty;

    public List<IntroVideo> DetectedLocalVideos { get; set; } = new List<IntroVideo>();

    public List<Guid> DefaultLocalVideos { get; set; } = new List<Guid>();

    public List<TagIntro> TagIntros { get; set; } = new List<TagIntro>();
    public List<GenreIntro> GenreIntros { get; set; } = new List<GenreIntro>();
    public List<StudioIntro> StudioIntros { get; set; } = new List<StudioIntro>();
    public List<CurrentDateRangeIntro> CurrentDateIntros { get; set; } = new List<CurrentDateRangeIntro>();
    
    public IEnumerable<ISpecialIntro> SpecialIntros => 
    Enumerable.Empty<ISpecialIntro>()
        .Concat(TagIntros)
        .Concat(GenreIntros)
        .Concat(StudioIntros)
        .Concat(CurrentDateIntros);
}

public class IntroVideo
{
    public string Name { get; set; }

    public Guid ItemId { get; set; }
}

public record Criteria(HashSet<string> tags, HashSet<string> genres, HashSet<string> studios, DateTime now, bool IsMovie, bool IsShow)
{
    public bool Matches(ISpecialIntro intro) => intro.MatchesCriteria(this);
}

public interface ISpecialIntro
{
    bool MatchesCriteria(Criteria criteria);
    Guid IntroId { get; set; }
    int Precedence { get; set; }
    int Prevalence { get; set; }
    bool? ForMovies { get; set; }
    bool? ForShows { get; set; }
}

public class TagIntro : ISpecialIntro
{
    
    public Guid IntroId { get; set; }
    public string TagName { get; set; }
    public int Precedence { get; set; }
    public int Prevalence { get; set; }
    public bool? ForMovies { get; set; }
    public bool? ForShows { get; set; }
    public bool MatchesCriteria(Criteria criteria)
     =>
     (
      criteria.IsShow == (this.ForShows ?? true) ||
      criteria.IsMovie == (this.ForMovies ?? true) 
     ) && criteria.tags.Any(x => x.Equals(TagName, StringComparison.OrdinalIgnoreCase));
}
public class CurrentDateRangeIntro : ISpecialIntro
{
    public Guid IntroId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public int Precedence { get; set; }
    public int Prevalence { get; set; }
    public bool? ForMovies { get; set; }
    public bool? ForShows { get; set; }
    public bool MatchesCriteria(Criteria criteria)
     =>
     (
      criteria.IsShow == (this.ForShows ?? true) ||
      criteria.IsMovie == (this.ForMovies ?? true) 
     ) && criteria.now.Date <= DateEnd && criteria.now.Date >= DateStart;
}
public class GenreIntro : ISpecialIntro
{
    public Guid IntroId { get; set; }
    public string GenreName { get; set; }
    public int Precedence { get; set; }
    public int Prevalence { get; set; }
    public bool? ForMovies { get; set; }
    public bool? ForShows { get; set; }
    public bool MatchesCriteria(Criteria criteria)
     =>
     (
      criteria.IsShow == (this.ForShows ?? true) ||
      criteria.IsMovie == (this.ForMovies ?? true) 
     ) && criteria.genres.Any(x => x.Equals(GenreName, StringComparison.OrdinalIgnoreCase));
}
public class StudioIntro : ISpecialIntro
{
    public Guid IntroId { get; set; }
    public string StudioName { get; set; }
    public int Precedence { get; set; }
    public int Prevalence { get; set; }
    public bool? ForMovies { get; set; }
    public bool? ForShows { get; set; }
    public bool MatchesCriteria(Criteria criteria)
     =>
     (
      criteria.IsShow == (this.ForShows ?? true) ||
      criteria.IsMovie == (this.ForMovies ?? true) 
     ) && criteria.studios.Any(x => x.Equals(StudioName, StringComparison.OrdinalIgnoreCase));
}
