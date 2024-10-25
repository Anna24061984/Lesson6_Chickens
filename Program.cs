namespace Lesson6_Chickens
{
    internal class Program
    {
        enum Action { Feed, TakeEgg, DoNothing }
        enum ChickenState { Alive, Dead }
        const int chickenCount = 3;
        static int[] chickenEggs = new int[chickenCount];                       //массив наличия яиц у куриц
        static ChickenState[] chickenState = new ChickenState[chickenCount];    //массис состояний куриц
        static int[] chickenGrain = new int[chickenCount];                      //количество зерен у курицы 
        static int grainsForHappyChicken = 2;                                          //количество зерен для курицы чтобы наесться
        static int minGrainCount = 3;
        static int maxGrainCount = 5;

        static string ActionName(Action action)
        {
            switch (action)
            { 
                case Action.Feed: return "Покормить курицу";
                case Action.TakeEgg: return "Забрать яйцо";
                case Action.DoNothing: return "Ничего не делать";
                default: return "Неизвестное значение";
            }
        }

        static string ChickenStateName(ChickenState state)
        {
            switch (state)
            { 
                case ChickenState.Alive: return "Живая";
                case ChickenState.Dead: return "Мертвая";
                default: return "Неизвестное значение";
            }
        }
                
        static void Main(string[] args)
        {
            for (int i = 0; i < chickenCount; i++)
                chickenState[i] = ChickenState.Alive;                   //считаем, что все курицы изначально живы 
            bool isAnybobyAlive = true;
            bool isFinish = false;            
            while (isAnybobyAlive && !isFinish)                         //пока есть живые курицы и пользователю не надоело играть
            { 
                PrintChickenState();                                    //на каждом ходе выдаем информацию о текущем состоянии дел
                for (int i = 0; i < chickenCount; i++)
                {                       
                    string message = $"Выберите доступные действие для курицы номер {i + 1} (";
                    if (chickenState[i] is ChickenState.Alive)          //живую курицу можно покормить
                    {
                        message += $"F - {ActionName(Action.Feed)}, ";
                    }
                    if (chickenEggs[i] > 0)                             //если есть яйца, их можно забрать
                    {
                        message += $"T - {ActionName(Action.TakeEgg)}, ";
                    }
                    message += $"N - {ActionName(Action.DoNothing)}):";                //удобная заглушка
                    Console.Write(message);                    
                    switch (MyTryParse())
                    {
                        case 'F': FeedChicken(i);break;
                        case 'T': TakeEggFromChicken(i);break;
                        case 'N': DoNothingWithChicken(i); break;                        
                        default: break;
                    }                    
                    ChickenHasLunch(i);                                 //какое бы действие не выбрал пользователь, но курица должна покушать за ход
                }
                isAnybobyAlive = false;                                 //предположим, что все умерли
                for (int i = 0; i < chickenCount; i++)
                { 
                    if (chickenState[i] is ChickenState.Alive)          //если еще не всех заморили, продолжаем
                        isAnybobyAlive = true;
                }
                Console.Write("Если хотите закончить, введите 'Q', для продолжения - любую другую клавишу: ");                
                if (MyTryParse() == 'Q')
                {
                    isFinish = true;                    
                }
            }
            Console.WriteLine("Игра закончена!");
            PrintChickenState();
            Console.ReadLine();
        }

        /// <summary>
        /// Вывод информации о курицах
        /// </summary>
        static void PrintChickenState()
        {            
            for (int i = 0; i < chickenCount; i++)
            {
                Console.WriteLine($"Курица {i + 1}: состояние {ChickenStateName(chickenState[i])}, количество доступных яиц {chickenEggs[i]}, количество зерен {chickenGrain[i]}.");
            }
        }

        /// <summary>
        /// Когда лень обновить студию, можно самой написать парсер
        /// </summary>        
        static char MyTryParse()
        {
            try
            {
                char c = char.Parse(Console.ReadLine().ToUpper());
                return c;
            }
            catch 
            {
                return '\0';
            }            
        }

        /// <summary>
        /// Кормление курицы (за раз можно положить 3-5 зерен)
        /// </summary>        
        static void FeedChicken(int currentChicken)
        {            
            int grainCount = 0;
            while (grainCount < minGrainCount || grainCount > maxGrainCount)
            {
                try
                {
                    Console.Write($"Введите количество зерен от {minGrainCount} до {maxGrainCount}: ");
                    grainCount = int.Parse(Console.ReadLine());
                    if (grainCount >= minGrainCount && grainCount <= maxGrainCount)
                    {
                        break;
                    }
                }
                catch { }
            }
            chickenGrain[currentChicken] += grainCount;             //увеличили количество зерен в куриной кормушке            
        }

        /// <summary>
        /// Прием пищи курицей (за каждый ход она ест определенное количество зерен или умирает, если зерен недостаточно)
        /// </summary>        
        static void ChickenHasLunch(int currentChicken)
        {
            if (chickenGrain[currentChicken] < grainsForHappyChicken)                   //если вдруг оказалось, что зерен не хватает на покушать, то курочка уходит на радугу
            {
                chickenState[currentChicken] = ChickenState.Dead;
            }
            else
            {
                chickenGrain[currentChicken] -= grainsForHappyChicken;                  //курица поела
                chickenEggs[currentChicken]++;                                          //и снесла яйцо
            }
        }

        /// <summary>
        /// Забираем яйцо у курицы
        /// </summary>        
        static void TakeEggFromChicken(int currentChicken)
        {
            if (chickenEggs[currentChicken] > 0)
                chickenEggs[currentChicken]--;
        }

        /// <summary>
        /// Ничего не делаем
        /// </summary>        
        static void DoNothingWithChicken(int currentChicken)
        {
        }
    }
}
