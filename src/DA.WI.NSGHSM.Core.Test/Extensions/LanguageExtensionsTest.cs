using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.XUnitExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static DA.WI.NSGHSM.Core.App.Configuration.ApplicationConfig;

namespace DA.WI.NSGHSM.Core.Test.Extensions
{
    public class LanguageExtensionsTest
    {
        [FactWithAutomaticDisplayName]
        public void GetLanguageCode_IsCode_En_When_Blank_Characters()
        {
            //LanguageConfig sut = new LanguageConfig();

            string sut = "en  -  US";
            var result = sut.GetLanguageCode();

            Assert.Equal("en", result);
        }
        [FactWithAutomaticDisplayName]
        public void GetLanguageCode_IsCode_En()
        {
            // LanguageConfig sut = new LanguageConfig();

            string sut = "en-US";
            var result = sut.GetLanguageCode();

            Assert.Equal("en",result);
        }

        [FactWithAutomaticDisplayName]
        public void GetLanguageCode_No_Character_Hyphen_Return_Complete_String()
        {
            //LanguageConfig sut = new LanguageConfig();

            string sut = "enUS";
            var result = sut.GetLanguageCode();

            Assert.Equal("enUS", result);
        }

       

        [FactWithAutomaticDisplayName]
        public void GetLanguageCode_Empty_Return_Empty()
        {
           //LanguageConfig sut = new LanguageConfig();

            string sut = string.Empty;

            var result = sut.GetLanguageCode();

            Assert.Equal(string.Empty, result);
        }



        
        [FactWithAutomaticDisplayName]
        public void GetCountryCode_IsCode_US_When_Blank_Characters()
        {
            //LanguageConfig sut = new LanguageConfig();

            string sut = "en  -  US";
            var result = sut.GetCountryCode();

            Assert.Equal("US", result);
        }
        [FactWithAutomaticDisplayName]
        public void GetCountryCode_IsCode_US()
        {
            //LanguageConfig sut = new LanguageConfig();

            string sut = "en-US";
            var result = sut.GetCountryCode();

            Assert.Equal("US", result);
        }

        [FactWithAutomaticDisplayName]
        public void GetCountryCode_No_Character_Hyphen_Return_Null()
        {
            //LanguageConfig sut = new LanguageConfig();

            string sut = "enUS";
            Assert.Throws<IndexOutOfRangeException>(() => sut.GetCountryCode());
           
        }


        [FactWithAutomaticDisplayName]
        public void GetCountryCode_Empty_Throws_IndexOutOfRangeException()
        {
           // LanguageConfig sut = new LanguageConfig();

            string sut = string.Empty;

            Assert.Throws<IndexOutOfRangeException>(() => sut.GetCountryCode());
        }


        [FactWithAutomaticDisplayName]
        public void IsCodeValid_ReturnTrue()
        {
            string sut = "en-US";
            bool res = sut.IsCodeValid();
            Assert.True(res);
        }

        [FactWithAutomaticDisplayName]
        public void IsCodeValid_Return_False_When_Code_incorrect()
        {
            string sut = "enUS";
            bool res = sut.IsCodeValid();
            Assert.False(res);
        }

        [FactWithAutomaticDisplayName]
        public void IsCodeValid_Return_False_Code_Null()
        {
            string sut = null;
            bool res = sut.IsCodeValid();
            Assert.False(res);
        }

        [FactWithAutomaticDisplayName]
        public void IsCodeValid_Return_False_Code_Empty()
        {
            string sut = string.Empty;
            bool res = sut.IsCodeValid();
            Assert.False(res);
        }

    }
}
