using System.Reflection;
using Pepro.Business;

namespace Pepro.Presentation.Extensions;

public static class ControlExtensions
{
    public static void SetLocationY(this Control control, int y)
    {
        control.Location = new Point(control.Location.X, y);
    }

    public static void ExecuteWithoutEvent<T>(
        this T control,
        string eventName,
        EventHandler handler,
        Action action
    )
    {
        EventInfo eventInfo =
            typeof(T).GetEvent(eventName)
            ?? throw new ArgumentException(
                $"Event '{eventName}' not found on {typeof(T).Name}"
            );

        Type eventHandlerType =
            eventInfo.EventHandlerType
            ?? throw new ArgumentException(
                $"Event '{eventName}' on {typeof(T).Name} does not have a delegate type."
            );

        Delegate eventHandler = Delegate.CreateDelegate(
            eventHandlerType,
            handler.Target,
            handler.Method
        );

        eventInfo.RemoveEventHandler(control, eventHandler);
        try
        {
            action();
        }
        finally
        {
            eventInfo.AddEventHandler(control, eventHandler);
        }
    }

    public static void ApplyPermission(
        this Control control,
        string key,
        bool disable = false
    )
    {
        if (PermissionBusiness.Instance.Has(key))
        {
            return;
        }

        if (disable)
        {
            control.Enabled = false;
            return;
        }

        control.Parent?.Controls.Remove(control);
    }
}
