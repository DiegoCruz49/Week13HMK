using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week13
{
    public class User
    {
        public static string Ask()
        {
            Console.Clear();

            //var user = LIBRARY.Store.Get("USERNAME");
            var user = Environment.GetEnvironmentVariable("USERNAME", EnvironmentVariableTarget.User);


            if (!string.IsNullOrEmpty(user))
            {
                Console.WriteLine($"Are you {user}?");
                Console.WriteLine("[y] That's me!");
                Console.WriteLine("[n] That's not me!");
                Screen.WaitFor(new[] { ConsoleKey.Y, ConsoleKey.N }, out var answer);

                if(answer == ConsoleKey.Y) 
                {
                    return user;
                }
            }

            Console.WriteLine("What is your name?");
            user = Console.ReadLine();

            Console.WriteLine("Hang on...");

            Environment.SetEnvironmentVariable("USERNAME", user, EnvironmentVariableTarget.User);

            return user ?? "Anonymous";
        }
    }
}
