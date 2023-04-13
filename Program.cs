using LIBRARY;

using System.Drawing;
using System.Runtime.InteropServices;
using Week13;

var directions = new[]
{
    ConsoleKey.RightArrow, ConsoleKey.LeftArrow,
    ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Escape
};

var user = User.Ask();
Screen.Setup($"{user}");
var service = new LIBRARY.HighScoreService();
var best = service.GetHighScoreByName(user);
var scores = service.GetHighScores();

Console.WriteLine($"Hello, {user}, would you like to play again? Your current high score is: {best!.Score}");
var box = Box.Draw(1, 1, 20, 40);
var currentDirection = ConsoleKey.Escape;
var headBackground = ConsoleColor.White;

var headLocation = HeadStartingPoisition();
var previousHeadLocation = headLocation;

var score = 0;
var gameOver = false;

var fruitLocation = headLocation;
var fruitValue = 0;

playGame();

void playGame()
{
    SpawnFruit();
    while (!gameOver)
    {
        HandleScoring();

        RedrawTheHead();

        // check to see if the user is pressing a key
        directions.CheckFor(out currentDirection, otherwise: currentDirection);

        MoveTheHead();

        didYouHitTheWall();

        ChangeHeadBackground();

        Thread.Sleep(100);
    }


    if (best is null || best.Score < score)
    {
        service.UpdateHighScore(user, score);
    }

    //var scores = service.GetHighScores();
    Console.Clear();
    foreach (var item in scores.OrderByDescending(s => s.Score))
    {
        Console.WriteLine($"{item.Name.PadRight(20)} {item.Score:N0}");
    }
    playAgain();
}




void SpawnFruit()
{
    while (fruitLocation == headLocation)
    {
        var x = Random.Shared.Next(box.Left + 1, box.Right + 1);
        var y = Random.Shared.Next(box.Top + 1, box.Bottom + 1);
        fruitLocation = new Point(x, y);
    }

    fruitValue = Random.Shared.Next(1, 10);

    string scoreString = "Score:";
    string highScore = "Highscore:";
    string bestScore = $"{best!.Score}";
    scoreString.WriteAt(box.Right + 5, 4);
    score.ToString("N0").WriteAt(box.Right + 5, 5);
    highScore.WriteAt(box.Right + 5, 7);
    bestScore.WriteAt(box.Right + 5, 8);

    fruitValue.ToString().WriteAt(fruitLocation);
}

Point HeadStartingPoisition()
{
    var x = Random.Shared.Next(box.Left + 1, box.Right + 1);
    var y = Random.Shared.Next(box.Top + 1, box.Bottom + 1);
    return new Point(x, y);
}

void HandleScoring()
{
    if (headLocation == fruitLocation)
    {
        score = score + fruitValue;
        SpawnFruit();

    }
}

void RedrawTheHead()
{
    " ".WriteAt(previousHeadLocation);
    var head = currentDirection.ToHead();
    head.WriteAt(headLocation, background: headBackground);
}

void MoveTheHead()
{
    previousHeadLocation = headLocation;

    headLocation = currentDirection switch
    {
        ConsoleKey.UpArrow => new Point(headLocation.X, headLocation.Y - 1),
        ConsoleKey.DownArrow => new Point(headLocation.X, headLocation.Y + 1),
        ConsoleKey.LeftArrow => new Point(headLocation.X - 1, headLocation.Y),
        ConsoleKey.RightArrow => new Point(headLocation.X + 1, headLocation.Y),
        _ => headLocation,
    };
}

void didYouHitTheWall()
{
    gameOver = currentDirection switch
    {
        ConsoleKey.UpArrow when headLocation.Y == box.Top => true,
        ConsoleKey.DownArrow when headLocation.Y == box.Bottom + 1 => true,
        ConsoleKey.LeftArrow when headLocation.X == box.Left => true,
        ConsoleKey.RightArrow when headLocation.X == box.Right + 1 => true,
        _ => false,
    };
}

void ChangeHeadBackground()
{
    headBackground = currentDirection switch
    {
        ConsoleKey.UpArrow => ConsoleColor.Magenta,
        ConsoleKey.DownArrow => ConsoleColor.Blue,
        ConsoleKey.LeftArrow => ConsoleColor.Green,
        ConsoleKey.RightArrow => ConsoleColor.Yellow,
        _ => headBackground,
    };
}

void playAgain()
{
    Console.WriteLine("");
    Console.WriteLine($"{user.PadRight(20)} {score}");
    Console.WriteLine($"{user}, would you like to play again? Your current high score is: {best!.Score}");
    Console.WriteLine("[y] Absolutely!");
    Console.WriteLine("[n] Maybe Later!");

    Screen.WaitFor(new[] { ConsoleKey.Y, ConsoleKey.N }, out var answer);
    if (answer == ConsoleKey.Y)
    {
        Console.Clear();
        service = new LIBRARY.HighScoreService();
        best = service.GetHighScoreByName(user);
        scores = service.GetHighScores();
        gameOver = false;
        score = 0;
        headLocation = HeadStartingPoisition();
        Console.WriteLine($"Hello, {user}, would you like to play again? Your current high score is: {best!.Score}");
        box = Box.Draw(1, 1, 20, 40);
        currentDirection = ConsoleKey.Escape;
        headBackground = ConsoleColor.White;
        playGame();
    }
    else
    {
        return;
    }
    return;
}

