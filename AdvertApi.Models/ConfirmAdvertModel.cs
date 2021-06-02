using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertApi.Models
{
    public enum AdvertStatus
    {
        Pending = 1,
        Active = 2
    }

    public class ConfirmAdvertModel
    {
        public string Id { get; set; }
        public AdvertStatus  Status { get; set; }

    }
}
