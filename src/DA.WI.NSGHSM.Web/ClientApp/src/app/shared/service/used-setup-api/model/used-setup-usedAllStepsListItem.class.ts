import { UsedSetupDetail } from './used-setup-detail.class';

export class UsedAllStepsListItem extends UsedSetupDetail {

    public stepNo: number;
    public passNo: number;
    public standNo: number;       // Stand No
    public enabledStand: number;   // Enabled Stand  ---->  lookup cboEnbledStand  (Postaman ---> lookups)
    public agcConMode: number;
    public entryWidth: number;      // Entry/Exit Width  (Entry)
    public exitWidth: number;       // Entry/Exit Width  (Exit)
    public entryThk: number;     // Entry/Exit Thickness  (Entry)
    public exitThk: number;      // Entry/Exit Thickness  (Exit)
    public entryLength: number;
    public exitLength: number;
    public pvrThkHead: number;
    public pvrLenHead: number;
    public pvrThkTail: number;
    public pvrLenTail: number;
    public edgerTrgWidth: number;
    public edgerForce: number;
    public edgerSpeed: number;
    public edgerStiffness: number;
    public edgerDeltaWdtHead: number;
    public edgerDeltaLenHead: number;
    public edgerDeltaWdtTail: number;
    public edgerDeltaLenTail: number;
    public entryStripRough: number;
    public exitStripRough: number;
    public entryTemp: number;       // Entry/Exit Temp (Entry)
    public exitTemp: number;        // Entry/Exit Temp  (Exit)
    public specEntryTension: number;    // Entry/Exit Tension[kN] (Entry)
    public specExitTension: number;     // Entry/Exit Tension[kN]  (Exit)
    public looperAngle: number;
    public threadingSpeed: number;   // Threading Speed
    public millSpeed: number;     // Body Max Speed
    public fslip: number;
    public bslip: number;
    public draft: number;       // Draft
    public biteAngle: number;
    public reduction: number;      // Reduction
    public gap: number;
    public stiffness: number;
    public force: number;    // Body Force
    public motorTorque: number;
    public motorPower: number;
    public avgYs: number;
    public muEst: number;
    public stretchEstEn: number;
    public hEntryTemp: number;    // Entry/Exit Tension[MPa] (Entry)
    public hExitTemp: number;     // Entry/Exit Tension[MPa]  (Exit)
    public hEnrtyWidth: number;
    public hExitWidth: number;
    public hEntryThk: number;
    public hExitThk: number;
    public hGap: number;
    public hStiffness: number;
    public hFslip: number;       // Head Forward Slip
    public hBslip: number;
    public hForce: number;      //  Head Force
    public hMotorTorque: number;
    public hMotorPower: number;
    public hWrBend: number;
    public hGapOffset: number;
    public entryShearGap: number;
    public exitShearGap: number;
    public exitCrown: number;
    public exitCrownElle: number;
    public passTargetProfileA2: number;
    public passTargetProfileA4: number;
    public wrBend: number;        //  WR Bending
    public dwrBend: number;
    public gapOffset: number;
    public thExpOffset: number;
    public wrBendSlope: number;    //   Bending/Force Slope
    public wrBendCrown: number;
    public wrShift: number;     //  WR Shifting
    public headNoCooling: number;
    public tailNoCooling: number;
    public wrCoolingFlow: number;
    public wrCoolingNarrow: number;
    public wrCoolingMiddle: number;
    public wrCoolingWide: number;
    public stripCoolingFlow: number;
    public forceToEntryThk: number;
    public forceToExitThk: number;
    public forceToEntryTens: number;
    public forceToExitTens: number;
    public forceToMillSpeed: number;
    public validitySpeedRange: number;
    public speedToExitTemp: number;
    public stripCoolToExitTemp: number;
    public wrbToFlatness: number;
    public wrbToProfile: number;
    public rollExtTemp: number;
    public thkLastDev: number;
    public rollBiteOilPerc: number;   // Oil in Water HeadCropLength: number;
    public headCropLength: number;    //  Crop Shear Head Cut length
    public tailCropLength: number;    //  Crop Shear Tail Cut length

}
