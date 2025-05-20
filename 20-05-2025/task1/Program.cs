public class Posts
{
    public class Post
    {
        public string Caption {get;set;}
        public int Likes {get;set;}

        public Post(string caption, int likes)
        {
            Caption = caption;
            Likes = likes;
        }
    }

    public static int GetInputNumber(string promt)
    {
        int num;
        Console.Write(promt);
        while (!int.TryParse(Console.ReadLine(), out num))
            Console.WriteLine("Please enter a valid number.");

        return num;
    }

    public static string GetInputString(string promt)
    {
        string UserInput;
        while (true)
        {
            Console.Write(promt);
            UserInput = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrEmpty(UserInput))
            {
                Console.WriteLine("Please Enter a valid input.");
            }
            else
            {
                return UserInput;
            }
        }
    }

    public static Post[] GetPostDetails(string user)
    {
        Console.WriteLine($"Enter post details for ${user} : ");
        int Posts = GetInputNumber("Enter no. of posts : ");
        Post[] PostDetails = new Post[Posts];
        for (int i = 0; i < Posts; i++)
        {
            string Caption = GetInputString($"Enter caption for post {i + 1} : ");
            int Likes = GetInputNumber($"Enter likes for post {i + 1} : ");
            PostDetails[i] = new Post(Caption, Likes);
        }
        return PostDetails;
    }


    public static void Main(string[] args)
    {
        int Users = GetInputNumber("Please enter no. of users : ");
        Post[][] InstaPosts = new Post[Users][];
        for (int i = 0; i < Users; i++)
        {
            InstaPosts[i] = GetPostDetails($"User {i + 1}");
        }

        for (int i = 0; i < InstaPosts.Length; i++)
        {
            Post[] posts = InstaPosts[i];
            Console.WriteLine($"Post Details of user {i+1} : ");
            for (int j = 0; j < posts.Length; j++)
            {
                Console.WriteLine($"    Post {j + 1} : ");
                Console.WriteLine($"        Caption : {posts[j].Caption}");
                Console.WriteLine($"        Likes : {posts[j].Likes}");
            }

        }
    }
}