import { TdbAlloySpec } from './tdb-alloy-spec.class';
import { TdbChemCompBase } from '../../models';

export class TdbAlloySpecDetail extends TdbAlloySpec {
    public chemComp: TdbChemCompBase[];
    public chemCompMax: number;
    public chemCompMin: number;
}
