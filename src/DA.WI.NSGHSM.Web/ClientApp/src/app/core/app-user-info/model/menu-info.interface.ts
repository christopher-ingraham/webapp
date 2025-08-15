export interface MenuInfo {

    readonly key: string;
    readonly default: string;
    readonly icon: string;
    readonly path: string;
    readonly roles: string[];
    readonly children: MenuInfo[];
}
