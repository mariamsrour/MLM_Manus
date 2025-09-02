using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog
{
    public class TempProductPicture : BaseEntity
    {
        public int PictureId { get; set; }

        public string SessionId { get; set; }

        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

        public int DisplayOrder { get; set; }
    }
}

