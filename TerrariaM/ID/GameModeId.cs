namespace TerrariaM.ID
{
    //Terraria.ID.GameModeID now become internal
    public static class GameModeId
    {
        public const short Normal = 0;
        public const short Expert = 1;
        public const short Master = 2;
        public const short Creative = 3;
        public const short Count = 4;

        public static bool IsValid(int gameMode) => gameMode >= 0 && gameMode < 4;
    }
}