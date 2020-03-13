using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace APBD
{
    class Program
    {
        static void Main(string[] args)
        {
            String Wejscie;
            String Wyscie;
            String Format;
            String Bledy = @"D:\łog.txt"; 

            if (args.Length == 3)
            {
                Wejscie = args[0];
                Wyscie = args[1];
                Format = args[2];
            }
            else 
            {
                Wejscie = @"D:\dane.csv";
                Wyscie = @"D:\zesult.xml";
                Format = "xml";
            }

            var hash = new HashSet<Student>(new OwnComparer());

            XmlTextWriter xmlWriter = new XmlTextWriter(Wyscie, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("uczelnia");
            xmlWriter.WriteStartElement("studenci");

            var lines = File.ReadLines(Wejscie);

            foreach (var line in lines)
            {
                var splittedLine = line.Split(",");

                var stud = new Student
                {
                    Imie = splittedLine[0],
                    Nazwisko = splittedLine[1],
                    Studia = splittedLine[2],
                    Tryb = splittedLine[3],
                    Index = splittedLine[4],
                    DataUrodzenia = splittedLine[5],
                    Email = splittedLine[6],
                    ImieMatki = splittedLine[7],
                    ImieOjca = splittedLine[8]
                };

                if (!hash.Add(stud))
                {
                    Console.WriteLine(splittedLine[0]);
                    using (StreamWriter writer = new StreamWriter(Bledy, true))
                    {
                        writer.WriteLine(line);
                    }
                }
                else
                {
                    xmlWriter.WriteStartElement("strudent");
                    xmlWriter.WriteElementString("index", stud.Index);
                    xmlWriter.WriteElementString("fname", stud.Imie);
                    xmlWriter.WriteElementString("lname", stud.Nazwisko);
                    xmlWriter.WriteElementString("birthdata", stud.DataUrodzenia);
                    xmlWriter.WriteElementString("email", stud.Email);
                    xmlWriter.WriteElementString("mothersName", stud.ImieMatki);
                    xmlWriter.WriteElementString("fathersName", stud.ImieOjca);
            
                    xmlWriter.WriteStartElement("uczelnia");
                    xmlWriter.WriteElementString("fathersName", stud.Studia);
                    xmlWriter.WriteElementString("fathersName", stud.ImieOjca);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
            }
           
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            xmlWriter.Close();
        }
    }
}
