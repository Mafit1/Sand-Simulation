public class SandGrid
{
    public byte[,] Grid { get; private set; }
    public int Width { get; }
    public int Height { get; }

    public SandGrid(int width, int height)
    {
        Width = width;
        Height = height;
        Grid = new byte[width, height];
    }


    public void AddSand(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return;

        Grid[x, y] = 1;
    }


    public void Update()
    {
        for (int y = Height - 2; y >= 0; y--)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Grid[x, y] == 1)
                {
                    // Если под пикселем есть пустое место
                    if (Grid[x, y + 1] == 0)
                    {
                        Grid[x, y] = 0;
                        Grid[x, y + 1] = 1;
                    } // Если места нет, падает налево/направо
                    else
                    {
                        int dir = (new Random().Next(0, 2) == 0) ? -1 : 1;
                        int newX = x + dir;
                        if (newX >= 0 && newX < Width && Grid[newX, y + 1] == 0)
                        {
                            Grid[x, y] = 0;
                            Grid[newX, y + 1] = 1;
                        }
                    }
                }
            }
        }
    }
}
