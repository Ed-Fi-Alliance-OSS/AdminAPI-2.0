// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using EdFi.Admin.DataAccess.Contexts;
using EdFi.Admin.DataAccess.Models;

namespace EdFi.Ods.AdminApi.Infrastructure.Database.Queries;

public interface IGetProfilesQuery
{
    List<Profile> Execute();
    List<Profile> Execute(int offset, int limit, int? id, string? name);
}

public class GetProfilesQuery : IGetProfilesQuery
{
    private readonly IUsersContext _usersContext;

    public GetProfilesQuery(IUsersContext usersContext)
    {
        _usersContext = usersContext;
    }

    public List<Profile> Execute()
    {
        return _usersContext.Profiles.OrderBy(p => p.ProfileName).ToList();
    }

    public List<Profile> Execute(int offset, int limit, int? id, string? name)
    {
        return _usersContext.Profiles
            .Where(p => id == null || p.ProfileId == id)
            .Where(p => name == null || p.ProfileName == name)
            .OrderBy(p => p.ProfileName)
            .Skip(offset).Take(limit).ToList();
    }
}
