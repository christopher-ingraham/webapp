import { ProducedCoilDetail } from './produced-coil-detail.class';

export class CurrentOutCoilMeas extends ProducedCoilDetail {

    public exitThk: number;      // Exit Thickness [mm] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc)
    public exitWdt: number;      // Exit Width [mm] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
    public exitStripTemp: number;   // Finishing Temperature [^C] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
    public downcoilTemp: number;    // Coiling Temperature [^C] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
    public profile: number;        // Strip Profile [mm] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
    public stripEdgeDrop: number;   // Strip Edge Flatness [I-Units] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
    public stripQbFlatness: number;  // Strip QTR Flatness [I-Units] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
    public intermediateTemp: number;  //Intermediate Temperature [°C] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)

}