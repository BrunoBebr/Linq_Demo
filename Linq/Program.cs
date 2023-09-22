
using Linq.Model;

internal class Program
{
    private static void Main(string[] args)
    {
        var cities = File.ReadAllLines("data/cities.csv");
        var states = File.ReadAllLines("data/states.csv");

        var x = cities.Skip(1).Select(m => m.Split(",")).Select(m => new citiesModel() {
            LatD = Int32.Parse(m[0]),
            LatM = Int32.Parse(m[1]),
            LatS = Int32.Parse(m[2]),
            NS = m[3],
            LonD = Int32.Parse(m[4]),
            LonM = Int32.Parse(m[5]),
            LonS = Int32.Parse(m[6]),
            EW = m[7],
            City = m[8].Trim(new char[] { ' ', '"' }),
            State = m[9].Trim(new char[] { ' ' })
        }).ToList();


        var y = states.Skip(1).Select(d => d.Split(",")).Select(d => new statesModel()
        {
            State = d[0].Trim('"'),
            Abbreviation = d[1].Trim('"')
        }).ToList();

        bool exitToken = false;
        int limit = 0;
        int start = 0;
        int pageNum = 1;



        Console.Write("Pocet zaznamu na strance –>");
        while (limit == 0)
        {
            try
            {
                limit = Int32.Parse(Console.ReadLine()!);
            }
            catch
            {
                Console.WriteLine("Toto neni cislo!");
            }
        }

        List<string> items = CitiesTexas(x);

        // List<string> items = CitiesByState(x);

        // List<string> items = DistinctStates(x, y);

        // List<string> items = CombinedName(x, y);

        int pages = items.Count() / limit;

        if (pages == 0)
        {
            pages = 1;
        }

        while (!exitToken)
        {
            start = (pageNum * limit) - limit;

            Console.Clear();

            Console.WriteLine("=========== Page no." + pageNum + " of " + pages + " ===========\n");

            items.Skip(start).Take(limit).ToList().ForEach(x =>
            {
                Console.WriteLine(x);
            });


            bool state = false;

            while (!state)
            {

                Console.WriteLine("\n========= [int] jump to page =========");
                var inp = Console.ReadLine();

                if(Int32.TryParse(inp, out int input))
                {
                    if (input > 0 && input <= pages)
                    {
                        pageNum = input;
                        state = true;
                    }
                    else
                    {
                        Console.WriteLine("Number is out of range!");
                    }
                }
                else
                {
                    Console.WriteLine("Input is not number!");
                }
            }
        }
    }

   public static List<string> CitiesTexas(List<citiesModel> cities)
    {
        List<string> items = new List<string> { };

        cities.Where(n => n.State.Equals("TX"))
                            .OrderByDescending(x => x.LatD)                    
                            .ToList().ForEach(item =>
                            {
                                items.Add(item.City);
                            });
        return items;
    }

    public static void CitiesByState(List<citiesModel> cities)
    {
        List<string> items = new List<string> { };

        cities.OrderBy(n => n.State).ToList().ForEach(item =>
        {
            items.Add(item.City + "-" + item.State);
        });

        Console.WriteLine();

        cities.OrderBy(n => n.City)
              .ToList()
              .ForEach(item =>
                {
                    Console.WriteLine(item.City + "-" + item.State);
        });
    }

    public static void DistinctStates(List<citiesModel> cities, List<statesModel> states)
    {
        List<string> items = new List<string> { };

        cities.Join(
            states,
            cities => cities.State,
            states => states.Abbreviation,
            (cities, states) => new { states.State})
            .GroupBy(n => n.State)
            .Select(group => new {group.Key, count = group.Count()})
            .OrderByDescending(
                n => n.count)
             .ToList().ForEach(item =>
                {
                   items.Add(item.Key + " contains " + item.count + " cities");
             });
    }

    public static void CombinedName(List<citiesModel> cities, List<statesModel> states)
    {
        List<string> items = new List<string> { };

        cities.Join(
            states,
            cities => cities.State,
            states => states.Abbreviation,
            (cities, states) => new { cities.City, cities.LatD, cities.LonD, states.State })
            .ToList()
            .ForEach(item =>
            {
                items.Add("City: " + item.City + "\n -> Position\n  LatD: " + item.LatD + "\n  LonD: " + item.LonD + "\n -> State: " + item.State + "\n");
            });
    }
}