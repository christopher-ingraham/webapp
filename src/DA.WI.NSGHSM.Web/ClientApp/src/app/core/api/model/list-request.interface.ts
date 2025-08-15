import { PagingRequest } from './paging-request.interface';
import { SortRequestDto } from './sort-request-dto.interface';

export interface ListRequest<TFilteringRequest> {
    filter: TFilteringRequest;
    page: PagingRequest;
    sort: SortRequestDto;
}
