using RankingLibrary.CalculationEngine;
using RankingLibrary.Properties;
using RankingLibrary.Security;
using RankingLibrary.SupportObjects.PlayerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.SupportObjects.RatingObjects
{
    public class Rating : Player
    {
        private TextValidator textValidator = new TextValidator();
        private readonly RatingCalculator ratingCalculator;

        public double RatingValue { get; private set; }
        public double RatingDeviation { get; private set; }
        public double Volatility { get; private set; }

        public int NumberOfResults { get; private set; }

        internal double WorkingRating { get;  set; }
        internal double WorkingRatingDeviation { get;  set; }
        internal double WorkingVolatility { get;  set; }


        public Rating(int id, string usName, RatingCalculator ratingCalculator) : base(id, usName)
        {
            

            this.ratingCalculator = ratingCalculator;
            this.RatingValue = ratingCalculator.GetDefaultRating();
            this.RatingDeviation = ratingCalculator.GetDefaultRatingDeviation();
            this.Volatility = ratingCalculator.GetDefaultVolatility();

        }

        public Rating(int id, string usName, RatingCalculator ratingCalculator, double initRating, double initDeviation, double initVolatility) : base(id, usName)
        {
            this.ratingCalculator = ratingCalculator;

            if (initRating <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initRating));
            }
            if (initDeviation <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initDeviation));
            }
            if (initVolatility <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initVolatility));
            }
            this.RatingValue = initRating;
            this.RatingDeviation = initDeviation;
            this.Volatility = initVolatility;
           
        }

        /// <summary>
        /// Return the average skill value of the player scaled down
	    /// to the scale used by the algorithm's internal workings.
        /// </summary>
        /// <returns></returns>
        public double GetGlicko2Rating()
        {
            return ratingCalculator.ConvertRatingToGlicko2Scale(RatingValue);
        }

        /// <summary>
        /// Set the average skill value, taking in a value in Glicko2 scale.
        /// </summary>
        /// <param name="rating"></param>
        public void SetGlicko2Rating(double newRating)
        {
            RatingValue = ratingCalculator.ConvertRatingToOriginalGlickoScale(newRating);
        }

        /// <summary>
        /// Return the rating deviation of the player scaled down
	    /// to the scale used by the algorithm's internal workings.
        /// </summary>
        /// <returns></returns>
        public double GetGlicko2RatingDeviation()
        {
            return ratingCalculator.ConcertRatingDeviationToGlicko2Scale(RatingDeviation);
        }

        /// <summary>
        /// Set the rating deviation, taking in a value in Glicko2 scale.
        /// </summary>
        /// <param name="ratingDeviation"></param>
        public void SetGlicko2RatingDeviation(double newRatingDeviation)
        {
            RatingDeviation = ratingCalculator.ConvertRatingDeviationToOriginalGlickoScale(newRatingDeviation);
        }

        // <summary>
        /// Used by the calculation engine, to move interim calculations into their "proper" places.
        /// </summary>
        public void FinalizeRating()
        {
            SetGlicko2Rating(WorkingRating);
            SetGlicko2RatingDeviation(WorkingRatingDeviation);
            Volatility = WorkingVolatility;

            WorkingRatingDeviation = 0;
            WorkingRating = 0;
            WorkingVolatility = 0;

        }

        public void IncrementNumberOfResults(int increment)
        {
            NumberOfResults += increment;
        }
    }
}
