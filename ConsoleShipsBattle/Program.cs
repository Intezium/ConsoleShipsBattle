using System;

namespace ConsoleShipsBattle
{
    interface IShip
    {
        bool IsAlive { get; }
        string Name { get; }

        void Attack(IShip enemy);
        void Defend(int damage);
    }

    class Corvette : IShip
    {
        string name;

        int health;

        int minDamage;
        int maxDamage;

        int critChange;
        int critMod;

        public bool IsAlive
        {
            get
            {
                return health > 0;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }

        public Corvette()
        {
            name = "Corvette";

            health = 175;

            minDamage = 10;
            maxDamage = 15;

            critChange = 25;
            critMod = 2;
        }
        public Corvette(string name, int health, int minDamage, int maxDamage, int critChange, int critMod)
        {
            this.name = name;

            this.health = health;

            this.minDamage = minDamage;
            this.maxDamage = maxDamage;

            this.critChange = critChange;
            this.critMod = critMod;
        }

        public void Attack(IShip enemy)
        {
            int damage = Rand.Next(minDamage, maxDamage);

            if (critChange >= Rand.Next(100))
                damage *= critMod;

            enemy.Defend(damage);

            Console.WriteLine("{0} нанес {1} урона {2}", Name, damage, enemy.Name);
        }
        public void Defend(int damage)
        {
            try
            {
                checked
                {
                    if (IsAlive)
                        health -= damage;
                }
            }
            catch { health = 0; }
        }
    }
    class Carrier : IShip
    {
        IShip[] fighters = new IShip[5]
        {
            new Fighter(),
            new Fighter(),
            new Fighter(),
            new Fighter(),
            new Fighter()
        };

        string name;

        int health;

        public bool IsAlive
        {
            get
            {
                return health > 0;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }

        public Carrier()
        {
            name = "Carrier";

            health = 100;
        }
        public Carrier(string name, int health)
        {
            this.name = name;

            this.health = health;
        }

        public void Attack(IShip enemy)
        {
            for (int i = 0; i < fighters.Length; i++)
                fighters[0].Attack(enemy);
        }
        public void Defend(int damage)
        {
            try
            {
                checked
                {
                    if (IsAlive && fighters.Length > 0)
                    {
                        switch (Rand.Next(1))
                        {
                            case 0: health -= damage; break;
                            case 1:
                                {
                                    int n = Rand.Next(fighters.Length);

                                    fighters[n].Defend(damage);

                                    if (!fighters[n].IsAlive)
                                        fighters = Array.FindAll(fighters, p => p != fighters[n]);
                                }
                                break;
                        }
                    }
                    else
                        health -= damage;
                }
            }
            catch { health = 0; }
        }

        class Fighter : IShip
        {
            string name;

            int health = 10;

            int minDamage = 3;
            int maxDamage = 7;

            int critChange = 25;
            int critMod = 2;

            public bool IsAlive
            {
                get
                {
                    return health > 0;
                }
            }
            public string Name
            {
                get
                {
                    return name;
                }
            }

            public Fighter()
            {
                name = "Fighter";
            }

            public void Attack(IShip enemy)
            {
                int damage = Rand.Next(minDamage, maxDamage);

                if (critChange >= Rand.Next(100))
                    damage *= critMod;

                enemy.Defend(damage);

                Console.WriteLine("{0} нанес {1} урона {2}", Name, damage, enemy.Name);
            }
            public void Defend(int damage)
            {
                try
                {
                    checked
                    {
                        if (IsAlive)
                            health -= damage;
                    }
                }
                catch { health = 0; }
            }
        }
    }

    static class Battle
    {
        public static void Begin(IShip ship1, IShip ship2)
        {
            while (ship1.IsAlive && ship2.IsAlive)
            {
                ship1.Attack(ship2);

                if (ship2.IsAlive)
                    ship2.Attack(ship1);
            }

            if (ship1.IsAlive)
                Console.WriteLine("\nПобедил {0}", ship1.Name);
            else
                Console.WriteLine("\nПобедил {0}", ship2.Name);
        }
    }

    static class Rand
    {
        static Random rand;

        static Rand()
        {
            rand = new Random();
        }

        public static int Next(int a)
        {
            return rand.Next(a);
        }
        public static int Next(int a, int b)
        {
            return rand.Next(a, b);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Battle.Begin(new Corvette(), new Carrier());

            Console.ReadKey();
        }
    }
}
