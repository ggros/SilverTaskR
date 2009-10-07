namespace Apprenda.Taskr.Client
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Tag class represents a set of attributes that can be assigned
    /// to tasks. Each tag has a unique label used to assign an attribute
    /// to a task.
    /// </summary>
    [DataContract(Name = "Tag", Namespace = "Apprenda.Taskr")]
    public class TagDTO : IEquatable<TagDTO>
    {
        [DataMember(Name = "Id")]
        private Guid id;
        [DataMember(Name = "Label")]
        private string label;

        public TagDTO() { }

        public TagDTO(Guid id)
        {
            this.id = id;
        }

        public TagDTO(Guid id, string label)
        {
            this.id = id;
            this.label = label;
        }

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public bool Equals(TagDTO other)
        {
            if (other == null)
                return false;

            return (label == other.label);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TagDTO) || obj == null)
                return false;

            return Equals((TagDTO)obj);
        }

        public override int GetHashCode()
        {
            return label.GetHashCode();
        }
    }
}
