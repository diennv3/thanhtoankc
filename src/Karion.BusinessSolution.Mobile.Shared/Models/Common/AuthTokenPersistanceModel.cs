using System;
using Abp.AutoMapper;
using Karion.BusinessSolution.Sessions.Dto;

namespace Karion.BusinessSolution.Models.Common
{
    [AutoMapFrom(typeof(ApplicationInfoDto)),
     AutoMapTo(typeof(ApplicationInfoDto))]
    public class ApplicationInfoPersistanceModel
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}