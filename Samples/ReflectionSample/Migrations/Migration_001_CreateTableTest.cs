﻿using VivaVictoria.Chaos.Reflection.Attributes;
using VivaVictoria.Chaos.Reflection.Interfaces;

namespace ReflectionSample.Migrations
{
    [Migration(Version = 1, Name = "create table test")]
    public class Migration_001_CreateTableTest : IMigration {
        public string Up()
        {
            return "create table test(id int primary key)";
        }

        public string Down()
        {
            return "drop table test";
        }
    }
}