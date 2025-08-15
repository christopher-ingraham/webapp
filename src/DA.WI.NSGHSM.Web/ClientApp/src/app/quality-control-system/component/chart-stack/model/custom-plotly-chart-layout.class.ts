import { CustomPlotlyChartLayoutTitle } from './custom-plotly-chart-layout-title.class';

const defaultBackgroundColor = 'rgb(37, 37, 37)';
const defaultScene = '{camera: {eye: {x: 1.87, y: 0.88, z: -0.64}}}';

export class CustomPlotlyChartLayout {
    public autosize: boolean;
    public title: CustomPlotlyChartLayoutTitle;
    public paper_bgcolor: string;
    public plot_bgcolor: string;

    constructor() {
        this.autosize = true;
        this.title = new CustomPlotlyChartLayoutTitle();
        this.paper_bgcolor = defaultBackgroundColor;
        this.plot_bgcolor = defaultBackgroundColor;
    }
}
