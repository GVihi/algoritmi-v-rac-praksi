using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP_Vaja1
{
    class Program
    {
        static void CountingSort(string path)
        {
            Console.WriteLine("You chose Counting Sort!");
            StreamReader reader = new StreamReader(path);
            List<int> numbers = new List<int>();

            //Reading numbers from file to List
            string[] line = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < line.Length; i++)
            {
                numbers.Add(Convert.ToInt32(line[i]));
            }

            //Beginning of actual Counting Sort algorithm
            int min = numbers[0];
            int max = numbers[0];
            //Getting the smallest number in List
            for(int i = 1; i < numbers.Count; i++)
            {
                if (min > numbers[i]) min = numbers[i];
            }

            if (min < 0)
            {
                //if min < 0, deducting,for example, -9999 from -9999, makes it -9999 - (-9999), which equals -9999 + 9999, which equals 0
                DeductMin(ref numbers, min);
            }
            
            //Getting the biggest number in List
            for(int i = 0; i < numbers.Count; i++)
            {
                if (max < numbers[i]) max = numbers[i];
            }

            //Initializing C to 0
            List<int> C = new List<int>();
            for(int i = 0; i < max + 1; i++)
            {
                C.Add(0);
            }

            //For each numbers[i]: C[numbers[i]] = C[numbers[i]] + 1
            for(int i = 0; i < numbers.Count; i++)
            {
                C[numbers[i]] = C[numbers[i]] + 1;
            }

            //Iterating through C, for each iteration C[i] += C[i+1], starting at 1, because C[i] does not have a predecessor
            for(int i = 1; i < C.Count; i++)
            {
                C[i] += C[i - 1];
            }

            //Initializing B to 0's
            List<int> B = new List<int>();
            for(int i = 0; i < numbers.Count; i++)
            {
                B.Add(0);
            }

            for(int i = numbers.Count - 1; i >= 0; i--)
            {
                B[C[numbers[i]] - 1] = numbers[i];
                C[numbers[i]] -= 1;
            }

            if(min < 0)
            {
                //if min < 0, adding the minimal value back to all elements of List
                AddMin(ref B, min);
            }

            //Writing List to file out.txt
            StreamWriter writer = new System.IO.StreamWriter("out.txt");
            for(int i = 0; i < B.Count; i++)
            {
                writer.Write(B[i] + " ");
            }
            //B.ForEach(writer.WriteLine);
            //B.ForEach(writer.Write);
            writer.Close();


        }

        static void RomanSort(string path)
        {
            Console.WriteLine("You chose Roman Sort!");
            StreamReader reader = new StreamReader(path);
            List<int> numbers = new List<int>();

            //Reading from file into List
            string[] line = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < line.Length; i++)
            {
                numbers.Add(Convert.ToInt32(line[i]));
            }

            //Beginning of actual Counting Sort algorithm
            int min = numbers[0];
            int max = numbers[0];
            //Getting the smallest number in List
            for (int i = 1; i < numbers.Count; i++)
            {
                if (min > numbers[i]) min = numbers[i];
            }

            if (min < 0)
            {
                //if min < 0, deducting,for example, -9999 from -9999, makes it -9999 - (-9999), which equals -9999 + 9999, which equals 0
                DeductMin(ref numbers, min);
            }

            //Getting the biggest number in List
            for (int i = 0; i < numbers.Count; i++)
            {
                if (max < numbers[i]) max = numbers[i];
            }

            //Initializing C to 0
            List<int> C = new List<int>();
            for (int i = 0; i < max + 1; i++)
            {
                C.Add(0);
            }

            //For each numbers[i]: C[numbers[i]] = C[numbers[i]] + 1
            for (int i = 0; i < numbers.Count; i++)
            {
                C[numbers[i]] = C[numbers[i]] + 1;
            }

            List<int> B = new List<int>();
            for (int i = 0; i < numbers.Count; i++)
            {
                B.Add(0);
            }

            //Example: C[2] = 4, B[current] = 4, B[current + 1] = 4
            int itB = 0;
            for(int i = 0; i < C.Count; i++)
            {
                if(C[i] != 0)
                {
                    for(int j = 0; j < C[i]; j++)
                    {
                        B[itB] = i;
                        itB++;
                    }
                }
            }

            if (min < 0)
            {
                AddMin(ref B, min);
            }

            //StreamWriter used to write List B to a file, specifically out.txt. Cool stuff.
            StreamWriter writer = new System.IO.StreamWriter("out.txt");
            for (int i = 0; i < B.Count; i++)
            {
                writer.Write(B[i] + " ");
            }
            //B.ForEach(writer.WriteLine);
            //B.ForEach(writer.Write);
            writer.Close();
        }

        //Subtracting minimum value from all elements of List. In reality adding, since minimum value is negative ¯\_(ツ)_/¯
        static void DeductMin(ref List<int> numbers, int min)
        {
            for(int i = 0; i < numbers.Count; i++)
            {
                numbers[i] -= min;
            }
        }

        //Adding minimum value to all elements of List. In reality subtracting, since minimum value is negative ¯\_(ツ)_/¯
        static void AddMin(ref List<int> numbers, int min)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                numbers[i] += min;
            }
        }
        static void Main(string[] args)
        {
            //If user doesn't enter command line arguments, exit
            if(args.Length == 0)
            {
                Console.WriteLine("Please enter command line arguments.\nExample: ARP_Vaja1 0 in.txt");
                Console.ReadKey();
                return;
            }

            
            if(args[0] == "0")
            {
                //vaja1 0 in.txt
                CountingSort(args[1]);
            }else if(args[0] == "1")
            {
                //vaja1 1 in.txt
                RomanSort(args[1]);
            }
        }
    }
}
