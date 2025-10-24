using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Editions.Dto;

namespace Karion.BusinessSolution.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}