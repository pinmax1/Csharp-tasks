using System;

class Calculator
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Введите первое число (или 'q' для выхода):");
            string input1 = Console.ReadLine();

            if (input1.ToLower() == "q")
                break;

            double num1 = Convert.ToDouble(input1);

            Console.WriteLine("Введите второе число:");
            double num2 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Выберите операцию (+, -, *, /):");
            string operation = Console.ReadLine();

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

        Console.WriteLine("Программа завершена.");
    }
}