﻿using System;
namespace App.Models
{
    public class PowerUpItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        public int UserId { get; set; }
    }
}

