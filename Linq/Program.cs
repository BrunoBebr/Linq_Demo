
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
            City = m[8],
            State = m[9].Trim(new char[] { ' ' })
        }).ToList();

      
        var y = states.Skip(1).Select(d => d.Split(",")).Select(d => new statesModel()
        {
            State = d[0],
            Abbreviation = d[1].Trim('"')
        }).ToList();

        //CitiesTexas(x);

        //CitiesByState(x);

        //DistinctStates(x, y);

        //CombinedName(x, y);

        Console.ReadLine();
    }

   public static void CitiesTexas(List<citiesModel> cities)
    {
        cities.Where(n => n.State.Equals(" TX"))
                            .OrderByDescending(x => x.LatD)
                            .ToList().ForEach(item =>
                            {
                                Console.WriteLine(item.City.Trim(new char[] { ' ', '"' }));
                            });
    }

    public static void CitiesByState(List<citiesModel> cities)
    {
        cities.OrderBy(n => n.State).ToList().ForEach(item =>
        {
            Console.WriteLine(item.City.Trim(new char[] { ' ', '"' }) + "-" + item.State.Trim(new char[] {' '}));
        });

        Console.WriteLine();

        cities.OrderBy(n => n.City)
              .ToList()
              .ForEach(item =>
                {
                    Console.WriteLine(item.City.Trim(new char[] { ' ', '"' }) + "-" + item.State.Trim(new char[] { ' ' }));
        });
    }

    public static void DistinctStates(List<citiesModel> cities, List<statesModel> states)
    {
        cities.Join(
            states,
            cities => cities.State,
            states => states.Abbreviation,
            (cities, states) => new { states.State })
            .GroupBy(n => n.State)
            .Distinct()
            .OrderByDescending(
                n => n.Count())
             .ToList().ForEach(item =>
                {
                   Console.WriteLine(item.Key.Trim('"') + " contains " + item.Count() + " cities");
             });
    }

    public static void CombinedName(List<citiesModel> cities, List<statesModel> states)
    {
        cities.Join(
            states,
            cities => cities.State,
            states => states.Abbreviation,
            (cities, states) => new { cities.City, cities.LatD, cities.LonD, states.State })
            .ToList()
            .ForEach(item =>
            {
                Console.WriteLine("City: " + item.City.Trim('"', ' ') + "\n -> Position\n  LatD: " + item.LatD + "\n  LonD: " + item.LonD + "\n -> State: " + item.State.Trim('"') + "\n");
            });
    }
}