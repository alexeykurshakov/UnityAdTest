using System;

public static class Interop
{
    public static event EventHandler ShowAdControl;

    public static event EventHandler HideAdControl;

    public static void RaiseShowAdControl()
    {
        if (ShowAdControl != null)
        {
            ShowAdControl(null, null);
        }
    }

    public static void RaiseHideAdControl()
    {
        if (HideAdControl != null)
        {
            HideAdControl(null, null);
        }
    }
}