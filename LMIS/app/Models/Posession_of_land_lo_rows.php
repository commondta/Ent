<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Posession_of_land_lo_rows extends Model
{
    use HasFactory;

    protected $table = 'Posession_of_land_lo_rows';

    protected $fillable = [
        'deed_id',
        'lo_name',
        'so',
        'lo_cnic',
        'contact_no',
    ];

    public function Possession_certificate()
    {
        return $this->belongsTo(Possession_certificate::class, 'deed_id');
    }
}
