using System;
using System.Collections.Generic;
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

}

public class IntroVideo
{
    public string Name { get; set; }

    public Guid ItemId { get; set; }
}

public interface ISpecialIntro
{
    Guid IntroId { get; set; }
    int Precedence { get; set; }
    int Prevalence { get; set; }
}

public class TagIntro : ISpecialIntro
{
    public Guid IntroId { get; set; }
    public string TagName { get; set; }
    public int Precedence { get; set; }
    public int Prevalence { get; set; }
}
public class CurrentDateRangeIntro : ISpecialIntro
{
    public Guid IntroId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public int Precedence { get; set; }
    public int Prevalence { get; set; }
    public CurrentDateRepeatRangeType RepeatType { get; set; } = CurrentDateRepeatRangeType.None;
    public bool IsDateInRange(DateTime currentDate)
    {
        switch (RepeatType)
        {
            case CurrentDateRepeatRangeType.None:
                return currentDate >= DateStart && currentDate <= DateEnd;
            case CurrentDateRepeatRangeType.Weekly:
                return currentDate.DayOfWeek >= DateStart.DayOfWeek && currentDate.DayOfWeek <= DateEnd.DayOfWeek;
            case CurrentDateRepeatRangeType.Monthly:
                return currentDate.Day >= DateStart.Day && currentDate.Day <= DateEnd.Day;
            case CurrentDateRepeatRangeType.Yearly:
                var currentYear = currentDate.Year;
                var pretendCurrentDate = new DateTime(currentYear, currentDate.Month, currentDate.Day);
                var pretendDateEnd = new DateTime(currentYear, DateEnd.Month, DateEnd.Day);
                var pretendDateStart = new DateTime(currentYear, DateStart.Month, DateStart.Day);
                return pretendCurrentDate >= pretendDateStart && pretendCurrentDate <= pretendDateEnd;
            default:
                throw new ArgumentOutOfRangeException($"RepeatType Invalid: {RepeatType}");
        }
    }
}
public enum CurrentDateRepeatRangeType
{
    None,
    Weekly,
    Monthly,
    Yearly
}
public class GenreIntro : ISpecialIntro
{
    public Guid IntroId { get; set; }
    public string GenreName { get; set; }
    public int Precedence { get; set; }
    public int Prevalence { get; set; }
}
public class StudioIntro : ISpecialIntro
{
    public Guid IntroId { get; set; }
    public string StudioName { get; set; }
    public int Precedence { get; set; }
    public int Prevalence { get; set; }
}