using DA.DB.Utils;
using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.AuxValue;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.Production
{
    internal class HrmJobRepo<TDataSource> : IHrmJobRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public HrmJobRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public string Create(HrmJobForInsertDto dto)
        {
            string insert = "INSERT INTO HRM_JOB (Job_Id, Job_Seq, Remark, Status, Order_Start_Date, Order_End_Date, Operator, Revision) " +
            "VALUES (:JobId, :JobSeq, :Remark, :Status, :OrderStartDate, :OrderEndDate, :Operator, :Revision)";
            ctx.ExecuteNonQuery(insert,
                ctx.CreateParameter("JobId", dto.JobId),
                ctx.CreateParameter("JobSeq", dto.JobSeq),
                ctx.CreateParameter("Remark", dto.Remark),
                ctx.CreateParameter("Status", dto.Status),
                ctx.CreateParameter("OrderStartDate", dto.OrderStartDate),
                ctx.CreateParameter("OrderEndDate", dto.OrderEndDate),
                ctx.CreateParameter("Operator", dto.Operator),
                ctx.CreateParameter("Revision", dto.Revision)
            );

            return dto.JobId;
        }

        public void Delete(string JobId)
        {
            string delete = "DELETE FROM HRM_JOB WHERE Job_Id = rpad(:JobId, 32, ' ') ";
            ctx.ExecuteNonQuery(delete, ctx.CreateParameter("JobId", JobId));
        }

        public void Update(HrmJobForUpdateDto dto, string JobId)
        {
            string update = "UPDATE HRM_JOB SET " +
            "Job_Seq = :JobSeq, Remark = :Remark, Status = :Status, Order_Start_Date = :OrderStartDate, Order_End_Date = :OrderEndDate, " +
            "Operator = :Operator, Revision = :Revision WHERE Job_Id = rpad(:JobId, 32, ' ') ";

            int affectedRows = ctx.ExecuteNonQuery(update,
                ctx.CreateParameter("JobSeq", dto.JobSeq),
                ctx.CreateParameter("Remark", dto.Remark),
                ctx.CreateParameter("Status", dto.Status),
                ctx.CreateParameter("OrderStartDate", dto.OrderStartDate),
                ctx.CreateParameter("OrderEndDate", dto.OrderEndDate),
                ctx.CreateParameter("Operator", dto.Operator),
                ctx.CreateParameter("Revision", dto.Revision),
                ctx.CreateParameter("JobId", JobId)
            );
        }



        public ListResultDto<HrmJobListItemDto> SelJobsList(ListRequestDto<HrmJobListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT A.JOB_SEQ                                                 AS JobSeq
                       , TRIM(A.JOB_ID)                                                       AS JobId
                       , TRIM(A.REMARK)                                                       AS Remark
                       , A.STATUS                                                             AS Status
                       , TRIM(TO_CHAR(B.INTEGER_VALUE, '999')) || ': ' || TRIM(B.VALUE_LABEL) AS StatusLabel
                       , A.ORDER_START_DATE                                                   AS OrderStartDate
                       , TRIM(A.OPERATOR)                                                     AS Operator
                       , A.REVISION                                                           AS Revision
                  FROM HRM_JOB A
                  LEFT OUTER JOIN AUX_VALUE   B
                  ON  B.VARIABLE_ID = 'STATUS_JOB' AND A.STATUS = B.INTEGER_VALUE WHERE 1 = 1 ";


            string queryCount = "SELECT COUNT(JOB_ID) " +
                                "FROM HRM_JOB " +
                                "WHERE 1 = 1 ";


            var searchProductionStatus = listRequest.Filter?.SearchProductionStatus;
            if (searchProductionStatus != null)
            {
                query += "AND (STATUS LIKE :SearchProductionStatus||'%') ";
                queryCount += "AND (STATUS LIKE :SearchProductionStatus||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchProductionStatus", searchProductionStatus));
                queryCountParam.Add(ctx.CreateParameter("SearchProductionStatus", searchProductionStatus));
            }

            var searchJobId = listRequest.Filter?.SearchJobId;
            if (searchJobId.IsNullOrEmpty() == false)
            {
                query += "AND (JOB_ID LIKE :SearchJobId||'%') ";
                queryCount += "AND (JOB_ID LIKE :SearchJobId||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchJobId", searchJobId));
                queryCountParam.Add(ctx.CreateParameter("SearchJobId", searchJobId));
            }

            var searchDateFrom = listRequest.Filter?.SearchDataFrom;
            if (searchDateFrom != null)
            {
                query += "AND A.ORDER_START_DATE >= :SearchDataFrom ";
                queryCount += "AND ORDER_START_DATE >= :SearchDataFrom ";
                queryParam.Add(ctx.CreateParameter("SearchDataFrom", searchDateFrom, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchDataFrom", searchDateFrom, DbType.DateTimeOffset));
            }


            var searchDataTo = listRequest.Filter?.SearchDataTo;
            if (searchDataTo != null)
            {
                query += "AND A.ORDER_START_DATE <= :SearchDataTo ";
                queryCount += "AND ORDER_START_DATE <= :SearchDataTo ";
                queryParam.Add(ctx.CreateParameter("SearchDataTo", searchDataTo, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchDataTo", searchDataTo, DbType.DateTimeOffset));
            }

            query += "ORDER BY A.ORDER_START_DATE DESC";

            return ReadListPage<HrmJobListItemDto, HrmJobListFilterDto>(
                query,
                queryCount,
                listRequest,
                queryParam,
                queryCountParam
            );
        }


        public HrmJobDetailDto SelJobById(string JobId)
        {
            string query = @"SELECT A.JOB_SEQ                                                  AS JobSeq
                       , TRIM(A.JOB_ID)                                                        AS JobId
                       , TRIM(A.REMARK)                                                        AS Remark
                       , A.STATUS                                                              AS Status
                       , LTRIM(TO_CHAR(B.INTEGER_VALUE, '999')) || ': ' || TRIM(B.VALUE_LABEL) AS StatusLabel
                       , A.ORDER_START_DATE                                                    AS OrderStartDate
                       , TRIM(A.OPERATOR)                                                      AS Operator
                       , A.NUM_INPUT_PIECES                                                    AS TotalNumberOf
                       , A.REVISION                                                            AS Revision
                  FROM HRM_JOB  A
                  LEFT OUTER JOIN AUX_VALUE   B
                  ON  B.VARIABLE_ID = 'STATUS_JOB' AND A.STATUS = B.INTEGER_VALUE WHERE a.JOB_ID = rpad(:JobId, 32, ' ') ";
            HrmJobDetailDto result = ctx.GetEntity<HrmJobDetailDto>(query, ctx.CreateParameter("JobId", JobId));

            if (result == null)
                throw new NotFoundException(typeof(HrmJobDetailDto), JobId);

            return result;
        }

        public HrmJobDetailDto Read(string JobId)
        {
            string query = @"SELECT A.JOB_SEQ                                                  AS JobSeq
                       , TRIM(A.JOB_ID)                                                        AS JobId
                       , TRIM(A.REMARK)                                                        AS Remark
                       , A.STATUS                                                              AS Status
                       , LTRIM(TO_CHAR(B.INTEGER_VALUE, '999')) || ': ' || TRIM(B.VALUE_LABEL) AS StatusLabel
                       , A.ORDER_START_DATE                                                    AS OrderStartDate
                       , TRIM(A.OPERATOR)                                                      AS Operator
                       , A.REVISION                                                            AS Revison
                  FROM HRM_JOB  A
                  LEFT OUTER JOIN AUX_VALUE   B
                  ON  B.VARIABLE_ID = 'STATUS_JOB' AND A.STATUS = B.INTEGER_VALUE WHERE a.JOB_ID = rpad(:JobId, 32, ' ') ";
            HrmJobDetailDto result = ctx.GetEntity<HrmJobDetailDto>(query, ctx.CreateParameter("JobId", JobId));

            if (result == null)
                throw new NotFoundException(typeof(HrmJobDetailDto), JobId);

            return result;
        }

        public bool Exists(string id)
        {
            try
            {
                Read(id);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public ListResultDto<HrmJobLookupDto> Lookup(ListRequestDto<HrmJobLookupDto> listRequest)
        {
            string entityTable = "HRM_JOB";
            string constraint = "(STATUS BETWEEN 10 AND 99)";
            string entityQuery = String.Format(
                @"SELECT
                    TRIM(JOB_ID) AS Display,
                    TRIM(JOB_ID) AS Value
                FROM
                    {0}
                WHERE
                    {1}
                ORDER BY
                    JOB_ID ASC",
                entityTable,
                constraint
            );
            string entityCountQuery = String.Format(
                @"SELECT
                    COUNT(JOB_ID)
                FROM
                    {0}
                WHERE
                    {1}
                    AND
                    (1 = 1)",
                entityTable,
                constraint
            );

            return ReadListPage<HrmJobLookupDto, HrmJobLookupDto>(
                entityQuery,
                entityCountQuery,
                listRequest
            );
        }

        private ListResultDto<TEntityListItem> ReadListPage<TEntityListItem, TEntityListFilter>(
            string entitySelectQuery,
            string entityCountQuery,
            ListRequestDto<TEntityListFilter> listRequest,
            List<IDbDataParameter> entitySelectParams = null,
            List<IDbDataParameter> entityCountParams = null
        ) {
            int limit = Convert.ToInt32(listRequest.Page?.Take ?? 0);
            int offset = Convert.ToInt32(listRequest.Page?.Skip ?? 0);

            IDbDataParameter[] selectParams = OptionalParameters(entitySelectParams);
            IDbDataParameter[] countParams = OptionalParameters(entityCountParams);

            var data = ctx.GetEntities<TEntityListItem>(ctx.PaginateSqlStatement(entitySelectQuery, limit, offset), selectParams).ToArray();
            var total = ctx.GetEntity<long>(entityCountQuery, countParams);

            return new ListResultDto<TEntityListItem>
            {
                Data = data,
                Total = total
            };
        }

        private IDbDataParameter[] OptionalParameters(List<IDbDataParameter> paramList = null) {
            if (paramList == null) {
                return new IDbDataParameter[0];
            } else {
                return paramList.ToArray();
            }
        }

    }

}
