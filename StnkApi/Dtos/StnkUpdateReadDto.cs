﻿namespace StnkApi.Dtos
{
    public class StnkUpdateReadDto
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerNik { get; set; } = string.Empty;
        public int CarType { get; set; }
        public string CarName { get; set; } = string.Empty;
        public int EngineSize { get; set; }
        public decimal CarPrice { get; set; }
        public decimal LastTaxPrice { get; set; }
    }
}
