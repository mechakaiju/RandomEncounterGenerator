using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon
{
    public class DirectionPicker
    {
        private readonly List<DirectionType> directionsPicked = new List<DirectionType>();
        private readonly DirectionType previousDirection;
        private readonly int changeDirectionModifier;

        public DirectionPicker(DirectionType previousDirection, int changeDirectionModifier)
        {
            this.previousDirection = previousDirection;
            this.changeDirectionModifier = changeDirectionModifier;
        }

        public bool HasNextDirection
        {
            get { return directionsPicked.Count < 4; }
        }

        public DirectionType GetNextDirection()
        {
            Random rnd = new Random();
            if (!HasNextDirection) throw new InvalidOperationException("No directions available");

            DirectionType directionPicked;

            do
            {
                directionPicked = MustChangeDirection ? (DirectionType)rnd.Next(3) : previousDirection;
            } while (directionsPicked.Contains(directionPicked));

            directionsPicked.Add(directionPicked);

            return directionPicked;
        }

        private bool MustChangeDirection
        {
            get
            {
                // changeDirectionModifier of 100 will always change direction
                // value of 0 will never change direction
                Random rnd = new Random();

                return changeDirectionModifier > rnd.Next(0, 99);
            }
        }
    }
}
