using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Data.Extensions;

namespace Nop.Data.Migrations;
[NopMigration("2025-07-22 00:00:00", "AddClaimsAndReports")]
public class AddClaimsAndReports : ForwardOnlyMigration
{
    public override void Up()
    {

        Create.Table("CustomerClaims")
            .WithColumn(nameof(CustomerClaims.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(CustomerClaims.CustomerId)).AsInt32().ForeignKey<Customer>()
            .WithColumn(nameof(CustomerClaims.AdId)).AsInt32().ForeignKey<Product>()
            .WithColumn(nameof(CustomerClaims.Title)).AsString(100)
            .WithColumn(nameof(CustomerClaims.Username)).AsString(100)
            .WithColumn(nameof(CustomerClaims.PhoneNumber)).AsString(50)
            .WithColumn(nameof(CustomerClaims.Email)).AsString(100)
            .WithColumn(nameof(CustomerClaims.Message)).AsString(500).Nullable()
            .WithColumn(nameof(CustomerClaims.AdminNote)).AsString(500).Nullable()
            .WithColumn(nameof(CustomerClaims.Status)).AsInt32().WithDefaultValue(1)
            .WithColumn(nameof(CustomerClaims.AttachemntIds)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(CustomerClaims.CreatedOnUtc)).AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);
        ;


        Create.Table("AdReportReason")
        .WithColumn(nameof(AdReportReason.Id)).AsInt32().PrimaryKey().Identity()
        .WithColumn(nameof(AdReportReason.LanguageId)).AsInt32().ForeignKey<Language>()
        .WithColumn(nameof(AdReportReason.Title)).AsString(500).Nullable();


        Create.Table("AdReports")
          .WithColumn(nameof(AdReports.Id)).AsInt32().PrimaryKey().Identity()
          .WithColumn(nameof(AdReports.ReportReason)).AsInt32().ForeignKey<AdReportReason>()
          .WithColumn(nameof(AdReports.AdId)).AsInt32().ForeignKey<Product>()
          .WithColumn(nameof(AdReports.Description)).AsString(500).Nullable()
          .WithColumn(nameof(AdReports.CreatedOnUtc)).AsDateTime().Nullable();

        ;
    }


}