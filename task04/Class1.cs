namespace task04;

public interface ISpaceship
{
    void MoveForward();      // Движение вперед
    void Rotate(int angle);  // Поворот на угол (градусы)
    void Fire();             // Выстрел ракетой
    int Speed { get; }       // Скорость корабля
    int FirePower { get; }   // Мощность выстрела
}

public class Cruiser : ISpaceship
{
    public int Speed { get; set; }
    public int FirePower { get; set; }
    public Cruiser()
    {
        Speed = 50;
        FirePower = 100;
    }
    public void MoveForward() => System.Console.WriteLine("Наш крейсер летит вперёд, Капитан!");
    public void Rotate(int angle) => System.Console.WriteLine($"Мы повернулись на угол в {angle} градусов, Капитан!");
    public void Fire() => System.Console.WriteLine($"Пау-пау! Мощно стреляем по врагам, нанеся им {FirePower} урона!");
}

public class Fighter : ISpaceship
{
    public int Speed { get; set; }
    public int FirePower { get; set; }
    public Fighter()
    {
        Speed = 100;
        FirePower = 50;
    }
    public void MoveForward() => System.Console.WriteLine("Наш истребитель мчит вперёд, Капитан!");
    public void Rotate(int angle) => System.Console.WriteLine($"Мы повернулись на угол в {angle} градусов, Капитан!");
    public void Fire()=>System.Console.WriteLine($"Пиу-пиу! Быстро стреляем по врагам, нанеся им {FirePower} урона!");
}
