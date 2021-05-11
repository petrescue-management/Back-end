using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ConstantHelper
{
    public class FinderFormStatusConst
    {
        public const int PROCESSING = 1;
        public const int RESCUING = 2;
        public const int ARRIVED = 3;
        public const int DONE = 4;
        public const int DROPPED = 5;

    }

    public class PetAttributeConst
    {
        public const int LOST = 1;
        public const int ABANDONED = 2;
        public const int INJURED = 3;
        public const int GIVEAWAY = 4;
        public const int RETURNED = 5;
    }
}
