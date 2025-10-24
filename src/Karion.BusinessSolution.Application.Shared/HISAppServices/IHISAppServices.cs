using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.Authorization.Users.Profile.Dto;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.HISAppServices
{
    public interface IHISAppServices : IApplicationService
    {
        Task<List<HISDtoGetAllRegistrationResult>> GetAllRegistrationList(HISDtoGetAllRegistrationInput input);
        Task<ConfirmRegistrationSavedResponse> ConfirmRegistrationSaved(ConfirmRegistrationSavedInput input);

        Task<UpdatePaymentAmountResponse> UpdatePaymentAmount(
            UpdatePaymentAmountInput input);
    }
}