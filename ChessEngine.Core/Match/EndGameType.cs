namespace ChessEngine.Core.Match
{
    [Flags]
    public enum EndGameType : ushort
    {
        GameIsNotFinished = 0,

        WhiteIsCheckmated = 1,
        BlackIsCheckmated = 2,
        WhiteHasResigned = 4,
        BlackHasResigned = 8,
        WhiteHasFlagFall = 16,
        BlackHasFlagFall = 32,
        DrawOnStalemate = 64,
        DrawOnDeadPosition = 128,
        DrawOnAgreement = 256,
        DrawOnThreefoldRepetition = 512,
        DrawOnFivefoldRepetition = 1024,
        DrawOnFiftyMove = 2048,
        DrawOnSeventyFiveMove = 4096,

        WhiteWon = BlackIsCheckmated | BlackHasResigned | BlackHasFlagFall,
        BlackWon = WhiteIsCheckmated | WhiteHasResigned | WhiteHasFlagFall,
        Draw = DrawOnStalemate | DrawOnDeadPosition | DrawOnAgreement | DrawOnThreefoldRepetition | DrawOnFivefoldRepetition | DrawOnFiftyMove | DrawOnSeventyFiveMove,

        Checkmate = BlackIsCheckmated | WhiteIsCheckmated,
        Resign = BlackHasResigned | WhiteHasResigned,
        Flagfall = BlackHasFlagFall | WhiteHasFlagFall,

        GameIsFinished = WhiteWon | BlackWon | Draw
    }
}
