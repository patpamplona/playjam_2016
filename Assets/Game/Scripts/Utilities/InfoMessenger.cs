public class InfoMessenger
{
    private static bool didPlayerWin = false;
    public static bool DidPlayerWin
    {
        get
        {
            return didPlayerWin;
        }
    }

    public static void Reset()
    {
        didPlayerWin = false;
    }

    public static void SetPlayerWin(bool win)
    {
        didPlayerWin = win;
    }
}