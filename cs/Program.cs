using System;

namespace asciiLasers {
    internal static class Program {
        private static void Main(string[] args) {
            Board board = BoardFactory.CreateBoardFromFile("examples/plus1.al");
            
            Console.WriteLine(board);
            
            board.Init();
            int tick = 0;
            while (!board.ShouldStop) {
                board.Eval();
                tick++;
            }

            Console.WriteLine($"Exit code: {board.ExitCode}\nTicks: {tick}");
        }
    }
}
