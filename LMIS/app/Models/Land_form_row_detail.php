<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Land_form_row_detail extends Model
{
    use HasFactory;

    protected $table = 'land_detail_rows';
    protected $fillable = [
        'land_form_id',
        'lo_cod',
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
        'land_category'
    ];
}
