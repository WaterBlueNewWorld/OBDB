using System;
using System.Diagnostics;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;

//TODO: Remove case sensitive queries
namespace objectDatabase
{
    class Games
    {
        static string _file = "store.db4o";
        static IObjectContainer db = Db4oFactory.OpenFile(_file);
        public string _gameName;
        public int _gameYear;
        public string _gameRating;

        // public Games() { }

        public Games(string gameName, int gameYear, string gameRating)
        {
            _gameName = gameName;
            _gameYear = gameYear;
            _gameRating = gameRating;
        }

        public void ChangeGames(String newName, int newYear, String newRating)
        {
            _gameName = newName;
            _gameYear = newYear;
            _gameRating = newRating;
        }
        
        public override string ToString()
        {
            return _gameName + " / " + _gameRating + " / " + _gameYear;
        }

        private static void ListData(IObjectSet result)
        {
            //Console.WriteLine(result.Count);
            if (result.Count == 0)
            {
                Console.WriteLine("There is no data in the database");
            }
            else{
                foreach (object it in result)
                {
                    Console.WriteLine(it);
                }
                
            }
        }

        private static void GatherGames(IObjectContainer db)
        {
            //Games gam = new Games(null, 0,null);
            //IObjectSet result = db.QueryByExample(gam);
            IQuery query = db.Query();
            query.Constrain(typeof(Games));
            query.Descend("_gameName").OrderAscending();
            IObjectSet result = query.Execute();
            
            ListData(result);
        }

        private static void SortByYear(IObjectContainer db)
        {
            IQuery query = db.Query();
            query.Constrain(typeof(Games));
            query.Descend("_gameYear").OrderAscending();
            IObjectSet result = query.Execute();
            
            ListData(result);
        }
        
        private static void SortByRating(IObjectContainer db)
        {
            IQuery query = db.Query();
            query.Constrain(typeof(Games));
            query.Descend("_gameRating").OrderAscending();
            IObjectSet result = query.Execute();
            
            ListData(result);
        }

        private static void DeleteAll(IObjectContainer db)
        {
            Games gam = new Games(null, 0, null);
            IObjectSet result = db.QueryByExample(typeof(Games));
            while (result.HasNext()) {
                db.Delete(result.Next());
            }
        }

        private static void UpdateByName(IObjectContainer db, String data1)
        {
            IObjectSet result = db.QueryByExample(new Games(data1, 0, null));
            Games found = (Games) result.Next();
            Console.WriteLine("Which is going to be the new data?");
            String newName = Console.ReadLine();
            found.ChangeGames(newName,found._gameYear,found._gameRating);
            db.Store(found);
            ListData(result);
        }

        private static void UpdateByYear(IObjectContainer db, String data)
        {
           IObjectSet result = db.QueryByExample(new Games(data, 0, null));
           Games found = (Games) result.Next();
           Console.WriteLine("Which is going to be the new Year?");
           int newYear = Int32.Parse(Console.ReadLine());
           found.ChangeGames(found._gameName, newYear, found._gameRating);
           db.Store(found);
           ListData(result);
        }

        private static void UpdateByRating(IObjectContainer db, String data)
        {
            IObjectSet result = db.QueryByExample(new Games(data, 0, null));
            Games found = (Games) result.Next();
            Console.WriteLine("Which is going to be the new Rating?");
            String newRate = Console.ReadLine();
            found.ChangeGames(found._gameName, found._gameYear, newRate);
            db.Store(found);
            ListData(result);
        }

        private static void DeleteOne( IObjectContainer db, String data)
        {
            IObjectSet result = db.QueryByExample(new Games(data, 0, null));
            Games gamDel = (Games) result.Next();
            db.Delete(gamDel);
            Console.WriteLine("Deleted");
        }

       /* private static object QueryByName(IObjectContainer db, String name)
        {
            //char[] arr = name.ToCharArray();
            
            IQuery query = db.Query();
            query.Constrain(typeof(Games));
            query.Descend("_gameName").Constrain(name);
            IObjectSet result = query.Execute();
            ListData(result);
            return result;
        }*/

        public static void Main(string[] args)
        {
            Console.WriteLine("Select the action you want to perform \n" + 
                              "1) Sort by name \n" +
                              "2) Sort by year \n" +
                              "3) Sort by rating \n" +
                              "4) Store a game \n" +
                              "5) Update game \n" +
                              "6) Delete one game \n" +
                              "9) Delete all \n" +
                              "0) Exit the program");
            int cont = Int32.Parse(Console.ReadLine());
            try
            {
                switch (cont)
                {
                    case 1:
                        GatherGames(db);
                        Main(null);
                        break;
                    case 2:
                        SortByYear(db);
                        Main(null);
                       break;
                    case 3:
                        SortByRating(db);
                        Main(null);
                        break;
                    case 4:
                        String[] data = new string[2];
                        int dataYear = 0;
                        Console.WriteLine("Please write the name of the game");
                        data[0] = Console.ReadLine();
                        Console.WriteLine("Please write the year of the game");
                        dataYear = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Please write the ESRB rating of the game");
                        data[1] = Console.ReadLine();
                        Games newGame = new Games(data[0], dataYear, data[1]);
                        db.Store(newGame);
                        Main(null);
                        break;
                    case 5:
                        int dummyData = 0;
                        Console.WriteLine("Please specify the criteria to modify an entry \n" +
                                          "1) Change the name of a game \n" +
                                          "2) Change the year of a game \n" +
                                          "3) Change the rating of a game \n");
                        dummyData = Int32.Parse(Console.ReadLine());
                        switch (dummyData)
                        {
                            case 1:
                                Console.WriteLine("Specify the game to change the name");
                                String dummyName = Console.ReadLine();
                                UpdateByName(db, dummyName);
                                Main(null);
                                break;
                            case 2:
                                Console.WriteLine("Specify the game to change the year");
                                String dummyYear = Console.ReadLine();
                                UpdateByYear(db, dummyYear);
                                Main(null);
                                break;
                            case 3:
                                Console.WriteLine("Specify the game to change the rating");
                                String dummyRate = Console.ReadLine();
                                UpdateByRating(db, dummyRate);
                                Main(null);
                                break;
                            default:
                                Main(null);
                                break;
                        }
                        break;
                    case 6:
                        Console.WriteLine("Specify the game to be deleted");
                        String nameToDelete = Console.ReadLine();
                        DeleteOne(db, nameToDelete);
                        Main(null);
                        break;
                    case 9:
                        DeleteAll(db);
                        Main(null);
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please select a valid option");
                        Main(null);
                        break;
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "Unexpected error");
                throw;
            }
        }
    }
    
}