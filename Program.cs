using System.Text;

namespace InfaTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetWindowSize(Console.LargestWindowWidth - 2, Console.LargestWindowHeight - 2);

            Console.Title = "Calculator";
            Console.ForegroundColor = ConsoleColor.Magenta;


            GetInput();

        }

        private static void CreateBorder()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("=");
            }
        }
        private static void GetInput()
        {
            while (true)
            {
                GetHelp();
                Console.WriteLine("Введите номер операции: ");

                string input = Console.ReadLine();

                if (!int.TryParse(input, out int operation))
                {
                    Console.WriteLine("Неверно введена команда. Попробуйте еще раз. ");
                    continue;
                }

                try
                {
                    if (operation == 1) FirstFunction();
                    else if (operation == 2) SecondFunction();
                    else if (operation == 3) ThirdFunction();
                    else if (operation == 4) FourthFunction();
                    else Console.WriteLine("Неверно введена команда, попробуйте еще раз");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException exp)
                {
                    Console.WriteLine("Возникла какая-то магическая и неведомая ошибка. Скорее всего, калькулятор не справился с работой и умер. Попробуйте еще раз позже :(");
                }

                CreateBorder();
                Console.WriteLine("Введите любой символ, чтобы продолжить");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static void GetHelp()
        {
            Console.WriteLine("Это консольное приложение является калькулятором для чисел в доп.коде от -128 до 127, а также служит калькулятором для перевода и сложения чисел с плавающей точкой.");
            Console.WriteLine("Выполнил Устьянцев Е.Ю. Группа: ПрИ-102");
            CreateBorder();
            Console.WriteLine("Список команд: ");
            Console.WriteLine("Команда 1: Перевод целых чисел в дополнительный код.");
            Console.WriteLine("Команда 2: Сложение целых (положительных и отрицательных) чисел с использованием дополнительного кода.");
            Console.WriteLine("Команда 3: Реализовать процесс перевода вещественных чисел в формат с плавающей точкой");
            Console.WriteLine("Команда 4: Реализовать процесс сложения вещественных чисел в формат с плавающей точкой.");
            CreateBorder();

        }

        private static void FirstFunction()
        {
            int number = GetIntInput();

            Console.WriteLine($"Перевод числа {number} в дополнительный код.");

            if (number >= 0)
            {
                Console.WriteLine($"Положительное число в доп.коде равно самому себе же: {Convert.ToString(number, 2).PadLeft(8, '0')}");
            }
            else
            {
                string doubleSSNumber = Convert.ToString(Math.Abs(number), 2);

                Console.WriteLine($"Сначал преобразуем модуль числа в двоичную СС: {doubleSSNumber.PadLeft(8, '0')}");

                doubleSSNumber = doubleSSNumber.PadLeft(8, '0');
                doubleSSNumber = doubleSSNumber.Replace("1", "2");
                doubleSSNumber = doubleSSNumber.Replace("0", "1");
                doubleSSNumber = doubleSSNumber.Replace("2", "0");

                Console.WriteLine($"Затем инвертируем разряды числа в двоичной СС: {doubleSSNumber}");

                int tempNumber = Convert.ToInt32(doubleSSNumber, 2);
                tempNumber += 1;

                doubleSSNumber = Convert.ToString(tempNumber, 2);

                Console.WriteLine($"Затем прибавить 1 к этому числу и получаем: {doubleSSNumber}");

                Console.WriteLine($"Получили число {number} в доп.коде {doubleSSNumber}");
            }
        }

        private static void SecondFunction()
        {
            Console.WriteLine("Сложение чисел в доп.коде (числа от -128 до 127).");

            int firstNumber = GetIntInput();
            int secondNumber = GetIntInput();

            string firstExtra = GetExtraCode(firstNumber);
            string secondExtra = GetExtraCode(secondNumber);

            Console.WriteLine($"Сначала приводим оба числа к доп.коду: {firstNumber} : {firstExtra}, {secondNumber} : {secondExtra}");
            Console.WriteLine("Подробнее описывается в Команде 1 в этом приложении. За руководством - туда.");

            Console.WriteLine($"Выполняем складывание чисел: {firstExtra} + {secondExtra}");
            string result = Convert.ToString(Convert.ToInt32(firstExtra, 2) + Convert.ToInt32(secondExtra, 2), 2);
            Console.WriteLine(result);
            Console.WriteLine($"{firstExtra} + {secondExtra} = {result}");

            result = result.PadLeft(8, '0');

            if (result.Length > 8) result = result.Substring(result.Length - 8, result.Length - 1);

            Console.WriteLine(result);
            if (Math.Abs(firstNumber + secondNumber) > 127)
            {
                Console.WriteLine("Произошло переполнение(2-сс представдение числа превзошло 8 бит). Результат будет некорректен");
            }
            Console.WriteLine(result);
            if (result[0] == '1')
            {
                Console.WriteLine("Так как на старшем разряде стоит 1, то число отрицательное. Получаем его доп.код, переводим из 2-СС в 10-СС и приписываем минус");
                Console.WriteLine($"Результат: -{Convert.ToInt32(GetExtraCode(-Convert.ToInt32(result, 2)), 2)}");
            }
            else
            {
                Console.WriteLine("Так как на старшем разряде стоит 0, то число положительное. переводим из 2-СС в 10-СС и получаем:");
                Console.WriteLine($"Результат: {Convert.ToInt32(result, 2)}");
            }
        }

        private static void ThirdFunction()
        {
            ConvertFromFLoatToBinary();
        }

        private static void FourthFunction()
        {
            ConvertSumFromFloatToBinary();
        }

        private static int GetIntInput()
        {
            Console.WriteLine("Введите целое число -128 до 127 (соответствует 8-бит). В случае введение числа не из диапазона результат будет некорректным.");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int number) && number <= 127 && number >= -128)
            {
                throw new ArgumentException("Некорректное число. Попробуйте еще раз.");
            }
            return Convert.ToInt32(input);
        }
        private static string GetExtraCode(int number)
        {
            if (number >= 0)
            {
                return (Convert.ToString(number, 2)).PadLeft(8, '0');
            }
            else
            {
                string doubleSSNumber = Convert.ToString(Math.Abs(number), 2);

                doubleSSNumber = doubleSSNumber.PadLeft(8, '0');

                doubleSSNumber = doubleSSNumber.Replace("1", "2");
                doubleSSNumber = doubleSSNumber.Replace("0", "1");
                doubleSSNumber = doubleSSNumber.Replace("2", "0");

                int tempNumber = Convert.ToInt32(doubleSSNumber, 2);
                tempNumber += 1;

                doubleSSNumber = Convert.ToString(tempNumber, 2);

                return doubleSSNumber;
            }
        }

        private static string ConvertFromFLoatToBinary()
        {
            Console.WriteLine("Введите число с плавающей запятой(пример: 123,03), которое вы хотите привести к нормализованной записи:");
            string readLine = Console.ReadLine().Trim();

            if (double.TryParse(readLine, out double doubleNumber1))
                doubleNumber1 = doubleNumber1;
            else
                throw new ArgumentException("Некорректное число");

            string Result = ConverterFromFloatToBinaryFloat(doubleNumber1);

            return Result;
        }
        private static void ConvertSumFromFloatToBinary()
        {
            Console.WriteLine("Введите 2 числа с плавающей запятой через пробел. (разделитель дробной части - запятая)");

            string sumReadLine = Console.ReadLine().Trim();
            string[] sumSplit = sumReadLine.Split(" ");

            if (sumSplit.Length < 2)
            {
                throw new ArgumentException("Не все числа введены");
            }
            if (sumSplit.Length > 2)
            {
                throw new ArgumentException("Слишком много чисел!");
            }

            if (double.TryParse(sumSplit[0], out double number1))
                number1 = number1;
            else
                throw new ArgumentException("Число 1 некорректно");

            if (double.TryParse(sumSplit[1], out double number2))
                number2 = number2;
            else
                throw new ArgumentException("Число 2 некоректно");

            Console.WriteLine();
            Console.WriteLine("Определеям знак ответа:");

            double number1Abs = Math.Abs(number1);
            double number2Abs = Math.Abs(number2);
            double maxNumberAbs = Math.Max(number1Abs, number2Abs);

            int resultSign;

            if (maxNumberAbs == number1Abs)
            {
                Console.WriteLine("{0} больше по модулю {1}, знак ответа будет совпадать со знаком {0}", number1, number2);
                resultSign = Math.Sign(number1);
            }
            else
            {
                Console.WriteLine("{0} больше по модулю {1}, знак ответа совпадет со знаком {0}", number2, number1);
                resultSign = Math.Sign(number2);
            }

            if (resultSign == -1)
            {
                resultSign = 1;
            }
            else
            {
                resultSign = 0;
            }

            Console.WriteLine("Знак итога сложения: {0}", resultSign);

            Console.WriteLine();
            Console.WriteLine("Перевод обоих чисел в норм. эксп. формат:");

            string binaryFloatNumber1 = ConverterFromFloatToBinaryFloat(number1);
            string binaryFloatNumber2 = ConverterFromFloatToBinaryFloat(number2);

            Console.WriteLine("Суммируем два числа записи:");
            Console.WriteLine();

            Console.WriteLine("Для начала получаем равенство порядков:");
            Console.WriteLine();

            string numberSign1 = binaryFloatNumber1.Substring(0, 1);
            string numberOrder1 = binaryFloatNumber1.Substring(2, 8);
            string mantissa1 = binaryFloatNumber1.Substring(11);

            string numberSign2 = binaryFloatNumber2.Substring(0, 1);
            string numberOrder2 = binaryFloatNumber2.Substring(2, 8);
            string mantissa2 = binaryFloatNumber2.Substring(11);

            if (ToDec(numberOrder1, 2) == ToDec(numberOrder2, 2))
            {
                Console.WriteLine("Действий не требуется, порядки равны.");
            }

            else if (ToDec(numberOrder1, 2) > ToDec(numberOrder2, 2))
            {
                Alignment(binaryFloatNumber1, ref binaryFloatNumber2, numberOrder1, numberSign2, ref numberOrder2, ref mantissa2);
            }
            else
            {
                Alignment(binaryFloatNumber2, ref binaryFloatNumber1, numberOrder2, numberSign1, ref numberOrder1, ref mantissa1);
            }
            Console.WriteLine();
            Console.WriteLine("Числа после уравнивания порядка:");
            Console.WriteLine("Число 1: {0}", binaryFloatNumber1);
            Console.WriteLine("Число 2: {0}", binaryFloatNumber2);
            Console.WriteLine();
            Console.WriteLine("Складывем мантиссы и подставляе полученный знак:");
            string resultOrder = numberOrder2;
            string resultMantissa = CorrectSum(mantissa1, mantissa2);

            if (resultMantissa.Length > 23)
            {
                Console.WriteLine();
                Console.WriteLine("Итоговая мантисса имеет длину > 23, значит увеличиваем смещенной порядок на 1, убираем самый первый элемент мантиссы и сдвигаем на 1 вправо.");
                resultOrder = Sum(resultOrder, "1");
                resultMantissa = resultMantissa.Substring(1);
                resultMantissa = "0" + resultMantissa;
                resultMantissa = resultMantissa.Substring(0, 23);
            }

            Console.WriteLine();

            string resultBinary = resultSign + "|" + resultOrder + "|" + resultMantissa;

            float resultDec = (float)number1 + (float)number2;


            Console.WriteLine("Нормализованный ответ: {0}", resultBinary);
            Console.WriteLine("Ответ в десястичной СС: {0}", resultDec);

        }

        public static string Sum(string number1, string number2)
        {

            StringBuilder sum = new StringBuilder();

            string num1 = number1;
            string num2 = number2;

            int base1 = 2;

            int NumD1 = ToDec(num1, 2);
            int NumD2 = ToDec(num2, 2);
            int len = 0;
            string maxNum = "";
            string minNum = "";

            if (NumD1 > NumD2)
            {
                len = num1.Length;
                maxNum = num1;
                minNum = num2;
            }
            else
            {
                len = num2.Length;
                maxNum = num2;
                minNum = num1;
            }

            maxNum = new string(maxNum.Reverse().ToArray());
            minNum = new string(minNum.Reverse().ToArray());


            int des = 0;
            for (int i = 0; i < len; i++)
            {
                int res = 0;
                int digit1 = maxNum[i] - '0';
                int digit2 = 0;
                if (i < minNum.Length)
                    digit2 = minNum[i] - '0';

                res = des + digit1 + digit2;

                if (res >= base1)
                {

                    sum.Append(res - base1);
                    if (i == len - 1)
                    {
                        sum.Append("1");

                    }
                    else
                    {
                        des = 1;
                    }
                }
                else
                {
                    des = 0;
                    sum.Append(res);
                }
            }

            maxNum = new string(maxNum.Reverse().ToArray());
            minNum = new string(minNum.Reverse().ToArray());
            string res1 = sum.ToString();
            res1 = new string(res1.Reverse().ToArray());
            int lenRes = res1.Length;

            return res1.ToString();
        }

        public static string CorrectSum(string number1, string number2)
        {

            StringBuilder sum = new StringBuilder();

            string num1 = number1;
            string num2 = number2;

            int base1 = 2;

            int NumD1 = ToDec(num1, 2);
            int NumD2 = ToDec(num2, 2);
            int len = 0;
            string maxNum = "";
            string minNum = "";

            if (NumD1 > NumD2)
            {
                len = num1.Length;
                maxNum = num1;
                minNum = num2;
            }
            else
            {
                len = num2.Length;
                maxNum = num2;
                minNum = num1;
            }
            Console.WriteLine("Производим большее + меньшее");

            Console.Write(" ");
            Console.WriteLine(maxNum);
            Console.WriteLine("+");
            Console.Write(" ");

            Console.WriteLine(minNum.PadLeft(len));
            Console.Write(" ");
            for (int i = 0; i < len; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();

            maxNum = new string(maxNum.Reverse().ToArray());
            minNum = new string(minNum.Reverse().ToArray());

            Console.WriteLine("Посимвольное сложение чисел:");
            int des = 0;
            for (int i = 0; i < len; i++)
            {
                int res = 0;
                int digit1 = maxNum[i] - '0';
                int digit2 = 0;
                if (i < minNum.Length)
                    digit2 = minNum[i] - '0';

                res = des + digit1 + digit2;
                if (des == 0)
                {
                    Console.WriteLine("{0}+{1}", digit1, digit2);
                }
                else
                {
                    Console.WriteLine("{0}+{1}, добавим 1 из предыдущего разряда", digit1, digit2);
                }

                Console.WriteLine(res);

                if (res >= base1)
                {

                    sum.Append(res - base1);
                    if (i == len - 1)
                    {
                        Console.WriteLine("У нас получилось число большее {0}, это последнее число в суммировании. Значит записываем число {1} и приписываем слева 1", base1 - 1, res - base1);
                        sum.Append("1");

                    }
                    else
                    {
                        Console.WriteLine("У нас получилось число большее {0}, значит записываем число {1} и прибавляем 1 к след. разряду", base1 - 1, res - base1);
                        des = 1;
                    }
                }
                else
                {
                    Console.WriteLine("Число получилось меньше чем 2, значит записываем число {0}", res);
                    des = 0;
                    sum.Append(res);
                }
            }

            maxNum = new string(maxNum.Reverse().ToArray());
            minNum = new string(minNum.Reverse().ToArray());
            string res1 = sum.ToString();
            res1 = new string(res1.Reverse().ToArray());
            int lenRes = res1.Length;

            Console.Write(" ");
            Console.WriteLine(maxNum.PadLeft(lenRes));
            Console.WriteLine("+");
            Console.Write(" ");

            Console.WriteLine(minNum.PadLeft(lenRes));
            Console.Write(" ");
            for (int i = 0; i < lenRes; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            Console.Write(" ");
            Console.WriteLine(res1);

            return res1.ToString();
        }

        private static void Alignment(string binaryFloatNumber1, ref string binaryFloatNumber2, string numberOrder1, string numberSign2, ref string numberOrder2, ref string mantissa2)
        {
            Console.WriteLine("Число {0} порядок больше,\n" +
                                "ззначит увеличиваем порядок числа {1}", binaryFloatNumber1, binaryFloatNumber2);
            Console.WriteLine();
            Console.WriteLine("Поэтому сдвигаем мантиссу вправо и увеличиваем значение смещенного порядка на 1 пока порядки не сравняются: ");

            Console.WriteLine();
            bool flag = true;
            while (numberOrder2 != numberOrder1)
            {
                numberOrder2 = Sum(numberOrder2, "1");
                if (flag)
                {
                    mantissa2 = "1" + mantissa2;
                    flag = false;
                }
                else
                {
                    mantissa2 = "0" + mantissa2;
                }

                mantissa2 = mantissa2.Substring(0, 23);
                Console.WriteLine(numberSign2 + "|" + numberOrder2 + "|" + mantissa2);
            }

            binaryFloatNumber2 = numberSign2 + "|" + numberOrder2 + "|" + mantissa2;
        }
        private static string ConverterFromFloatToBinaryFloat(double doubleNumber1)
        {
            string str = Convert.ToString(doubleNumber1);
            string[] parts = str.Split(',');

            int intIntegerPartNumber = int.Parse(parts[0]);
            string floatPartNumber;
            if (parts.Length == 1)
            {
                floatPartNumber = "0";
            }
            else
            {
                floatPartNumber = parts[1];
            }

            string stringDoubleNumber1 = intIntegerPartNumber.ToString() + "," + floatPartNumber;
            Console.WriteLine();
            Console.WriteLine("Перевед числа {0} в 2-СС", stringDoubleNumber1);
            Console.WriteLine("Перевод целой части числа в 2-СС: ");

            string binaryIntegerPartNumber = IntegerPartNumberToBinary(stringDoubleNumber1, intIntegerPartNumber);
            int lenOfbinaryIntegerPartNumber;
            if (doubleNumber1 < 0 && intIntegerPartNumber == 0)
            {
                binaryIntegerPartNumber = "-" + binaryIntegerPartNumber;
            }
            if (binaryIntegerPartNumber.Substring(0, 1) == "-")
            {
                lenOfbinaryIntegerPartNumber = binaryIntegerPartNumber.Length - 1;
            }
            else
            {
                lenOfbinaryIntegerPartNumber = binaryIntegerPartNumber.Length;
            }
            Console.WriteLine();
            Console.WriteLine("Перевод дробной части числа 2-СС: ");
            Console.WriteLine();
            string resultStringBinaryFloatPartNumber = FloatPartNumberToBinary(stringDoubleNumber1, ref floatPartNumber, lenOfbinaryIntegerPartNumber);

            Console.WriteLine();
            Console.WriteLine("Результат: число {0} имеет вид: {1},{2} в 2-СС", stringDoubleNumber1, binaryIntegerPartNumber, resultStringBinaryFloatPartNumber);

            string resultBinaryNumber = binaryIntegerPartNumber + resultStringBinaryFloatPartNumber;
            Console.WriteLine();
            Console.WriteLine("Теперь переведем число в формат со смещенным порядком и мантиссой:");
            Console.WriteLine();
            Console.WriteLine("Представим число в нормализованной экспоненциальной форме:");

            string numberSign = "";
            string mantissa = "";
            int numberOrder = 0;

            if (intIntegerPartNumber != 0)
            {
                numberSign = resultBinaryNumber.Substring(0, 1 + binaryIntegerPartNumber.Length - lenOfbinaryIntegerPartNumber);
                mantissa = resultBinaryNumber.Substring(1 + binaryIntegerPartNumber.Length - lenOfbinaryIntegerPartNumber);
                numberOrder = lenOfbinaryIntegerPartNumber - 1;
            }
            else
            {

                numberSign = resultStringBinaryFloatPartNumber.TrimStart('0').Substring(0, 1);
                if (doubleNumber1 < 0)
                {
                    numberSign = "-" + numberSign;
                }
                mantissa = resultStringBinaryFloatPartNumber.TrimStart('0').Substring(1);

                numberOrder = -(resultStringBinaryFloatPartNumber.Length - mantissa.Length);

                if (mantissa.Length == 0)
                {
                    mantissa = "0";
                }
            }
            Console.WriteLine("Получаем: {0},{1}*2^{2}", numberSign, mantissa, numberOrder);
            Console.WriteLine();

            Console.WriteLine("Получаем смещенный порядок и переводим его в 2-СС");
            int shiftedNumberOrder = 127 + numberOrder;
            Console.WriteLine("Смещенный порядок в 2-СС: 127 + {0} = {1}", numberOrder, shiftedNumberOrder);

            Console.WriteLine("Перевод смещенной порядка в 2-СС:");

            string binaryShiftedNumberOrder = RFromDecemberToBin(shiftedNumberOrder);
            Console.WriteLine();
            Console.WriteLine("Составляем представление числа {0} в формате нормализованной записи", stringDoubleNumber1);
            Console.WriteLine();
            string Result = "";
            if (doubleNumber1 < 0)
            {
                Console.WriteLine("Первая цифра 1, т.к число отрицательное");
                Console.WriteLine("Затем пишем значение смещенного порядка: {0}", binaryShiftedNumberOrder.TrimStart('-'));
                Console.WriteLine("Записываем мантиссу: {0}", mantissa.PadRight(23, '0'));
                Console.WriteLine("Когда значение мантиссы меньше 23 мы справа заполняем оставшееся место нулями.");

                Result = "1" + "|" + binaryShiftedNumberOrder.TrimStart('-').PadLeft(8, '0') + "|" + mantissa.PadRight(23, '0');
            }
            else
            {
                Console.WriteLine("Первая цифра 0, т.к число положительное");
                Console.WriteLine("Затем пишем значение смещенного порядка: {0}", binaryShiftedNumberOrder.TrimStart('-'));
                Console.WriteLine("Записываем мантиссу: {0}", mantissa.PadRight(23, '0'));
                Console.WriteLine("Когда значение мантиссы меньше 23 мы справа заполняем оставшееся место нулями.");
                Result = "0" + "|" + binaryShiftedNumberOrder.TrimStart('-').PadLeft(8, '0') + "|" + mantissa.PadRight(23, '0');
            }

            Console.WriteLine();
            Console.WriteLine("Число {0} в формате нормализованной записи: {1}", stringDoubleNumber1, Result);
            Console.WriteLine();
            return Result;
        }
        private static string FloatPartNumberToBinary(string stringDoubleNumber1, ref string stringFloatPartNumber, int lenOfbinaryIntegerPartNumber)
        {
            Console.WriteLine("Чтобы перевести число 0,{0} из 10-СС в 2-СС нужно умножать число на 2,а затем", stringFloatPartNumber);
            Console.WriteLine("записывать получившуюся целую часть, до тех пор, пока число 0,{0} не станет целым", stringFloatPartNumber);
            Console.WriteLine("В случае, если 0,{0} в 2-СС ибесконечно,", stringFloatPartNumber);
            Console.WriteLine("записываем первые {0} цифр этого числа(дабы избежать переполнение мантиссы)", 23 - lenOfbinaryIntegerPartNumber + 1);
            Console.WriteLine();
            int lenOfFloatPartNumber = stringFloatPartNumber.Length;
            int floatPartNumber = int.Parse(stringFloatPartNumber);
            StringBuilder resultBinaryFloatPartNumber = new StringBuilder();
            for (int i = 0; i < 23 - lenOfbinaryIntegerPartNumber + 1; i++)
            {
                Console.WriteLine("0|{0}", floatPartNumber.ToString().PadLeft(lenOfFloatPartNumber, '0'));
                Console.WriteLine("*");
                Console.WriteLine(" |" + "2".PadLeft(lenOfFloatPartNumber, ' '));
                Console.WriteLine("".PadRight(lenOfFloatPartNumber + 2, '-'));
                floatPartNumber = floatPartNumber * 2;
                string floatStringPartNumber = floatPartNumber.ToString().PadLeft(lenOfFloatPartNumber + 1, '0');

                string stringPartBeforeI = floatStringPartNumber.Substring(0, 1);
                string stringPartAfterI = floatStringPartNumber.Substring(1);
                resultBinaryFloatPartNumber.Append(stringPartBeforeI);

                Console.Write(stringPartBeforeI);

                Console.WriteLine("|" + stringPartAfterI);
                floatPartNumber = int.Parse(stringPartAfterI);
                Console.WriteLine();

                if (stringPartAfterI == "".PadLeft(lenOfFloatPartNumber, '0'))
                {
                    break;
                }
            }

            string resultStringBinaryFloatPartNumber = resultBinaryFloatPartNumber.ToString();
            Console.WriteLine();

            Console.WriteLine("Дробная часть числа {0} в 2-СС: 0,{1}", stringDoubleNumber1, resultStringBinaryFloatPartNumber);

            return resultStringBinaryFloatPartNumber;
        }

        private static string IntegerPartNumberToBinary(string stringDoubleNumber1, int intIntegerPartNumber)
        {
            bool ifLessThanZero = false;
            if (intIntegerPartNumber < 0)
            {
                Console.WriteLine();
                Console.WriteLine("Т.к. число {0} отриц., переведем в 2-СС модуль этого числа и просто допишем слева \"-\"", intIntegerPartNumber);

                ifLessThanZero = true;
            }

            intIntegerPartNumber = Math.Abs(intIntegerPartNumber);

            string binaryIntegerPartNumber = RFromDecemberToBin(intIntegerPartNumber);

            if (ifLessThanZero)
            {
                Console.WriteLine();
                Console.WriteLine("Т.к. число -{0} отриц., то допишем слева \"-\"", intIntegerPartNumber);
                binaryIntegerPartNumber = "-" + binaryIntegerPartNumber;
            }
            Console.WriteLine();

            Console.WriteLine("Целая часть числа {0} в 2-СС: {1}", stringDoubleNumber1, binaryIntegerPartNumber);

            return binaryIntegerPartNumber;
        }

        static string RFromDecemberToBin(int number)
        {
            int baze = 2;
            int numberStart = number;
            StringBuilder builder = new StringBuilder();

            Console.WriteLine();

            Console.WriteLine("Перевод {0} из 10-СС в 2-СС", number);

            Console.WriteLine("Находим остатки от деления числа {0} на число 2, пока деление возможно", number, baze);
            do
            {
                int mod = number % baze;
                char c = (char)('0' + mod);
                Console.WriteLine("Остаток : {0} / 2 = {1}", number, mod);
                builder.Append(c);
                number /= baze;
            } while (number >= baze);

            Console.WriteLine("Т.к. деление {0} на 2 больше невозможно, то последним остатком будет число {0}", number);
            if (number != 0)
            {

                builder.Append((char)('0' + number));
            }
            Console.WriteLine();
            Console.WriteLine("Инвертируем запись остатков.");
            string result = string.Join("", builder.ToString().Reverse());


            Console.WriteLine();
            Console.WriteLine("{0} в 2-СС: {1}", numberStart, result);


            return result;
        }

        static int ToDec(string number, int baze)
        {

            long result = 0;
            int digitsCount = number.Length;
            int num;

            for (int i = 0; i < digitsCount; i++)
            {
                char c = number[i];
                num = c - '0';
                result *= baze;
                result += num;
            }

            return (int)result;
        }
    }
}