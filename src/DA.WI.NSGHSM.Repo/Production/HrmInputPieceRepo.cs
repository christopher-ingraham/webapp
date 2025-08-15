using System.ComponentModel;
using System.Xml;
using DA.DB.Utils;
using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.Production
{
    internal class HrmInputPieceRepo<TDataSource> : IHrmInputPieceRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public HrmInputPieceRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public void Create(HrmInputPieceForInsertDto dto)
        {
            string insert = @"INSERT INTO HRM_INPUT_PIECE (
                          PIECE_NO
                        , PIECE_SEQ                  
                        , JOB_PIECE_SEQ              
                        , PIECE_ID                   
                        , JOB_ID                     
                        , HEAT_NO                    
                        , HEAT_SEQ                   
                        , HEAT_PIECE_SEQ             
                        , TRANSITION                 
                        , SOURCE_CODE_ID             
                        , DEST_CODE_ID     
                        , BASE_GRADE_ID
                        , USE_BASE_GRADE
                        , PRELIMINARY_MAT_GRADE_ID          
                        , MATERIAL_SPEC_ID           
                        , PRELIMINARY_THK                  
                        , PRELIMINARY_THK_HEAD             
                        , PRELIMINARY_THK_TAIL             
                        , PRELIMINARY_WDT                      
                        , PRELIMINARY_WDT_HEAD                  
                        , PRELIMINARY_WDT_TAIL
                        , PRELIMINARY_WDT_CHG
                        , PRELIMINARY_LEN                      
                        , WEIGHT                      
                        , ENTRY_TEMP                  
                        , FURNACE_DISCHARGE_TEMP      
                        , TARGET_WIDTH                
                        , TARGET_WIDTH_PTOL           
                        , TARGET_WIDTH_NTOL           
                        , TARGET_THICKNESS            
                        , TARGET_THICKNESS_PTOL       
                        , TARGET_THICKNESS_NTOL       
                        , TARGET_WEIGHT               
                        , TARGET_WEIGHT_PTOL          
                        , TARGET_WEIGHT_NTOL          
                        , TARGET_EXIT_TEMP            
                        , TARGET_EXIT_TEMP_PTOL       
                        , TARGET_EXIT_TEMP_NTOL       
                        , TARGET_TEMP_FM_CUSTOMER_TOL 
                        , TARGET_TEMP_DC              
                        , TARGET_TEMP_DC_PTOL         
                        , TARGET_TEMP_DC_NTOL         
                        , TARGET_TEMP_DC_CUSTOMER_TOL 
                        , TARGET_PROFILE              
                        , TARGET_PROFILE_PTOL         
                        , TARGET_PROFILE_NTOL         
                        , TARGET_PROFILE_CUSTOMER_TOL 
                        , TARGET_FLATNESS             
                        , TARGET_FLATNESS_PTOL        
                        , TARGET_FLATNESS_NTOL        
                        , TARGET_FLATNESS_CUSTOMER_TOL
                        , TARGET_INTERNAL_DIAMETER
                        , USE_MEAS_WIDTH               
                        , MEASURED_WIDTH_HEAD          
                        , MEASURED_WIDTH_TAIL          
                        , USE_MEAS_TEMP                
                        , MEASURED_TEMP  
                        , CREATION_DATETIME
                        , STATUS                       
                        , REMARK                       
                        , OPERATOR                     
                        , REVISION                     
                    ) VALUES (:PieceNo, :PieceSeq, :JobPieceSeq, :PieceId, :JobId, :HeatNo, :HeatSeq, :HeatPieceSeq, :Transition, :SourceCodeId, :DestCodeId,
                     :BaseGradeId, :UseBaseGrade, :PreliminaryMatGradeId, :MaterialSpecId, :PreliminaryThk, :PreliminaryThkHead, :PreliminaryThkTail, :PreliminaryWdt,
                     :PreliminaryWdtHead, :PreliminaryWdtTail, :PreliminaryWdtChg, :PreliminaryLen, :Weight, :EntryTemp, :FurnaceDischargeTemp, :TargetWidth,
                     :TargetWidthPTol, :TargetWidthNTol, :TargetThickness, :TargetThicknessPTol, :TargetThicknessNTol, :TargetWeight, :TargetWeightPTol, :TargetWeightNTol,
                     :TargetExitTemp, :TargetExitTempPTol, :TargetExitTempNTol, :TargetTempFmCustomerTol, :TargetTempDc, :TargetTempDcPTol, :TargetTempDcNTol,
                     :TargetTempDcCustomerTol, :TargetProfile, :TargetProfilePTol, :TargetProfileNTol, :TargetProfileCustomerTol, :TargetFlatness, :TargetFlatnessPtol,
                     :TargetFlatnessNTol, :TargetFlatnessCustomerTol, :TargetInternalDiameter, :UseMeasWidth, :MeasuredWidthHead, :MeasuredWidthTail,
                     :UseMeasTemp, :MeasuredTemp, :CreationDatetime, :Status, :Remark, :Operator, :Revision )";

            int outPieceNo = findMax("PIECE_NO", "HRM_INPUT_PIECE") + 1;

            ctx.ExecuteNonQuery(insert,
                 ctx.CreateParameter("PieceNo", outPieceNo),
                 ctx.CreateParameter("PieceSeq", dto.PieceSeq),
                 ctx.CreateParameter("JobPieceSeq", dto.JobPieceSeq),
                 ctx.CreateParameter("PieceId", dto.PieceId),
                 ctx.CreateParameter("JobId", dto.JobId),        // tendina lookup
                 ctx.CreateParameter("HeatNo", dto.HeatNo),
                 ctx.CreateParameter("HeatSeq", dto.HeatSeq),
                 ctx.CreateParameter("HeatPieceSeq", dto.HeatPieceSeq),
                 ctx.CreateParameter("Transition", dto.Transition),
                 ctx.CreateParameter("SourceCodeId", dto.SourceCodeId),
                 ctx.CreateParameter("DestCodeId", dto.DestCodeId),
                 ctx.CreateParameter("BaseGradeId", dto.BaseGradeId),
                 ctx.CreateParameter("UseBaseGrade", dto.UseBaseGrade),
                 ctx.CreateParameter("PreliminaryMatGradeId", dto.MaterialGradeId),
                 ctx.CreateParameter("MaterialSpecId", dto.MaterialSpecId),
                 ctx.CreateParameter("PreliminaryThk", dto.PreliminaryThk),
                 ctx.CreateParameter("PreliminaryThkHead", dto.PreliminaryThkHead),
                 ctx.CreateParameter("PreliminaryThkTail", dto.PreliminaryThkTail),
                 ctx.CreateParameter("PreliminaryWdt", dto.PreliminaryWdt),
                 ctx.CreateParameter("PreliminaryWdtHead", dto.PreliminaryWdtHead),
                 ctx.CreateParameter("PreliminaryWdtTail", dto.PreliminaryWdtTail),
                 ctx.CreateParameter("PreliminaryWdtChg", dto.PreliminaryWdtChg),
                 ctx.CreateParameter("PreliminaryLen", dto.PreliminaryLen),
                 ctx.CreateParameter("Weight", dto.Weight),
                 ctx.CreateParameter("EntryTemp", dto.EntryTemp),
                 ctx.CreateParameter("FurnaceDischargeTemp", dto.FurnaceDischargeTemp),
                 ctx.CreateParameter("TargetWidth", dto.TargetWidth),
                 ctx.CreateParameter("TargetWidthPTol", dto.TargetWidthPtol),
                 ctx.CreateParameter("TargetWidthNTol", dto.TargetWidthNtol),
                 ctx.CreateParameter("TargetThickness", dto.TargetThickness),
                 ctx.CreateParameter("TargetThicknessPTol", dto.TargetThicknessPtol),
                 ctx.CreateParameter("TargetThicknessNTol", dto.TargetThicknessNtol),
                 ctx.CreateParameter("TargetWeight", dto.TargetWeight),
                 ctx.CreateParameter("TargetWeightPTol", dto.TargetWeightPtol),
                 ctx.CreateParameter("TargetWeightNTol", dto.TargetWeightNtol),
                 ctx.CreateParameter("TargetExitTemp", dto.TargetExitTemp),
                 ctx.CreateParameter("TargetExitTempPTol", dto.TargetExitTempPtol),
                 ctx.CreateParameter("TargetExitTempNTol", dto.TargetExitTempNtol),
                 ctx.CreateParameter("TargetTempFmCustomerTol", dto.TargetTempFmCustomertol),
                 ctx.CreateParameter("TargetTempDc", dto.TargetTempDC),
                 ctx.CreateParameter("TargetTempDcPTol", dto.TargetTempDCPtol),
                 ctx.CreateParameter("TargetTempDcNTol", dto.TargetTempDCNtol),
                 ctx.CreateParameter("TargetTempDcCustomerTol", dto.TargetTempDCCustomertol),
                 ctx.CreateParameter("TargetProfile", dto.TargetProfile),
                 ctx.CreateParameter("TargetProfilePTol", dto.TargetProfilePtol),
                 ctx.CreateParameter("TargetProfileNTol", dto.TargetProfileNtol),
                 ctx.CreateParameter("TargetProfileCustomerTol", dto.TargetProfileCustomertol),
                 ctx.CreateParameter("TargetFlatness", dto.TargetFlatness),
                 ctx.CreateParameter("TargetFlatnessPtol", dto.TargetFlatnessPtol),
                 ctx.CreateParameter("TargetFlatnessNTol", dto.TargetFlatnessNtol),
                 ctx.CreateParameter("TargetFlatnessCustomerTol", dto.TargetFlatnessCustomertol),
                 ctx.CreateParameter("TargetInternalDiameter", dto.TargetInternalDiameter),
                 ctx.CreateParameter("UseMeasWidth", dto.UseMeasWidth),
                 ctx.CreateParameter("MeasuredWidthHead", dto.MeasuredWidthHead),
                 ctx.CreateParameter("MeasuredWidthTail", dto.MeasuredWidthTail),
                 ctx.CreateParameter("UseMeasTemp", dto.UseMeasTemp),
                 ctx.CreateParameter("MeasuredTemp", dto.MeasuredTemp),
                 ctx.CreateParameter("CreationDatetime", DateTime.Now),
                 ctx.CreateParameter("Status", dto.Status),
                 ctx.CreateParameter("Remark", dto.Remark),
                 ctx.CreateParameter("Operator", dto.Operator),
                 ctx.CreateParameter("Revision", DateTime.Now)
             );

            InsHrmOrder(dto, outPieceNo);
        }

        public void InsHrmOrder(HrmInputPieceForInsertDto dto, int pieceNo)
        {
            string insert = @"INSERT INTO HRM_ORDER (
                          ORDER_CNT 
                        , PIECE_NO  
                        , JOB_ID              
                        , ORDER_NUMBER   
                        , ORDER_POSITION           
                        , CUSTOMER_ID         
                        , ENDUSE_SURFACE_RATING  
                        , AREA_TYPE         
                        , TRIAL_FLAG         
                        , TRIAL_NO           
                        , CARRIER_MODE 
                        , END_USE         
                        , OPERATOR  
                        , REVISION  
                    ) VALUES (
                          :OrderCnt
                        , :PieceNo       
                        , :JobId              
                        , :OrderNumber   
                        , :OrderPosition           
                        , :CustomerId         
                        , :EnduseSurfaceRating  
                        , :AreaType         
                        , :TrialFlag         
                        , :TrialNo           
                        , :CarrierMode 
                        , :EndUse         
                        , :Operator  
                        , :CurrentTimestamp
                    ) ";

            ctx.ExecuteNonQuery(insert,
                 ctx.CreateParameter("OrderCnt", findMax("ORDER_CNT", "HRM_ORDER") + 1),
                 ctx.CreateParameter("PieceNo", pieceNo),
                 ctx.CreateParameter("JobId", dto.JobId),
                 ctx.CreateParameter("OrderNumber", dto.OrderNumber),
                 ctx.CreateParameter("OrderPosition", dto.OrderPosition),
                 ctx.CreateParameter("CustomerId", dto.CustomerId),
                 ctx.CreateParameter("EnduseSurfaceRating", dto.EnduseSurfaceRating),
                 ctx.CreateParameter("AreaType", dto.AreaType),
                 ctx.CreateParameter("TrialFlag", dto.TrialFlag),
                 ctx.CreateParameter("TrialNo", dto.TrialNo),
                 ctx.CreateParameter("CarrierMode", dto.CarrierMode),
                 ctx.CreateParameter("EndUse", dto.EndUse),
                 ctx.CreateParameter("Operator", dto.Operator),
                 ctx.CreateParameter("CurrentTimestamp", DateTime.Now)
            );

        }

        public void Delete(int pieceNo)
        {
            string delete1 = "DELETE FROM HRM_ORDER WHERE PIECE_NO = :PieceNo ";
            string delete2 = "DELETE FROM HRM_INPUT_PIECE WHERE PIECE_NO = :PieceNo ";

            ctx.ExecuteNonQuery(delete1, ctx.CreateParameter("PieceNo", pieceNo));
            ctx.ExecuteNonQuery(delete2, ctx.CreateParameter("PieceNo", pieceNo));
        }

        public void Update(HrmInputPieceForUpdateDto dto, int pieceNo)
        {
            string update = $@"UPDATE HRM_INPUT_PIECE SET
                          PIECE_SEQ                         = :PieceSeq
                        , JOB_PIECE_SEQ                     = :JobPieceSeq  
                        , PIECE_ID                          = :PieceId
                        , JOB_ID                            = :JobId
                        , HEAT_NO                           = :HeatNo
                        , HEAT_SEQ                          = :HeatSeq
                        , HEAT_PIECE_SEQ                    = :HeatPieceSeq
                        , TRANSITION                        = :Transition
                        , SOURCE_CODE_ID                    = :SourceCodeId
                        , DEST_CODE_ID                      = :DestCodeId
                        , USE_BASE_GRADE                    = :UseBaseGrade
                        , BASE_GRADE_ID                     = :BaseGradeId
                        , PRELIMINARY_MAT_GRADE_ID          = :PreliminaryMatGradeId
                        , MATERIAL_GRADE_ID                 = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :MaterialGradeId ELSE MATERIAL_GRADE_ID END)
                        , MATERIAL_SPEC_ID                  = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :MaterialSpecId  ELSE MATERIAL_SPEC_ID  END)
                        , PRELIMINARY_THK                   = :PreliminaryThk
                        , THICKNESS                         = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :PreliminaryThk         ELSE THICKNESS         END)
                        , PRELIMINARY_THK_HEAD              = :PreliminaryThkHead
                        , THICKNESS_HEAD                    = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :PreliminaryThkHead    ELSE THICKNESS_HEAD    END)
                        , PRELIMINARY_THK_TAIL              = :PreliminaryThkTail
                        , THICKNESS_TAIL                    = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :PreliminaryThkTail    ELSE THICKNESS_TAIL    END)
                        , PRELIMINARY_WDT                   = :PreliminaryWdt           
                        , WIDTH                             = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :PreliminaryWdt             ELSE WIDTH             END)
                        , PRELIMINARY_WDT_HEAD              = :PreliminaryWdtHead
                        , WIDTH_HEAD                        = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :PreliminaryWdtHead        ELSE WIDTH_HEAD        END)
                        , PRELIMINARY_WDT_TAIL              = :PreliminaryWdtTail
                        , WIDTH_TAIL                        = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :PreliminaryWdtTail        ELSE WIDTH_TAIL        END)
                        , PRELIMINARY_WDT_CHG               = :PreliminaryWdtChg
                        , WIDTH_CHANGE                      = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :PreliminaryWdtChg      ELSE WIDTH_CHANGE      END)
                        , PRELIMINARY_LEN                   = :PreliminaryLen
                        , LENGTH                            = (CASE  WHEN (PRELIMINARY_MODE >= 3) THEN :PreliminaryLen            ELSE LENGTH           END)
                        , WEIGHT                            = :Weight
                        , ENTRY_TEMP                        = :EntryTemp
                        , FURNACE_DISCHARGE_TEMP            = :FurnaceDischargeTemp
                        , TARGET_WIDTH                      = :TargetWidth   
                        , TARGET_WIDTH_PTOL                 = :TargetWidthPTol
                        , TARGET_WIDTH_NTOL                 = :TargetWidthNTol
                        , TARGET_THICKNESS                  = :TargetThickness
                        , TARGET_THICKNESS_PTOL             = :TargetThicknessPTol
                        , TARGET_THICKNESS_NTOL             = :TargetThicknessNTol
                        , TARGET_WEIGHT                     = :TargetWeight
                        , TARGET_WEIGHT_PTOL                = :TargetWeightPTol  
                        , TARGET_WEIGHT_NTOL                = :TargetWeightNTol  
                        , TARGET_EXIT_TEMP                  = :TargetExitTemp
                        , TARGET_EXIT_TEMP_PTOL             = :TargetExitTempPTol
                        , TARGET_EXIT_TEMP_NTOL             = :TargetExitTempNTol
                        , TARGET_TEMP_FM_CUSTOMER_TOL       = :TargetTempFmCustomerTol
                        , TARGET_TEMP_DC                    = :TargetTempDc
                        , TARGET_TEMP_DC_PTOL               = :TargetTempDcPTol
                        , TARGET_TEMP_DC_NTOL               = :TargetTempDcNTol
                        , TARGET_TEMP_DC_CUSTOMER_TOL       = :TargetTempDcCustomerTol
                        , TARGET_PROFILE                    = :TargetProfile
                        , TARGET_PROFILE_PTOL               = :TargetProfilePTol
                        , TARGET_PROFILE_NTOL               = :TargetProfileNTol
                        , TARGET_PROFILE_CUSTOMER_TOL       = :TargetProfileCustomerTol
                        , TARGET_FLATNESS                   = :TargetFlatness
                        , TARGET_FLATNESS_PTOL              = :TargetFlatnessPTol
                        , TARGET_FLATNESS_NTOL              = :TargetFlatnessnTol
                        , TARGET_FLATNESS_CUSTOMER_TOL      = :TargetFlatnessCustomerTol
                        , TARGET_INTERNAL_DIAMETER          = :TargetInternalDiameter
                        , USE_MEAS_WIDTH                    = :UseMeasWidth
                        , MEASURED_WIDTH_HEAD               = :MeasuredWidthHead
                        , MEASURED_WIDTH_TAIL               = :MeasuredWidthTail
                        , USE_MEAS_TEMP                     = :UseMeasTemp
                        , MEASURED_TEMP                     = :MeasuredTemp
                        , STATUS                            = :Status
                        , REMARK                            = :Remark
                        , OPERATOR                          = :Operator
                        , REVISION                          = :Revision
                    WHERE PIECE_NO                          = :PieceNo ";

            int affectedRows = ctx.ExecuteNonQuery(update,
                ctx.CreateParameter("PieceNo", pieceNo),
                ctx.CreateParameter("PieceSeq", dto.PieceSeq),
                 ctx.CreateParameter("JobPieceSeq", dto.JobPieceSeq),
                 ctx.CreateParameter("PieceId", dto.PieceId),
                 ctx.CreateParameter("JobId", dto.JobId),        // tendina lookup
                 ctx.CreateParameter("HeatNo", dto.HeatNo),
                 ctx.CreateParameter("HeatSeq", dto.HeatSeq),
                 ctx.CreateParameter("HeatPieceSeq", dto.HeatPieceSeq),
                 ctx.CreateParameter("Transition", dto.Transition),
                 ctx.CreateParameter("SourceCodeId", dto.SourceCodeId),
                 ctx.CreateParameter("DestCodeId", dto.DestCodeId),
                 ctx.CreateParameter("BaseGradeId", dto.BaseGradeId),
                 ctx.CreateParameter("UseBaseGrade", dto.UseBaseGrade),
                 ctx.CreateParameter("PreliminaryMatGradeId", dto.MaterialGradeId),
                 ctx.CreateParameter("MaterialGradeId", dto.MaterialGradeId),
                 ctx.CreateParameter("MaterialSpecId", dto.MaterialSpecId),
                 ctx.CreateParameter("PreliminaryThk", dto.PreliminaryThk),
                 ctx.CreateParameter("PreliminaryThkHead", dto.PreliminaryThkHead),
                 ctx.CreateParameter("PreliminaryThkTail", dto.PreliminaryThkTail),
                 ctx.CreateParameter("PreliminaryWdt", dto.PreliminaryWdt),
                 ctx.CreateParameter("PreliminaryWdtHead", dto.PreliminaryWdtHead),
                 ctx.CreateParameter("PreliminaryWdtTail", dto.PreliminaryWdtTail),
                 ctx.CreateParameter("PreliminaryWdtChg", dto.PreliminaryWdtChg),
                 ctx.CreateParameter("PreliminaryLen", dto.PreliminaryLen),
                 ctx.CreateParameter("Weight", dto.Weight),
                 ctx.CreateParameter("EntryTemp", dto.EntryTemp),
                 ctx.CreateParameter("FurnaceDischargeTemp", dto.FurnaceDischargeTemp),
                 ctx.CreateParameter("TargetWidth", dto.TargetWidth),
                 ctx.CreateParameter("TargetWidthPTol", dto.TargetWidthPtol),
                 ctx.CreateParameter("TargetWidthNTol", dto.TargetWidthNtol),
                 ctx.CreateParameter("TargetThickness", dto.TargetThickness),
                 ctx.CreateParameter("TargetThicknessPTol", dto.TargetThicknessPtol),
                 ctx.CreateParameter("TargetThicknessNTol", dto.TargetThicknessNtol),
                 ctx.CreateParameter("TargetWeight", dto.TargetWeight),
                 ctx.CreateParameter("TargetWeightPTol", dto.TargetWeightPtol),
                 ctx.CreateParameter("TargetWeightNTol", dto.TargetWeightNtol),
                 ctx.CreateParameter("TargetExitTemp", dto.TargetExitTemp),
                 ctx.CreateParameter("TargetExitTempPTol", dto.TargetExitTempPtol),
                 ctx.CreateParameter("TargetExitTempNTol", dto.TargetExitTempNtol),
                 ctx.CreateParameter("TargetTempFmCustomerTol", dto.TargetTempFmCustomertol),
                 ctx.CreateParameter("TargetTempDc", dto.TargetTempDC),
                 ctx.CreateParameter("TargetTempDcPTol", dto.TargetTempDCPtol),
                 ctx.CreateParameter("TargetTempDcNTol", dto.TargetTempDCNtol),
                 ctx.CreateParameter("TargetTempDcCustomerTol", dto.TargetTempDCCustomertol),
                 ctx.CreateParameter("TargetProfile", dto.TargetProfile),
                 ctx.CreateParameter("TargetProfilePTol", dto.TargetProfilePtol),
                 ctx.CreateParameter("TargetProfileNTol", dto.TargetProfileNtol),
                 ctx.CreateParameter("TargetProfileCustomerTol", dto.TargetProfileCustomertol),
                 ctx.CreateParameter("TargetFlatness", dto.TargetFlatness),
                 ctx.CreateParameter("TargetFlatnessPtol", dto.TargetFlatnessPtol),
                 ctx.CreateParameter("TargetFlatnessNTol", dto.TargetFlatnessNtol),
                 ctx.CreateParameter("TargetFlatnessCustomerTol", dto.TargetFlatnessCustomertol),
                 ctx.CreateParameter("TargetInternalDiameter", dto.TargetInternalDiameter),
                 ctx.CreateParameter("UseMeasWidth", dto.UseMeasWidth),
                 ctx.CreateParameter("MeasuredWidthHead", dto.MeasuredWidthHead),
                 ctx.CreateParameter("MeasuredWidthTail", dto.MeasuredWidthTail),
                 ctx.CreateParameter("UseMeasTemp", dto.UseMeasTemp),
                 ctx.CreateParameter("MeasuredTemp", dto.MeasuredTemp),
                 ctx.CreateParameter("CreationDatetime", DateTime.Now),
                 ctx.CreateParameter("Status", dto.Status),
                 ctx.CreateParameter("Remark", dto.Remark),
                 ctx.CreateParameter("Operator", dto.Operator),
                 ctx.CreateParameter("Revision", DateTime.Now)
            );

            string query = @"SELECT ORDER_CNT
                            FROM HRM_ORDER
                            WHERE PIECE_NO = :PieceNo ";
            int orderCnt = ctx.GetEntity<int>(query, ctx.CreateParameter("PieceNo", pieceNo));
            UpdHrmOrder(dto, pieceNo, orderCnt);
            UpdHrmCustomer(dto);
        }

        public void UpdHrmOrder(HrmInputPieceForUpdateDto dto, int pieceNo, int orderCnt)
        {
            string update = @"UPDATE HRM_ORDER SET
                          PIECE_NO = :PieceNo         
                        , JOB_ID   = :JobId               
                        , ORDER_NUMBER   = :OrderNumber    
                        , ORDER_POSITION = :OrderPosition             
                        , CUSTOMER_ID    = :CustomerId           
                        , ENDUSE_SURFACE_RATING = :EnduseSurfaceRating 
                        , AREA_TYPE  = :AreaType             
                        , TRIAL_FLAG = :TrialFlag             
                        , TRIAL_NO   = :TrialNo               
                        , CARRIER_MODE = :CarrierMode          
                        , END_USE   = :EndUse                
                        , OPERATOR  = :Operator
                        , REVISION  = :CurrentTimestamp
                    WHERE ORDER_CNT = :OrderCnt ";

            ctx.ExecuteNonQuery(update,
                 ctx.CreateParameter("OrderCnt", orderCnt),
                 ctx.CreateParameter("PieceNo", pieceNo),
                 ctx.CreateParameter("JobId", dto.JobId),
                 ctx.CreateParameter("OrderNumber", dto.OrderNumber),
                 ctx.CreateParameter("OrderPosition", dto.OrderPosition),
                 ctx.CreateParameter("CustomerId", dto.CustomerId),
                 ctx.CreateParameter("EnduseSurfaceRating", dto.EnduseSurfaceRating),
                 ctx.CreateParameter("AreaType", dto.AreaType),
                 ctx.CreateParameter("TrialFlag", dto.TrialFlag),
                 ctx.CreateParameter("TrialNo", dto.TrialNo),
                 ctx.CreateParameter("CarrierMode", dto.CarrierMode),
                 ctx.CreateParameter("EndUse", dto.EndUse),
                 ctx.CreateParameter("Operator", dto.Operator),
                 ctx.CreateParameter("CurrentTimestamp", DateTime.Now)
            );
        }

        public void UpdHrmCustomer(HrmInputPieceForUpdateDto dto)
        {
            string customerId = dto.CustomerId.PadRight(32, ' ');  ;
            
            string update = @"UPDATE HRM_CUSTOMER SET
                          CUSTOMER_NAME = :CustomerName         
                        , CUSTOMER_CONTACT_NAME  = :CustomerContactName               
                    WHERE CUSTOMER_ID = :CustomerId ";

            ctx.ExecuteNonQuery(update,
                 ctx.CreateParameter("CustomerId", customerId),
                 ctx.CreateParameter("CustomerName", dto.CustomerName),
                 ctx.CreateParameter("CustomerContactName", dto.CustomerContactName)
            );
        }

        public ListResultDto<HrmInputPieceListItemDto> SelInputPiecesList(ListRequestDto<HrmInputPieceListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT A.PIECE_NO                                                          AS PieceNo
                        , TRIM(A.PIECE_ID)                                                              AS PieceId
                        , TRIM(H.HEAT_ID)                                                               AS HeatId
                        , TRIM(A.JOB_ID)                                                                AS JobId
                        , A.JOB_PIECE_SEQ                                                               AS JobPieceSeq
                        , A.TRANSITION                                                                  AS Transition
                        , RTRIM(F.CHAR_VALUE) || ': ' || RTRIM(F.VALUE_LABEL)                           AS TransitionLabel
                        , A.THICKNESS                                                                   AS EntryThickness 
                        , A.THICKNESS_HEAD                                                              AS ThicknessHead
                        , A.THICKNESS_TAIL                                                              AS ThicknessTail
                        , A.WIDTH                                                                       AS Width
                        , A.WIDTH_HEAD                                                                  AS WidthHead
                        , A.WIDTH_TAIL                                                                  AS WidthTail
                        , A.WEIGHT                                                                      AS Weight
                        , A.LENGTH                                                                      AS Length
                        , TRIM(A.MATERIAL_GRADE_ID)                                                     AS MaterialGradeId
                        , B.GRADE_GROUP_ID                                                              AS GradeGroupId
                        , LTRIM(TO_CHAR(C.GRADE_GROUP_ID,'999' )) || ': ' || RTRIM(C.GRADE_GROUP_LABEL) AS GradeGroupLabel
                        , A.TARGET_THICKNESS                                                            AS TargetThickness
                        , A.TARGET_WIDTH                                                                AS TargetWidth
                        , A.TARGET_INTERNAL_DIAMETER                                                    AS TargetInternalDiameter
                        , TRIM(ORD.ORDER_NUMBER)                                                        AS OrderNumber
                        , TRIM(ORD.ORDER_POSITION)                                                      AS OrderPosition
                        , TRIM(CUST.CUSTOMER_NAME)                                                      AS CustomerName
                        , ORD.TRIAL_FLAG                                                                AS TrialFlag
                        , LTRIM(TO_CHAR(ORD.TRIAL_FLAG, '999')) || ': ' || RTRIM(G.VALUE_LABEL)         AS TrialFlagLabel
                        , A.STATUS                                                                      AS Status
                        , LTRIM(TO_CHAR(E.INTEGER_VALUE, '999')) || ': ' || RTRIM(E.VALUE_LABEL)        AS StatusLabel
                        , A.CREATION_DATETIME                                                           AS CreationDatetime
                        , TRIM(A.OPERATOR)                                                              AS Operator
                        , A.REVISION                                                                    AS Revision
                    FROM HRM_INPUT_PIECE A
                    LEFT OUTER JOIN TDB_MATERIAL_GRADE B
                    ON A.MATERIAL_GRADE_ID = B.MATERIAL_GRADE_ID
                    LEFT OUTER JOIN TDB_GRADE_GROUP C
                    ON C.GRADE_GROUP_ID = B.GRADE_GROUP_ID
                    LEFT OUTER JOIN HRM_HEAT  H
                    ON H.HEAT_NO = A.HEAT_NO
                    LEFT OUTER JOIN HRM_ORDER  ORD
                    ON ORD.PIECE_NO = A.PIECE_NO
                    LEFT OUTER JOIN HRM_CUSTOMER CUST
                    ON CUST.CUSTOMER_ID = ORD.CUSTOMER_ID
                    LEFT OUTER JOIN AUX_VALUE G
                    ON G.INTEGER_VALUE  = ORD.TRIAL_FLAG
                    AND G.VARIABLE_ID   = 'YESNO_FLAG'
                       ,AUX_VALUE E
                       ,AUX_VALUE F
                  WHERE E.VARIABLE_ID  = 'STATUS_INPUT'
                  AND E.INTEGER_VALUE  = A.STATUS
                  AND F.VARIABLE_ID    = 'TRANSITION'
                  AND F.INTEGER_VALUE  = A.TRANSITION ";


            string queryCount = @"SELECT COUNT(PIECE_ID)
                                FROM HRM_INPUT_PIECE A
                    LEFT OUTER JOIN TDB_MATERIAL_GRADE B
                    ON A.MATERIAL_GRADE_ID = B.MATERIAL_GRADE_ID
                    LEFT OUTER JOIN TDB_GRADE_GROUP C
                    ON C.GRADE_GROUP_ID = B.GRADE_GROUP_ID
                    LEFT OUTER JOIN HRM_HEAT  H
                    ON H.HEAT_NO = A.HEAT_NO
                    LEFT OUTER JOIN HRM_ORDER  ORD
                    ON ORD.PIECE_NO = A.PIECE_NO
                    LEFT OUTER JOIN HRM_CUSTOMER CUST
                    ON CUST.CUSTOMER_ID = ORD.CUSTOMER_ID
                    LEFT OUTER JOIN AUX_VALUE G
                    ON G.INTEGER_VALUE  = ORD.TRIAL_FLAG
                    AND G.VARIABLE_ID   = 'YESNO_FLAG'
                       ,AUX_VALUE E
                       ,AUX_VALUE F
                  WHERE E.VARIABLE_ID  = 'STATUS_INPUT'
                  AND E.INTEGER_VALUE  = A.STATUS
                  AND F.VARIABLE_ID    = 'TRANSITION'
                  AND F.INTEGER_VALUE  = A.TRANSITION ";


            var searchSlabNo = listRequest.Filter?.SearchSlabNo;
            if (searchSlabNo.IsNullOrEmpty() == false)
            {
                query += "AND A.PIECE_ID LIKE :SearchSlabNo||'%' ";
                queryCount += "AND A.PIECE_ID LIKE :SearchSlabNo||'%' ";
                queryParam.Add(ctx.CreateParameter("SearchSlabNo", searchSlabNo));
                queryCountParam.Add(ctx.CreateParameter("SearchSlabNo", searchSlabNo));
            }

            var searchStringNo = listRequest.Filter?.SearchStringNo;
            if (searchStringNo.IsNullOrEmpty() == false)
            {
                query += "AND A.JOB_ID LIKE :SearchStringNo||'%' ";
                queryCount += "AND A.JOB_ID LIKE :SearchStringNo||'%' ";
                queryParam.Add(ctx.CreateParameter("SearchStringNo", searchStringNo));
                queryCountParam.Add(ctx.CreateParameter("SearchStringNo", searchStringNo));
            }


            var searchProductionStatusFrom = listRequest.Filter?.SearchProductionStatusFrom;
            if (searchProductionStatusFrom != null)
            {
                query += "AND (A.STATUS >= :SearchProductionStatusFrom) ";
                queryCount += "AND (A.STATUS >= :SearchProductionStatusFrom) ";
                queryParam.Add(ctx.CreateParameter("SearchProductionStatusFrom", searchProductionStatusFrom));
                queryCountParam.Add(ctx.CreateParameter("SearchProductionStatusFrom", searchProductionStatusFrom));
            }

            var searchProductionStatusTo = listRequest.Filter?.SearchProductionStatusTo;
            if (searchProductionStatusTo != null)
            {
                query += "AND (A.STATUS <= :SearchProductionStatusTo) ";
                queryCount += "AND (A.STATUS <= :SearchProductionStatusTo) ";
                queryParam.Add(ctx.CreateParameter("SearchProductionStatusTo", searchProductionStatusTo));
                queryCountParam.Add(ctx.CreateParameter("SearchProductionStatusTo", searchProductionStatusTo));
            }

            var searchCreationTimeFrom = listRequest.Filter?.SearchCreationTimeFrom;
            if (searchCreationTimeFrom != null)
            {
                query += "AND A.CREATION_DATETIME >= :SearchCreationTimeFrom ";
                queryCount += "AND A.CREATION_DATETIME >= :SearchCreationTimeFrom ";
                queryParam.Add(ctx.CreateParameter("SearchCreationTimeFrom", searchCreationTimeFrom, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchCreationTimeFrom", searchCreationTimeFrom, DbType.DateTimeOffset));
            }

            var searchCreationTimeTo = listRequest.Filter?.SearchCreationTimeTo;
            if (searchCreationTimeTo != null)
            {
                query += "AND A.CREATION_DATETIME <= :SearchCreationTimeTo ";
                queryCount += "AND A.CREATION_DATETIME <= :SearchCreationTimeTo ";
                queryParam.Add(ctx.CreateParameter("SearchCreationTimeTo", searchCreationTimeTo, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchCreationTimeTo", searchCreationTimeTo, DbType.DateTimeOffset));
            }

            var searchCustomerName = listRequest.Filter?.SearchCustomerName;
            if (searchCustomerName.IsNullOrEmpty() == false)
            {
                query += "AND CUST.CUSTOMER_NAME LIKE :SearchCustomerName||'%' ";
                queryCount += "AND CUST.CUSTOMER_NAME LIKE :SearchCustomerName||'%' ";
                queryParam.Add(ctx.CreateParameter("SearchCustomerName", searchCustomerName));
                queryCountParam.Add(ctx.CreateParameter("SearchCustomerName", searchCustomerName));
            }

            var searchCustomerOrderNo = listRequest.Filter?.SearchCustomerOrderNo;
            if (searchCustomerOrderNo.IsNullOrEmpty() == false)
            {
                query += "AND ORD.ORDER_NUMBER LIKE :SearchCustomerOrderNo||'%' ";
                queryCount += "AND ORD.ORDER_NUMBER LIKE :SearchCustomerOrderNo||'%' ";
                queryParam.Add(ctx.CreateParameter("SearchCustomerOrderNo", searchCustomerOrderNo));
                queryCountParam.Add(ctx.CreateParameter("SearchCustomerOrderNo", searchCustomerOrderNo));
            }

            var searchHeatNo = listRequest.Filter?.SearchHeatNo;
            if (searchHeatNo != null)
            {
                query += "AND (H.HEAT_ID LIKE :SearchHeatNo||'%') ";
                queryCount += "AND (H.HEAT_ID LIKE :SearchHeatNo||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchHeatNo", searchHeatNo));
                queryCountParam.Add(ctx.CreateParameter("SearchHeatNo", searchHeatNo));
            }

            var searchPieceStatus = listRequest.Filter?.SearchPieceStatus;
            if (searchPieceStatus != null)
            {
                query += "AND (A.STATUS = :SearchPieceStatus) ";
                queryCount += "AND (A.STATUS = :SearchPieceStatus) ";
                queryParam.Add(ctx.CreateParameter("SearchPieceStatus", searchPieceStatus));
                queryCountParam.Add(ctx.CreateParameter("SearchPieceStatus", searchPieceStatus));
            }


            query += "ORDER BY A.CREATION_DATETIME DESC";

            var data = ctx.GetEntities<HrmInputPieceListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<HrmInputPieceListItemDto>
            {
                Data = data,
                Total = total
            };
        }

        public HrmInputPieceForInsertDto Read(int pieceNo)
        {
            string query = "SELECT * FROM HRM_INPUT_PIECE WHERE PIECE_NO = :PieceNo ";
            HrmInputPieceForInsertDto result = ctx.GetEntity<HrmInputPieceForInsertDto>(query, ctx.CreateParameter("PieceNo", pieceNo));

            if (result == null)
                throw new NotFoundException(typeof(HrmInputPieceDetailDto), pieceNo);

            return result;
        }

        public ListResultDto<HrmInputPieceLookupDto> SelInPieceForProdCoil(ListRequestDto<HrmInputPieceLookupDto> listRequest)
        {

            string query = @"SELECT    TRIM(PIECE_ID)       AS Display
		    				          ,PIECE_NO             AS Value
		    				  FROM     HRM_INPUT_PIECE
		    				  WHERE    STATUS    BETWEEN 55 AND 120 ";

            string queryCount = @"SELECT COUNT(PIECE_ID)
                                FROM HRM_INPUT_PIECE
                                WHERE STATUS BETWEEN 55 AND 120 
                                AND 1=1 ";

            query += "ORDER BY  PIECE_ID ASC ";

            var data = ctx.GetEntities<HrmInputPieceLookupDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0))).ToArray();
            var total = ctx.GetEntity<long>(queryCount);

            return new ListResultDto<HrmInputPieceLookupDto>
            {
                Data = data,
                Total = total
            };
        }

        public HrmInputPieceDetailDto GetCurrentInputPieceMng(int pieceNo)
        {
            string query = @" SELECT  TRIM(A.PIECE_ID)      AS PieceId
                         ,A.PIECE_SEQ                       AS PieceSeq
                         ,A.STATUS                          AS Status
                         ,TRIM(A.JOB_ID)                    AS JobId
                         ,A.HEAT_NO                         AS HeatNo
                         ,TRIM(B.HEAT_ID)                   AS HeatId
                         ,A.HEAT_SEQ                        AS HeatSeq
                         ,A.PASS_NO                         AS PassNo
                         ,A.HEAT_PIECE_SEQ                  AS HeatPieceSeq
                         ,A.JOB_PIECE_SEQ                   AS JobPieceSeq
                         ,A.SOURCE_CODE_ID                  AS SourceCodeId
                         ,A.DEST_CODE_ID                    AS DestCodeId
                         ,A.TRANSITION                      AS Transition
                         ,A.PRELIMINARY_WDT_CHG             AS PreliminaryWdtChg
                         ,A.PRELIMINARY_THK                 AS PreliminaryThk
                         ,A.PRELIMINARY_THK_HEAD            AS PreliminaryThkHead
                         ,A.PRELIMINARY_THK_TAIL            AS PreliminaryThkTail
                         ,A.PRELIMINARY_WDT                 AS PreliminaryWdt
                         ,A.PRELIMINARY_WDT_HEAD            AS PreliminaryWdtHead
                         ,A.PRELIMINARY_WDT_TAIL            AS PreliminaryWdtTail
                         ,A.ENTRY_TEMP                      AS EntryTemp
                         ,A.WEIGHT                          AS Weight
                         ,A.PRELIMINARY_LEN                 AS PreliminaryLength       
                         ,A.USE_MEAS_WIDTH                  AS UseMeasWidth
                         ,A.MEASURED_WIDTH_HEAD             AS MeasuredWidthHead
                         ,A.MEASURED_WIDTH_TAIL             AS MeasuredWidthTail
                         ,A.USE_MEAS_TEMP                   AS UseMeasTemp
                         ,A.MEASURED_TEMP                   AS MeasuredTemp
                         ,TRIM(A.MATERIAL_GRADE_ID)         AS MaterialGradeId
                         ,TRIM(PRELIMINARY_MAT_GRADE_ID)    AS PreliminaryMatGradeId
                         ,A.FURNACE_DISCHARGE_TEMP          AS FurnaceDischargeTemp
                         ,A.TARGET_EXIT_TEMP                AS TargetExitTemp
                         ,A.TARGET_EXIT_TEMP_NTOL           AS TargetExitTempNtol
                         ,A.TARGET_EXIT_TEMP_PTOL           AS TargetExitTempPtol
                         ,A.TARGET_TEMP_FM_CUSTOMER_TOL     AS TargetTempFmCustomertol
                         ,A.TARGET_TEMP_DC                  AS TargetTempDC
                         ,A.TARGET_TEMP_DC_NTOL             AS TargetTempDCNtol
                         ,A.TARGET_TEMP_DC_PTOL             AS TargetTempDCPtol
                         ,A.TARGET_TEMP_DC_CUSTOMER_TOL     AS TargetTempDCCustomertol
                         ,A.TARGET_PROFILE                  AS TargetProfile
                         ,A.TARGET_PROFILE_NTOL             AS TargetProfileNtol
                         ,A.TARGET_PROFILE_PTOL             AS TargetProfilePtol
                         ,A.TARGET_PROFILE_CUSTOMER_TOL     AS TargetProfileCustomertol
                         ,A.TARGET_FLATNESS                 AS TargetFlatness
                         ,A.TARGET_FLATNESS_NTOL            AS TargetFlatnessNtol
                         ,A.TARGET_FLATNESS_PTOL            AS TargetFlatnessPtol
                         ,A.TARGET_FLATNESS_CUSTOMER_TOL    AS TargetFlatnessCustomertol
                         ,A.TARGET_THICKNESS                AS TargetThickness
                         ,A.TARGET_THICKNESS_NTOL           AS TargetThicknessNtol
                         ,A.TARGET_THICKNESS_PTOL           AS TargetThicknessPtol
                         ,A.TARGET_WIDTH                    AS TargetWidth
                         ,TRIM(A.BASE_GRADE_ID)             AS BaseGradeId
                         ,A.USE_BASE_GRADE                  AS UseBaseGrade
                         ,A.TARGET_WIDTH_NTOL               AS TargetWidthNtol
                         ,A.TARGET_WIDTH_PTOL               AS TargetWidthPtol
                         ,A.TARGET_WEIGHT                   AS TargetWeight
                         ,A.TARGET_WEIGHT_NTOL              AS TargetWeightNtol
                         ,A.TARGET_WEIGHT_PTOL              AS TargetWeightPtol
                         ,A.TARGET_INTERNAL_DIAMETER        AS TargetInternalDiameter
                         ,TRIM(A.MATERIAL_SPEC_ID)          AS MaterialSpecId
                         ,A.REMARK                          AS Remark
                         ,G.GRADE_GROUP_ID                  AS GradeGroupId
                         ,D.ALLOY_SPEC_CNT                  AS AlloySpecCnt
                         ,TRIM(D.ALLOY_SPEC_CODE)           AS AlloySpecCode
                         ,D.ALLOY_SPEC_VERSION              AS AlloySpecVersion
                         ,TRIM(E.ORDER_NUMBER)              AS OrderNumber
                         ,TRIM(E.ORDER_POSITION)            AS OrderPosition
                         ,E.TRIAL_FLAG                      AS TrialFlag
                         ,TRIM(E.TRIAL_NO)                  AS TrialNo
                         ,TRIM(E.CUSTOMER_ID)               AS CustomerId
                         ,TRIM(H.CUSTOMER_NAME)             AS CustomerName
                         ,TRIM(H.CUSTOMER_CONTACT_NAME)     AS CustomerContactName
                         ,TRIM(E.END_USE)                   AS EndUse
                         ,E.ENDUSE_SURFACE_RATING           AS EnduseSurfaceRating
                         ,TRIM(E.CARRIER_MODE)              AS CarrierMode
                         ,TRIM(E.AREA_TYPE)                 AS AreaType
                         ,TRIM(A.OPERATOR)                  AS Operator
                         ,A.REVISION                        AS Revision
                   FROM HRM_INPUT_PIECE A
                   LEFT OUTER JOIN HRM_HEAT B
                   ON A.HEAT_NO = B.HEAT_NO
                   LEFT OUTER JOIN HRM_ORDER E
                   ON A.PIECE_NO   = E.PIECE_NO
                   LEFT OUTER JOIN HRM_CUSTOMER H
                   ON E.CUSTOMER_ID = H.CUSTOMER_ID
                   LEFT OUTER JOIN TDB_MATERIAL_SPEC C
                   ON C.MATERIAL_SPEC_ID = A.MATERIAL_SPEC_ID
                   LEFT OUTER JOIN TDB_ALLOY_SPEC D
                   ON D.ALLOY_SPEC_CNT = C.ALLOY_SPEC_CORE
                   LEFT OUTER JOIN TDB_MATERIAL_GRADE G
                   ON G.MATERIAL_GRADE_ID = A.MATERIAL_GRADE_ID
                   WHERE A.PIECE_NO = :PieceNo ";
            HrmInputPieceDetailDto result = ctx.GetEntity<HrmInputPieceDetailDto>(query, ctx.CreateParameter("PieceNo", pieceNo));
            result.PieceNo = pieceNo;

            if (result == null)
                throw new NotFoundException(typeof(HrmInputPieceDetailDto), pieceNo);
            return result;
        }


        public bool Exists(int pieceNo)
        {
            try
            {
                Read(pieceNo);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public ListResultDto<HrmInputPieceLookupDto> Lookup(ListRequestDto<HrmInputPieceLookupDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = String.Format(
                @"SELECT
                    TRIM(JOB_ID) AS Display,
                    TRIM(JOB_ID) AS Value
                FROM
                    HRM_JOB"
            );

            string queryCount = String.Format(
                @"SELECT
                    COUNT(JOB_ID)
                FROM
                    HRM_JOB"
            );

            var data = ctx.GetEntities<HrmInputPieceLookupDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<HrmInputPieceLookupDto>
            {
                Data = data,
                Total = total
            };
        }

        private int findMax(string field, string table)
        {

            string query = $@" SELECT MAX({field}) FROM {table} ";

            var result = ctx.GetEntity<int>(query);

            return result;
        }

    }

}
