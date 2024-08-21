
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

    public CalendarResult ParseCalendarObject(CalendarItem calendarItem)
    {
        return new CalendarResult
        {
            Item = calendarItem,
            Remaining = GetRemainingPeriod(GetHoursRemainingToToday(calendarItem.Date))
        };
    }
    public CalendarResult GetTheNextDateInsideCalendar(List<CalendarItem> calendar)
    {
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
      var firstItem = 0;

      return ParseCalendarObject(calendar[firstItem]);
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

public class CalendarResult
{
    public CalendarItem Item { get; set; }
    public RemainingPeriod Remaining { get; set; }

}

public class CalendarItem
{
  public string Date { get; set; }
  public string Description { get; set; }
  
}