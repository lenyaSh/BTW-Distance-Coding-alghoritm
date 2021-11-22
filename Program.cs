using System;
using System.Text;

namespace ConsoleApp1 {
    class Program {
        static void Main(string[] args) {
            const string str = "СС______LLWWWWISNUUAANNHWDD_AAOOO_____OOEEDTESFDSE";

            byte[] buffer_in = Encoding.UTF8.GetBytes(str);
            byte[] buffer_out = new byte[buffer_in.Length];
            byte[] buffer_decode = new byte[buffer_in.Length];

            BWTImplementation bwt = new();

            int primary_index = 0;
            bwt.Bwt_encode(buffer_in, buffer_out, buffer_in.Length, ref primary_index);
            bwt.Bwt_decode(buffer_out, buffer_decode, buffer_in.Length, primary_index);
            /*Console.WriteLine("Encoded string: {0}", Encoding.UTF8.GetString(buffer_out));
            Console.WriteLine("Decoded string: {0}", Encoding.UTF8.GetString(buffer_decode));*/

            //DC coder = new(Encoding.UTF8.GetString(buffer_decode));
            DC coder = new(str);
            foreach (int elem in coder.Encode()) {
                Console.Write($"{elem}, ");
            }
            Console.WriteLine();
        }
    }



}
