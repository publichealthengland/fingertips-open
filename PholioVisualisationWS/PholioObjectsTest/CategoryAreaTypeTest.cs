﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PholioVisualisation.PholioObjects;

namespace PholioVisualisation.PholioObjectsTest
{
    [TestClass]
    public class CategoryAreaTypeTest
    {
        public const string ShortName = "b";

        [TestMethod]
        public void TestNew_SpoofAreaTypeIdDefined()
        {
            Assert.AreEqual(10005, CategoryAreaType.New(CategoryType()).Id);
        }

        [TestMethod]
        public void TestNew_NamesAreDefinedFromShortNameOfCategoryType()
        {
           var type = CategoryAreaType.New(CategoryType());
           Assert.AreEqual(ShortName, type.Name);
           Assert.AreEqual(ShortName, type.ShortName);
        }

        [TestMethod]
        public void TestCategoryAreaTypeImplementsIAreaType()
        {
            Assert.IsNotNull(CategoryAreaType.New(CategoryType()) as IAreaType);
        }

        [TestMethod]
        public void TestGetCategoryTypeIdFromAreaTypeId()
        {
            Assert.AreEqual(5, CategoryAreaType.GetCategoryTypeIdFromAreaTypeId(10005));
        }

        [TestMethod]
        public void TestGetAreaTypeIdFromCategoryTypeId()
        {
            Assert.AreEqual(10005, CategoryAreaType.GetAreaTypeIdFromCategoryTypeId(5));
        }

        [TestMethod]
        public void TestIsCategoryAreaTypeIdFalseForStandardAreaType()
        {
            Assert.IsFalse(CategoryAreaType.IsCategoryAreaTypeId(AreaTypeIds.CcgsPostApr2019));
        }

        [TestMethod]
        public void TestIsCategoryAreaTypeIdTrueForCategoryAreaType()
        {
            var areaTypeId = CategoryAreaType.GetAreaTypeIdFromCategoryTypeId(
                CategoryTypeIds.DeprivationDecileCountyAndUA2010);
            Assert.IsTrue(CategoryAreaType.IsCategoryAreaTypeId(areaTypeId));
        }

        private CategoryType CategoryType()
        {
            return new CategoryType {
                Id = 5,
                Name = "a",
                ShortName = ShortName,
                Description = "Sample description",
                Notes = "Sample notes"
            };
        }
    }
}
