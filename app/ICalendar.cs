public interface IDateService {
  public DateTime SetDateToLib(string date);
  public int GetHoursRemainingToToday(string date);
  public DateTime GetToday();
  public RemainingPeriod GetRemainingPeriod(int period);
  public CalendarResultAsDateComming GetTheNextDateInsideCalendar(List<CalendarItem> calendar);
  public CalendarResultAsInsidePeriod GetTheDayInsidePeriod(List<CalendarItem> calendar);

}