import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {
  Area,
  AreaType,
  AreaTextSearchResult,
  NearByAreas,
  AreaAddress,
  ParentAreaType
} from '../../../typings/FT';
import { FTHelperService } from '../helper/ftHelper.service';
import { Parameters } from './parameters';
import { HttpService } from './http.service'

@Injectable()
export class AreaService {

  private version: string = this.ftHelperService.version();

  constructor(private httpService: HttpService, private ftHelperService: FTHelperService) {
  }

  getAreaSearchByText(
    text: string,
    areaTypeId: number,
    shouldSearchRetreiveCoordinates: boolean,
    parentAreasToIncludeInResults: boolean
  ): Observable<AreaTextSearchResult> {
    const params = new Parameters(this.version);
    params.addPolygonAreaTypeId(areaTypeId);
    params.addNoCache();
    params.addIncludeCoordinates(shouldSearchRetreiveCoordinates);
    params.addParentAreasToIncludeInResults(parentAreasToIncludeInResults);
    params.addSearchText(text);

    return this.httpService.httpGet('api/area_search_by_text', params);
  }

  getAreaSearchByProximity(
    easting: number,
    northing: number,
    areaTypeId: number
  ): Observable<NearByAreas> {
    const params = new Parameters(this.version);
    params.addAreaTypeId(areaTypeId);
    params.addEasting(easting);
    params.addNorthing(northing);

    return this.httpService.httpGet('api/area_search_by_proximity', params);
  }

  getAreaAddressesByParentAreaCode(
    parentAreaCode: string,
    areaTypeId: number
  ): Observable<AreaAddress> {
    const params = new Parameters(this.version);
    params.addAreaTypeId(areaTypeId);
    params.addParentAreaCode(parentAreaCode);

    return this.httpService.httpGet('api/area_addresses/by_parent_area_code', params);
  }

  getParentAreas(profileId: number): Observable<ParentAreaType[]> {
    const params = new Parameters(this.version);
    params.addProfileId(profileId);

    return this.httpService.httpGet('api/area_types/parent_area_types', params);
  }

  getAreaTypes(): Observable<AreaType[]> {
    const params = new Parameters(this.version);

    return this.httpService.httpGet('api/area_types/with_data', params);
  }

  getAreas(areaTypeId: number): Observable<Area[]> {
    const params = new Parameters(this.version);
    params.addAreaTypeId(areaTypeId);
    params.addNoCache();

    return this.httpService.httpGet('api/areas/by_area_type', params);
  }

  getParentToChildAreas(profileId: number, childAreaTypeId: number, parentAreaTypeId: number, nearestNeighbourCode: string = ''): Observable<Map<string, string[]>> {
    const params = new Parameters(this.version);
    params.addProfileId(profileId);
    params.addChildAreaTypeId(childAreaTypeId);
    params.addParentAreaTypeId(parentAreaTypeId);

    if (this.ftHelperService.isNearestNeighbours() && nearestNeighbourCode != '') {
      params.addNearestNeighbourCode(nearestNeighbourCode);
    }

    return this.httpService.httpGet('api/parent_to_child_areas', params);
  }
}
