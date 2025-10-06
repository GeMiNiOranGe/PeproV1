namespace Pepro.Presentation.Extensions;

public static class DateTimePickerExtensions
{
    public static void SetValue(
        this DateTimePicker dateTimePicker,
        DateTime actual
    )
    {
        dateTimePicker.Value =
            actual < dateTimePicker.MinDate ? DateTime.Now : actual;
    }

    public static void SetValue(
        this DateTimePicker dateTimePicker,
        DateOnly actual
    )
    {
        dateTimePicker.Value =
            actual < DateOnly.FromDateTime(dateTimePicker.MinDate)
                ? DateTime.Now
                : actual.ToDateTime(TimeOnly.MinValue);
    }
}
