﻿using System.Collections.Generic;
using System.Linq;
using PholioVisualisation.DataAccess;
using PholioVisualisation.DataSorting;
using PholioVisualisation.PholioObjects;

namespace PholioVisualisation.DataConstruction
{
    public class ProfilePerIndicatorBuilder
    {
        private bool _isEnvironmentLive;

        public ProfilePerIndicatorBuilder(bool isEnvironmentLive)
        {
            _isEnvironmentLive = isEnvironmentLive;
        }

        public Dictionary<int, List<ProfilePerIndicator>> Build(IList<int> indicatorIds, int areaTypeId)
        {
            var response = new Dictionary<int, List<ProfilePerIndicator>>();

            if (indicatorIds == null || indicatorIds.Any() == false) return response;

            var profileReader = ReaderFactory.GetProfileReader();

            IList<ProfilePerIndicator> profiles;

            if (areaTypeId == AreaTypeIds.Undefined)
            {
                profiles = profileReader.GetProfilesForIndicators(indicatorIds.ToList());
            }
            else
            {
                profiles = profileReader.GetProfilesForIndicators(indicatorIds.ToList(), areaTypeId);
            }
            
            profiles = ProfileFilter.RemoveSystemProfiles(profiles);

            var uniqueIndicators = profiles.Select(x => x.IndicatorId).Distinct().ToList();

            foreach (var indicator in uniqueIndicators)
            {
                var profilesForCurrentIndicator = profiles.Where(x => x.IndicatorId == indicator).ToList();
                var distinctProfiles = (
                    from p in profilesForCurrentIndicator 
                    group p by p.ProfileName into g 
                    select g.First()
                    ).ToList();
                SetProfilesUrl(distinctProfiles);
                response.Add(indicator, distinctProfiles);
            }
            return response;
        }

        private void SetProfilesUrl(List<ProfilePerIndicator> profiles)
        {
            foreach (var profile in profiles)
            {
                profile.Url = new ProfileUrl(profile, _isEnvironmentLive).DataUrl;
            }
        }
    }

}
