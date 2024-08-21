
using System.Globalization;
  class DateLib: IDateLib {
    private const string DateFormat = "yyyy-MM-dd";
    public DateTime Parse(string date) {
      var cultureInfo = new CultureInfo("pt-BR");

      if (System.Text.RegularExpressions.Regex.IsMatch(date, @"^[0-9]{2}-[0-9]{2}$"))
        {
            return DateTime.ParseExact($"{Now().Year}-{date}", DateFormat, cultureInfo);
        }
        return DateTime.ParseExact(date, DateFormat, cultureInfo);
    }
    public DateTime Now() {
      return DateTime.Now;
    }
  }