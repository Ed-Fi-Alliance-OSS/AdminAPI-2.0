// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using EdFi.Security.DataAccess.Contexts;
using EdFi.Security.DataAccess.Models;

namespace EdFi.Ods.AdminApi.Infrastructure.Database.Queries;

public interface IGetAuthStrategiesQuery
{
    List<AuthorizationStrategy> Execute();
    List<AuthorizationStrategy> Execute(int offset, int limit);
}

public class GetAuthStrategiesQuery : IGetAuthStrategiesQuery
{
    private readonly ISecurityContext _context;

    public GetAuthStrategiesQuery(ISecurityContext context)
    {
        _context = context;
    }

    public List<AuthorizationStrategy> Execute()
    {
        return _context.AuthorizationStrategies.OrderBy(v => v.AuthorizationStrategyName).ToList();
    }

    public List<AuthorizationStrategy> Execute(int offset, int limit)
    {
        return _context.AuthorizationStrategies.OrderBy(v => v.AuthorizationStrategyName).Skip(offset).Take(limit).ToList();
    }
}
