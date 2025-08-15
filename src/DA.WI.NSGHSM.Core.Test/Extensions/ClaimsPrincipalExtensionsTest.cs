using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DA.WI.NSGHSM.Core.Extensions;
using System.Security.Claims;
using System.Security.Principal;
using FluentAssertions;
using DA.WI.NSGHSM.XUnitExtensions;

namespace DA.WI.NSGHSM.Core.Test.Extensions
{
    public class ClaimsPrincipalExtensionsTest
    {
        #region GetName

        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_Null_GetName_Returns_Null()
        {
           ClaimsPrincipal sut = null;

            var result = sut.GetName();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_Identity_Null_GetName_Returns_Null()
        {
            var sut = new ClaimsPrincipal();

            var result = sut.GetName();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_Identity_Name_A_GetName_Returns_A()
        {
            var sut = new ClaimsPrincipal(new FakeIdentity() { Name = "A"});

            var result = sut.GetName();

            result.Should().Be("A");
        }

        #endregion

        #region GetEmail

        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_Null_GetEmail_Returns_Null()
        {
            ClaimsPrincipal sut = null;

            var result = sut.GetEmail();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_EmailClaim_NotFound_GetEmail_Returns_Null()
        {
            var sut = new ClaimsPrincipal();

            var result = sut.GetEmail();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_EmailClaim_FoundOne_GetEmail_Returns_EmailFromClaim()
        {
            var identity = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, "me@test.net") });
            var sut = new ClaimsPrincipal(identity);

            var result = sut.GetEmail();

            result.Should().Be("me@test.net");
        }


        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_EmailClaims_FoundTwo_GetEmail_Returns_EmailFromFirstClaim()
        {
            var identity = new ClaimsIdentity(new Claim[] 
            {
                new Claim(ClaimTypes.Email, "me1@test.net"),
                new Claim(ClaimTypes.Email, "me2@test.net")
            });
            var sut = new ClaimsPrincipal(identity);

            var result = sut.GetEmail();

            result.Should().Be("me1@test.net");
        }

        #endregion

        #region GetRoles
        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_Null_GetRoles_Returns_Null()
        {
            ClaimsPrincipal sut = null;

            var result = sut.GetRoles();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_RoleClaims_NotFound_GetRoles_Returns_Empty()
        {
            var sut = new ClaimsPrincipal();

            var result = sut.GetRoles();

            Assert.Empty(result);
        }

        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_RoleClaim_FoundOne_GetRoles_Returns_RoleFromClaim()
        {
            var identity = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Role, "role1") });
            var sut = new ClaimsPrincipal(identity);

            var result = sut.GetRoles();

            result.Should().BeEquivalentTo(new[] { "role1" });
        }


        [FactWithAutomaticDisplayName]
        public void ClaimsPrincipal_RoleClaim_FoundTwo_GetRoles_Returns_TwoRolesFromClaims()
        {
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "role1"),
                new Claim(ClaimTypes.Role, "role2")
            });
            var sut = new ClaimsPrincipal(identity);

            var result = sut.GetRoles();

            result.Should().BeEquivalentTo(new[] { "role1", "role2" });
        }

        #endregion


        private class FakeIdentity : IIdentity
        {
            public string AuthenticationType => String.Empty;

            public bool IsAuthenticated => true;

            public string Name { get; set; }
        }
    }
}
