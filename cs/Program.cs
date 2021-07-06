using System;

namespace asciiLasers {
    internal static class Program {
        private static void Main() {
            Board board = BoardFactory.CreateBoardFromFile("examples/numbers.al");
            
            //Console.WriteLine(board);
            
            board.Init();
            int tick = 0;
            Console.WriteLine();
            while (!board.ShouldStop && (tick < 10000)) {
                board.Eval();
                tick++;
            }

            Console.WriteLine($"Exit code: {board.ExitCode}\nTicks: {tick}");
        }
    }
}
