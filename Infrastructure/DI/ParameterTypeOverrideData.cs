using System;

namespace Infrastructure.DI
{
    public class ParameterTypeOverrideData : ParameterOverrideData
    {
        public Type ParameterType { get; set; }
    }
}
