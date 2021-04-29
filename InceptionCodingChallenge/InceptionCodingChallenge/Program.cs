using System;

namespace InceptionCodingChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator.Process(2, "T");
            Calculator.Process(2, "DTW");
            Calculator.Process(2, "TDTTDTTTTTWTW");
            Calculator.Process(3, "DTWTDTTTW");
            Calculator.Process(2, "DDDTTTTTTTTWWW");

            Console.ReadKey();
        }
    }

    class Calculator
    {
        // total[0..4] = 0
        // currentTick = 0
        // currentLevel = 0
        // perceivedTotal = 0
        //
        // T:
        //   currentTick += stepSize
        //   perceivedTotal++
        // D:
        //   currentLevel++
        //   start[currentLevel] = currentTick
        // W:
        //   total[currentLevel] += currentTick - start[currentLevel]
        //   currentLevel--
        //   if (currentTick % StepSize != 0) currentTick += stepSize - (currentTick % stepSize)
        //
        // stepSize => factor ^ (4 - currentLevel)
        public static void Process(int factor, string stream)
        {
            const int MaxDepth = 4;

            var total = new int[MaxDepth + 1];
            var start = new int[MaxDepth + 1];
            var currentTick = 0;
            var currentLevel = 0;
            var perceivedTotal = 0;

            int Pow(int x, int y) => (int)Math.Pow(x, y);
            int getStepSize() => Pow(factor, MaxDepth - currentLevel);

            foreach (var c in stream)
            {
                if (c == 'T' && currentLevel != 0)
                {
                    currentTick += getStepSize();
                    perceivedTotal++;
                }
                if (c == 'D')
                {
                    currentLevel++;
                    start[currentLevel] = currentTick;
                }
                if (c == 'W')
                {
                    total[currentLevel] += currentTick - start[currentLevel];
                    currentLevel--;
                    if (currentTick % getStepSize() != 0)
                    {
                        currentTick += getStepSize() - (currentTick % getStepSize());
                    }
                }
            }

            var realTotal = total[1] / Pow(factor, MaxDepth) + (total[1] % Pow(factor, MaxDepth) == 0 ? 0 : 1);

            Console.WriteLine(stream);
            Console.WriteLine(perceivedTotal);
            Console.WriteLine(realTotal);
            for (int i = 1; i <= MaxDepth; i++) Console.WriteLine(total[i] / Pow(factor, (MaxDepth - i)));
            Console.WriteLine();
        }
    }
}
