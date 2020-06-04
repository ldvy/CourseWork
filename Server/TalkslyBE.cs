using System;

class TalkslyBE
{
    public static void Main()
    {
        try
        {
            new Server(9081);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}