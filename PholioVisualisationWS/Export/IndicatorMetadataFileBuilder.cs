﻿using System.Collections.Generic;
using System.Linq;
using PholioVisualisation.Analysis;
using PholioVisualisation.DataConstruction;
using PholioVisualisation.DataSorting;
using PholioVisualisation.PholioObjects;

namespace PholioVisualisation.Export
{
    public class IndicatorMetadataFileBuilder
    {
        private IndicatorMetadataProvider _indicatorMetadataProvider;
        private GroupingListProvider _groupingListProvider;
        private IList<Polarity> _polarities;

        public IndicatorMetadataFileBuilder(IndicatorMetadataProvider indicatorMetadataProvider,
            GroupingListProvider groupingListProvider, IList<Polarity> polarities)
        {
            _indicatorMetadataProvider = indicatorMetadataProvider;
            _groupingListProvider = groupingListProvider;
            _polarities = polarities;
        }

        public byte[] GetFileForGroups(IList<int> groupIds, int profileId = ProfileIds.Undefined)
        {
            var groupings = new List<Grouping>();

            foreach (var groupId in groupIds)
            {
                groupings.AddRange(GetGroupingsForGroup(groupId));
            }

            return GetFileForMultipleIndicators(groupings, profileId);
        }

        public byte[] GetFileForSpecifiedIndicators(IList<int> indicatorIds, int profileId)
        {
            var groupings = GetGroupingsFromIndicatorIds(indicatorIds, profileId);
            return GetFileForMultipleIndicators(groupings, profileId);
        }

        private List<Grouping> GetGroupingsForGroup(int group_id)
        {
            var groupings = _groupingListProvider
                .GetGroupingsByGroup(group_id)
                .OrderBy(x => x.Sequence)
                .ToList();
            return groupings;
        }

        private IList<Grouping> GetGroupingsFromIndicatorIds(IList<int> indicatorIds, int profileId)
        {
            IList<Grouping> groupings;
            if (profileId == ProfileIds.Undefined)
            {
                // Request came from either search or indicator list
                groupings = _groupingListProvider.GetGroupings(indicatorIds);
            }
            else
            {
                groupings = _groupingListProvider.GetGroupings(new List<int> { profileId },
                    indicatorIds);
            }
            return groupings;
        }

        private byte[] GetFileForMultipleIndicators(IList<Grouping> groupings, int profileId = ProfileIds.Undefined)
        {
            var props = GetIndicatorMetadataTextProperties();

            if (groupings.Any())
            {
                var metadataList = _indicatorMetadataProvider.GetIndicatorMetadata(groupings,
                    IndicatorMetadataTextOptions.OverrideGenericWithProfileSpecific, profileId);

                // Sort metadata according to grouping order
                var orderedMetadata = new List<IndicatorMetadata>();
                var orderedPolarities = new List<Polarity>();
                foreach (var indicatorId in groupings.Select(x => x.IndicatorId).Distinct())
                {
                    var polarity = GetMostCommonPolarity(groupings, indicatorId);
                    orderedPolarities.Add(polarity);
                    orderedMetadata.Add(metadataList.First(x => x.IndicatorId == indicatorId));
                }

                return new MultipleIndicatorMetadataFileWriter().GetMetadataFileAsBytes(
                    orderedMetadata, orderedPolarities, props);
            }
            else
            {
                // No indicator groupings
                return new MultipleIndicatorMetadataFileWriter().GetMetadataFileAsBytes(
                    new List<IndicatorMetadata>(), new List<Polarity>(), props);
            }
        }

        /// <summary>
        /// Get most frquently used polarity
        /// </summary>
        private Polarity GetMostCommonPolarity(IList<Grouping> groupings, int indicatorId)
        {
            groupings = groupings.Where(x => x.IndicatorId == indicatorId).ToList();
            var polarityId = new GroupingSorter(groupings).GetMostCommonPolarityId();
            return _polarities.First(x => x.Id == polarityId);
        }

        private IList<IndicatorMetadataTextProperty> GetIndicatorMetadataTextProperties()
        {
            var properties = _indicatorMetadataProvider.IndicatorMetadataTextProperties;
            return properties;
        }
    }
}