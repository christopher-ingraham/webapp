import { ComboBoxItem } from './combobox-item.class';

export class ComboBoxItemListBuilder {

    public static build<TSrcListItem, TValue, TLabel, TDstListItem extends ComboBoxItem<TValue, TLabel>> (
        list: TSrcListItem[],
        // tslint:disable-next-line
        dstListItemConstructor: { new(id: TValue, label: TLabel): TDstListItem; },
        idName: string,
        labelName: string
    ): TDstListItem[] {
        if (list && list.length) {
            return list.map((item) => new dstListItemConstructor(item[idName], item[labelName]));
        }
        return [];
    }

}
