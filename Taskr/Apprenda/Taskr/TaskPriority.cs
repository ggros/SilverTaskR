namespace Apprenda.Taskr
{
    using System.Runtime.Serialization;

    public enum TaskPriority
    {
        [EnumMember]
        Unspecified = 0,
        [EnumMember]
        Low = 1,
        [EnumMember]
        Medium = 2,
        [EnumMember]
        High = 3
    }
}
