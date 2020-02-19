using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace HashCodePizza1
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var solutionA = CalculatePizzas("a_example.in");
            var solutionB = CalculatePizzas("b_small.in");
            var solutionC = CalculatePizzas("c_medium.in");
            var solutionD = CalculatePizzas("d_quite_big.in");
            var solutionE = CalculatePizzas("e_also_big.in");
        }

        private static void WriteResultToFile(string fileName, List<int> bestSolution)
        {
            using var fileStream = File.CreateText($"results/result_{fileName}");
            fileStream.WriteLine(bestSolution.Count);
            fileStream.WriteLine(string.Join(' ', bestSolution.OrderBy(x => x)));
        }

        private static List<int> CalculatePizzas(string fileName)
        {
            var inputFile = File.ReadAllLines(fileName);

            var firstLine = inputFile[0];
            var secondLine = inputFile[1];

            var maxSlices = int.Parse(firstLine.Split(' ')[0]);
            var pizzaCount = int.Parse(firstLine.Split(' ')[1]);

            var pizzas = secondLine.Split(' ').Select(int.Parse).ToImmutableList();

            // Console.WriteLine($"MaxSlices: {maxSlices} with {pizzaCount} pizzas");
            // Console.WriteLine($"Pizzas: {string.Join(",", pizzas)}");

            var bestSolution = new List<int>();
            var bestSolutionSum = 0;

            for (var activeIndexRound = pizzaCount - 1; activeIndexRound >= 0; activeIndexRound--)
            {
                var currentSolution = new List<int>();
                var currentSolutionSum = 0;
                for (int i = activeIndexRound; i >= 0; i--)
                {
                    var activePizza = pizzas[i];

                    var possibleNewSum = currentSolutionSum + activePizza;
                    if (possibleNewSum > maxSlices)
                    {
                        continue;
                    }

                    currentSolution.Add(i);
                    currentSolutionSum = possibleNewSum;

                    if (possibleNewSum == i)
                    {
                        break;
                    }
                }

                if (currentSolutionSum == maxSlices)
                {
                    bestSolution = currentSolution;
                    bestSolutionSum = currentSolutionSum;
                    break;
                }

                if (currentSolutionSum < bestSolutionSum)
                {
                    continue;
                }

                bestSolution = currentSolution;
                bestSolutionSum = currentSolutionSum;
            }

            // Console.WriteLine($"Best solution is: {string.Join(" ", bestSolution)}");
            // Console.WriteLine($"Best solution sum is: {bestSolutionSum}");
            Console.WriteLine($"{fileName} points: {bestSolutionSum} from max {maxSlices}");
            WriteResultToFile(fileName, bestSolution);
            return bestSolution;
        }
    }
}