﻿using System.ComponentModel.DataAnnotations;

namespace twaProject.Classes;

public class WebUser
{
    public int WebUserId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Password { get; set; }
}