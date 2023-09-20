using System;
namespace Linq.Model
{
	public class citiesModel
	{
		public int LatD { get; set; } 

        public int LatM { get; set; }

        public int LatS { get; set; }

        public string NS { get; set; } = string.Empty;

        public int LonD { get; set; }

        public int LonM { get; set; }

        public int LonS { get; set; }

        public string EW { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;
    }
}

