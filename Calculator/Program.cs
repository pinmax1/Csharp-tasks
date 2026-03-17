using System;

class Calculator
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Введите первое число (или 'q' для выхода):");
            string? input1 = Console.ReadLine();

            if (input1?.ToLower() == "q")
                break;

            if (!double.TryParse(input1, out double num1))
            {
                Console.WriteLine("Ошибка: некорректное число!");
                continue;
            }

            Console.WriteLine("Введите второе число:");
            string? input2 = Console.ReadLine();
        
            if (!double.TryParse(input2, out double num2))
            {
                Console.WriteLine("Ошибка: некорректное число!");
                continue;
            }

            Console.WriteLine("Выберите операцию (+, -, *, /):");
            string? operation = Console.ReadLine();

            double result = 0;

            switch (operation)
            {
                case "+":
                    result = num1 + num2;
                    break;

                case "-":
                    result = num1 - num2;
                    break;

                case "*":
                    result = num1 * num2;
                    break;

                case "/":
                    if (num2 == 0)
                    {
                        Console.WriteLine("Ошибка: деление на ноль!");
                        continue;
                    }
                    result = num1 / num2;
                    break;

                default:
                    Console.WriteLine("Неизвестная операция!");
                    continue;
            }

            Console.WriteLine("Результат: " + result);
            Console.WriteLine();
        }
    }
}