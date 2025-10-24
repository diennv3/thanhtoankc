using System;
using System.Security.Cryptography;
using System.Text;
using Abp.Extensions;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.MobileAppServices;
using Microsoft.AspNetCore.Mvc;

namespace Karion.BusinessSolution.GenQrCode
{

    public class GenQrCodeAppservices : BusinessSolutionAppServiceBase, IGenQrCodeAppservices
    {
        protected ushort NapasQR_CRCHash(byte[] bytes)
        {
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0xffff;
            ushort temp, a;
            ushort crc = initialValue;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort) (i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort) ((temp << 1) ^ poly);
                    else
                        temp <<= 1;
                    a <<= 1;
                }

                table[i] = temp;
            }

            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort) ((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }

            return crc;
        }

        protected const string InitCodeDynamicQRToBankAccount_init = "000201010212";
        protected const string InitCodeDynamicQRToBankAccount_type = "QRIBFTTA";
        protected const string InitCodeDynamicQRToBankAccount_curency_VND = "704";
        protected const string AIDNapas = "A000000727";
        [HttpPost]
        public string PayQrGenerator(QRInputDto input)
        {
            //t=hash noi dung
            //check t == input.token => đúng => true, sai : return 500
            
            string hashCodeQR = InitCodeDynamicQRToBankAccount_init;
            MobileDto.QRStructure bankAccountBlock = new MobileDto.QRStructure()
            {
                id = 38
            };
            MobileDto.QRStructure bankAccountBlock_napasAID = new MobileDto.QRStructure()
            {
                id = 0,
                data = AIDNapas
            };
            MobileDto.QRStructure bankAccountBlock_recieverBankInfo = new MobileDto.QRStructure()
            {
                id = 1
            };
            MobileDto.QRStructure bankAccountBlock_bankCode = new MobileDto.QRStructure()
            {
                id = 0,
                data = input.bankCode
            };
            MobileDto.QRStructure bankAccountBlock_bankAccount = new MobileDto.QRStructure()
            {
                id = 1,
                data = input.bankAccount
            };
            MobileDto.QRStructure bankAccountBlock_transferType = new MobileDto.QRStructure()
            {
                id = 2,
                data = InitCodeDynamicQRToBankAccount_type
            };
            MobileDto.QRStructure curency_block = new MobileDto.QRStructure()
            {
                id = 53,
                data = InitCodeDynamicQRToBankAccount_curency_VND
            };
            MobileDto.QRStructure amount_block = new MobileDto.QRStructure()
            {
                id = 54,
                data = input.amount.ToString()
            };
            MobileDto.QRStructure country_block = new MobileDto.QRStructure()
            {
                id = 58,
                data = "VN"
            };
            MobileDto.QRStructure note_block = new MobileDto.QRStructure()
            {
                id = 62,
                data = new MobileDto.QRStructure()
                {
                    id = 8,
                    data = input.noiDung.ToLower()
                }.dataString()
            };
            bankAccountBlock_recieverBankInfo.data = bankAccountBlock_bankCode.dataString() +
                                                     bankAccountBlock_bankAccount.dataString();
            var t = note_block.dataString();
            
            bankAccountBlock.data =
                bankAccountBlock_napasAID.dataString() + bankAccountBlock_recieverBankInfo.dataString() +
                bankAccountBlock_transferType.dataString();
            hashCodeQR += bankAccountBlock.dataString()
                          + curency_block.dataString()
                          + amount_block.dataString()
                          + country_block.dataString()
                          + note_block.dataString();

            //CRC
             hashCodeQR += "6304";
             return hashCodeQR + NapasQR_CRCHash(Encoding.ASCII.GetBytes(hashCodeQR)).ToString("X");
        }
        [HttpPost]
        public GeneratorOutput Generator(GeneratorInput input)
        {
            var hashToken =
                (input.amount + input.bankAccount + input.bankCode + "techberqr" + input.noiDung.ToMd5().ToLower())
                .ToMd5().ToLower();
            if (!hashToken.Equals(input.token))
            {
                DataGeneratorOutput ketqua = new DataGeneratorOutput();
                ketqua.status = false;
                ketqua.messenger = "Token không hợp lệ";
                
                return new GeneratorOutput()
                {
                    errorCode = 200,
                    data = ketqua
                };
            }
            else
            {
                var qrString = PayQrGenerator(new QRInputDto()
                {
                    amount = input.amount,
                    bankAccount = input.bankAccount,
                    bankCode = input.bankCode,
                    noiDung = input.noiDung
                });
                
                DataGeneratorOutput ketqua = new DataGeneratorOutput();
                ketqua.status = true;
                ketqua.messenger = "Thành công";
                ketqua.qrCodeString = qrString;
                
                return new GeneratorOutput()
                {
                    errorCode = 200,
                    data = ketqua
                };
            }
            
        }
    }
}