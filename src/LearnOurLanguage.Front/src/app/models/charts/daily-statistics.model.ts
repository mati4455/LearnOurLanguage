export class DailyStatistics {

    constructor(
        public gamesCount: number = 0,
        public answersRate: number = 0,
        public averageTime: number = 0,
        public totalTime: number = 0
    ) { }

    public minutes() {
        let me = this;
        return Math.floor(me.totalTime / 60);
    }

    public seconds() {
        let me = this;
        return me.totalTime % 60;
    }
}