// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using EdFi.Admin.DataAccess.Models;
using EdFi.Ods.AdminApi.Infrastructure.Database.Commands;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace EdFi.Ods.AdminApi.DBTests.Database.CommandTests;

[TestFixture]
public class EditOdsInstanceContextTests : PlatformUsersContextTestBase
{
    [Test]
    public void ShouldEditOdsInstanceContext()
    {
        var odsInstance1 = new OdsInstance
        {
            Name = "ODS Instance Name",
            InstanceType = "Ods1",
            ConnectionString = "Data Source=(local);Initial Catalog=EdFi_Ods;Integrated Security=True;Encrypt=False"
        };
        Save(odsInstance1);

        var odsInstance2 = new OdsInstance
        {
            Name = "ODS Instance Name",
            InstanceType = "Ods2",
            ConnectionString = "Data Source=(local);Initial Catalog=EdFi_Ods;Integrated Security=True;Encrypt=False"
        };
        Save(odsInstance2);

        var contextKey = "contextKey";
        var contextValue = "contextValue";
        var newOdsInstanceContext = new OdsInstanceContext
        {
            ContextKey = contextKey,
            ContextValue = contextValue,
            OdsInstanceId = odsInstance1.OdsInstanceId
        };
        Save(newOdsInstanceContext);

        var updateContextKey = "contextKey2";
        var updateContextValue = "contextValue2";
        var editOdsInstanceContext = new Mock<IEditOdsInstanceContextModel>();
        editOdsInstanceContext.Setup(x => x.OdsInstanceId).Returns(odsInstance2.OdsInstanceId);
        editOdsInstanceContext.Setup(x => x.ContextKey).Returns(updateContextKey);
        editOdsInstanceContext.Setup(x => x.ContextValue).Returns(updateContextValue);
        editOdsInstanceContext.Setup(x => x.Id).Returns(newOdsInstanceContext.OdsInstanceContextId);

        Transaction(usersContext =>
        {
            var command = new EditOdsInstanceContextCommand(usersContext);
            var updatedOdsInstanceContext = command.Execute(editOdsInstanceContext.Object);
            updatedOdsInstanceContext.ShouldNotBeNull();
            updatedOdsInstanceContext.OdsInstanceContextId.ShouldBeGreaterThan(0);
            updatedOdsInstanceContext.OdsInstanceContextId.ShouldBe(newOdsInstanceContext.OdsInstanceContextId);
            updatedOdsInstanceContext.OdsInstanceId.ShouldBe(odsInstance2.OdsInstanceId);
            updatedOdsInstanceContext.ContextKey.ShouldBe(updateContextKey);
            updatedOdsInstanceContext.ContextValue.ShouldBe(updateContextValue);
        });
    }
}
