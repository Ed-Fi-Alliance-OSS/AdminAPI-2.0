// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.


using EdFi.Admin.DataAccess.Models;
using EdFi.Ods.AdminApi.Infrastructure.Database.Commands;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace EdFi.Ods.AdminApi.DBTests.Database.CommandTests;

[TestFixture]
public class AddOdsInstanceContextTests : PlatformUsersContextTestBase
{
    [Test]
    public void ShouldAddOdsInstanceContext()
    {
        var odsInstance = new OdsInstance
        {
            Name = "ODS Instance Name",
            InstanceType = "Ods",
            ConnectionString = "Data Source=(local);Initial Catalog=EdFi_Ods;Integrated Security=True;Encrypt=False"
        };

        Save(odsInstance);

        var contextKey = "contextKey";
        var contextValue = "contextValue";

        var newOdsInstanceContext = new Mock<IAddOdsInstanceContextModel>();
        newOdsInstanceContext.Setup(x => x.OdsInstanceId).Returns(odsInstance.OdsInstanceId);
        newOdsInstanceContext.Setup(x => x.ContextKey).Returns(contextKey);
        newOdsInstanceContext.Setup(x => x.ContextValue).Returns(contextValue);

        var id = 0;
        Transaction(usersContext =>
        {
            var command = new AddOdsInstanceContextCommand(usersContext);
            id = command.Execute(newOdsInstanceContext.Object).OdsInstanceContextId;
            id.ShouldBeGreaterThan(0);
        });

        Transaction(usersContext =>
        {
            var odsInstanceContext = usersContext.OdsInstanceContexts.Single(v => v.OdsInstanceContextId == id);
            odsInstanceContext.OdsInstanceId.ShouldBe(odsInstance.OdsInstanceId);
            odsInstanceContext.ContextKey.ShouldBe(contextKey);
            odsInstanceContext.ContextValue.ShouldBe(contextValue);
        });
    }
}
