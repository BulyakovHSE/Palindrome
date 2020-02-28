using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palindrome_Client
{
    class Program
    {
        private static string _selectedPath;
        private static string[] _files;
        private static Dictionary<string, bool> _results;
        private static PalindromeClient _palindromeClient;
        private static int _threadCount;

        [STAThread]
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                if (!string.IsNullOrWhiteSpace(_selectedPath))
                    Console.WriteLine($"Выбранная папка: {_selectedPath}");

                switch (LabFunctions.ConsoleFuncs.HorizontalMenu("Выбрать папку", "Выполнить запросы", "Выход"))
                {
                    case 0:
                        {
                            SelectFolder();
                        }
                        break;
                    case 1:
                        {
                            RunRequests();
                        }
                        break;
                    default: return;
                }
            }
        }

        private static void SelectFolder()
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _selectedPath = folderBrowserDialog.SelectedPath;
                _files = Directory.GetFiles(_selectedPath);
                if (_files.Length > 127)
                {
                    Console.WriteLine("Файлов в директории должно быть меньше 127!\n");
                    _selectedPath = "";
                    _files = null;
                    PressKeyToContinue();
                }
            }
        }

        private static void RunRequests()
        {
            if (_files == null)
            {
                Console.WriteLine("Сначала необходимо выбрать папку!\n");
                PressKeyToContinue();
                return;
            }
            var queue = new Queue<string>(_files);          // Очередь файлов для обработки
            var texts = _files.ToDictionary(file => file,   // Чтение и сохранение содержимого файлов
                file => File.ReadAllText(file));
            _results = new Dictionary<string, bool>();
            _palindromeClient = new PalindromeClient();
            var firstLoop = true;                           // Условие выполнения первого цикла алгоритма
            var tasks = new List<Task>();                   // Список задач, чтобы потом подождать их завершения

            // Цикл выполняющийся до первой ошибки, чтобы узнать количество потоков для одновременного вычисления сервера
            do
            {
                if (queue.Count > 0)
                    tasks.Add(Task.Factory.StartNew(async (o) =>
                    {
                        var file = (string)o;

                        // Вычисление результата
                        var result = await _palindromeClient.IsPalindromeAsync(texts[file]);
                        if (!result.HasValue)
                        {
                            firstLoop = false;      // Останавливаем цикл при первом же сообщении об ошибке
                            queue.Enqueue(file);    // Возвращаем файл, который не был обработан
                        }
                        else
                        {
                            ShowResult(file, result.Value);   // Выводим результат вычисления в консоль
                            _threadCount++;                   // Увеличиваем счетчик потоков
                        }
                    }, queue.Dequeue()).Unwrap());      // Передача файла в Task параметром, unwrapping Task<Task> в Task
                Thread.Sleep(50);
            } while (firstLoop && queue.Count > 0);

            Task.WaitAll(tasks.ToArray());
            tasks.Clear();

            // Обработка оставшихся файлов
            while (queue.Count > 0)
            {
                for (int i = 0; i < _threadCount; i++)
                    if (queue.Count > 0)
                        tasks.Add(Task.Factory.StartNew(async (o) =>
                        {
                            var file = (string)o;
                            var result = await _palindromeClient.IsPalindromeAsync(texts[file]);
                            if (!result.HasValue)
                                queue.Enqueue(file);
                            else
                                ShowResult(file, result.Value);
                        }, queue.Dequeue()).Unwrap());

                Task.WaitAll(tasks.ToArray());
                tasks.Clear();
            }
            Console.WriteLine($"\nКоличество выполненных запросов: {_results.Count}\n");
            PressKeyToContinue();
        }

        private static void ShowResult(string key, bool value)
        {
            _results.Add(key, value);
            Console.WriteLine($"{value,6} {key}");
        }

        private static void PressKeyToContinue()
        {
            Console.WriteLine("Нажмите любую клавишу для продолжения");
            Console.Read();
        }
    }
}
