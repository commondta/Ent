<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Conveyance_row extends Model
{
    use HasFactory;
    protected $fillable = [
        'deed_id',
        'block_no',
        'rectangle_no',
        'khasra_no',
        
        'east_by',
        'west_by',
        'north_by',
        'south_by',
    ];
}
