import { TdbChemCompBase } from '../../models';

export class TdbAlloyBase {
    public alloyDescription: string;
    public alloyId: string;
    public chemComp: TdbChemCompBase[];
    public chemCompMax: number;
    public chemCompMin: number;
    public chemCompNom: number;
}

