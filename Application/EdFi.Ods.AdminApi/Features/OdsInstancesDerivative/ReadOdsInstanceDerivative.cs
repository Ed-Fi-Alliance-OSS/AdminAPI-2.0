// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using AutoMapper;
using EdFi.Ods.AdminApi.Infrastructure;
using EdFi.Ods.AdminApi.Infrastructure.Database.Queries;

namespace EdFi.Ods.AdminApi.Features.OdsInstancesDerivative;

public class ReadOdsInstanceDerivative : IFeature
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        AdminApiEndpointBuilder.MapGet(endpoints, "/odsInstancesDerivative", GetOdsInstanceDerivatives)
            .WithDefaultDescription()
            .WithRouteOptions(b => b.WithResponse<OdsInstanceDerivativeModel[]>(200))
            .BuildForVersions(AdminApiVersions.V2);

        AdminApiEndpointBuilder.MapGet(endpoints, "/odsInstancesDerivative/{id}", GetOdsInstanceDerivative)
            .WithDefaultDescription()
            .WithRouteOptions(b => b.WithResponse<OdsInstanceDerivativeModel>(200))
            .BuildForVersions(AdminApiVersions.V2);
    }

    internal Task<IResult> GetOdsInstanceDerivatives(IGetOdsInstanceDerivativesQuery getOdsInstanceDerivativesQuery, IMapper mapper, int offset, int limit)
    {
        var odsInstanceDerivativeList = mapper.Map<List<OdsInstanceDerivativeModel>>(getOdsInstanceDerivativesQuery.Execute(offset, limit));
        return Task.FromResult(Results.Ok(odsInstanceDerivativeList));
    }

    internal Task<IResult> GetOdsInstanceDerivative(IGetOdsInstanceDerivativeByIdQuery getOdsInstanceDerivativeByIdQuery, IMapper mapper, int id)
    {
        var odsInstanceDerivative = getOdsInstanceDerivativeByIdQuery.Execute(id);
        if (odsInstanceDerivative == null)
        {
            throw new NotFoundException<int>("odsInstanceDerivative", id);
        }
        var model = mapper.Map<OdsInstanceDerivativeModel>(odsInstanceDerivative);
        return Task.FromResult(Results.Ok(model));
    }
}
