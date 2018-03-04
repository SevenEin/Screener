using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;



namespace Checker
{
    class Files
    {
        public Files(bool a)
        {
            Old = a;
        }
        public string adress { get; set; }
        public bool Old { get; }

    } // принимает адресс файла и true - если база. 
    class General
    {
        static string[] ReadFile(FileStream FSTemp) // считываем файл в массив
        {
            StreamReader StrRead = new StreamReader(FSTemp);
            List<string> Temp = new List<string>();
            string sttemp;
            while(!StrRead.EndOfStream)
            {   
                sttemp = StrRead.ReadLine();
                Temp.Add(sttemp);
            }
            FSTemp.Close();
            return Temp.ToArray();
        } 

        static FileStream OpenFile (Files a) // открытие файла, передаем объект класса Files для дальнейшего получения адресса к файлу и информации о каком файле идет речь
        {
            FileStream Test;
            string stream;
            string oldstr = "Введите путь к базовому файлу(файл в котором содержаться недопустимые строки)";
            string youngstr = "Введите путь к проверяемому-файлу(файл который нужно проверить на наличие недопустимых строк)";
            A:
            if(a.Old)
                Console.WriteLine(oldstr);
            else
                Console.WriteLine(youngstr);
            Console.Write(">");
            stream = Console.ReadLine();
            try
            {
                Test = new FileStream(stream, FileMode.Open);
            }
            catch
            {
                Console.WriteLine("Что-то пошло не так!");
                goto A;
            }
            a.adress = stream;
            return Test;

        }

        static string[] CheckerArray(string[] ArrayOld, string[] ArrayYoung)  // проверка на совпадения
        {
            List<string> Temp = new List<string>();
            Temp.AddRange(ArrayYoung);
            foreach (string yng in Temp.ToArray())
            {
                foreach (string old in ArrayOld)
                {
                    if (old == yng)
                    {
                        Console.WriteLine(old);
                        Temp.Remove(old);
                    }
                }
            }
            
            return Temp.ToArray();
        }

        static void CreateResult(string[] Temp) // вывод в файл нужного результата
        {
            FileStream Do = new FileStream(@"D:\done.txt", FileMode.Create);
            StreamWriter Lets = new StreamWriter(Do);
            foreach (string temp in Temp)
            {
                Lets.WriteLine(temp);
            }
            Lets.Close();
            Do.Close();
        }

        static void ChangeOld(Files COld, string[] YoungGood, string[] Old ) //добавление отсеяного массива в старый файл
        {
            List<string> Temp = new List<string>();

            Temp.AddRange(YoungGood);
            Temp.AddRange(Old);
            
            FileStream Ok = new FileStream(COld.adress, FileMode.Truncate);
            StreamWriter Help = new StreamWriter(Ok);
            foreach (string strd in Temp.ToArray())
            {
                Help.WriteLine(strd);
            }
            Help.Close();
            Ok.Close();

        }

        static void Main()
        {
            Console.WriteLine("Программа принимает два файла. Из одного файла она достает n-количество строк, которые не должны находится в другом файле");
            Console.WriteLine("Она проверяет на наличие в другом файле, и выводит отсеяный результат в файл done.txt на диске D");
            Console.WriteLine("Также файл с недопустимым перезапишется, к нему добавится все элементы done.txt");
            Files Old = new Files(true);
            Files Young = new Files(false);
            FileStream Stream;
            Stream = OpenFile(Young);
            string[] ArrayForYoung = ReadFile(Stream);
            Stream = OpenFile(Old);
            string[] ArrayForOld = ReadFile(Stream);
            string[] GoodYoung = CheckerArray(ArrayForOld, ArrayForYoung);
            CreateResult(GoodYoung);
            ChangeOld(Old, GoodYoung, ArrayForOld);
            




            Console.WriteLine("Нажмите любую клавишу чтобы закончить");
            Console.ReadLine();
        }
    }
}
