using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Karion.BusinessSolution.Authorization.Users.Delegation.Dto;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.MultiTenancy;
using Newtonsoft.Json.Linq;

namespace Karion.BusinessSolution.Dto
{
    public class HanetAPIDto
    {
        public class HanetGetTokenDto
        {
            public string code { get; set; }
            public string grant_type { get; set; }
            public string client_id { get; set; }
            public string redirect_uri { get; set; }
            public string client_secret { get; set; }
        }

        public class HanetGetRefreshTokenDto
        {
            public string refresh_token { get; set; }
            public string grant_type { get; set; }
            public string client_id { get; set; }
            public string redirect_uri { get; set; }
            public string client_secret { get; set; }
        }

        public class HanetGetTokenResultDto
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string email { get; set; }
            public string userID { get; set; }
            public string expire { get; set; }
            public string token_type { get; set; }
        }

        public class HanetAccessTokenInputDto
        {
            public string token { get; set; }
        }

        public class HanetAddPlaceInputDto : HanetAccessTokenInputDto
        {
            public string name { get; set; }
            public string address { get; set; }
        }

        public class HanetRegisterUserInputDto : HanetAccessTokenInputDto
        {
            public string name { get; set; }
            public string url { get; set; }
            public string aliasID { get; set; }
            public string placeID { get; set; }
            public string title { get; set; }
            public string type { get; set; }
        }

        public class HanetGetDevicesByPlaceIdInputDto : HanetAccessTokenInputDto
        {
            public string placeID { get; set; }
        }
        public class HanetGetDevicesStatusDto : HanetAccessTokenInputDto
        {
            public string deviceIDs { get; set; }
        }
        public class HanetAddPlace
        {
            public string address { get; set; }
            public string name { get; set; }
            public int id { get; set; }
            public long userID { get; set; }
        }

        public class HanetAddPlaceResultDto
        {
            public int returnCode { get; set; }
            public string returnMessage { get; set; }
            public HanetAddPlace data { get; set; }

        }

        public class CommonHanetResponse
        {
            public int statusCode { get; set; }
            public int returnCode { get; set; }
            public string returnMessage { get; set; }
        }

        public class HanetGetDevices
        {
            public string address { get; set; }
            public string deviceID { get; set; }
            public string deviceName { get; set; }
            public string placeName { get; set; }
        }

        public class HanetGetDevicesByPlaceIdResult : CommonHanetResponse
        {
            public List<HanetGetDevices> data { get; set; }
        }
        public class HanetGetDevicesStatusResult : CommonHanetResponse
        {
            public JObject data { get; set; }
        }
    }
}