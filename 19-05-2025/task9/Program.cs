/*
9) Write a program that:

Has a predefined secret word (e.g., "GAME").

Accepts user input as a 4-letter word guess.

Compares the guess to the secret word and outputs:

X Bulls: number of letters in the correct position.

Y Cows: number of correct letters in the wrong position.

Continues until the user gets 4 Bulls (i.e., correct guess).

Displays the number of attempts.

Bull = Correct letter in correct position.

Cow = Correct letter in wrong position.

Secret Word	User Guess	Output	Explanation
GAME	GAME	4 Bulls, 0 Cows	Exact match
GAME	MAGE	1 Bull, 3 Cows	A in correct position, MGE misplaced
GAME	GUYS	1 Bull, 0 Cows	G in correct place, rest wrong
GAME	AMGE	2 Bulls, 2 Cows	A, E right; M, G misplaced
NOTE	TONE	2 Bulls, 2 Cows	O, E right; T, N misplaced

*/
public class Game
{
    public class GuessGame
    {
        private readonly string Secret;
        private string UserInput = "";
        public int Bulls
        {get;set;} = 0 ;
        public int Cows
        {get;set;}  = 0;
        public int Attempts = 0;

        public GuessGame(string secret)
        {
            Secret = secret;
        }

        public void GetUserInput(string promt)
        {
            while(true)
            {
                Console.Write(promt);
                UserInput = (Console.ReadLine() ?? "").Trim().ToUpper();
                if(string.IsNullOrEmpty(UserInput) || UserInput.Length != 4)
                {
                    Console.WriteLine("Please Enter a valid input.");
                }else{
                    break;
                }
            }
        }

        public void CountBullsAndCows()
        {
            List<int> UsedIndex = new List<int>();
            List<char> NonMatched = new List<char>();

            for(int i=0;i<4;i++)
            {
                if(Secret[i]==UserInput[i])
                {
                    Bulls++;
                    UsedIndex.Add(i);
                }
                else
                    NonMatched.Add(Secret[i]);
            }

            for(int i=0;i<4;i++)
            {
               if(!UsedIndex.Contains(i) && NonMatched.Contains(UserInput[i]))
               {
                    Cows++;
                    NonMatched.Remove(UserInput[i]);
               } 
            }

            Console.WriteLine($"{Bulls} Bulls, {Cows} Cows");
        }

        public void StartGame()
        {
            while(true)
            {
                GetUserInput("Please Enter the Guess Word : ");
                CountBullsAndCows();
                Attempts++;
                if(Bulls == 4)
                {
                    Console.WriteLine($"Voilaa !! You guessed the correct word in {Attempts} attemps.");
                    break;
                }
                Bulls = 0;
                Cows = 0;
            }
        }

    }
    public static void Main(string[] args)
    {
        GuessGame game = new GuessGame("WORD");
        game.StartGame();
    }
}