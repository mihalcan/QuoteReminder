﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuoteReminder.Models
{
    public class Quote
    {
        public int QuoteId { get; set; }

        [MaxLength(100)]
        public string Group { get; set; }

        [MaxLength(400)]
        public string Text { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastRemind { get; set; }

        public DateTime NextRemind { get; set; }
        
        [NotMapped]
        public EditType EditType { get; set; }
    }
}