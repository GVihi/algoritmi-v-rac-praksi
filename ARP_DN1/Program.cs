using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP_DN1
{
    class Program
    {
        static void RadixSort(string path)
        {
            StreamReader reader = new StreamReader(path);
            List<string> numbers = new List<string>();

            //Reading numbers from file to List
            string[] line = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < line.Length; i++)
            {
                numbers.Add(Convert.ToString(line[i]));
            }

            int maxLength = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                //Console.Write(numbers[i] + " ");
                if(numbers[i].Length > maxLength)
                {
                    maxLength = numbers[i].Length;
                }
            }

            List<int> numbersInt = new List<int>();
            for(int i = 0; i < numbers.Count; i++)
            {
                numbersInt.Add(0);
            }
            
            string filler = "";
            int diff;
            for(int i = 0; i < numbers.Count; i++)
            {
                filler = "";
                if(numbers[i].Length < maxLength)
                {
                    diff = maxLength - numbers[i].Length;
                    for(int j = 0; j < diff; j++)
                    {
                        filler = filler + "0";
                    }
                    numbers[i] = filler + numbers[i];
                }
            }
            
            for(int i = maxLength-1; i >= 0; i--)
            {
                for(int j = 0; j < numbers.Count; j++)
                {
                    //char[] chars = numbers[j].ToCharArray();
                    //Console.Write(Convert.ToString(chars[i]) + " ");
                    numbersInt[j] = (int)Char.GetNumericValue(numbers[j], i);
                    //Console.Write(numbersInt[j] + " ");
                }
                CountingSort(ref numbersInt, ref numbers);
                for (int k = 0; k < numbers.Count; k++)
                {
                    Console.Write(numbers[k] + " ");
                }
                Console.Write("\n");
            }

            //Writing List to file out.txt
            StreamWriter writer = new System.IO.StreamWriter("out.txt");
            for (int i = 0; i < numbers.Count; i++)
            {
                writer.Write(numbers[i] + " ");
            }
            writer.Close();
        }
        static void CountingSort(ref List<int> numbers, ref List<string> numbersString)
        {
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

            //Iterating through C, for each iteration C[i] += C[i+1], starting at 1, because C[i] does not have a predecessor
            for (int i = 1; i < C.Count; i++)
            {
                C[i] += C[i - 1];
            }

            //Initializing B to 0's
            List<int> B = new List<int>();
            List<string> D = new List<string>();
            for (int i = 0; i < numbers.Count; i++)
            {
                B.Add(0);
                D.Add("");
            }

            for (int i = numbers.Count - 1; i >= 0; i--)
            {
                B[C[numbers[i]] - 1] = numbers[i];
                D[C[numbers[i]] - 1] = numbersString[i];
                C[numbers[i]] -= 1;
            }

            if (min < 0)
            {
                //if min < 0, adding the minimal value back to all elements of List
                AddMin(ref B, min);
            }
            numbers = B;
            numbersString = D;
            /*
            //Writing List to file out.txt
            StreamWriter writer = new System.IO.StreamWriter("out.txt");
            for (int i = 0; i < B.Count; i++)
            {
                writer.Write(B[i] + " ");
            }
            //B.ForEach(writer.WriteLine);
            //B.ForEach(writer.Write);
            writer.Close();
            */

        }

        //Subtracting minimum value from all elements of List. In reality adding, since minimum value is negative ¯\_(ツ)_/¯
        static void DeductMin(ref List<int> numbers, int min)
        {
            for (int i = 0; i < numbers.Count; i++)
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
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter command line arguments.\nExample: ARP_DN1 in.txt");
                Console.ReadKey();
                return;
            }

            RadixSort(args[0]);
            Console.ReadKey();
        }
    }
}
