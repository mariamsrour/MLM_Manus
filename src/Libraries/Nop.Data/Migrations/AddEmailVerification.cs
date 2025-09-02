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
[NopMigration("2025-07-25 00:00:00", "AddEmailVerification")]
public class AddEmailVerification : ForwardOnlyMigration
{
    public override void Up()
    {

        Create.Table("EmailVerificationCode")
            .WithColumn(nameof(EmailVerificationCode.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(EmailVerificationCode.CustomerId)).AsInt32().ForeignKey<Customer>()
            .WithColumn(nameof(EmailVerificationCode.Code)).AsString(50).NotNullable()
            .WithColumn(nameof(EmailVerificationCode.CreatedOnUtc)).AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn(nameof(EmailVerificationCode.IsUsed)).AsBoolean().Nullable();


       
    }


}