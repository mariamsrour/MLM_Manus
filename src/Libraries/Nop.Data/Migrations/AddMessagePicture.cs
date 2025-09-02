//using System.Data;
//using FluentMigrator;
//using Nop.Core.Domain.Common;
//using Nop.Core.Domain.Orders;
//using Nop.Data.Extensions;
//using Nop.Core.Domain.Catalog;
//using Nop.Core.Domain.Vendors;
//using Nop.Core.Domain.Forums;


//namespace Nop.Data.Migrations
//{
//    [NopSchemaMigration("2025-06-01 01:00:00", "AddMessagePicture")]

//    public partial class AddMessagePicture : ForwardOnlyMigration
//    {
//        public override void Up()
//        {
//            // Product table updates
//            var customerTable = "Forums_PrivateMessage";

//            if (!Schema.Table(customerTable).Column(nameof(PrivateMessage.PictureId)).Exists())
//                Alter.Table(customerTable)
//                    .AddColumn(nameof(PrivateMessage.PictureId)).AsInt32().Nullable().WithDefaultValue(0);

//        }

//    }
//}
