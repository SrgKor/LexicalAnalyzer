using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace LexicalAnalyzer
{
    // Уровень доступа к данным. Считывание исходного текстового файла. Запись результата в файл.
    class DAL
    {
        public List<string> ReadTextFile()
        {
            List<string> allWords = new List<string>();
            try
            {
                StreamReader file = new StreamReader("Text.txt", Encoding.Default);

                string line = "";
                while ((line = file.ReadLine()) != null)
                    allWords.Add(line);

                file.Dispose();
            }
            catch (FileNotFoundException)
            {
                return null; 
            }
           
            return allWords;
        }

        public void WriteResultsFile(List<string> allWords)
        {
            StreamWriter file = new StreamWriter(new FileStream("Results.txt", FileMode.Create), Encoding.Default);
            file.WriteLine("Результат работы программы. \n");
            foreach (string item in allWords)
            {
                file.WriteLine(item);
            }
           
            file.Dispose();
        }
    }
}
