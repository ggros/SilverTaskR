namespace Apprenda.Taskr.Service
{
    using System.Runtime.Serialization;

    public enum TaskPriorityDTO : int
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
