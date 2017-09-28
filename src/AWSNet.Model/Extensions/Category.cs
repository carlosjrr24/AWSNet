using System;

namespace AWSNet.Model
{
    public partial class Category
    {
        partial void OnCreated()
        {
            CreationDate = DateTime.UtcNow;
        }
    }
}
