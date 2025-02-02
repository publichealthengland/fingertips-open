﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PholioVisualisation.PholioObjects;

namespace PholioVisualisation.ServicesTest
{
    [TestClass]
    public class IndicatorMetadataControllerEndPointTest
    {
        [TestMethod]
        public void TestGetIndicatorMetadata_All_Indicators()
        {
            var url = "indicator_metadata/all";
            byte[] data = GetData(url);

            TestHelper.IsData(data);
        }

        [TestMethod]
        public void TestGetIndicatorMetadata_For_Group()
        {
            var url = "indicator_metadata/by_group_id?" +
                      "group_ids=" + GroupIds.Phof_HealthProtection;
            byte[] data = GetData(url);

            TestHelper.IsData(data);
        }

        [TestMethod]
        public void TestGetIndicatorMetadata_For_Indicator_Ids()
        {
            var url = "indicator_metadata/by_indicator_id?" +
                      "indicator_ids=" + IndicatorIds.AdultSmokingRelatedDeaths +
                      "&restrict_to_profile_ids=" + ProfileIds.Tobacco;
            byte[] data = GetData(url);

            TestHelper.IsData(data);
        }

        [TestMethod]
        public void TestGetIndicatorMetadataTextProperties()
        {
            byte[] data = GetData("indicator_metadata_text_properties");
            TestHelper.IsData(data);
        }

        [TestMethod]
        public void TestGetIndicatorMetadataCsvForIndicatorList()
        {
            var url = "indicator_metadata/csv/by_indicator_id?" +
                      "indicator_ids=" + IndicatorIds.ChildrenInLowIncomeFamilies;
            byte[] data = TestHelper.GetData(url);

            TestHelper.IsData(data);
        }

        [TestMethod]
        public void TestGetIndicatorMetadataCsvForProfile()
        {
            var url = "indicator_metadata/csv/by_profile_id?" +
                      "profile_id=" + ProfileIds.HealthProfiles;
            byte[] data = TestHelper.GetData(url);

            TestHelper.IsData(data);
        }

        [TestMethod]
        public void TestGetIndicatorMetadataCsvForAllIndicators()
        {
            var url = "indicator_metadata/csv/all";
            byte[] data = TestHelper.GetData(url);

            TestHelper.IsData(data);
        }

        [TestMethod]
        public void TestGetIndicatorNamesByGroupId()
        {
            var url = "indicator_names/by_group_id?" +
                      "group_ids=" + GroupIds.Phof_WiderDeterminantsOfHealth;
            byte[] data = TestHelper.GetData(url);

            TestHelper.IsData(data);
        }

        public static byte[] GetData(string path)
        {
            return DataControllerEndPointTest.GetData(path);
        }
    }
}
