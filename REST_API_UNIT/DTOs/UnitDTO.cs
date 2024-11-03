using static REST_API_UNIT.DTO.Base.DTOBase;
using System.ComponentModel.DataAnnotations;
using REST_API_UNIT.Models;

namespace REST_API_UNIT.DTO
{
    public class UnitDTO 
    {
        public class DTOUnitNewRequest
        {
            public required string Name { get; set; }
            public required bool IsActive { get; set; }
            public required string Type { get; set; }
        }

        public class DTOUnitGetResponse : DTOResponseBase
        {
            public IList<Unit>? units { get; set; }
        }

        public class DTOUnitGetSingleResponse : DTOResponseBase
        {
            public Unit? unit { get; set; }
        }

    }
}
