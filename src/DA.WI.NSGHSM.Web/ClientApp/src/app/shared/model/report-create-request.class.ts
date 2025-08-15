export type ReportCreateRequestLanguage = 'English' | 'Usa';

export class ReportCreateRequest {
    public reportLanguage: ReportCreateRequestLanguage;
    public reportType: 0 | 1 | 2 | 3 | 4 | 5;
    public reportParam1?: string;
    public reportParam2?: string;
    public reportParam3?: string;
    public reportParam4?: string;
    public reportParam5?: string;
    public reportParam6?: string;
    public reportParam7?: string;

    constructor(options?: Partial<ReportCreateRequest>) {
        if (options) {
            Object.assign(this, options);
        }
    }
}
