public interface IDateService {
  public DateTime SetDateToLib(string date);
  public int GetHoursRemainingToToday(string date);
  public DateTime GetToday();
  public RemainingPeriod GetRemainingPeriod(int period);
  public CalendarResult ParseCalendarObject(CalendarItem calendarItem);
  public CalendarResult GetTheNextDateInsideCalendar(List<CalendarItem> calendar);


}