import {
    CoilGeneralReportListItem,
    PracticeReportListItem,
    ShiftReportListItem,
    StoppageReportListItem,
} from '../service';

export type ReportListItem =
    CoilGeneralReportListItem |
    ShiftReportListItem |
    PracticeReportListItem |
    StoppageReportListItem;

