import { CustomPlotlyChartLayout } from './custom-plotly-chart-layout.class';
import { CustomPlotlyChartData } from './custom-plotly-chart-data.class';

export class CustomPlotlyChart {
    public data: CustomPlotlyChartData[];
    public layout: CustomPlotlyChartLayout;
    public toPrint: boolean;
    public visible: boolean;

    constructor() {
        this.data = [];
        this.layout = new CustomPlotlyChartLayout();
        this.toPrint = false;
        this.visible = true;
    }
}
