
public class DateService: IDateService
{
    private readonly IDateLib _dateLib;
    
    private const int HoursInADay = 24;
    private const int HoursInAWeek = HoursInADay * 7;
    private const int HoursInAMonth = HoursInAWeek * 30;

    public DateService(IDateLib dateLib)
    {
        _dateLib = dateLib;
    }

    public DateTime SetDateToLib(string date)
    {
        return _dateLib.Parse(date);
    }

    public int GetHoursRemainingToToday(string date)
    {
        return (int)(SetDateToLib(date) - GetToday()).TotalHours;
    }

    public DateTime GetToday()
    {
        return _dateLib.Now();
    }

    public RemainingPeriod GetRemainingPeriod(int period)
    {
        if (period < HoursInADay)
        {
            return new RemainingPeriod { Value = period, Type = "hours" };
        }

        if (period >= HoursInADay && period < HoursInAWeek)
        {
            return new RemainingPeriod { Value = ConvertHoursToRemainingDays(period), Type = "days" };
        }

        if (period >= HoursInAWeek && period < HoursInAMonth)
        {
            return new RemainingPeriod { Value = ConvertHoursToRemainingWeeks(period), Type = "weeks" };
        }

        return new RemainingPeriod { Value = ConvertHoursToRemainingMonths(period), Type = "months" };
    }

    private int ConvertHoursToRemainingDays(int hours)
    {
        var hoursPlusEntireDay = hours % HoursInADay;

        if (hours < HoursInADay)
        {
            return hours;
        }

        return hoursPlusEntireDay == 0 ? hours / HoursInADay : ((hours - hoursPlusEntireDay) / HoursInADay) + 1;
    }

    private int ConvertHoursToRemainingWeeks(int hours)
    {
        var hoursPlusEntireWeek = hours % HoursInAWeek;

        if (hours < HoursInAWeek)
        {
            return 0;
        }

        return hoursPlusEntireWeek == 0 ? hours / HoursInAWeek : ((hours - hoursPlusEntireWeek) / HoursInAWeek) + 1;
    }

    private int ConvertHoursToRemainingMonths(int hours)
    {
        var hoursPlusEntireMonth = hours % HoursInAMonth;

        if (hours < HoursInAMonth)
        {
            return 0;
        }

        return hoursPlusEntireMonth == 0 ? hours / HoursInAMonth : ((hours - hoursPlusEntireMonth) / HoursInAMonth) + 1;
    }

    private List<CalendarItem> SortCalendarByMostNextToday(List<CalendarItem> calendar) {
        
        calendar.Sort((CalendarItem itemA, CalendarItem itemB) =>
        {
          // Obtém a diferença de horas entre hoje e a data do item
          int itemADiff = GetHoursRemainingToToday(itemA.Date);
          int itemBDiff = GetHoursRemainingToToday(itemB.Date);

          // Se a diferença do itemA for negativa, significa que está no passado, então coloca ele após o itemB
          if (itemADiff < 0) return 1;
          if (itemBDiff < 0) return -1;

          // Se as diferenças forem iguais, os itens são considerados iguais para a ordenação
          if (itemADiff == itemBDiff) return 0;

          // Ordena do menor para o maior
          return itemADiff.CompareTo(itemBDiff);
        });

        return calendar;
    }

    private CalendarResultAsDateComming ParseCalendarObject(CalendarItem calendarItem, CalendarItem today)
    {
      var isThisDateToday = Math.Abs(GetHoursRemainingToToday(today.Date)) < HoursInADay;

      return new CalendarResultAsDateComming
      {
          Item = calendarItem,
          Remaining = GetRemainingPeriod(GetHoursRemainingToToday(calendarItem.Date)),
          Today = isThisDateToday ? today : null
      };
    }
    public CalendarResultAsDateComming GetTheNextDateInsideCalendar(List<CalendarItem> calendar)
    {
      var sorted = SortCalendarByMostNextToday(calendar);
      var nextDate = 0;
      var lastDate = sorted.Count - 1;

      return ParseCalendarObject(
        sorted[nextDate],
        sorted[lastDate]
      );
    }

    public CalendarResultAsInsidePeriod GetTheDayInsidePeriod(List<CalendarItem> calendar)
    {
      var sorted = SortCalendarByMostNextToday(calendar);
      var currentDay = sorted.Count - 1;
      var nextDay = 0;
     
      return new CalendarResultAsInsidePeriod
      {
        Item = new CalendarPeriodItem {
          DateFrom = sorted[currentDay].Date,
          DateTo = sorted[nextDay].Date,
          Description = sorted[currentDay].Description,
        }
      };
    }
  }

public interface IDateLib
{
    public DateTime Parse(string date);
    public DateTime Now();
}



public class RemainingPeriod
{
    public int Value { get; set; }
    public string Type { get; set; }

}

public class CalendarResultAsDateComming
{
    public CalendarItem Item { get; set; }
    public RemainingPeriod Remaining { get; set; }
    public CalendarItem? Today { get; set; }

}

public class CalendarResultAsInsidePeriod
{
    public CalendarPeriodItem Item { get; set; }
}

public class CalendarResultAsToday
{
    public CalendarItem Item { get; set; }
}

public class CalendarItem
{
  public string Date { get; set; }
  public string Description { get; set; }
  
}

public class CalendarPeriodItem
{
  public string DateFrom { get; set; }
  public string DateTo { get; set; }
  public string Description { get; set; }
  
}