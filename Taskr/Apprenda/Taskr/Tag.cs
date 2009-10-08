using System.Data.Services;
using System.Data.Services.Common;

namespace Apprenda.Taskr
{
    using System;
    using System.Runtime.Serialization;
    using Home = global::Taskr;
    using System.ServiceModel;

    /// <summary>
    /// The Tag class represents a set of attributes that can be assigned
    /// to tasks. Each tag has a unique label used to assign an attribute
    /// to a task.
    /// </summary>
    [DataServiceKey("Id")]
    public partial class Tag : IEquatable<Tag>
    {
        #region Constructor
        public Tag(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new FaultException(Home.Resources.Tag_MissingLabelNew);

            this.Label = label;
        }
        #endregion

        #region Validation
        partial void OnLabelChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new FaultException(Home.Resources.Tag_MissingLabel);
            }
        }
        #endregion

        #region Utility Methods
        public bool Equals(Tag other)
        {
            if (other == null)
                return false;

            return (Label == other.Label);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tag) || obj == null)
                return false;

            return Equals((Tag)obj);
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }
        #endregion
    }
}
