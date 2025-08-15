using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class RepHmSetupBaseDto
    {
    }

    public class RepHmSetupDto : RepHmSetupBaseDto
    {
        public string AreaId { get; set; }
        public string CenterId { get; set; }     // Center ID
        public int InPieceNo { get; set; }      
        public string InPieceId { get; set; }      // Input Piece ID
        public string HeatId { get; set; }
        public string JobId { get; set; }
        public string PracticeId { get; set; }      // Practice ID
        public int MillMode { get; set; }           // Mill Mode  --->  Vedi lookups ---> cboMillMode  (Postman)
        public int DisabledStandBitmask { get; set; }
        public string MaterialGradeId { get; set; }     // Material Grade ID
        public string GradeGroupLabel { get; set; }     // Grade Groud ID
        public double EntryWdt { get; set; }              // Entry Width
        public double EntryThk { get; set; }
        public double EntryTemp { get; set; }       // Entry Temperature
        public double PieceLength { get; set; }        // Entry Length
        public double PieceWeight { get; set; }        // Entry Weight      
        public double Density { get; set; }            // Density
        public double ThermalExpCoeff { get; set; }     //  Thermal Exp Coeff
        public double TransferBarThk { get; set; }        // Thickness  (Transfer Bar)
        public double TransferBarWdt { get; set; }        // Width  (Transfer Bar)
        public double TransferBarTemp { get; set; }       // Temperature  (Transfer Bar)
        public double TargetWdt { get; set; }          // Target Cold Width
        public int TargetWdtUpTol { get; set; }
        public int TargetWdtLoTol { get; set; }
        public double TargetThk { get; set; }         // Target Cold Thickness       
        public int TargetThkUpTol { get; set; }
        public int TargetThkLoTol { get; set; }
        public int DescalerMode { get; set; }         // Descaler Mode --->  Vedi lookups ---> cboDescalerMode  (Postman)   [Ancillary Setup]
        public double DescalerHeadOffset { get; set; }    // Descaler Head Offset Delay          [Ancillary Setup]
        public double IcHeadOffset { get; set; }          // Intensive Cooling Head Offset       [Ancillary Setup]
        public int SetupMode { get; set; }
        public string SetupModelLabel { get; set; }   
        public string Operator { get; set; }         //  Operator 
        public DateTime Revision { get; set; }       // Revision
        public RepHmSetupAllStepsListItemDto[] GeneralSettings { get; set; }    //  TAB [General Data] + alcuni [Ancillary Setup]
    }

    public class RepHmSetupListItemDto : RepHmSetupDto
    {

    }

    public class RepHmSetupForInsertDto : RepHmSetupBaseDto
    {
    }

    public class RepHmSetupForUpdateDto : RepHmSetupBaseDto
    {
    }

    public class RepHmSetupDetailDto : RepHmSetupDto
    {
   
    }

    public class RepHmSetupListFilterDto
    {
    }
}