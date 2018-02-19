using System;
using System.IO;

namespace LiteParcer.Logik
{
    class ExceptionCatch
    {
        public static void WriteException(Exception Exc)
        {
            File.AppendAllText("Log.txt", string.Format("\nExcception Message : {0} \n" +
                                                        "Exception Date Time Utc : {1} \n",
                                                        Exc.Message, DateTime.UtcNow));
        }
    }
}
