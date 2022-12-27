//поле - двумерный массив размера N на M
//портал - объект, пренадлежит полю
//стены - объекты, прендалежат полю, по краям поля сделать стены
//герой - объект, НЕ пренадлежит полю, рисуется поверх полю

//1) генерация поля и стен на нём
//2) генерация портала в поле (перандомировать поле если мы оказались заперты)
//3) передвижение героя по полю
//4) вход героя в портал и переход к шагу 1

using ConsoleAppHeroAdventure;

Random random = new Random();


// присваиваем лвл + строки и столбцы, переменные координат героя, собаки
int currentLevel = 1;
int Deaths = 0;
int rows, cols;
Cell[,] field;

int iHero, jHero;
int iDogs, jDogs;  // координаты собаки NEW

int[] StepDogs = new int { 0, 1, 2, 3 };  // массив шагов собаки, после присвоем каждому индеку своё действие NEW
Random MoveDogs = new StepDogs[]; // рандом шагов собаки = её движение после хода игрока NEW

bool heroInAdventure;

int currentWallPercent = (int)Constants.WallPercent;

// int[,] dogs = new int[,]
// {
//     { 0, 0 },
//     { 0, 0 }
// };
//
// int iDog, jDog;


// задаём цикл, генерируем мин и макс количество строк и столбцов
while (true)
{
    rows = random.Next((int)Constants.MinRows, (int)Constants.MaxRows + 1);
    cols = random.Next((int)Constants.MinCols, (int)Constants.MaxCols + 1);
    // задаём поле в виде массива  
    field = new Cell[rows, cols];
    // через цикл задаём пустые поля *
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            field[i, j] = Cell.Empty;
        }
    }
    // через цикл задаём границы X по строке
    for (int i = 0; i < rows; i++)
    {
        field[i, 0] = Cell.Bound;
        field[i, cols - 1] = Cell.Bound;
    }
    // через цикл задаём гранцы Х по столбцу
    for (int j = 0; j < cols; j++)
    {
        field[0, j] = Cell.Bound;
        field[rows - 1, j] = Cell.Bound;

        // задаём координаты старта нашего героя 
        iHero = (int)Constants.StartIHero;
        jHero = (int)Constants.StartJHero;

        //задаём начальные координаты @ рандомом, c проверкой на пересечения героя, портала. NEW

        do
        {
            iDogs = random.Next(2, rows - 1);
            jDogs = random.Next(2, cols - 1);
        } while (iDogs == iHero && jDogs == jHero
            && field[iDogs, jDogs] = Cell.Wall
            && field[iDogs, jDogs] = Cell.Portal);

        field[iDogs, jDogs] = Cell.Dogs;

        // задаем рандомную точку спавна портала, с учётом чтобы координаты героя и портала не пересекались, иначе перерандом
        int iPortal, jPortal;
        do
        {
            iPortal = random.Next(1, rows - 1);
            jPortal = random.Next(1, cols - 1);
        } while (iPortal == iHero && jPortal == jHero);

        field[iPortal, jPortal] = Cell.Portal;

        // задаем процент спавна стен в рандомном положении с проверкой чтобы стена не спанилась в герое/портале/стене иначе перерандом
        int countWalls = (int)((rows - 2) * (cols - 2) * currentWallPercent / 100.0);
        for (int i = 0; i < countWalls; i++)
        {
            int iWall, jWall;
            do
            {
                iWall = random.Next(1, rows - 1);
                jWall = random.Next(1, cols - 1);
            } while (iWall == iHero && jWall == jHero
                     || field[iWall, jWall] == Cell.Portal
                     || field[iWall, jWall] == Cell.Wall);

            field[iWall, jWall] = Cell.Wall;
        }
        // запускаем героя в игру и разукрашиваем наши переменные
        heroInAdventure = true;
        while (heroInAdventure)
        {
            Console.Clear();

            Console.ResetColor();

            Console.WriteLine($"Current Level = {currentLevel}");
            Console.WriteLine($"Deaths = {Deaths}"); // Задали счётчик смертей на консоль NEW
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == iHero && j == jHero)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write((char)Constants.HeroSkin);
                    }
                    else
                    {
                        switch (field[i, j])
                        {
                            case Cell.Empty:
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case Cell.Wall:
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case Cell.Portal:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case Cell.Bound:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                        }

                        Console.Write((char)field[i, j]);
                    }
                }

                Console.WriteLine();
            }
            // задаем ввод с клавы по направлениям с условиями стен/порталов
            ConsoleKey key = Console.ReadKey(false).Key;
            switch (key)
            
          {  case ConsoleKey.A:
                if (field[iHero, jHero - 1] == Cell.Empty || field[iHero, jHero - 1] == Cell.Portal)
                {
                    jHero--;
                }

                break;

            case ConsoleKey.W:
                if (field[iHero - 1, jHero] == Cell.Empty || field[iHero - 1, jHero] == Cell.Portal)
                {
                    iHero--;
                }

                break;

            case ConsoleKey.D:
                if (field[iHero, jHero + 1] == Cell.Empty || field[iHero, jHero + 1] == Cell.Portal)
                {
                    jHero++;
                }

                break;

            case ConsoleKey.S:
                if (field[iHero + 1, jHero] == Cell.Empty || field[iHero + 1, jHero] == Cell.Portal)
                {
                    iHero++;
                }

                break;

            case ConsoleKey.R:
                heroInAdventure = false;
                break;
            }

            // задаём рандом хода собаки после нажатия клавиши игроком, через массив, рандомом с условиями  NEW

            while (ConsoleKey)
            {
                Console.Write(MoveDogs[new StepDogs().Next(0, MoveDogs.Length)]);
                {
                    if Console.Write ([0])
                    {
                        field[iDogs, jDogs - 1] == Cell.Empty || field[iDogs, jDogs - 1] == Constants.HeroSkin

                            {
                            jDogs--
                            }
                    }
                    break;
                    if Console.Write([1])
                    {
                        field[iDogs - 1, jDogs] == Cell.Empty || field[iDogs - 1, jDogs] == Constants.HeroSkin
                            {
                            jDogs--
                            }
                    }
                    break;
                    if Console.Write([2])
                    {
                        field[iDogs, jDogs + 1] == Cell.Empty || field[iDogs, jDogs + 1] == Constants.HeroSkin
                           {
                            jDogs++
                           }
                    }
                    break;
                    if Console.Write([3])
                    {
                        field[iDogs + 1, jDogs] == Cell.Empty || field[iDogs + 1, jDogs] == Constants.HeroSkin
                           {
                            jDogs++
                           }
                    }
                    break;
                }
            }

        }


        // делаем условие если герой на портале тогда переходим на сл уровень, и +1 к уровню, +5 процентов к стенам, Герой не в игре
        if (field[iHero, jHero] == Cell.Portal)
        {
            currentLevel++;
            currentWallPercent += 5;
            heroInAdventure = false;
        }

        // Условие если @ будет на Герое  NEW
        if (field[iDogs, jDogs] == Constants.HeroSkin)
        {
            Deaths++;
            heroInAdventure = false;
        }
    }
}