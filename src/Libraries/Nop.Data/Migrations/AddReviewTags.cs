using FluentMigrator;
using Nop.Core.Domain.Vendors;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations
{
    [NopMigration("2025-05-03 00:00:00", "FeedbackTags table creation")]
    public class AddFeedbackTags : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("FeedbackTag")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(FeedbackTag.Tag))
                    .AsString(50)
                    .NotNullable()
                    .Unique();

            Create.Table("ReviewTag")
                    .WithColumn(nameof(ReviewTag.ReviewId))
                    .AsInt32()
                    .NotNullable()
                    .ForeignKey("VendorReview", "Id")
            .WithColumn(nameof(ReviewTag.TagId))
                    .AsInt32()
                    .NotNullable()
                    .ForeignKey("FeedbackTag", "Id")
            .WithColumn(nameof(ReviewTag.VendorId))
                    .AsInt32()
                    .NotNullable()
                    .ForeignKey("Vendor", "Id");

        }
    }
}