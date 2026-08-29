<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Sellere_profile_land_row extends Model
{
    use HasFactory;
    protected $fillable = [
        'deed_id',
        'khewat_no',
        'khatooni_no',
        'rectangle_no',
        'muraba_no',
        'khasra_no',
        'kanal',
        'marla',
        'sq_feet',
    ];
}
