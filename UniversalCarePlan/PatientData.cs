using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace UniversalCarePlan
{

    public partial class PatientData
    {
        public Patient Patient { get; set; }
        public Condition[] Condition { get; set; }
    }

    public partial class Condition
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public Treatment[] Treatment { get; set; }
    }

    public partial class Treatment
    {
        public string Drug { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
    }

    public partial class Patient
    {
        public long Age { get; set; }
        public string Sex { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
    }
}