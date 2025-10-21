﻿using MindNose.Domain.Nodes;
using MindNose.Domain.Request;

namespace MindNose.Domain.Interfaces.UseCases.MindNoseCore;

public interface IGetOrCreateLinksUseCase
{
    Task<Links> ExecuteAsync(LinksRequest request);
}