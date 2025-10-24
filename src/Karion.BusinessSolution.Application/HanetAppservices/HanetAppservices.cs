using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.UI;
using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.Common;
using Karion.BusinessSolution.Configuration;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.HanetTenant;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.MobileAppServices;
using Karion.BusinessSolution.MultiTenancy;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.TBHostConfigure;
using Karion.BusinessSolution.VersionControl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Karion.BusinessSolution.HanetAppservices.HanetAppservices
{
    public class HanetAppservices : BusinessSolutionAppServiceBase, IHanetAppservices
    {
        private readonly IRepository<ThongTinDonVi> _thongTinDonViRepository;
        private readonly IRepository<TechberConfigure> _configureRepository;
        private readonly IRepository<HanetTenantPlaceDatas> _hanetPlaceRepository;
        private readonly IRepository<HanetTenantDeviceDatas> _hanetDevicesRepository;
        private readonly IRepository<HanetFaceDetected, long> _hanetFacesRepository;
        private readonly IRepository<HanetTenantLog> _hanetLogRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public HanetAppservices(
            IRepository<ThongTinDonVi> thongTinDonViRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<TechberConfigure> configureRepository,
            IRepository<HanetTenantPlaceDatas> hanetPlaceRepository,
            IRepository<HanetTenantDeviceDatas> hanetDevicesRepository,
            IRepository<HanetTenantLog> hanetLogRepository,
            IRepository<HanetFaceDetected, long> hanetFacesRepository
        )
        {
            _thongTinDonViRepository = thongTinDonViRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _configureRepository = configureRepository;
            _hanetPlaceRepository = hanetPlaceRepository;
            _hanetDevicesRepository = hanetDevicesRepository;
            _hanetLogRepository = hanetLogRepository;
            _hanetFacesRepository = hanetFacesRepository;
        }


        [HttpGet]
        public async Task HanetWebhookGetAuthorizationCode(string code)
        {
            var thongTinAuthorization =
                await _configureRepository.FirstOrDefaultAsync(p => p.Key == "AUTHORIZATION_CODE");
            if (thongTinAuthorization.isNull())
            {
                _configureRepository.Insert(new TechberConfigure()
                {
                    Key = "AUTHORIZATION_CODE",
                    Value = code
                });
            }
            else
            {
                thongTinAuthorization.Value = code;
                _configureRepository.Update(thongTinAuthorization);
            }
        }

        [HttpPost]
        public async Task<MobileDto.CommonResponse> HanetWebhookGetAccessToken()
        {
            var thongTinAuthorization =
                await _configureRepository.FirstOrDefaultAsync(p => p.Key == "AUTHORIZATION_CODE");
            if (thongTinAuthorization.isNull())
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = "AUTHORIZATION_CODE configuration is missing [[KEY: AUTHORIZATION_CODE]]"
                };
            }

            var accessToken = await _configureRepository.FirstOrDefaultAsync(p => p.Key == "ACCESS_TOKEN");
            if (!accessToken.isNull())
            {
                var expireTime = await _configureRepository.FirstOrDefaultAsync(p => p.Key == "ACCESS_TOKEN_EXPIRE");
                int NowTimestamp = Helper.GetTimeStamp();
                if (Int32.Parse(expireTime.Value) <= NowTimestamp)
                {
                    // return new MobileDto.CommonResponse()
                    // {
                    //     status = false,
                    //     message = "Access Token Expire!"
                    // };

                    var refreshToken = await _configureRepository.FirstOrDefaultAsync(p => p.Key == "REFRESH_TOKEN");

                    // refresh token
                    var hanetGetTokenUri =
                        await _configureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_GET_TOKEN_URI");
                    if (hanetGetTokenUri.isNull())
                    {
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = "hanetGetTokenUri configuration is missing [[KEY: HANET_GET_TOKEN_URI]]"
                        };
                    }

                    var hanetClientID =
                        await _configureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_GET_CLIENT_ID");
                    if (hanetClientID.isNull())
                    {
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = "hanetClientID configuration is missing [[KEY: HANET_GET_CLIENT_ID]]"
                        };
                    }

                    var hanetClientSecret =
                        await _configureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_GET_CLIENT_SECRET");
                    if (hanetClientSecret.isNull())
                    {
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = "hanetClientSecret configuration is missing [[KEY: HANET_GET_CLIENT_SECRET]]"
                        };
                    }


                    HanetAPIDto.HanetGetRefreshTokenDto hanetGetTokenDto = new HanetAPIDto.HanetGetRefreshTokenDto()
                    {
                        refresh_token = refreshToken.Value,
                        grant_type = "refresh_token",
                        client_id = hanetClientID.Value,
                        redirect_uri = "",
                        client_secret = hanetClientSecret.Value
                    };
                    string SerializeData = JsonConvert.SerializeObject(hanetGetTokenDto);
                    string Result = Request.webRequest(hanetGetTokenUri.Value, SerializeData,
                        new List<Request.RequestHeader>(), "POST", "application/json");
                    var DeserializeResult = JObject.Parse(Result);
                    if (DeserializeResult.ContainsKey("err"))
                    {
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = "Get Token Error: " + DeserializeResult["msg"].ToString()
                        };
                    }

                    accessToken.Value = DeserializeResult["access_token"].ToString();
                    refreshToken.Value = DeserializeResult["refresh_token"].ToString();

                    _configureRepository.Update(accessToken);
                    _configureRepository.Update(refreshToken);

                    return new MobileDto.CommonResponse()
                    {
                        status = true,
                        message = L("Refresh Token Success")
                    };
                }

                return new MobileDto.CommonResponse()
                {
                    status = true,
                    message = L("Refresh Token Success")
                };
            }
            else
            {
                var hanetGetTokenUri =
                    await _configureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_GET_TOKEN_URI");
                if (hanetGetTokenUri.isNull())
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        message = "hanetGetTokenUri configuration is missing [[KEY: HANET_GET_TOKEN_URI]]"
                    };
                }

                var hanetClientID =
                    await _configureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_GET_CLIENT_ID");
                if (hanetClientID.isNull())
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        message = "hanetClientID configuration is missing [[KEY: HANET_GET_CLIENT_ID]]"
                    };
                }

                var hanetClientSecret =
                    await _configureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_GET_CLIENT_SECRET");
                if (hanetClientSecret.isNull())
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        message = "hanetClientSecret configuration is missing [[KEY: HANET_GET_CLIENT_SECRET]]"
                    };
                }


                HanetAPIDto.HanetGetTokenDto hanetGetTokenDto = new HanetAPIDto.HanetGetTokenDto()
                {
                    code = thongTinAuthorization.Value,
                    grant_type = "authorization_code",
                    client_id = hanetClientID.Value,
                    redirect_uri = "",
                    client_secret = hanetClientSecret.Value
                };
                string SerializeData = JsonConvert.SerializeObject(hanetGetTokenDto);
                string Result = Request.webRequest(hanetGetTokenUri.Value, SerializeData,
                    new List<Request.RequestHeader>(), "POST", "application/json");
                var DeserializeResult = JObject.Parse(Result);
                if (DeserializeResult.ContainsKey("err"))
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        message = "Get Token Error: " + DeserializeResult["msg"].ToString()
                    };
                }

                _configureRepository.Insert(new TechberConfigure()
                {
                    Key = "ACCESS_TOKEN",
                    Value = DeserializeResult["access_token"].ToString()
                });
                _configureRepository.Insert(new TechberConfigure()
                {
                    Key = "REFRESH_TOKEN",
                    Value = DeserializeResult["refresh_token"].ToString()
                });
                _configureRepository.Insert(new TechberConfigure()
                {
                    Key = "HANET_ACCOUNT_EMAIL",
                    Value = DeserializeResult["email"].ToString()
                });
                _configureRepository.Insert(new TechberConfigure()
                {
                    Key = "HANET_USERID",
                    Value = DeserializeResult["userID"].ToString()
                });
                _configureRepository.Insert(new TechberConfigure()
                {
                    Key = "ACCESS_TOKEN_EXPIRE",
                    Value = DeserializeResult["expire"].ToString()
                });
                _configureRepository.Insert(new TechberConfigure()
                {
                    Key = "ACCESS_TOKEN_TYPE",
                    Value = DeserializeResult["token_type"].ToString()
                });
                return new MobileDto.CommonResponse()
                {
                    status = true,
                    message = L("Get Token Success")
                };
            }
        }

        protected async Task checkTokenValidAndRefreshToken()
        {
            var expireTime = await _configureRepository.FirstOrDefaultAsync(p => p.Key == "ACCESS_TOKEN_EXPIRE");
            int NowTimestamp = Helper.GetTimeStamp();
            if (Int32.Parse(expireTime.Value) <= NowTimestamp)
            {
                await HanetWebhookGetAccessToken();
                CurrentUnitOfWork.SaveChanges();
            }
        }

        protected async Task<string> getAccessToken()
        {
            var hanetAccessToken =
                await _configureRepository.FirstOrDefaultAsync(p => p.Key == "ACCESS_TOKEN");
            if (hanetAccessToken.isNull())
            {
                throw new UserFriendlyException("accessToken is missing [[KEY: ACCESS_TOKEN]]");
            }

            return hanetAccessToken.Value;
        }

        public async Task HanetWebhookAddPlace(HanetTenantPlaceDatasDto input)
        {
            await checkTokenValidAndRefreshToken();
            var access_token = await getAccessToken();
            var hanetAddPlaceUri =
                await _configureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_ADD_PLACE_URI");
            if (hanetAddPlaceUri.isNull())
            {
                throw new UserFriendlyException(
                    "hanetAddPlaceUri configuration is missing [[KEY: HANET_ADD_PLACE_URI]]");
            }

            HanetAPIDto.HanetAddPlaceInputDto inputToken = new HanetAPIDto.HanetAddPlaceInputDto()
            {
                token = access_token,
                address = input.placeAddress,
                name = input.placeName
            };
            string SerializeData = JsonConvert.SerializeObject(inputToken);
            string Result = Request.webRequest(hanetAddPlaceUri.Value, SerializeData,
                new List<Request.RequestHeader>(), "POST", "application/json");
            HanetAPIDto.HanetAddPlaceResultDto DeserializeResult =
                JsonConvert.DeserializeObject<HanetAPIDto.HanetAddPlaceResultDto>(Result);
            if (DeserializeResult.returnCode == 1)
            {
                input.userId = DeserializeResult.data.userID;
                input.placeId = DeserializeResult.data.id.ToString();
                await _hanetPlaceRepository.InsertAsync(new HanetTenantPlaceDatas()
                {
                    placeAddress = input.placeAddress,
                    placeId = input.placeId,
                    placeName = input.placeName,
                    tenantId = input.tenantId,
                    userId = input.userId
                });
            }
            else
            {
                throw new UserFriendlyException(Result);
            }
        }

        public async Task UpdateDevices(int placeid)
        {
            await checkTokenValidAndRefreshToken();
            var place = await _hanetPlaceRepository.FirstOrDefaultAsync(placeid);
            if (place.isNull())
            {
                throw new UserFriendlyException("Place id #" + placeid + " not found");
            }

            var access_token = await getAccessToken();
            var hanetGetDevicesUri =
                await _configureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_GET_DEVICES_URI");
            if (hanetGetDevicesUri.isNull())
            {
                throw new UserFriendlyException(
                    "hanetGetDevicesUri configuration is missing [[KEY: HANET_GET_DEVICES_URI]]");
            }

            HanetAPIDto.HanetGetDevicesByPlaceIdInputDto inputToken = new HanetAPIDto.HanetGetDevicesByPlaceIdInputDto()
            {
                token = access_token,
                placeID = place.placeId
            };
            string SerializeData = JsonConvert.SerializeObject(inputToken);
            string Result = Request.webRequest(hanetGetDevicesUri.Value, SerializeData,
                new List<Request.RequestHeader>(), "POST", "application/json");
            HanetAPIDto.HanetGetDevicesByPlaceIdResult DeserializeResult =
                JsonConvert.DeserializeObject<HanetAPIDto.HanetGetDevicesByPlaceIdResult>(Result);
            if (DeserializeResult.returnCode == 1)
            {
                await _hanetDevicesRepository.DeleteAsync(p =>
                    p.HanetTenantPlaceDatasId.Equals(placeid));
                foreach (var VARIABLE in DeserializeResult.data)
                {
                    await _hanetDevicesRepository.InsertAsync(new HanetTenantDeviceDatas()
                    {
                        deviceId = VARIABLE.deviceID,
                        deviceName = VARIABLE.deviceName,
                        deviceStatus = false,
                        lastCheck = DateTime.Now,
                        tenantId = place.tenantId,
                        HanetTenantPlaceDatasId = place.Id
                    });
                }
            }
            else
            {
                throw new UserFriendlyException(Result);
            }
        }

        public async Task updateDevicesStatus()
        {
            await checkTokenValidAndRefreshToken();

            var access_token = await getAccessToken();
            var hanetGetDevicesUri =
                await _configureRepository.FirstOrDefaultAsync(p => p.Key == "HANET_GET_DEVICES_STATUS_URI");
            if (hanetGetDevicesUri.isNull())
            {
                throw new UserFriendlyException(
                    "hanetgetConnectionStatusUri configuration is missing [[KEY: HANET_GET_DEVICES_STATUS_URI]]");
            }

            var hanetDevices = await _hanetDevicesRepository.GetAllListAsync();
            string inputstring = "";
            foreach (var VARIABLE in hanetDevices)
            {
                inputstring += VARIABLE.deviceId;
                if (hanetDevices.IndexOf(VARIABLE) != (hanetDevices.Count - 1))
                {
                    inputstring += ",";
                }
            }

            HanetAPIDto.HanetGetDevicesStatusDto inputToken = new HanetAPIDto.HanetGetDevicesStatusDto()
            {
                token = access_token,
                deviceIDs = inputstring
            };
            string SerializeData = JsonConvert.SerializeObject(inputToken);
            string Result = Request.webRequest(hanetGetDevicesUri.Value, SerializeData,
                new List<Request.RequestHeader>(), "POST", "application/json");
            HanetAPIDto.HanetGetDevicesStatusResult DeserializeResult =
                JsonConvert.DeserializeObject<HanetAPIDto.HanetGetDevicesStatusResult>(Result);
            if (DeserializeResult.returnCode == 1)
            {
                foreach (var VARIABLE in hanetDevices)
                {
                    VARIABLE.deviceStatus = (bool) (DeserializeResult.data[VARIABLE.deviceId]);
                    VARIABLE.lastCheck = DateTime.Now;
                    _hanetDevicesRepository.Update(VARIABLE);
                }
            }
            else
            {
                throw new UserFriendlyException(Result);
            }
        }

        public async Task Webhook(JObject input)
        {
            if (!((string) input["personTitle"]).IsNullOrEmpty()) //co nhan dang
            {
                var now = DateTime.Now;

                var checkExist = await _hanetFacesRepository.FirstOrDefaultAsync(p =>
                    p.userDetectedId.Equals((string)input["aliasID"]) && p.CreationTime >= now.AddMinutes(-20));

                if (checkExist.isNull())
                {
                    _hanetFacesRepository.Insert(new HanetFaceDetected()
                    {
                        mask = (string) input["mask"],
                        deviceId = (string) input["deviceID"],
                        placeId = (string) input["placeID"],
                        detectImageUrl = (string) input["detected_image_url"],
                        userDetectedId = (string) input["aliasID"],
                        CreationTime = now,
                        IsDeleted = false,
                    });
                }
            }
        }

        public async Task RegisterUser(NguoiBenhDto input, bool isEditMode)
        {
            var access_token = await getAccessToken();
            
                var places =
                    await _hanetPlaceRepository.GetAllListAsync(p =>
                        p.tenantId.Equals(CurrentUnitOfWork.GetTenantId()));
                foreach (var VARIABLE in places)
                {
                    var devices =
                        await _hanetDevicesRepository.GetAllListAsync(
                            p => p.HanetTenantPlaceDatasId.Equals(VARIABLE.Id));


                    HanetAPIDto.HanetRegisterUserInputDto hanetRegister = new HanetAPIDto.HanetRegisterUserInputDto()
                    {
                        name = input.HoVaTen,
                        title = input.HoVaTen,
                        token = access_token,
                        type = "0",
                        url = input.ProfilePicture,
                        aliasID = input.Id.ToString(),
                        placeID = VARIABLE.placeId
                    };
                    string SerializeData = JsonConvert.SerializeObject(hanetRegister);
                    string Result = Request.webRequest(!isEditMode?"https://partner.hanet.ai/person/registerByUrl":"https://partner.hanet.ai/person/updateByFaceUrlByAliasID", SerializeData,
                        new List<Request.RequestHeader>(), "POST", "application/json");
                
            }
        }
    }
}