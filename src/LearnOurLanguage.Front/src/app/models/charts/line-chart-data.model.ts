import { SingleSerieData } from './single-serie-data.model';

export class LineChartData {

    constructor(
        public labels: Array<string> = [],
        public data: Array<SingleSerieData> = []
    ) { }

}