﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PholioVisualisation.PholioObjects;

namespace PholioVisualisation.ServicesTest
{
    /// <summary>
    /// Tests that make sure each end point returns data.
    /// </summary>
    [TestClass]
    public class AreasControllerEndPointTest
    {
        [TestMethod]
        public void TestGetAreaSearchResults()
        {
            byte[] data = GetData("area_search_by_text?" +
                "search_text=hip" +
                "&polygon_area_type_id=" + AreaTypeIds.CountyAndUnitaryAuthorityPreApr2019 +
                "&include_coordinates=" + true);
            IsData(data);
        }

        [TestMethod]
        public void TestGetAreaSearchResultsWhereNoIndexes()
        {
            byte[] data = GetData("area_search_by_text?" +
                                  "search_text=cam" +
                                  "&polygon_area_type_id=" + AreaTypeIds.CountyAndUnitaryAuthority +
                                  "&parent_areas_to_include_in_results=" + AreaTypeIds.CountyAndUnitaryAuthority);
            IsData(data);
        }

        [TestMethod]
        public void TestGetAreaCategories()
        {
            byte[] data = GetData("area_categories?" +
                "profile_id=" + ProfileIds.Phof +
                "&child_area_type_id=" + AreaTypeIds.CountyAndUnitaryAuthorityPreApr2019 +
                "&category_type_id=" + CategoryTypeIds.DeprivationDecileCountyAndUA2010);
            IsData(data);
        }

        [TestMethod]
        public void TestGetParentAreaGroups()
        {
            byte[] data = GetData("area_types/parent_area_types?" +
                "profile_id=" + ProfileIds.Phof);
            IsData(data);
        }

        [TestMethod]
        public void TestGetParentAreas()
        {
            byte[] data = GetData("area/parent_areas?" +
                "child_area_code=" + AreaCodes.CountyUa_Bexley +
                "&parent_area_type_ids=" + AreaTypeIds.GoRegion);
            IsData(data);
        }

        [TestMethod]
        public void TestGetAreaAddress()
        {
            byte[] data = GetData("area_address?" +
                "area_code=" + AreaCodes.Gp_AdamHouseSandiacre);
            IsData(data);
        }

        [TestMethod]
        public void TestNearbyAreas()
        {
            string easting = "352107", northing = "529262";
            int areaTypeId = 7;

            byte[] data = GetData("area_search_by_proximity?" +
                "easting=" + easting +
                "&northing=" + northing +
                "&area_type_id=" + areaTypeId);
            IsData(data);
        }

        [TestMethod]
        public void TestGetChildAreas()
        {
            byte[] data = GetData("areas/by_parent_area_code?" +
                "parent_area_code=" + AreaCodes.Gor_EastMidlands +
                "&area_type_id=" + AreaTypeIds.CountyAndUnitaryAuthorityPreApr2019);
            IsData(data);
        }

        [TestMethod]
        public void TestGetChildAreas_When_Parent_Is_Category()
        {
            byte[] data = GetData("areas/by_parent_area_code?" +
                "area_type_id=" + AreaTypeIds.CountyAndUnitaryAuthorityPreApr2019 +
                "&parent_area_code=" + "cat-2-7" +
                "&profile_id=" + ProfileIds.Phof);

            IsData(data);
        }

        [TestMethod]
        public void TestGetAreas()
        {
            byte[] data = GetData("areas/by_area_code?" +
                "area_codes=" + AreaCodes.Gor_EastMidlands);
            IsData(data);
        }

        [TestMethod]
        public void TestGetChildAreasWithAddresses()
        {
            byte[] data = GetData("area_addresses/by_parent_area_code?" +
                "parent_area_code=" + AreaCodes.Ccg_AireDaleWharfdaleAndCraven +
                "&area_type_id=" + AreaTypeIds.GpPractice);
            IsData(data);
        }

        [TestMethod]
        public void TestGetChildAreasWithAddressesAsCsv()
        {
            byte[] data = GetData("area_addresses/csv/by_parent_area_code?" +
                "parent_area_code=" + AreaCodes.Ccg_AireDaleWharfdaleAndCraven +
                "&area_type_id=" + AreaTypeIds.GpPractice);
            IsData(data);
        }

        [TestMethod]
        public void TestGetParentAreaOfSpecificTypeForChildAreas()
        {
            byte[] data = GetData("parent_area_of_specific_type_for_child_areas?" +
                "parent_area_code=" + AreaCodes.Ccg_Chiltern +
                "&child_area_type_id=" + AreaTypeIds.GpPractice +
                "&parent_area_type_id=" + AreaTypeIds.Shape);
            IsData(data);
        }

        [TestMethod]
        public void TestGetAreasOfAreaType()
        {
            byte[] data = GetData("areas/by_area_type?" +
                "area_type_id=" + AreaTypeIds.CountyAndUnitaryAuthorityPreApr2019 +
                "&profile_id=" + ProfileIds.Phof);

            IsData(data);
        }

        public static void IsData(byte[] data)
        {
            TestHelper.IsData(data);

            // Test not empty array
            var s = TestHelper.GetDataString(data);
            Assert.AreNotEqual("[]", s);
        }

        public byte[] GetData(string path)
        {
            return DataControllerEndPointTest.GetData(path);
        }
    }
}
