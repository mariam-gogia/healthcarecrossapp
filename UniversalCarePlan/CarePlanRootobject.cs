using System;
using System.Collections.Generic;

namespace UniversalCarePlan
{
    //class for care plan objects
    public class CarePlanRootobject
    {
        public Patient Patient { get; set; }
        public IList<Result> results { get; set; }
    }
}
