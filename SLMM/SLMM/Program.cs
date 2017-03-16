using System;
using System.Linq;
using System.Threading;

namespace SLMM
{
    enum Direction
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4
    }

    class Program
    {
        private static Direction direction = Direction.Right;

        private static readonly int minWidth = 1;
        private static int maxWidth;

        private static readonly int minHeight = 1;
        private static int maxHeight;

        private static int XPos = minWidth;
        private static int YPos = minHeight;

        private static bool exit = false;
        private static bool dataInvalid = true;

        static void Main()
        {
            while (dataInvalid == true) {
                DataValidation();
            }

            while (exit == false) {
                CommandInput();
            }
        }


        private static void CommandInput()
        {
            Console.WriteLine("Please enter a command.");
            string command = Console.ReadLine();

            switch (command.ToUpper())
            {
                case "TL":
                    Console.WriteLine(TurnLeft());
                    break;

                case "TR":
                    Console.WriteLine(TurnRight());
                    break;

                case "MF":
                    Console.WriteLine(MoveForward());
                    break;

                case "ML":
                    Console.WriteLine(MowLawn());
                    break;

                case "EXIT":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Command not found.");
                    break;
            }
        }

        private static string TurnLeft()
        {
            const int threadSleepTime = 10000;
            Thread.Sleep(threadSleepTime);

            if (direction == Direction.Left)
            {
                direction = Direction.Down;
                return MowerStatus(threadSleepTime, "turned the mower down");
            }

            if (direction == Direction.Right)
            {
                direction = Direction.Up;
                return MowerStatus(threadSleepTime, "turned the mower up");
            }

            if (direction == Direction.Up)
            {
                direction = Direction.Left;
                return MowerStatus(threadSleepTime, "turned the mower left");
            }

            if (direction == Direction.Down)
            {
                direction = Direction.Right;
                return MowerStatus(threadSleepTime, "turned the mower right");
            }

            throw new ArgumentException("Should not reach this point of the program Turn Left Method.");
        }

        private static string TurnRight()
        {
            const int threadSleepTime = 10000;
            Thread.Sleep(threadSleepTime);

            if (direction == Direction.Left)
            {
                direction = Direction.Up;
                return MowerStatus(threadSleepTime, "turned the mower up");
            }

            if (direction == Direction.Right)
            {
                direction = Direction.Down;
                return MowerStatus(threadSleepTime, "turned the mower down");
            }

            if (direction == Direction.Up)
            {
                direction = Direction.Right;
                return MowerStatus(threadSleepTime, "turned the mower right");
            }

            if (direction == Direction.Down)
            {
                direction = Direction.Left;
                return MowerStatus(threadSleepTime, "turned the mower left");
            }

            throw new ArgumentException("Should not reach this point of the program Turn Right Method.");
        }

        private static string MoveForward()
        {
            const int threadSleepTime = 15000;

            if (direction == Direction.Left) {
                XPos--;
            }

            if (direction == Direction.Right) {
                XPos++;
            }

            if (direction == Direction.Up) {
                YPos--;
            }

            if (direction == Direction.Down) {
                YPos++;
            }

            XPos = Clamp(XPos, minWidth, maxWidth);
            YPos = Clamp(YPos, minHeight, maxHeight);

            Thread.Sleep(threadSleepTime);

            return MowerStatus(threadSleepTime, "moved the mower forward");
        }

        private static string MowLawn()
        {
            const int threadSleepTime = 120000;
            Thread.Sleep(threadSleepTime);

            return MowerStatus(threadSleepTime, "mowed the current location");
        }

        private static string MowerStatus(int time, string action)
        {
            string timeString = time.ToString();
            return String.Format("Time: {0} seconds - Action: {1} - Position: X-{2}, Y-{3}.",
                                   timeString.Remove(timeString.Length - 3), action, XPos, YPos);
        }

        private static void DataValidation()
        {
            Console.WriteLine("Enter width and height {width, height}, type exit to finish");
            string input = Console.ReadLine();

            dataInvalid = false;

            if (input.ToLower() == "exit")
            {
                exit = true;
                return;
            }

            if (input.Count(i => i == ',') == 1)
            {
                string[] diemensions = input.Split(',');

                if (int.TryParse(diemensions[0], out maxWidth) == false)
                {
                    dataInvalid = true;
                    Console.WriteLine("Please enter numbers for the width.");
                }

                if (int.TryParse(diemensions[1], out maxHeight) == false)
                {
                    dataInvalid = true;
                    Console.WriteLine("Please enter numbers for the height.");
                }
            }
            else
            {
                dataInvalid = true;
                Console.WriteLine("Data isn't in correct format.");
            }
        }

        public static int Clamp(int value, int min, int max) {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
