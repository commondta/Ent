<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Exemption_inventory_row extends Model
{
    use HasFactory;

    protected $table = 'exemption_inventory_rows';

    protected $fillable = [
        'exemption_inventory_id',
        'category',
        'inventory_type',
        'size_of_file',
        'no_of_files',
        'rate_file_plot',
        'total_cost',
        'eighty_percent',
        'twenty_percent',
        'remark',
        'isDeleted',
    ];

    public function inventory()
    {
        return $this->belongsTo(Exemption_inventory_approval::class, 'exemption_inventory_id');
    }
}
