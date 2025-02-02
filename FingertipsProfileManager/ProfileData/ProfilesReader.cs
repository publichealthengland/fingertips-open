﻿using Fpm.ProfileData.Entities.Core;
using Fpm.ProfileData.Entities.LookUps;
using Fpm.ProfileData.Entities.Profile;
using Fpm.ProfileData.Entities.User;
using Fpm.ProfileData.Repositories;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fpm.ProfileData
{
    public class ProfilesReader : BaseReader, IProfilesReader
    {
        public const int MaxAreaResults = 1000;

        public ProfilesReader(ISessionFactory sessionFactory)
            : base(sessionFactory)
        { }

        public IList<ProfileDetails> GetProfiles()
        {
            IQuery q = CurrentSession.CreateQuery("from ProfileDetails p where p.Id != " + ProfileIds.Search);
            return q.List<ProfileDetails>();
        }

        public IEnumerable<ProfileCollection> GetProfileCollections()
        {
            IQuery q = CurrentSession.CreateQuery("from ProfileCollection pc");
            return q.List<ProfileCollection>();
        }

        public ProfileCollection GetProfileCollection(int collectionId)
        {
            IQuery q = CurrentSession.CreateQuery("from ProfileCollection pc where pc.id = :collectionId");
            q.SetParameter("collectionId", collectionId);
            return q.UniqueResult<ProfileCollection>();
        }

        public int GetProfileIdFromUrlKey(string urlKey)
        {
            IQuery q = CurrentSession.CreateQuery("select p.Id from ProfileDetails p where p.UrlKey = :urlKey");
            q.SetParameter("urlKey", urlKey);
            return q.UniqueResult<int>();
        }

        public string GetProfileUrlKeyFromId(int profileId)
        {
            IQuery q = CurrentSession.CreateQuery("select p.UrlKey from ProfileDetails p where p.Id = :id");
            q.SetParameter("id", profileId);
            return q.UniqueResult<string>();
        }

        public ProfileDetails GetProfileDetails(string urlKey)
        {
            IQuery q = CurrentSession.CreateQuery("from ProfileDetails p where p.UrlKey = :urlKey");
            q.SetParameter("urlKey", urlKey);
            return q.UniqueResult<ProfileDetails>();
        }

        public ProfileDetails GetProfileDetailsByProfileId(int profileId)
        {
            return CurrentSession.CreateCriteria<ProfileDetails>()
                .Add(Restrictions.Eq("Id", profileId))
                .UniqueResult<ProfileDetails>();
        }

        public ProfileDetails GetOwnerProfilesByIndicatorIds(int indicatorId)
        {
            IQuery q = CurrentSession.CreateQuery(
                 "select p from ProfileDetails p, IndicatorMetadata i where p.Id = i.OwnerProfileId and i.IndicatorId = :indicatorId");
            q.SetParameter("indicatorId", indicatorId);
            return q.UniqueResult<ProfileDetails>();
        }

        public IList<UserGroupPermissions> GetUserGroupPermissionsByUserId(int userId)
        {
            IQuery q = CurrentSession.CreateQuery("from UserGroupPermissions up where up.UserId = :userId");
            q.SetParameter("userId", userId);
            return q.List<UserGroupPermissions>();
        }

        public IList<UserGroupPermissions> GetUserGroupPermissionsByProfileId(int profileId)
        {
            IQuery q = CurrentSession.CreateQuery("from UserGroupPermissions ugp where ugp.ProfileId = :profileId");
            q.SetParameter("profileId", profileId);
            return q.List<UserGroupPermissions>();
        }

        public IList<ProfileDetails> GetProfilesEditableByUser(int userId)
        {
            IQuery q = CurrentSession.CreateQuery("select distinct p from ProfileDetails p, UserGroupPermissions ugp where p.Id = ugp.ProfileId and ugp.UserId = :userId and p.Id != :profileId");
            q.SetParameter("userId", userId);
            q.SetParameter("profileId", ProfileIds.Search);
            return q.List<ProfileDetails>();
        }

        public IList<Grouping> GetGroupings(int groupId)
        {
            IQuery q = CurrentSession.CreateQuery("from Grouping g where g.GroupId = :groupId order by g.Sequence");
            q.SetParameter("groupId", groupId);
            return q.List<Grouping>();
        }

        public IList<int> GetGroupingIndicatorIds(IList<int> groupIds)
        {
            return CurrentSession.CreateCriteria<Grouping>()
                .Add(Restrictions.In("GroupId", groupIds.ToList()))
                .SetProjection(Projections.Property("IndicatorId"))
                .List<int>();
        }

        public IList<int> GetGroupingIds(int profileId)
        {
            IQuery q = CurrentSession.CreateQuery("select g.id from GroupingMetadata g where g.ProfileId = :profileId");
            q.SetParameter("profileId", profileId);
            return q.List<int>();
        }

        public IList<Grouping> GetGroupingsByGroupIdAndAreaTypeId(int groupId, int areaTypeId)
        {
            return CurrentSession.CreateCriteria<Grouping>()
                .Add(Restrictions.Eq("GroupId", groupId))
                .Add(Restrictions.Eq("AreaTypeId", areaTypeId))
                .AddOrder(Order.Asc("Sequence"))
                .List<Grouping>();
        }

        public IList<Grouping> GetGroupingsByGroupIdAreaTypeIdIndicatorIdAndSexIdAndAgeId(int groupId, int areaTypeId,
            int indicatorId, int sexId, int ageId)
        {
            return CurrentSession.CreateCriteria<Grouping>()
                .Add(Restrictions.Eq("GroupId", groupId))
                .Add(Restrictions.Eq("AreaTypeId", areaTypeId))
                .Add(Restrictions.Eq("IndicatorId", indicatorId))
                .Add(Restrictions.Eq("SexId", sexId))
                .Add(Restrictions.Eq("AgeId", ageId))
                .List<Grouping>();
        }

        public IList<Grouping> GetGroupings(IList<int> groupIds, int areaTypeId,
            int indicatorId, int sexId, int ageId, int yearRange)
        {
            return CurrentSession.CreateCriteria<Grouping>()
                .Add(Restrictions.In("GroupId", groupIds.ToList()))
                .Add(Restrictions.Eq("AreaTypeId", areaTypeId))
                .Add(Restrictions.Eq("IndicatorId", indicatorId))
                .Add(Restrictions.Eq("SexId", sexId))
                .Add(Restrictions.Eq("AgeId", ageId))
                .Add(Restrictions.Eq("YearRange", yearRange))
                .List<Grouping>();
        }

        public IList<GroupingMetadata> GetGroupingMetadataList(IList<int> groupIds)
        {
            if (groupIds.Any() == false)
            {
                return new List<GroupingMetadata>();
            }

            IQuery q = CurrentSession.CreateQuery("from GroupingMetadata g where g.GroupId in (:groupIds) order by g.Sequence");
            q.SetParameterList("groupIds", groupIds);
            return q.List<GroupingMetadata>();
        }

        public IList<int> GetGroupingMetadataGroupIdListByProfileId(int profileId)
        {
            return CurrentSession.CreateCriteria<GroupingMetadata>()
                .Add(Restrictions.Eq("ProfileId", profileId))
                .SetProjection(Projections.Property("GroupId"))
                .List<int>();
        }

        public GroupingMetadata GetGroupingMetadata(int groupId)
        {
            return GetGroupingMetadataList(new List<int> { groupId }).ToList().FirstOrDefault();
        }

        public IList<Grouping> GetGroupingByIndicatorId(List<int> indicatorIds)
        {
            CurrentSession.Clear();

            IQuery q = CurrentSession.CreateQuery("from Grouping g where g.IndicatorId in (:indicatorIds) order by g.GroupId");
            q.SetParameterList("indicatorIds", indicatorIds);
            q.SetCacheable(false);
            return q.List<Grouping>();
        }

        public IList<Grouping> GetGroupingByIndicatorIdAndSexIdAndAgeId(int indicatorId, int sexId, int ageId)
        {
            CurrentSession.Clear();

            IQuery q = CurrentSession.CreateQuery("from Grouping g where g.IndicatorId = :indicatorId and g.SexId = :sexId and g.AgeId = :ageId order by g.GroupId");
            q.SetParameter("indicatorId", indicatorId);
            q.SetParameter("sexId", sexId);
            q.SetParameter("ageId", ageId);
            q.SetCacheable(false);
            return q.List<Grouping>();
        }

        public IList<IndicatorMetadataTextProperty> GetIndicatorMetadataTextProperties()
        {
            IQuery q = CurrentSession.CreateQuery("from IndicatorMetadataTextProperty i");
            return q.List<IndicatorMetadataTextProperty>().OrderBy(property => property.PropertyId).ToList();
        }

        public IList<IndicatorMetadataTextValue> SearchIndicatorMetadataTextValuesByText(string searchText)
        {
            return CurrentSession.CreateCriteria<IndicatorMetadataTextValue>()
                .Add(Restrictions.Like("Name", searchText))
                .Add(Restrictions.IsNull("ProfileId"))
             .List<IndicatorMetadataTextValue>();
        }

        public IList<IndicatorMetadataTextValue> SearchIndicatorMetadataTextValuesByIndicatorId(int indicatorId)
        {
            return CurrentSession.CreateCriteria<IndicatorMetadataTextValue>()
                .Add(Restrictions.Like("IndicatorId", indicatorId))
                .Add(Restrictions.IsNull("ProfileId"))
             .List<IndicatorMetadataTextValue>();
        }

        public IList<IndicatorMetadataTextValue> GetIndicatorMetadataTextValuesByProfileId(int profileId)
        {
            return CurrentSession.CreateCriteria<IndicatorMetadataTextValue>()
                .Add(Restrictions.Eq("ProfileId", profileId))
                .List<IndicatorMetadataTextValue>();
        }

        public IList<IndicatorMetadataTextValue> GetIndicatorMetadataTextValuesByIndicatorIdsAndProfileId(
            IList<int> indicatorIds, int profileId)
        {
            return CurrentSession.CreateCriteria<IndicatorMetadataTextValue>()
                .Add(Restrictions.In("IndicatorId", indicatorIds.ToList()))
                .Add(Restrictions.Disjunction()
                    .Add(Restrictions.Eq("ProfileId", profileId))
                    .Add(Restrictions.Eq("ProfileId", null))
                )
                .List<IndicatorMetadataTextValue>();
        }

        public IList GetIndicatorDefaultTextValues(int indicatorId)
        {
            IQuery q = CurrentSession.GetNamedQuery("GetIndicatorDefaultMetadataTextValues");
            q.SetParameter("indicatorId", indicatorId);
            IList results = q.List();
            return results;
        }

        public IList<IndicatorText> GetIndicatorTextValues(int indicatorId, IList<IndicatorMetadataTextProperty> properties, int? profileId)
        {
            // Integer parameter required
            if (profileId.HasValue == false)
            {
                profileId = ProfileIds.Undefined;
            }

            IQuery q = CurrentSession.GetNamedQuery("GetIndicatorMetadataTextValues");
            q.SetParameter("indicatorId", indicatorId);
            q.SetParameter("profileId", profileId);
            IList results = q.List();

            if (results.Count == 0)
            {
                return new List<IndicatorText>();
            }

            return HydrateIndicatorTextList(properties, results);
        }

        public IList<IndicatorText> GetOverriddenIndicatorTextValuesForSpecificProfileId(int indicatorId, IList<IndicatorMetadataTextProperty> properties, int? profileId)
        {
            IQuery q = CurrentSession.GetNamedQuery("GetOverriddenIndicatorMetadataTextValuesForProfileId");
            q.SetParameter("indicatorId", indicatorId);
            q.SetParameter("profileId", profileId);
            IList results = q.List();

            if (results.Count == 0)
            {
                return new List<IndicatorText>();
            }

            return HydrateIndicatorTextList(properties, results);
        }

        public IndicatorMetadata GetIndicatorMetadata(int indicatorId)
        {
            IQuery q = CurrentSession.CreateQuery("from IndicatorMetadata i where i.IndicatorId = :indicatorId");
            q.SetParameter("indicatorId", indicatorId);
            IndicatorMetadata metadata = q.UniqueResult<IndicatorMetadata>();
            return metadata;
        }

        public IList<IndicatorMetadata> GetIndicatorMetadata(List<int> indicatorIds)
        {
            return CurrentSession.CreateCriteria<IndicatorMetadata>()
                .Add(Restrictions.In("IndicatorId", indicatorIds))
                .List<IndicatorMetadata>();
        }

        public IEnumerable<Grouping> DoesIndicatorExistInMoreThanOneGroup(int indicatorId, int ageId, int sexId)
        {
            IQuery q = CurrentSession.CreateQuery("from Grouping g where g.IndicatorId = :indicatorId and g.SexId = :sexId and g.AgeId = :ageId");
            q.SetParameter("indicatorId", indicatorId);
            q.SetParameter("ageId", ageId);
            q.SetParameter("sexId", sexId);
            return q.List<Grouping>();
        }

        public virtual IList<double> GetAllComparatorConfidences()
        {
            return CurrentSession.CreateCriteria<ComparatorConfidence>()
                .Add(Restrictions.Not(Restrictions.Eq("ConfidenceValue", 98D)))
                .SetProjection(Projections.Distinct(Projections.Property("ConfidenceValue")))
                .List<double>();
        }

        public virtual IList<int> GetAllAgeIds()
        {
            IQuery q = CurrentSession.CreateQuery("select a.AgeID from Age a");
            return q.List<int>();
        }

        public virtual IList<Age> GetAllAges()
        {
            return CurrentSession.CreateCriteria<Age>()
                .List<Age>();
        }

        public virtual IList<int> GetAllSexIds()
        {
            IQuery q = CurrentSession.CreateQuery("select s.SexID from Sex s");
            return q.List<int>();
        }

        public virtual IList<Sex> GetAllSexes()
        {
            return CurrentSession.CreateCriteria<Sex>()
                .List<Sex>();
        }

        public virtual IList<int> GetAllValueNoteIds()
        {
            IQuery q = CurrentSession.CreateQuery("select vn.ValueNoteId from ValueNote vn");
            return q.List<int>();
        }

        public virtual IList<ValueNote> GetAllValueNotes()
        {
            return CurrentSession.CreateCriteria<ValueNote>()
                .List<ValueNote>();
        }

        public virtual IList<string> GetAllAreaCodes()
        {
            IQuery q = CurrentSession.CreateQuery("select a.AreaCode from Area a ");
            return q.List<string>();
        }

        public virtual IList<DisclosureControl> GetAllDisclosureControl()
        {
            return CurrentSession.CreateCriteria<DisclosureControl>()
                .List<DisclosureControl>();
        }

        public virtual IList<TargetConfig> GetAllTargets()
        {
            return CurrentSession.CreateCriteria<TargetConfig>()
                .List<TargetConfig>();
        }

        public virtual TargetConfig GetTargetById(int targetId)
        {
            return CurrentSession.CreateCriteria<TargetConfig>()
                .Add(Restrictions.Eq("Id", targetId))
                .UniqueResult<TargetConfig>();

        }

        public virtual CategoryType GetCategoryType(int categoryTypeId)
        {
            return CurrentSession.CreateCriteria <CategoryType>()
                .Add(Restrictions.Eq("Id", categoryTypeId))
                .UniqueResult<CategoryType>();
        }

        public virtual IList<Category> GetCategoriesByCategoryTypeId(int categoryTypeId)
        {
            return CurrentSession.CreateCriteria<Category>()
                .Add(Restrictions.Eq("CategoryTypeId", categoryTypeId))
                .List<Category>();
        }

        public virtual IList<Category> GetAllCategories()
        {
            return CurrentSession.CreateCriteria<Category>()
                .List<Category>();
        }

        public IEnumerable<Area> GetAreas(string searchText, int? areaTypeId)
        {
            searchText = "%" + searchText + "%";

            var criteria = CurrentSession.CreateCriteria<Area>()
                .Add(Restrictions.And(
                    Restrictions.Eq("IsCurrent", true),
                    Restrictions.Or(
                        Restrictions.Like("AreaCode", searchText),
                        Restrictions.Like("AreaName", searchText))
                    ));

            if (areaTypeId.HasValue)
            {
                criteria.Add(Restrictions.In("AreaTypeId", GetComponentAreaTypeIds(areaTypeId.Value).ToArray()));
            }

            var areas = criteria.AddOrder(Order.Asc("AreaTypeId"))
                .AddOrder(Order.Asc("AreaCode"))
                .SetMaxResults(MaxAreaResults)
                .List<Area>();

            // Remove areas with non current area types
            var areaTypeIds = GetNonCurrentAreaTypes().Select(x => x.Id);
            return areas.Where(x => areaTypeIds.Contains(x.AreaTypeId) == false);
        }

        public IList<ComparatorMethod> GetAllComparatorMethods()
        {
            return CurrentSession.CreateCriteria<ComparatorMethod>()
                .Add(Restrictions.Eq("IsCurrent", true))
                .AddOrder(Order.Asc("Name"))
                .List<ComparatorMethod>();
        }

        public IList<Polarity> GetAllPolarities()
        {
            return CurrentSession.CreateCriteria<Polarity>()
                .AddOrder(Order.Asc("Name"))
                .List<Polarity>();
        }

        public IList<AreaType> GetAllAreaTypes()
        {
            return CurrentSession.CreateCriteria<AreaType>()
                .Add(Restrictions.Eq("IsCurrent", true))
                .AddOrder(Order.Asc("ShortName"))
                .List<AreaType>();
        }

        public IList<AreaType> GetSpecificAreaTypes(List<int> areaTypeIds)
        {
            return CurrentSession.CreateCriteria<AreaType>()
                .Add(Restrictions.In("Id", areaTypeIds))
                .AddOrder(Order.Asc("ShortName"))
                .List<AreaType>();
        }

        public IList<AreaType> GetSupportedAreaTypes()
        {
            return CurrentSession.CreateCriteria<AreaType>()
                .Add(Restrictions.Eq("IsCurrent", true))
                .Add(Restrictions.Eq("IsSupported", true))
                .AddOrder(Order.Asc("ShortName"))
                .List<AreaType>();
        }

        public IEnumerable<IndicatorAudit> GetIndicatorAudit(List<int> indicatorIds)
        {
            IQuery q = CurrentSession.CreateQuery("from IndicatorAudit ia where ia.IndicatorId in (:indicatorIds) order by ia.Timestamp desc");
            q.SetParameterList("indicatorIds", indicatorIds);
            return q.List<IndicatorAudit>();
        }

        public IEnumerable<ContentAudit> GetContentAuditList(List<int> contentIds)
        {
            IQuery q = CurrentSession.CreateQuery("from ContentAudit ca where ca.ContentId in (:contentIds) order by ca.Timestamp desc");
            q.SetParameterList("contentIds", contentIds);
            return q.List<ContentAudit>();
        }

        public ContentAudit GetContentAuditFromId(int contentAuditId)
        {
            return CurrentSession.CreateCriteria<ContentAudit>()
                .Add(Restrictions.Eq("Id", contentAuditId))
                .UniqueResult<ContentAudit>();
        }

        public IList<CoreDataSet> GetCoreDataForIndicatorIds(List<int> indicatorId)
        {
            IQuery q = CurrentSession.CreateQuery("from CoreDataSet cds where cds.IndicatorId in (:indicatorId)");
            q.SetParameterList("indicatorId", indicatorId);
            return q.List<CoreDataSet>();
        }

        public IList<CoreDataSetArchive> GetCoreDataArchiveForIndicatorIds(List<int> indicatorId)
        {
            IQuery q = CurrentSession.CreateQuery("from CoreDataSetArchive cdsa where cdsa.IndicatorId in (:indicatorId)");
            q.SetParameterList("indicatorId", indicatorId);
            return q.List<CoreDataSetArchive>();
        }

        public IList<TimePeriod> GetCoreDataSetTimePeriods(GroupingPlusName groupingPlusNames)
        {
            const string query = "select distinct d.Year, d.Quarter, d.Month from CoreDataSet d, Area a" +
              " where d.AreaCode = a.AreaCode and d.IndicatorId = :indicatorId and d.YearRange = :yearRange" +
              " and d.SexId = :sexId and d.AgeId = :ageId and a.AreaTypeId in (:areaTypeIds)" +
              " order by d.Year, d.Quarter, d.Month";

            IQuery q = CurrentSession.CreateQuery(query);
            q.SetParameter("indicatorId", groupingPlusNames.IndicatorId);
            q.SetParameter("sexId", groupingPlusNames.SexId);
            q.SetParameter("ageId", groupingPlusNames.AgeId);
            q.SetParameter("yearRange", groupingPlusNames.YearRange);
            q.SetParameterList("areaTypeIds", GetComponentAreaTypeIds(groupingPlusNames.AreaTypeId));
            var rows = q.List<object[]>();

            List<TimePeriod> periods = new List<TimePeriod>();

            if (rows.LongCount() > 0 && rows[0] != null)
            {
                foreach (object[] row in rows)
                {
                    periods.Add(new TimePeriod
                    {
                        YearRange = groupingPlusNames.YearRange,
                        Year = (int)row[0],
                        Quarter = (int)row[1],
                        Month = (int)row[2]
                    });
                }
            }
            return periods;
        }

        public IList<ContentItem> GetProfileContentItems(int profileId)
        {
            IQuery q = CurrentSession.CreateQuery("from ContentItem fc where fc.ProfileId = :profileid");
            q.SetParameter("profileid", profileId);
            return q.List<ContentItem>();
        }

        public ContentItem GetContentItem(int contentId)
        {
            IQuery q = CurrentSession.CreateQuery("from ContentItem fc where fc.Id = :contentId");
            q.SetParameter("contentId", contentId);
            ContentItem contentItem = q.UniqueResult<ContentItem>();
            return contentItem;
        }

        public IList<ContentItem> GetContentItems(IList<int> contentIds)
        {
            IQuery q = CurrentSession.CreateQuery("from ContentItem fc where fc.Id in (:contentIds)");
            q.SetParameterList("contentIds", contentIds);
            return q.List<ContentItem>();
        }

        public ContentItem GetContentItem(string contentKey, int profileId)
        {
            //Always get the latest contentItem
            CurrentSession.Clear();

            return CurrentSession.CreateCriteria<ContentItem>()
                .Add(Restrictions.Eq("ContentKey", contentKey))
                .Add(Restrictions.Eq("ProfileId", profileId))
                .UniqueResult<ContentItem>();
        }

        public IEnumerable<UserGroupPermissions> GetUserPermissions()
        {
            IQuery q = CurrentSession.CreateQuery("from UserGroupPermissions ugp");
            return q.List<UserGroupPermissions>();
        }

        public FpmUser GetUserByUserName(string userName)
        {
            IQuery q = CurrentSession.CreateQuery("from FpmUser fu where fu.UserName = :userName");
            q.SetParameter("userName", userName);
            FpmUser user = q.UniqueResult<FpmUser>();
            return user;
        }

        public FpmUser GetUserByUserId(int userId)
        {
            IQuery q = CurrentSession.CreateQuery("from FpmUser fu where fu.Id = :userId");
            q.SetParameter("userId", userId);
            FpmUser user = q.UniqueResult<FpmUser>();
            return user;
        }

        public IList<FpmUser> GetUserListByUserIds(List<string> userIds)
        {
            IQuery q = CurrentSession.CreateQuery("from FpmUser fu where fu.Id in (:userIds) order by DisplayName");
            q.SetParameterList("userIds", userIds);
            return q.List<FpmUser>();
        }

        public IList<UserGroupPermissions> GetUserGroupPermissionsByUserAndProfile(int userId, int profileId)
        {
            IQuery q = CurrentSession.CreateQuery("from UserGroupPermissions ugp where ugp.ProfileId = :profileid and ugp.UserId = :userid");
            q.SetParameter("profileid", profileId);
            q.SetParameter("userid", userId);
            return q.List<UserGroupPermissions>();
        }

        public void ClearCache()
        {
            CurrentSession.Clear();
        }

        public IList<ProfileCollectionItem> GetProfileCollectionItems(int profileCollectionId)
        {
            IQuery q = CurrentSession.CreateQuery("from ProfileCollectionItem pci where pci.ProfileCollectionId = :profilecollectionid");
            q.SetParameter("profilecollectionid", profileCollectionId);
            return q.List<ProfileCollectionItem>();
        }

        public IndicatorMetadataTextValue GetMetadataTextValueForAnIndicatorById(int indicatorId, int profileId)
        {
            return CurrentSession.CreateCriteria<IndicatorMetadataTextValue>()
                .Add(Restrictions.Eq("IndicatorId", indicatorId))
                .Add(Restrictions.Eq("ProfileId", profileId))
                .UniqueResult<IndicatorMetadataTextValue>();
        }

        public IList<Document> GetDocumentsWithoutFileData(int profileId)
        {
            ProjectionList projectionList = GetDocumentProjectionListWithoutFileData();

            var documents = CurrentSession.QueryOver<Document>()
            .Where(x => x.ProfileId == profileId)
            .Select(projectionList)
            .TransformUsing(Transformers.AliasToBean<Document>())
            .List<Document>();

            return documents.OrderByDescending(x => x.UploadedOn).ToList();
        }

        public Document GetDocumentWithoutFileData(int id)
        {
            ProjectionList projectionList = GetDocumentProjectionListWithoutFileData();

            var doc = CurrentSession.QueryOver<Document>()
            .Where(d => d.Id == id)
            .Select(projectionList)
            .TransformUsing(Transformers.AliasToBean<Document>())
            .SingleOrDefault<Document>();

            return doc;
        }

        public Document GetDocumentWithFileData(int id)
        {
            ProjectionList projectionList = GetDocumentProjectionListWithFileData();

            var doc = CurrentSession.QueryOver<Document>()
                .Where(d => d.Id == id)
                .Select(projectionList)
                .TransformUsing(Transformers.AliasToBean<Document>())
                .SingleOrDefault<Document>();

            return doc;
        }

        public IList<Document> GetDocumentsWithoutFileData(string fileName)
        {
            ProjectionList projectionList = GetDocumentProjectionListWithoutFileData();

            var documents = CurrentSession.QueryOver<Document>()
                .Where(x => x.FileName == fileName)
                .Select(projectionList)
                .TransformUsing(Transformers.AliasToBean<Document>())
                .List<Document>();

            return documents;
        }

        public IEnumerable<SpineChartMinMaxLabel> GetSpineChartMinMaxLabelOptions()
        {
            return CurrentSession.CreateCriteria<SpineChartMinMaxLabel>()
                .List<SpineChartMinMaxLabel>();
        }

        public IList<AreaType> GetAreaTypes(int profileId)
        {
            var query = CurrentSession.GetNamedQuery("GetAreaTypesForProfileId")
                .SetParameter("profileId", profileId)
                .SetResultTransformer(Transformers.AliasToBean(typeof(AreaType)));
            return query.List<AreaType>();
        }

        public IList<AreaType> GetAreaTypesWhichContainsPdf(int profileId)
        {
            var query = CurrentSession.GetNamedQuery("GetPDFEnabledAreaTypesForProfileId")
                .SetParameter("profileId", profileId)
                .SetResultTransformer(Transformers.AliasToBean(typeof(AreaType)));
            return query.List<AreaType>();
        }

        public IList<UserGroupPermissions> GetProfileUsers(int profileId)
        {
            IQuery q = CurrentSession.CreateQuery("from UserGroupPermissions ugp where ugp.ProfileId = :profileid");
            q.SetParameter("profileid", profileId);
            return q.List<UserGroupPermissions>();
        }

        public IList<IndicatorMetadataReviewAudit> GetIndicatorMetadataReviewAudits(int indicatorId)
        {
            IQuery q = CurrentSession.CreateQuery("from IndicatorMetadataReviewAudit i where i.IndicatorId = :indicatorid");
            q.SetParameter("indicatorid", indicatorId);
            return q.List<IndicatorMetadataReviewAudit>().OrderByDescending(property => property.Timestamp).ToList();
        }

        private static List<IndicatorText> HydrateIndicatorTextList(IList<IndicatorMetadataTextProperty> properties, IList results)
        {
            IList<object> genericResults = (IList<object>)results[0];
            IList<object> specificResults = results.Count > 1
                ? (IList<object>)results[1] // first overwritten metadata
                : null;

            // IndicatorID and GroupID
            var valueIndex = 2;

            List<IndicatorText> indicatorTextList = new List<IndicatorText>();
            for (int i = 0; i < properties.Count; i++)
            {
                // Skip ID
                while (genericResults[valueIndex] is int)
                {
                    valueIndex++;
                }

                var property = properties[i];
                IndicatorText indicatorText = new IndicatorText { IndicatorMetadataTextProperty = property };

                indicatorText.ValueGeneric = (string)genericResults[valueIndex];

                if (specificResults != null)
                {
                    indicatorText.ValueSpecific = (string)specificResults[valueIndex];
                }

                indicatorTextList.Add(indicatorText);

                valueIndex++;
            }
            return indicatorTextList;
        }

        private IList<AreaType> GetNonCurrentAreaTypes()
        {
            return CurrentSession.CreateCriteria<AreaType>()
                .Add(Restrictions.Eq("IsCurrent", false))
                .List<AreaType>();
        }

        private ProjectionList GetDocumentProjectionListWithoutFileData()
        {
            Document documentTypeCast = null;

            return Projections.ProjectionList()
                .Add(Projections.Property<Document>(x => x.Id).WithAlias(() => documentTypeCast.Id))
                .Add(Projections.Property<Document>(x => x.ProfileId).WithAlias(() => documentTypeCast.ProfileId))
                .Add(Projections.Property<Document>(x => x.FileName).WithAlias(() => documentTypeCast.FileName))
                .Add(Projections.Property<Document>(x => x.UploadedOn).WithAlias(() => documentTypeCast.UploadedOn))
                .Add(Projections.Property<Document>(x => x.UploadedBy).WithAlias(() => documentTypeCast.UploadedBy));
        }

        private ProjectionList GetDocumentProjectionListWithFileData()
        {
            Document documentTypeCast = null;

            return Projections.ProjectionList()
                .Add(Projections.Property<Document>(x => x.Id).WithAlias(() => documentTypeCast.Id))
                .Add(Projections.Property<Document>(x => x.ProfileId).WithAlias(() => documentTypeCast.ProfileId))
                .Add(Projections.Property<Document>(x => x.FileName).WithAlias(() => documentTypeCast.FileName))
                .Add(Projections.Property<Document>(x => x.FileData).WithAlias(() => documentTypeCast.FileData))
                .Add(Projections.Property<Document>(x => x.UploadedOn).WithAlias(() => documentTypeCast.UploadedOn))
                .Add(Projections.Property<Document>(x => x.UploadedBy).WithAlias(() => documentTypeCast.UploadedBy));
        }

        private IList<int> GetComponentAreaTypeIds(int areaTypeId)
        {
            return new AreaTypeIdSplitter(new AreaTypeRepository())
                .GetComponentAreaTypeIds(areaTypeId);
        }
    }
}
