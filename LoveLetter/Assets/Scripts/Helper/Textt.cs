public static class Textt
{
    public static void GameLocal(string text)
    {
        NetworkHelper.Instance.SetGameText(text, false);
    }

    public static void ActionLocal(string text)
    {
        NetworkHelper.Instance.SetActionText(text, false);
    }

    public static void GameSync(string text)
    {
        NetworkHelper.Instance.SetGameText(text, true);
    }

    public static void ActionSync(string text)
    {
        NetworkHelper.Instance.SetActionText(text, true);
    }
}