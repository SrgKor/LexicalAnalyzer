using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LexicalAnalyzer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        DAL dal = new DAL();

        // Кнопка Запустить анализатор
        private void button_Analyze_Click(object sender, EventArgs e)
        {
            #region Возможный повторный анализ
            if (toolStripStatusLabel1.Text == "Анализ завершен")
            {
                DialogResult dr;
                dr = MessageBox.Show("Провести повторный анализ текста?", "Повторный анализ", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.No)
                    return;
            }
            #endregion

            List<string> allLines = dal.ReadTextFile();//список строк файла, null если файл не найден

            if (allLines == null)
            {
                MessageBox.Show("Отсутствует исходный файл Text.txt !", "Ошибка");
                toolStripStatusLabel1.Text = "Готов к работе";
                return;
            }

            List<string> allWords = new List<string>();//список слов

            foreach (string line in allLines)
            {
                string[] split = line.Split(new Char [] {' ', ',', '.', ':', '\t', ';', '-', '"', '#', '(',')' }, 
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in split)
                     allWords.Add(word);
            }


            Int64 totalNumberOfWords = allWords.Count; // общее кол-во слов

            #region Получение из списка всех слов 'allWords' - списка уникальных слов с указанием кол-ва вхождений 'result'
            //На входе: List<string> allWords; 
            //На выходе: List<string> result

            allWords.Sort();
            List<string> priorResult = new List<string>();

            for (int i = 0; i < allWords.Count; i++)
            {
                long currentWordCount = 1;
                priorResult.Add(allWords[i] + " 1");
                for (int j = i + 1; j < allWords.Count; j++)
                    if (allWords[i] == allWords[j])
                    {
                        currentWordCount++;
                        if (allWords[i] != "#")
                            priorResult[i] = allWords[i] + " " + currentWordCount;
                        allWords[j] = "#";
                    }
            }

            List<string> result = new List<string>();
            foreach (string item in priorResult)
            {
                if (!item.Contains('#'))
                    result.Add(item);
            }
            #endregion

            Int64 numberOfUniqueWords = result.Count; // кол-во уникальных слов

            result.Add("Общее кол-во слов в тексте: " + totalNumberOfWords);
            result.Add("Кол-во уникальных слов: " + numberOfUniqueWords);

            dal.WriteResultsFile(result);

            MessageBox.Show("Анализ успешно завершен. Результаты работы помещены в файл Results.txt", "Результат работы");
            toolStripStatusLabel1.Text = "Анализ завершен";
        }

        // Меню -> О программе
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Программа предназначена для проведения" +
                "\nлексического анализа текста по следующим показателям:" +
                "\n1) общее кол-во слов в тексте;" +
                "\n2) кол-во уникальных слов;" +
                "\n3) кол-во вхождений каждого слова." + 
                "\n\nАвтор: Сергей Королев (01.01.14)";
            MessageBox.Show(message, "О программе", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        // Меню -> Выход
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
