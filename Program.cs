using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp1 {
    class Program {

        static (Dictionary<byte, double>, int) GetBytesFromFile(string path, Dictionary<byte, double> dict) {
            List<byte> b = new List<byte>();
            int temp;
            char symbol;

            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate)) {
                fstream.Seek(0, SeekOrigin.Begin);

                temp = fstream.ReadByte();
                symbol = (char)temp;
                while (temp != -1) {
                    b.Add((byte)temp);
                    temp = fstream.ReadByte();
                    symbol = (char)temp;
                }
            }

            foreach (byte elem in b) {
                if (dict.ContainsKey(elem)) {
                    dict[elem]++;
                }
                else {
                    dict.Add(elem, 1);
                }
            }

            return (dict, b.Count);
        }

        static double GetEntropy(string pathToFile) {
            Dictionary<byte, double> result = new(), temp = new();

            int sizeFileInBytes;

            (result, sizeFileInBytes) = GetBytesFromFile(pathToFile, result);

            double probability;
            double entr = 0;

            foreach (KeyValuePair<byte, double> elem in result) {
                probability = elem.Value / sizeFileInBytes;
                entr -= probability * Math.Log2(probability);

                temp.Add(elem.Key, probability);
            }

            return entr;
        }


        static void Main() {
            string pathToSourceFile = @"D:/code.txt";
            string pathToDestinationFile = @"D:/code_encode.txt";

            byte[] buffer_in;
            using (StreamReader sr = new(pathToSourceFile)) {
                buffer_in = Encoding.UTF8.GetBytes(sr.ReadToEnd()); 
            }

            BWTImplementation bwt = new();
            byte[] buffer_out = new byte[buffer_in.Length];

            int primary_index = 0;
            bwt.Bwt_encode(buffer_in, buffer_out, buffer_in.Length, ref primary_index);
            
            DC coder = new(buffer_in);

            Console.WriteLine("Начался процесс сжатия файла...");
            // сжимаем
            (var listOfDistance, var indexOfNewSymbols) = coder.Encode();
            Console.WriteLine("Сжатие прошло успешно!");

            Console.WriteLine($"Энтропия исходного файла = {GetEntropy(pathToSourceFile)}");

            // формируем сжатый файл
            using (StreamWriter sw = new(pathToDestinationFile)) {
                foreach(KeyValuePair<byte, int> elem in indexOfNewSymbols) {
                    sw.Write($"{elem.Key} {elem.Value} ");
                }
                sw.WriteLine();

                foreach(int elem in listOfDistance) {
                    sw.Write($"{elem} ");
                }
            }

            Console.WriteLine($"Энтропия сжатого файла = {GetEntropy(pathToDestinationFile)}");

            string[] temp;

            var listOfDistance2 = new List<int>();
            var indexOfNewSymbols2 = new Dictionary<byte, int>();
            //считываем сжатый файл
            using(StreamReader sr = new(pathToDestinationFile)) {
                temp = sr.ReadLine().Split(" ");

                for(int i = 0; i < temp.Length - 1; i += 2) {
                    indexOfNewSymbols2.Add(Convert.ToByte(temp[i]), Convert.ToInt32(temp[i + 1]));
                }

                temp = sr.ReadLine().Split(" ");
                for (int i = 0; i < temp.Length - 1; i++) { 
                    listOfDistance2.Add(int.Parse(temp[i]));
                }
            }

            Console.WriteLine("Начался процесс декодирования файла...");
            //декодируем сжатый файл
            byte[] buffer_decode = coder.Decode(listOfDistance2, indexOfNewSymbols2, buffer_in.Length);
            bwt.Bwt_decode(buffer_out, buffer_decode, buffer_in.Length, primary_index);
            Console.WriteLine("Файл успешно декодирован!");

            // выводим декодированную последовательность в новый файл
            using StreamWriter sw_decode = new(@"D:/code_decode.txt");
            sw_decode.Write(Encoding.UTF8.GetString(buffer_decode));
        }
    }
}
