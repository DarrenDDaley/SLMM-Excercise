using System;
using System.Linq;
using System.Threading;

namespace SLMM
{
    // I used a enum to determine the direction 
    // as it can only be in one state at a time 
    // simplifying code and the names themselves make 
    // the code more clear and readable
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
        // Direction variable is there to keep track of the current way the lawn mower is facing
        // so that when the lawn mower moves forward it now which X or Y Position to move in
        private static Direction direction = Direction.Right;

        // The min and max values here are to make sure the lawm mower doesn't go out of bound
        private static readonly int minWidth = 1;
        private static int maxWidth;

        private static readonly int minHeight = 1;
        private static int maxHeight;

        // The X and Y Pos are here to track the lawm mowers position on the X and Y Posiiton
        // I picked the top left corner as the starting point of the grid because it's the 
        // one I use most in my previous programs as it's the default one for DirectX
        private static int XPos = minWidth;
        private static int YPos = minHeight;

        // The exit bool is used to exit the program all together
        private static bool exit = false;

        // The dataInvalid bool is used to exit the data validation loop 
        // once the data is verified to be correct
        private static bool dataInvalid = true;

        static void Main()
        {
            // the loop is exited once the data is valid
            while (dataInvalid == true) {
                DataValidation();
            }

            // the loop is exited once the exit command is entered
            while (exit == false) {
                CommandInput();
            }
        }

        // This method decideds which command to call based off the input the user enters
        private static void CommandInput()
        {
            Console.WriteLine("Please enter a command.");
            string command = Console.ReadLine();

            switch (command.ToUpper()) // Changed the casing just to make sure it absolutely matches
            {
                case "TL": // Turn Left
                    Console.WriteLine(TurnLeft());
                    break;

                case "TR": // Turn Right
                    Console.WriteLine(TurnRight());
                    break;

                case "MF": // Move Forward
                    Console.WriteLine(MoveForward());
                    break;

                case "ML": // Mow Lawn
                    Console.WriteLine(MowLawn());
                    break;

                case "EXIT": // Exit the program
                    exit = true;
                    break;

                default: // deals with unknown commands
                    Console.WriteLine("Command not found.");
                    break;
            }
        }

        // The Turn Left Function goes counter clockwise based off the current direction the lawn mower is facing
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


            // This exception should never occur, but if it does at least it gives useful information
            throw new ArgumentException("Should not reach this point of the program Turn Left Method.");
        }


        // The Turn Right Function goes clockwise based off the current direction the lawn mower is facing
        private static string TurnRight()
        {
            const int threadSleepTime = 10000;
            Thread.Sleep(threadSleepTime);

            if (direction == Direction.Left) {
                direction = Direction.Up;
                return MowerStatus(threadSleepTime, "turned the mower up");
            }

            if (direction == Direction.Right) {
                direction = Direction.Down;
                return MowerStatus(threadSleepTime, "turned the mower down");
            }

            if (direction == Direction.Up) {
                direction = Direction.Right;
                return MowerStatus(threadSleepTime, "turned the mower right");
            }

            if (direction == Direction.Down) {
                direction = Direction.Left;
                return MowerStatus(threadSleepTime, "turned the mower left");
            }

            // This exception should never occur, but if it does at least it gives useful information
            throw new ArgumentException("Should not reach this point of the program Turn Right Method.");
        }

        // The move forward method moves the lawn mower one position in the X or Y position 
        // based on the current direction the lawn mower is facing
        private static string MoveForward()
        {
            const int threadSleepTime = 15000;

            // Moves the lawn mower left by shifting it back one X Position 
            if (direction == Direction.Left) {
                XPos--;
            }

            // Moves the lawn mower right by shifting it forward one X Position
            if (direction == Direction.Right) {
                XPos++;
            }

            // Moves the lawn mower up by shifting the up one Y Position
            if (direction == Direction.Up) {
                YPos--;
            }

            // Moves the lawn mower down by shifting it down one Y Position
            if (direction == Direction.Down) {
                YPos++;
            }

            // Makes sure the X and Y Position does not go out of bounds
            XPos = Clamp(XPos, minWidth, maxWidth);
            YPos = Clamp(YPos, minHeight, maxHeight);

            Thread.Sleep(threadSleepTime);

            return MowerStatus(threadSleepTime, "moved the mower forward");
        }

        // This function mows the current position on the lawn
        private static string MowLawn()
        {
            const int threadSleepTime = 120000;
            Thread.Sleep(threadSleepTime);

            return MowerStatus(threadSleepTime, "mowed the current location");
        }

        // This method creates the output messages 
        private static string MowerStatus(int time, string action)
        {
            //This millisecond value is used as a divsor for the time 
            // to convert it from milliseconds to seconds
            const int milliseconds = 1000;


            // I used the Format method to cleanly construct the string
            return String.Format("Time: {0} seconds - Action: {1} - Position: X-{2}, Y-{3}.",
                                   time / milliseconds , action, XPos, YPos);
        }

        private static void DataValidation()
        {
            // I asked the user to seperate the width and height with a comma because it makes validation 
            // easier to do on the coding side
            Console.WriteLine("Enter width and height {width, height}, type exit to finish");
            string input = Console.ReadLine();

            dataInvalid = false;

            // Makes sure the exit command hasn't been invoked first
            // and it if has then the program simply sets exit to true and sends it back 
            // to the entry point of the program then exits
            if (input.ToLower() == "exit") { // Changed the casing just to make sure it absolutely matches
                exit = true;
                return;
            }


            // Checks to see if the comma is there only comma in the string, I used count as opposed to contains 
            // as contains only checks to see if there is a comma meaning multiple commas can be in there and still be correct
            // while the count specifys only one comma is neeeded
            if (input.Count(i => i == ',') == 1)
            {
                // Splits the string in two, the first value is supposed to be the width 
                // and the second supposed to be the right
                string[] dimensions = input.Split(',');

                // This checks to see if it is a number and if it isn't then it 
                // sends back an approrpiate message and if it is then assigns it as the 
                // max width of the lawn
                if (int.TryParse(dimensions[0], out maxWidth) == false) {
                    dataInvalid = true;
                    Console.WriteLine("Please enter numbers for the width.");
                }

                // This checks to see if it is a number and if it isn't then it 
                // sends back an approrpiate message and if it is then assigns it as the 
                // max height of the lawn
                if (int.TryParse(dimensions[1], out maxHeight) == false) {
                    dataInvalid = true;
                    Console.WriteLine("Please enter numbers for the height.");
                }
            }
            else // if the string doesn't have a comma then it invalid and a message is sent
            {
                dataInvalid = true;
                Console.WriteLine("Data isn't in correct format.");
            }
        }

        // This method makes sure the X and Y positions are kept within the min and max bounds 
        // It's based on a native method that is in the XNA Math library
        private static int Clamp(int value, int min, int max) {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
