using System;

namespace asciiLasers {
    internal static class Program {
        private static void Main() {
            Board board = BoardFactory.CreateBoardFromFile("examples/times2.al");
            
            Console.WriteLine(board);
            
            board.Init();
            int tick = 0;
            Console.WriteLine();
            while (!board.ShouldStop) {
                board.Eval();
                tick++;
            }

            Console.WriteLine($"Exit code: {board.ExitCode}\nTicks: {tick}");
        }
    }
}
