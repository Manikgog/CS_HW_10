using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Xml.Serialization;
using System.Xml;

namespace CS_HW_10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task_1();

        }

        public static void Task_1()
        {
            /*
                Подсчитать сколько раз слово встречается в заданном тексте.
            Вывести статистику по тексту в виде json и xml файлов
            Текст:
            Вот дом, который построил Джек. А это пшеница, которая в темном чулане хранится В доме, который построил
            Джек. А это веселая птица-синица, Которая часто ворует пшеницу, Которая в тёмном чулане хранится В доме,
            который построил Джек.
            */
            string str = "Вот дом, который построил Джек.\n" +
                "А это пшеница, которая в темном чулане хранится\n" +
                "В доме, который построил Джек\n" +
                "А это веселая птица-синица,\n" +
                "Которая часто ворует пшеницу,\n" +
                "Которая в тёмном чулане хранится\n" +
                "В доме, который построил Джек.\n";
            string file_path = "C:\\Users\\Maksim\\source\\repos\\CS_HW_10\\jack.txt";
            string stat_path = "C:\\Users\\Maksim\\source\\repos\\CS_HW_10\\statistics.json";
            string stat_path_xml = "C:\\Users\\Maksim\\source\\repos\\CS_HW_10\\statistics.xml";

            using (FileStream fstream = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] writeText = Encoding.Unicode.GetBytes(str);
                fstream.Write(writeText, 0, writeText.Length);
            }
            UnicodeEncoding unicode = new UnicodeEncoding();
            string word;
            Console.Write("Введите слово: ");
            word = Console.ReadLine();
            string statistics;
            using (StreamReader reader = new StreamReader(file_path, unicode))
            {
                String stroka = reader.ReadToEnd();
                string[] words = stroka.Split(new char[] { ' ', ',', '.', '!', '?', '(', ')', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                int countWords = 0;
                word = word.ToLower();
                string w;
                for (int i = 0; i < words.Length; i++)
                {
                    Console.WriteLine(words[i]);
                    w = words[i].ToLower();
                    if (w.Equals(word))
                    {
                        countWords++;
                    }
                }
                statistics = "Слово " + word + " встречается в тексте " + countWords + " раз(а).";
            }

            // опции для сериализации объекта statistics для чтения кирилицы
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(statistics, options);

            // запись текста статистики в json файл
            using (FileStream fstream = new FileStream(stat_path, FileMode.Append, FileAccess.Write))
            {
                byte[] writeText = Encoding.Unicode.GetBytes(json);
                fstream.Write(writeText, 0, writeText.Length);
            }

            // чтение статистики из json файла
            using (StreamReader reader = new StreamReader(stat_path, unicode))
            {
                Console.WriteLine(reader.ReadLine());
            }

            XmlSerializer xml = new XmlSerializer(typeof(string));

            // запись статистики в xml файл
            using (FileStream fstream = new FileStream(stat_path_xml, FileMode.Truncate))
            {
                xml.Serialize(fstream, statistics);
            }

            // чтение xml файла со статистикой
            XmlDocument doc = new XmlDocument();

            doc.Load(stat_path_xml);

            XmlElement root = doc.DocumentElement;

            if (root != null)
            {
                foreach (XmlText xnode in root)
                {
                    Console.WriteLine(xnode?.Value);
                }
            }


        }
    }
}
