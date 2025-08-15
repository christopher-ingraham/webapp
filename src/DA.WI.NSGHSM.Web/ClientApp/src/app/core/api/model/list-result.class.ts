export class ListResult<TItem> {
    total: number;
    data: TItem[];

    public static factory<TItem>(total: number, data: TItem[]): ListResult<TItem> {
        return Object.assign(new ListResult<TItem>(), { data, total });
    }
}
