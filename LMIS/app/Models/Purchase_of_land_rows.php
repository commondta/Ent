<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Purchase_of_land_rows extends Model
{
    use HasFactory;
    protected $fillable = [
        'deed_id',
        'khewat_no',
        'khatooni_no',
        'qatat',
        'measuring_k',
        'measuring_m',
        'measuring_sqft',
        'transfer_share',
        'land_measuring_k',
        'land_measuring_m',
        'land_measuring_sqft',
        'land_category',
        
    ];
}
