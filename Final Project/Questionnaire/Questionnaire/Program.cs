using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    class Program
    {
        static void Main(string[] args)
        {
            //set up "Question" variables 
            Console.WriteLine( "string answer2 = \"A) String\";");
            Console.WriteLine("string answer1 = \"B) int\";");
            Console.WriteLine("string answer3 = \"C) string\";");
            Console.WriteLine("string answer4 = \"D) obj\";");

            string input1;
            string input2;
            string input3;
            string input4;
            bool correct1 = false;
            bool correct2 = false;
            bool correct3 = false;
            bool correct4 = false;

            //set up real Variables
            string answer5 = "A) String";
            string answer6 = "B) int";          
            string answer7 = "C) string";
            string answer8 = "D) obj";

            string input;
            bool correct = false;

            while (correct1 == false)
            {
                Console.WriteLine("Please write in the correct \"loop\" to make the program come back to this point  if the player gets the question wrong");
                input1 = Console.ReadLine();

                if (input1 == "while (correct == false)")
                {
                    Console.WriteLine(" ");
                    correct1 = true;
                }
                else
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("You've made a mistake on this line");
                }
            }

            Console.WriteLine("while (correct == false)");
            Console.WriteLine("{");
            Console.WriteLine("Console.WriteLine(\"When creating variable to hold a word, what must you precede the name of the variable with ? \");");
            Console.WriteLine(" Console.WriteLine(answer5);");
            Console.WriteLine(" Console.WriteLine(answer6);");
            Console.WriteLine(" Console.WriteLine(answer7);");
            Console.WriteLine(" Console.WriteLine(answer8);");
            Console.WriteLine("input = Console.ReadLine();");

            Console.WriteLine(" if (input == \"B\" || input == \"b\" || input == \"C\" || input == \"c\" || input == \"D\" || input == \"d\")");
            Console.WriteLine("{");
            Console.WriteLine("Console.WriteLine(\"Incorrect!Please Try Again\");");

            while (correct2 == false)
            {
                Console.WriteLine("Please enter the \"if\" statement for when the user enters the correct answer");
                input2 = Console.ReadLine();

                if (input2 == "if (input == \"A\" || input == \"a\")")
                {
                    Console.WriteLine(" ");
                    correct2 = true;
                }
                else{
                    Console.WriteLine(" ");
                    Console.WriteLine("You've made a mistake on this line");
                }
            }

            Console.WriteLine("{");
            Console.WriteLine(" Console.WriteLine(\"Correct!\");");

            while (correct3 == false) {
                Console.WriteLine("Please set the boolean value for correct to true");
                input3 = Console.ReadLine();

                if (input3 == "correct = true") {
                    Console.WriteLine(" ");
                    correct3 = true;
                }
                else {
                    Console.WriteLine(" ");
                    Console.WriteLine("You've made a mistake on this line");
                }
            }

            Console.WriteLine("}");
            Console.WriteLine("Please type the name of the operation that should follow the \"if\" statement");
            Console.WriteLine("This will catch any incorrect answers");

            while (correct4 == false)
            {
                input4 = Console.ReadLine();

                if (input4 == "else")
                {
                    Console.WriteLine(" ");
                    correct4 = true;
                }
                else {
                    Console.WriteLine(" ");
                    Console.WriteLine("You've made a mistake on this line");
                }
            }

            Console.WriteLine("{");
            Console.WriteLine("Console.WriteLine(\"Please Select a Vailid answer(A, B, C or D)....\"); ");
            Console.WriteLine("}");
            Console.WriteLine("}");
            Console.WriteLine("Console.ReadLine();");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("You've completed the last task!");
            Console.WriteLine("Proceed to the final level....");
            Console.ReadLine();


            /*
            while (correct == false)
            {
                Console.WriteLine("When creating variable to hold a word, what must you precede the name of the variable with?");
                Console.WriteLine(answer5);
                Console.WriteLine(answer6);
                Console.WriteLine(answer7);
                Console.WriteLine(answer8);
                input = Console.ReadLine();

                

                if (input == "B" || input == "b" || input == "C" || input == "c" || input == "D" || input == "d")
                {
                    Console.WriteLine("Incorrect!    Please Try Again");
                }
                if (input == "A" || input == "a")
                {
                    Console.WriteLine("Correct!");
                    correct = true;
                }
                else
                {
                    Console.WriteLine("Please Select a Vailid answer (A, B, C or D)....");
                }               
             }*/
            Console.ReadLine();
        }
    }
}
