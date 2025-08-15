import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs';

import {
    RepHmPieceTrend,
    SubscriptionList,
    RepHmPiece,
    RepHmPieceSelectionHelper,
    RepHmPieceTrendSelectionHelper,
    UomValuePipe,
} from '@app/shared';
import { CustomPlotlyChart, CustomPlotlyChartLayoutTitle } from './model';
import { RepHmTrendsViewSelectionHelper, RepHmTrendsView } from 'src/app/shared/service/rep-hm-trends-view-api';

const unknownNumber = -42;
const unknownString = '?';

@Component({
    selector: 'app-chart-stack',
    templateUrl: './chart-stack.component.html',
    styleUrls: ['./chart-stack.component.css']
})
export class ChartStackComponent
    extends SubscriptionList
    implements OnInit, OnDestroy {

    /** @summary selected RepHmPiece */
    @Input() coil: RepHmPieceSelectionHelper;

    /** @summary OPTIONAL selected RepHmPieceTrend */
    @Input() trends?: RepHmPieceTrendSelectionHelper;

    /** @summary OPTIONAL selected RepHmTrendsView */
    @Input() mainSignals?: RepHmTrendsViewSelectionHelper;

    /** @summary emit expand (true) / collapse (false) events */
    @Input() expanded: Observable<boolean>;

    protected revision = 1;
    private get coilData(): Partial<RepHmPiece> {
        if (this.coil && this.coil.entity) {
            return this.coil.entity;
        } else {
            return {
                steelGradeId: unknownString,
                producedCoilId: unknownString,
                inputCoilId: unknownString,
                customerOrderNo: unknownString,
                customerLineNo: unknownString,
                heatNo: unknownNumber,
                thickness: unknownNumber,
                weight: unknownNumber,
                width: unknownNumber,
                externalDiameter: unknownNumber
            };
        }
    }


    public isWidthFull: boolean;

    public graph: CustomPlotlyChart[];

    public get producedCoilId(): string {
        return this.coilData.producedCoilId;
    }
    public get steelGradeId(): string {
        return this.coilData.steelGradeId;
    }

    public get heatNo(): number {
        return this.coilData.heatNo;
    }

    public get inputCoilId(): string {
        return this.coilData.inputCoilId;
    }

    public get customerOrderNo(): string {
        return this.coilData.customerOrderNo;
    }

    public get customerLineNo(): string {
        return this.coilData.customerLineNo;
    }

    public get coilWidth(): number {
        return this.uomValuePipe.transform(this.coilData.width, 'mm', 'in');
    }

    public get coilThickness(): number {
        return this.uomValuePipe.transform(this.coilData.thickness, 'mm', 'in');
    }

    public get externalDiameter(): number {
        return this.uomValuePipe.transform(this.coilData.externalDiameter, 'mm', 'in');
    }

    public get coilWeigth(): number {
        return this.uomValuePipe.transform(this.coilData.weight, 'kg', 'lb');
    }

    public set selectedChartToPrint(value: number) {
        for (let i = 0; i < this.graph.length; i++) {
            this.graph[i].toPrint = (value === i);
        }
    }
    public get selectedChartToPrint(): number {
        return this.findGraphToPrint();
    }
    public get printDisabled(): boolean {
        return !(this.graph[0].visible || this.graph[1].visible || this.graph[2].visible);
    }

    constructor(private uomValuePipe: UomValuePipe) {
        super();
        this.graph = [0, 1, 2].map((n) => new CustomPlotlyChart());
    }

    ngOnInit() {
        this.subscribe(
            this.expanded.subscribe((isExpanded) => this.processExpansion(isExpanded)),
        );

        /* IF @Input() coil defined */
        if (this.coil) {
            this.subscribe(
                this.coil.subject.subscribe((coil) => this.processCoil(coil)),
            );
        }

        /* IF @Input() trends defined */
        if (this.trends) {
            this.subscribe(

                this.trends.subject.subscribe((trendList) => this.processTrends(trendList)),
            );
            this.processTrends();
        }

        /* IF @Input() mainSignals defined */
        if (this.mainSignals) {
            this.subscribe(
                this.mainSignals.subject.subscribe((trendList) => this.processTrends(trendList)),
            );
            this.processTrendsView();
        }
    }

    ngOnDestroy() {
        this.unsubscribeAll();
    }

    public graphDoubleClicked(event, ...other) {
        // TODO ChartStackComponent.graphDoubleClicked
        alert('graphDoubleClicked NOT implemented!');
        event.preventDefault();
    }

    public printChart(event: KeyboardEvent) {
        // TODO ChartStackComponent.printChart
        const graphIndex = this.findGraphToPrint();
        const graphTitle = this.graph[graphIndex].layout.title.text;
        const title = `
        ${ChartStackComponent['name']}.printChart(event: KeyboardEvent))

        NOT implemented!

        GRAPH n. ${1 + graphIndex}: ${graphTitle}`;
        alert(title);
        event.preventDefault();
    }

    private findGraphToPrint(): number {
        for (let i = 0; i < this.graph.length; i++) {
            if (this.graph[i].toPrint) {
                return i;
            }
        }
        this.graph[0].toPrint = true;
        return 0;
    }

    private processCoil(coil?: RepHmPiece) {
        if (coil) {
            // TODO processCoil
        } else {
            this.graph.forEach((g) => g.visible = false);
        }
    }

    private processExpansion(isExpanded: boolean) {
        this.isWidthFull = isExpanded;
    }

    private processTrends(trendList: RepHmPieceTrend[] = []) {
        this.revision++;
        const l = trendList.length;
        this.graph[0].visible = (l > 0);
        this.graph[1].visible = (l > 1);
        this.graph[2].visible = (l > 2);

        if (this.graph[0].visible) {
            /*            const zdSide = 100;
                        const zdHSide = zdSide / 2;
                        const zdQSide = zdHSide / 2;
                        const zd = new Array<Array<number>>(zdSide);
                        for (let i = 0; i < zd.length; i++) {
                            zd[i] = new Array<number>(zdSide);
                            for (let j = 0; j < zd[i].length; j++) {
                                zd[i][j] = Math.cos((i - zdHSide) / zdQSide) * Math.cos((j - zdHSide) / zdQSide);
                            }
                        }
            */
            this.graph[0].data = [
                {
                    x: trendList[0].chartDataX,
                    y: trendList[0].chartDataY,
                    z: trendList[0].chartDataZ,
                    type: trendList[0].chartType
                }
            ];
            let layout = {
                title: new CustomPlotlyChartLayoutTitle(trendList[0].title),
                scene: {camera: {eye: {x: -1.32, y: -1.90, z: 0.78}}},
                autosize: true,
                paper_bgcolor: 'rgb(37, 37, 37)',
                plot_bgcolor: 'rgb(37, 37, 37)'
            };
            this.graph[0].layout = layout;
        }

        if (this.graph[1].visible) {
            this.graph[1].data = [
                {
                    x: trendList[1].chartDataX,
                    y: trendList[1].chartDataY,
                    z: trendList[1].chartDataZ,
                    type: trendList[1].chartType,
                }
            ];
            let layout = {
                title: new CustomPlotlyChartLayoutTitle(trendList[1].title),
                scene: {camera: {eye: {x: -1.32, y: -1.90, z: 0.78}}},
                autosize: true,
                paper_bgcolor: 'rgb(37, 37, 37)',
                plot_bgcolor: 'rgb(37, 37, 37)'
            };
            this.graph[1].layout = layout;

        }

        if (this.graph[2].visible) {
            this.graph[2].data = [
                {
                    x: trendList[2].chartDataX,
                    y: trendList[2].chartDataY,
                    z: trendList[2].chartDataZ,
                    type: trendList[2].chartType
                }
            ];
            let layout = {
                title: new CustomPlotlyChartLayoutTitle(trendList[2].title),
                scene: {camera: {eye: {x: -1.32, y: -1.90, z: 0.78}}},
                autosize: true,
                paper_bgcolor: 'rgb(37, 37, 37)',
                plot_bgcolor: 'rgb(37, 37, 37)'
            };
            this.graph[2].layout = layout;
        }
    }

    private processTrendsView(trendList: RepHmTrendsView[] = []) {

        //
    }

}
