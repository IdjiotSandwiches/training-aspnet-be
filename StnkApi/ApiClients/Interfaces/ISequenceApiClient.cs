﻿using SharedLibrary.Enums;

namespace StnkApi.ApiClients.Interfaces
{
    public interface ISequenceApiClient
    {
        Task<string?> GetSequence(SequenceTypeEnum type);
    }
}
