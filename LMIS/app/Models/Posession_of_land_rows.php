<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Posession_of_land_rows extends Model
{
    use HasFactory;
    protected $table = 'Posession_of_land_rows';
    protected $fillable = [
        'deed_id',
        'khewat_no',
        'khatooni_no',
        'muraba_no',
       // 'rectangle_no',
        'qatat',
        'sector',
        //'khasra_no',
        'kanal',
        'marla',
        'sq_feet',
       // 'block_no',
        // 'measuring_k',
        // 'measuring_m',
        // 'measuring_sqft',
       // 'transfer_share',
        'land_measuring_k',
        'land_measuring_m',
        'land_measuring_sqft',
        'possessed_k',
        'possessed_m',
        'possessed_sqft',
        'unpossessed_k',
        'unpossessed_m',
        'unpossessed_sqft',
        'land_category',
        'transferred_share',
    ];
}
