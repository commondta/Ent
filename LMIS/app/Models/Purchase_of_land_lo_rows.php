<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Purchase_of_land_lo_rows extends Model
{
    use HasFactory;

    protected $table = 'purchase_of_land_lo_rows';

    protected $fillable = [
        'deed_id',
        'lo_name',
        'so',
        'lo_cnic',
        'contact_no',
    ];

    public function purchase_of_land()
    {
        return $this->belongsTo(Purchase_of_land::class, 'deed_id');
    }
}
