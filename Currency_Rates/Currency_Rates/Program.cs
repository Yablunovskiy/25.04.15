using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Currency_Rates
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title="Курсы Валют.";
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Menu();
        }

        static void Menu()
        {

            bool next = true;
            int I = 0;
            string[] m1 = new string[] { " Online ", " Test " };
            Action<string, ConsoleColor, ConsoleColor> action = DispayMessage;
            
            Console.Clear();
            Console.WriteLine("\n\n\n");
            Console.WriteLine("\t\t\t   Отслеживание изменения курса валют.");
            for (int i = 0; i < m1.Length; i++)
            {
                if (i == I)
                    action(m1[i], ConsoleColor.Yellow, ConsoleColor.Blue);
                else
                {
                    action(m1[i], ConsoleColor.DarkBlue, ConsoleColor.DarkGray);
                }
            }
            Console.WriteLine("\t\t\t\t       Выход Esc.");

            while (next)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    --I;
                    if (I < 0) I = m1.Length - 1;
                }
                if (key.Key == ConsoleKey.DownArrow)
                {
                    ++I;
                    if (I > m1.Length - 1) I = 0;
                }

                Console.Clear();
                Console.WriteLine("\n\n\n");
                Console.WriteLine("\t\t\t   Отслеживание изменения курса валют.");
                for (int i = 0; i < m1.Length; i++)
                {
                    if (i == I)
                        action(m1[i], ConsoleColor.Yellow, ConsoleColor.Blue);
                    else
                    {
                        action(m1[i], ConsoleColor.DarkBlue, ConsoleColor.DarkGray);
                    }
                }
                Console.WriteLine("\t\t\t\t       Выход Esc.");

                if (key.Key == ConsoleKey.Enter)
                {
                    switch (I)
                    {
                        case 0:
                            Online();                     // сравниваем с данными на сайте
                            break;
                        case 1:
                            Test();                       // тестовый результат
                            break;
                    }
                }
                Console.Clear();
                Console.WriteLine("\n\n\n");
                Console.WriteLine("\t\t\t   Отслеживание изменения курса валют.");
                for (int i = 0; i < m1.Length; i++)
                {
                    if (i == I)
                        action(m1[i], ConsoleColor.Yellow, ConsoleColor.Blue);
                    else
                    {
                        action(m1[i], ConsoleColor.DarkBlue, ConsoleColor.DarkGray);
                    }
                }
                Console.WriteLine("\t\t\t\t       Выход Esc.");
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    next = false;
                }
            }
            Console.WriteLine("\n\n\n\t\t\t\t\t   Досвидания.\n\n\n");
        }

        static void Online()
        {
            Console.Clear();
            bool next = true;
            XmlDocument xmlDocument0 = new XmlDocument();
            XmlDocument xmlDocument1 = new XmlDocument();
            xmlDocument0.Load("Base/Base.xml");
            xmlDocument1.Load("http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
            
            while (next)
            {
                Console.Clear();
                var bank = xmlDocument1.DocumentElement.ChildNodes[1].ChildNodes[0].InnerText;
                Console.WriteLine("\n\t\t\t\t  " + bank);
                DateTime Date = DateTime.Parse(xmlDocument0.DocumentElement.ChildNodes[2].ChildNodes[0].Attributes["time"].Value);
                Console.WriteLine("\n\t\t\t\t     База на " + Date.Day + "." + Date.Month + "." + Date.Year);
                Date = DateTime.Parse(xmlDocument1.DocumentElement.ChildNodes[2].ChildNodes[0].Attributes["time"].Value);
                Console.WriteLine("\n\t\t\t\t   Last update " + Date.Day + "." + Date.Month + "." + Date.Year + "\n");
                int ch = 0;
                foreach (XmlNode node in xmlDocument0.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes)
                {
                    Console.Write("\t\t\t\t     " + node.Attributes["currency"].Value + " - ");
                    Compare(double.Parse(node.Attributes["rate"].Value.Replace(".", ",")), double.Parse(xmlDocument1.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes[ch].Attributes["rate"].Value.Replace(".", ",")));
                    ch++;
                }
                Console.WriteLine("\n\n\t\t\t\t        Выход Esc.");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    next = false;
                }
            }
            
        }

        static void Test()
        {
            XmlDocument xmlDocument = new XmlDocument();
            string address = "";
            bool next = true;
            int I = 1;
            address = "Sample/" + I + "Day.xml";
            xmlDocument.Load(address);
            Console.Clear();
            var bank = xmlDocument.DocumentElement.ChildNodes[1].ChildNodes[0].InnerText;
            Console.WriteLine("\n\t\t\t\t  " + bank);
            DateTime Date = DateTime.Parse(xmlDocument.DocumentElement.ChildNodes[2].ChildNodes[0].Attributes["time"].Value);
            Console.WriteLine("\n\t\t\t\t\t" + Date.Day + "." + Date.Month + "." + Date.Year + "\n");
            WriteXmlDocuments(xmlDocument);
            Console.WriteLine("\n\n\t\t\t\t     <-Back   Next->");
            Console.WriteLine("\n\n\t\t\t\t        Выход Esc.");
            
            while (next)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.RightArrow)
                {
                    ++I;
                    if (I > 5) I = 1;
                    WriteXml(I);
                }
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    --I;
                    if (I < 1 ) I = 5;
                    WriteXml(I);
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    next = false;
                }
            }
        }

        static void Compare(double A, double B)
        {
            if(A<B)
            {
                Result(A, B, " +",ConsoleColor.DarkGreen);
            }
            if(A>B)
            {
                Result(A, B, " ",ConsoleColor.Red);
            }
            else if(A==B)
            {
                Result(A, B, "  ",ConsoleColor.Gray);
            }

        }

        static void Result(double a, double b, string c, ConsoleColor color)
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(b + "\t"+ c + Math.Round((b-a),3));
            Console.ForegroundColor = prev;
        }

        static void DispayMessage(string msg, ConsoleColor color, ConsoleColor Bcolor)
        {
            ConsoleColor prev = Console.ForegroundColor;
            ConsoleColor Bprev = Console.BackgroundColor;
            Console.WriteLine();
            Console.Write("\t\t\t\t\t");
            Console.ForegroundColor = color;
            Console.BackgroundColor = Bcolor;
            Console.WriteLine(msg);
            Console.ForegroundColor = prev;
            Console.BackgroundColor = Bprev;
            Console.WriteLine();
        }

        static void WriteXml(int I)
        {
            int i = I - 1;
            if (i < 1) i = 5;
            Console.Clear();
            XmlDocument xmlDocument0 = new XmlDocument();
            XmlDocument xmlDocument1 = new XmlDocument();
            string address = "";
            address = "Sample/" + i + "Day.xml";
            xmlDocument0.Load(address);
            address = "Sample/" + I + "Day.xml";
            xmlDocument1.Load(address);
            var bank = xmlDocument1.DocumentElement.ChildNodes[1].ChildNodes[0].InnerText;
            Console.WriteLine("\n\t\t\t\t  " + bank);
            DateTime Date = DateTime.Parse(xmlDocument1.DocumentElement.ChildNodes[2].ChildNodes[0].Attributes["time"].Value);
            Console.WriteLine("\n\t\t\t\t\t" + Date.Day + "." + Date.Month + "." + Date.Year + "\n");
            int ch = 0;
            foreach (XmlNode node in xmlDocument0.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes)
            {
                Console.Write("\t\t\t\t  " + node.Attributes["currency"].Value + " - ");
                Compare(double.Parse(node.Attributes["rate"].Value.Replace(".", ",")), 
                    double.Parse(xmlDocument1.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes[ch].Attributes["rate"].Value.Replace(".", ",")));
                ch++;
            }
            Console.WriteLine("\n\n\t\t\t\t     <-Back   Next->");
            Console.WriteLine("\n\n\t\t\t\t        Выход Esc.");
        }

        static void WriteXmlDocuments(XmlDocument xmlDoc)
        {
            int i = 0;
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes)
            {
                Console.WriteLine("\t\t\t\t      " + node.Attributes["currency"].Value + " - " + double.Parse(node.Attributes["rate"].Value.Replace(".", ",")));
                 i++;
            }
        }

    }
}
