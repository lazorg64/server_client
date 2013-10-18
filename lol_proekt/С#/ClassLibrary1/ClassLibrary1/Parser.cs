using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
        public static String getto(String input)
        {
            int pos = input.IndexOf("<to>");
            int pos2 = input.IndexOf("</to>");
            if (pos == -1 || pos2 == -1)
                return "Ошибка парсинга";
            else
                return input.Substring(pos + 4, pos2 - pos - 4);
        }
        public static String getfrom(String input)
        {
            int pos = input.IndexOf("<from>");
            int pos2 = input.IndexOf("</from>");
            if (pos == -1 || pos2 == -1)
                return "Ошибка парсинга";
            else
                return input.Substring(pos + 6, pos2 - pos - 6);
        }
        public static String getAny(String input, String search)
        {
            String se1 = "<" + search + ">";
            String se2 = "</" + search + ">";
            int pos = input.IndexOf(se1);
            int pos2 = input.IndexOf(se2);
            if (pos == -1 || pos2 == -1)
                return "Ошибка парсинга";
            else
                return input.Substring(pos + search.Length + 2, pos2 - pos - (search.Length + 2));
        }

    }

