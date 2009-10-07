namespace Apprenda.Taskr.Service
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
        #region Fields
        [DataMember(Name = "Id")]
        private Guid id;

        [DataMember(Name = "Label")]
        private string label;
        #endregion

        #region Constructors
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
        #endregion

        #region Properties
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
        #endregion

        #region Mapping Methods
        public Tag MapTo()
        {
            Tag instance = new Tag(label);
            instance.Id = Guid.NewGuid();
            return instance;
        }

        public void MapInto(Tag instance)
        {
            instance.Label = label;
        }

        public void MapFrom(Tag instance)
        {
            id = instance.Id;
            label = instance.Label;
        }

        public static TagDTO StaticMapFrom(Tag instance)
        {
            if (null == instance) { return null; }

            TagDTO dto = new TagDTO();
            dto.MapFrom(instance);
            return dto;
        }
        #endregion

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

        public override string ToString()
        {
            return string.Format
            (
                "Id: '{0}'\nLabel: '{1}'\n",
                id,
                label
            );
        }
    }
}
