﻿using System;
using Model.Models.Database;

namespace Model.Models.Statistics
{
    public class Statistics
    {
        public DateTime Date { get; set; }
        public Game Game { get; set; }
        public decimal CorrectAnswers { get; set; }
        public decimal WrongAnswers { get; set; }
        public decimal AverageTime { get; set; }
    }
}
