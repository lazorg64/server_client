using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace ClassLibrary1
//{
    public class Parsing
    {
        public static String getname(String input)
        {
            int pos1 = input.IndexOf("<name>");
            int pos2 = input.IndexOf("</name>");
            if (pos1 == -1 || pos2 == -1)
                return "Ошибка парсинга";
            else
                return input.Substring(pos1 + 6, pos2 - pos1 - 6);
        }
        public static String getpass(String input)
        {
            int pos1 = input.IndexOf("<pass>");
            int pos2 = input.IndexOf("</pass>");
            if (pos1 == -1 || pos2 == -1)
                return "Ошибка парсинга";
            else
                return input.Substring(pos1 + 6, pos2 - pos1 - 6);
        }
        public static String getsend(String input)
        {
            int pos1 = input.IndexOf("<send>");
            int pos2 = input.IndexOf("</send>");
            if (pos1 == -1 || pos2 == -1)
                return "Ошибка парсинга";
            else
                return input.Substring(pos1 + 6, pos2 - pos1 - 6);
        }
        public static String getmessage(String input)
        {
            int pos1 = input.IndexOf("<message>");
            int pos2 = input.IndexOf("</message>");
            if (pos1 == -1 || pos2 == -1)
                return "Ошибка парсинга";
            else
                return input.Substring(pos1 + 9, pos2 - pos1 - 9);
        }
        public static String getto(String to)
        {
            int pos = to.IndexOf("<to>");
            int pos2 = to.IndexOf("</to>");
            if (pos == -1 || pos2 == -1)
                return "Ошибка парсинга";
            else
                return to.Substring(pos + 4, pos2 - pos - 4);
        }

    }
//}
