using System;

namespace LoopingBee.Shared.LitJson
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonSkipAttribute : Attribute
    {
    }
}