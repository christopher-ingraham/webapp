export class SystemOfUnitsInfo {

    public static souSI = 'SI';
    public static souUSCS = 'USCS';

    constructor (
        public readonly id: string,
        public readonly label: string
    ) { }

    public get isSI(): boolean {
        return (this.id === SystemOfUnitsInfo.souSI);
    }
}
