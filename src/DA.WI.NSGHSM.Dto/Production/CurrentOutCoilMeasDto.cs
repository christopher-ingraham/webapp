using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class CurrentOutCoilMeasBaseDto
    {

    }

    public class CurrentOutCoilMeasDto : CurrentOutCoilMeasBaseDto
    {
        public double ExitThk { get; set; }      // Exit Thickness [mm] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
        public double ExitWdt { get; set; }      // Exit Width [mm] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
        public double ExitStripTemp { get; set; }   // Finishing Temperature [^C] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
        public double DowncoilTemp { get; set; }    // Coiling Temperature [^C] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
        public double ProfileThk { get; set; }        // Strip Profile [mm] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
        public double ProfileWdt { get; set; }
        public double ProfileSym { get; set; }
        public double ProfileAsym { get; set; }
        public double StripEdgeDrop { get; set; }   // Strip Edge Flatness [I-Units] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
        public double StripQbFlatness { get; set; }  // Strip QTR Flatness [I-Units] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)
        public double IntermediateTemp { get; set; }  // Intermediate Temperature [°C] --> colonne seguendo l'ordine dell'oggetto (array[0] --> Avg, array[1] --> Min, ecc.)

    }

    public class CurrentOutCoilMeasListItemDto : CurrentOutCoilMeasDto
    {

    }

    public class CurrentOutCoilMeasForInsertDto : CurrentOutCoilMeasBaseDto
    {
    }

    public class CurrentOutCoilMeasForUpdateDto : CurrentOutCoilMeasBaseDto
    {
    }

    public class CurrentOutCoilMeasDetailDto : CurrentOutCoilMeasDto
    {
    

    }

    public class CurrentOutCoilMeasListFilterDto
    {
    }
}