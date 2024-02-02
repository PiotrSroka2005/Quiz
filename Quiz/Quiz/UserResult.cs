﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz
{
    public class UserResult
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public double TotalTime { get; set; }
        public int Score { get; set; }
    }
}
