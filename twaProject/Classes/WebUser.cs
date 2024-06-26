﻿using System.ComponentModel.DataAnnotations;

namespace twaProject.Classes;

public class WebUser
{
    public int WebUserId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Password { get; set; }
    
    //Navigation properties
    public List<Projekt>? Projekts { get; set; } = [];
    public List<Task>? Tasks { get; set; }= [];
    
}