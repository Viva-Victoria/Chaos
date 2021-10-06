﻿using System;
using System.ComponentModel.DataAnnotations;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Sql.Enums;

namespace VivaVictoria.Chaos.Reflection.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MigrationAttribute : Attribute
    {
        [Required]
        public long Version { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public TransactionMode TransactionMode { get; set; } = TransactionMode.Default;
    }
}